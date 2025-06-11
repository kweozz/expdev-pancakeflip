using UnityEngine;
using System.Collections.Generic;
using TMPro; // belangrijk zodat Unity de tekst herkent

public class MixingLogic : MonoBehaviour
{
    public List<string> requiredIngredients;
    public TextMeshProUGUI feedbackText;

    public GameObject batterVisual; // Assign a plane/cylinder in the bowl
    public Color completeColor = Color.green; // Color when all ingredients are in
    public Color defaultColor = new Color(0.8f, 0.7f, 0.5f, 1f); // Pancake batter color

    private List<string> currentIngredients = new List<string>();
    private Renderer batterRenderer;
    private Vector3 initialBatterScale;
    private bool isComplete = false;

    void Start()
    {
        if (batterVisual != null)
        {
            batterRenderer = batterVisual.GetComponent<Renderer>();
            initialBatterScale = batterVisual.transform.localScale;
            batterRenderer.material.color = defaultColor;
            SetBatterHeight(0);
        }
    }

void SetBatterHeight(float t)
{
    if (batterVisual != null)
    {
        var scale = initialBatterScale;
        // Make the Y scale grow more dramatically (e.g., up to 3x the original height)
        float minHeight = 0.01f * initialBatterScale.y;
        float maxHeight = initialBatterScale.y * 3f; // Change 3f to a higher value if you want even more growth
        scale.y = Mathf.Lerp(minHeight, maxHeight, Mathf.Clamp01(t));
        batterVisual.transform.localScale = scale;
    }
}

    public void AddIngredient(GameObject ingredient)
    {
        string name = ingredient.name.ToLower().Trim(); // veiligere check
        if (!currentIngredients.Contains(name))
        {
            currentIngredients.Add(name);
            Debug.Log(name + " toegevoegd!");

            if (feedbackText != null)
            {
                feedbackText.text += "\n✓ " + name + " toegevoegd!";
                Canvas.ForceUpdateCanvases(); // forceer update
            }
        }

        // Update batter visual
        if (batterVisual != null && requiredIngredients.Count > 0)
        {
            float t = Mathf.Clamp01((float)currentIngredients.Count / requiredIngredients.Count);
            SetBatterHeight(t);
        }

        CheckIfComplete();
    }

    void CheckIfComplete()
    {
        if (isComplete) return;
        // Only complete if all required ingredients are present and the count matches
        foreach (var req in requiredIngredients)
        {
            if (!currentIngredients.Contains(req.ToLower().Trim())) return;
        }
        if (currentIngredients.Count != requiredIngredients.Count) return;

        isComplete = true;
        Debug.Log("Mengsel compleet!");

        if (feedbackText != null)
            feedbackText.text += "\n✔ Mengsel compleet!";

        // Turn batter green
        if (batterRenderer != null)
            batterRenderer.material.color = completeColor;
    }
}

