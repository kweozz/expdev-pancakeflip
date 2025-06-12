using UnityEngine;
// Voor de Input System:
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody))]
public class PancakeFlip : MonoBehaviour
{
    [Header("Baktijden")]
    public float bakeTime  = 5f;
    public float tolerance = 1f;

    private float timer     = 0f;
    private bool  isFlipped = false;
    private bool  canFlip   = false;
    private bool  isBaking  = false;

    private Renderer rend;
    private Material pancakeMaterial;
    private readonly Color rawColor   = new Color(1f, 0.85f, 0.6f);
    private readonly Color brownColor = new Color(0.4f, 0.2f, 0.05f);

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            // Instantieer materiaal-instance
            pancakeMaterial      = rend.material;
            pancakeMaterial.color = rawColor;
        }

        // Zorg dat physics werkt (zodat de flip-kracht doorwerkt)
        GetComponent<Rigidbody>().isKinematic = false;
    }

    void Update()
    {
        // Alleen bakken/flips checken als we daadwerkelijk bakken
        if (!isBaking || isFlipped || !canFlip) 
            return;

        timer += Time.deltaTime;

        // Maak de pannenkoek geleidelijk bruin
        float t = Mathf.Clamp01(timer / bakeTime);
        if (pancakeMaterial != null)
            pancakeMaterial.color = Color.Lerp(rawColor, brownColor, t);

        // Input controleren
        #if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            AttemptFlip();
        #else
        if (Input.GetKeyDown(KeyCode.Space))
            AttemptFlip();
        #endif
    }

    /// <summary>
    /// Wordt aangeroepen door PourBatter juist nadat de pannenkoek gespawned is.
    /// Vanaf dat moment mag hij gaan bakken én mag er ge-flippt worden.
    /// </summary>
    public void StartBaking()
    {
        isBaking = true;
        timer     = 0f;
        canFlip   = true;
    }

    private void AttemptFlip()
    {
        if (!canFlip || isFlipped) 
            return;

        canFlip = false;
        float diff = Mathf.Abs(timer - bakeTime);

        if (diff <= tolerance)
            Debug.Log("✅ Perfect flip!");
        else if (timer > bakeTime + tolerance)
            Debug.Log("🔥 Te laat – verbrand!");
        else
            Debug.Log("🤢 Te vroeg – rauw!");

        PerformFlip();
    }

    private void PerformFlip()
    {
        isFlipped = true;

        var rb = GetComponent<Rigidbody>();
        // Forceer een opwaartse duw + rotatie
        rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
        rb.AddTorque(Vector3.right * 500f, ForceMode.Impulse);
    }
}
