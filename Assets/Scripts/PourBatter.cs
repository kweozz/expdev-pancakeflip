using UnityEngine;

public class PourBatter : MonoBehaviour
{
    public MixingLogic mixingLogic;
    public GameObject pancakePrefab;
    public Transform spawnPoint;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger met: " + other.name + " | Tag: " + other.tag);

        if (hasSpawned) return;

        // Batter object (dat de tag "Batter" heeft) wordt in de pan gegoten
        if (other.CompareTag("Batter") && mixingLogic != null && mixingLogic.IsComplete())
        {
            SpawnPancake();
        }
    }

    void SpawnPancake()
    {
        Quaternion flatRotation = Quaternion.Euler(0, 0, 0); // vlak in pan
        GameObject pancake = Instantiate(pancakePrefab, spawnPoint.position, flatRotation);
        hasSpawned = true;
        Debug.Log("Pannenkoek gespawned!");

        // Start het bakproces
        PancakeFlip flipScript = pancake.GetComponent<PancakeFlip>();
        if (flipScript != null)
        {
            flipScript.StartBaking();
        }
    }
}
