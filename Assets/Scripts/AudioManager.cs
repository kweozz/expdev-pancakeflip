using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float defaultVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>(soundName);
        if (clip == null)
        {
            Debug.LogWarning($"AudioManager: AudioClip '{soundName}' not found in Resources.");
            return;
        }

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, defaultVolume);
    }
}
