using UnityEngine;
using System.Collections.Generic;
using TMPro; // belangrijk zodat Unity de tekst herkent

public class MixingLogic : MonoBehaviour
{
    public List<string> requiredIngredients;
    public TextMeshProUGUI feedbackText;

    private List<string> currentIngredients = new List<string>();
    void CheckIfComplete()
    {
        foreach (var req in requiredIngredients)
        {
            if (!currentIngredients.Contains(req)) return;
        }

        Debug.Log("Mengsel compleet!");

        if (feedbackText != null)
            feedbackText.text += "\n✔ Mengsel compleet!";
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

        CheckIfComplete();
    }
}

