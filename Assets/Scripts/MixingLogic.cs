using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MixingLogic : MonoBehaviour
{
    public List<string> requiredIngredients;
    public TextMeshProUGUI feedbackText;
    public GameObject batterVisual;
    public Color completeColor = Color.green;
    public Color defaultColor = new Color(0.8f, 0.7f, 0.5f, 1f);

    private List<string> currentIngredients = new List<string>();
    private Renderer batterRenderer;
    private Vector3 initialBatterScale;
    private bool isComplete = false;

    void Start()
    {
        AudioManager.Instance.PlayMusic("lofi_background", 0.5f);

        if (batterVisual != null)
        {
            batterRenderer = batterVisual.GetComponent<Renderer>();
            initialBatterScale = batterVisual.transform.localScale;
            batterRenderer.material.color = defaultColor;
            SetBatterHeight(0);
        }

        UpdateIngredientListDisplay();
    }

    void SetBatterHeight(float t)
    {
        if (batterVisual != null)
        {
            var scale = initialBatterScale;
            float minHeight = 0.01f * initialBatterScale.y;
            float maxHeight = initialBatterScale.y * 3f;
            scale.y = Mathf.Lerp(minHeight, maxHeight, Mathf.Clamp01(t));
            batterVisual.transform.localScale = scale;
        }
    }

    public void AddIngredient(GameObject ingredient)
    {
        string name = ingredient.name.ToLower().Trim();
        if (!currentIngredients.Contains(name))
        {
            currentIngredients.Add(name);
            UpdateIngredientListDisplay();
        }

        if (batterVisual != null && requiredIngredients.Count > 0)
        {
            float t = (float)currentIngredients.Count / requiredIngredients.Count;
            SetBatterHeight(t);
        }

        CheckIfComplete();
    }

    void UpdateIngredientListDisplay()
    {
        if (feedbackText == null) return;

        feedbackText.text = "Nog toe te voegen:\n";

        foreach (var req in requiredIngredients)
        {
            string normalized = req.ToLower().Trim();
            if (!currentIngredients.Contains(normalized))
            {
                feedbackText.text += $"- {req}\n";
            }
        }

        Canvas.ForceUpdateCanvases();
    }

    void CheckIfComplete()
    {
        if (isComplete) return;

        foreach (var req in requiredIngredients)
        {
            if (!currentIngredients.Contains(req.ToLower().Trim()))
                return;
        }

        if (currentIngredients.Count != requiredIngredients.Count) return;

        isComplete = true;

        if (feedbackText != null)
            feedbackText.text = "<color=green> <b> Mengsel compleet!</b></color>";

        if (batterRenderer != null)
            batterRenderer.material.color = completeColor;

        Debug.Log("Mengsel compleet!");
    }

    public bool IsComplete()
    {
        return isComplete;
    }
}