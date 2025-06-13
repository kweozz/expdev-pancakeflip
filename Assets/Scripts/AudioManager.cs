using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private float defaultVolume = 1f;
    private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false;
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

    public void PlayMusic(string musicName, float volume = 0.5f)
    {
        AudioClip music = Resources.Load<AudioClip>(musicName);
        if (music == null)
        {
            Debug.LogWarning($"AudioManager: Music '{musicName}' not found in Resources.");
            return;
        }

        musicSource.clip = music;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }
}
