using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Respawnable respawnable = other.GetComponent<Respawnable>();
        if (respawnable != null)
        {
            respawnable.Respawn();
        }
    }
}
// Deze class detecteert objecten die de respawn zone binnenkomen en roept de Respawn methode aan.
// Zorg ervoor dat de respawn zone een Collider heeft met 'Is Trigger' aanstaan.