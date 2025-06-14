using UnityEngine;
using TMPro;

public class PourBatter : MonoBehaviour
{
    [Header("Benodigdheden")]
    public MixingLogic mixingLogic;
    public GameObject pancakePrefab;
    public Transform spawnPoint;
    public Transform panTransform;

    [Header("Game Logica & UI")]
    public GameLogic gameLogic;
    public Canvas feedbackCanvas;
    public TextMeshProUGUI feedbackText;

    [Header("Feedback")]
    public ParticleSystem successParticles;
    public ParticleSystem failParticles;
    public AudioClip successSFX;
    public AudioClip failSFX;

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

        // Start het bakproces en geef alles door aan PancakeFlip
        PancakeFlip flipScript = pancake.GetComponent<PancakeFlip>();
        if (flipScript != null)
        {
            flipScript.panTransform = panTransform;
            flipScript.gameLogic = gameLogic;
            flipScript.feedbackCanvas = feedbackCanvas;
            flipScript.feedbackText = feedbackText;
            flipScript.successParticles = successParticles;
            flipScript.failParticles = failParticles;
            flipScript.successSFX = successSFX;
            flipScript.failSFX = failSFX;

            flipScript.StartBaking();
        }
        else
        {
            Debug.LogWarning("Geen PancakeFlip script gevonden op prefab!");
        }
    }

    public void ResetPour()
    {
        hasSpawned = false;
    }
}
