using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public GameObject introPanel;
    public GameObject gameOverPanel;
    public GameObject gameplayRoot;

    public Transform xrRig;
    public Transform xrSpawnPoint;

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

         // Move XR rig to spawn location
        if (xrRig != null && xrSpawnPoint != null)
        xrRig.position = xrSpawnPoint.position;
    }

    public void GameComplete()
    {
        gameOverPanel.SetActive(true);
        gameplayRoot.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
