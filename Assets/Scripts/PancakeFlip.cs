using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
public class PancakeFlip : MonoBehaviour
{
    [Header("Baktijden")]
    public float bakeTime  = 5f;
    public float tolerance = 1f;

    [Header("Flip-kracht")]
    public float flipForce  = 4f;
    public float spinTorque = 500f;

    // intern
    private float timer     = 0f;
    private bool  isBaking  = false;
    private bool  canFlip   = false;
    private bool  isFlipped = false;

    // voor XR-input
    private InputDevice rightHand;
    private bool lastSecondaryButtonState = false;

    private Renderer rend;
    private Material pancakeMat;
    private readonly Color rawColor   = new Color(1f, 0.85f, 0.6f);
    private readonly Color brownColor = new Color(0.4f, 0.2f, 0.05f);

    void Awake()
    {
        // maak materiaal instance
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            pancakeMat = rend.material;
            pancakeMat.color = rawColor;
        }

        // physics on
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity  = true;

        // zoek de rechter controller
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHand = devices[0];
    }

    void Update()
    {
        if (!isBaking || isFlipped)
            return;

        // timer voor bakken + visuele kleur
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / bakeTime);
        if (pancakeMat != null)
            pancakeMat.color = Color.Lerp(rawColor, brownColor, t);

        // zodra we binnen de tolerance-zone zitten, mag er ge-flippt worden
        if (timer >= bakeTime - tolerance)
            canFlip = true;

        // lees B-knop (secondaryButton) van rechter controller
        if (canFlip && rightHand.isValid)
        {
            bool secondaryButtonState;
            if (rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonState))
            {
                // detecteer druk-actie (rising edge)
                if (secondaryButtonState && !lastSecondaryButtonState)
                    AttemptFlip();

                lastSecondaryButtonState = secondaryButtonState;
            }
        }
    }

    /// <summary>
    /// Start het bakproces; roep dit aan zodra je 'm in de pan hebt gegoten.
    /// </summary>
    public void StartBaking()
    {
        timer       = 0f;
        isBaking    = true;
        canFlip     = false;
        isFlipped   = false;
        lastSecondaryButtonState = false;
    }

    private void AttemptFlip()
    {
        if (!canFlip || isFlipped)
            return;

        isFlipped = true;
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
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * flipForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.right * spinTorque, ForceMode.Impulse);
    }
}