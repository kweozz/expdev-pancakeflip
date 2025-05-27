using UnityEngine;

public class BowlDetector : MonoBehaviour
{
    public MixingLogic mixingLogic;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ingredient"))
        {
            mixingLogic.AddIngredient(other.gameObject);
            Destroy(other.gameObject); // optioneel
        }
    }
}
// Deze class detecteert ingrediënten die in de kom vallen en voegt ze toe aan de MixingLogic.
// Zorg ervoor dat de kom een Collider heeft met 'Is Trigger' aanstaan.