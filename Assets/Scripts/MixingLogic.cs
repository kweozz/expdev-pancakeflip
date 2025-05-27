using UnityEngine;
using System.Collections.Generic;

public class MixingLogic : MonoBehaviour
{
    public List<string> requiredIngredients;
    private List<string> currentIngredients = new List<string>();

    public void AddIngredient(GameObject ingredient)
    {
        string name = ingredient.name;
        if (!currentIngredients.Contains(name))
        {
            currentIngredients.Add(name);
            Debug.Log(name + " toegevoegd!");
        }

        CheckIfComplete();
    }

    void CheckIfComplete()
    {
        foreach (var req in requiredIngredients)
        {
            if (!currentIngredients.Contains(req)) return;
        }

        Debug.Log("Mengsel compleet!");
        // hier kun je een next-step triggeren, bijv. kom gieten activeren
    }
}
