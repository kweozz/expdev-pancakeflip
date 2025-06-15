using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject gameOverPanel;
    public GameObject gameplayRoot;

    public Transform xrRig;
    public Transform xrSpawnPoint;

    public PourBatter pourScript; // ← Koppel je PourBatter hieraan

    public GameObject batterObject; // Sleep hier je beslag-object in via de inspector
    public Transform batterStartPoint; // Sleep hier een lege GameObject als startpositie



    void Start()
    {
        introPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        gameplayRoot.SetActive(false);
    }

    public void StartGame()
    {
        introPanel.SetActive(false);
        gameplayRoot.SetActive(true);

        if (xrRig && xrSpawnPoint)
            xrRig.position = xrSpawnPoint.position;
    }

    public void GameComplete()
    {
        gameOverPanel.SetActive(true);
        // gameplayRoot.SetActive(false); // <-- deze regel UITZETTEN of verwijderen
    }

    public void PrepareRetry()
    {
        gameplayRoot.SetActive(true);
        if (pourScript != null)
            pourScript.ResetPour();

        // Zet beslag terug naar startpositie
        if (batterObject != null && batterStartPoint != null)
        {
            batterObject.transform.position = batterStartPoint.position;
            batterObject.transform.rotation = batterStartPoint.rotation;
        }

        Debug.Log("🔁 Mislukte flip – giet opnieuw beslag om opnieuw te proberen.");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
