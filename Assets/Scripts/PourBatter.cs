using UnityEngine;

public class PourBatter : MonoBehaviour
{
    public MixingLogic mixingLogic;
    public GameObject pancakePrefab;
    public Transform spawnPoint;

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned) return;

        if (other.CompareTag("Bowl") && mixingLogic != null && mixingLogic.IsComplete())
        {
            SpawnPancake();
        }
    }

    public void TrySpawnPancake()
    {
        if (hasSpawned) return;

        if (mixingLogic != null && mixingLogic.IsComplete())
        {
            SpawnPancake();
        }
    }

    void SpawnPancake()
    {
        Instantiate(pancakePrefab, spawnPoint.position, spawnPoint.rotation);
        hasSpawned = true;
        Debug.Log("Pannenkoek gespawned!");
    }
}
