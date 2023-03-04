using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip menuClip;
    public AudioClip gameClip;

    private bool hasMusicChanged = false;

    private void Awake()
    {
        // Find the AudioSource component
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null)
        {
            // Create a new GameObject with an AudioSource component
            GameObject musicObject = new GameObject("Music");
            audioSource = musicObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = true;
        }
        DontDestroyOnLoad(audioSource.gameObject);
    }

    private void Start()
    {
        // Play the music if it's not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (hasMusicChanged == false)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "EndlessMode")
            {
                hasMusicChanged = true;
                audioSource.clip = gameClip;
                audioSource.Play();
            }
        }
    }

    private void OnDisable()
    {
        // Stop the music when the scene changes
        SceneManager.sceneUnloaded -= SceneUnloaded;
    }

    private void OnEnable()
    {
        // Start playing the music again when a new scene is loaded
        SceneManager.sceneUnloaded += SceneUnloaded;
    }

    private void SceneUnloaded(Scene scene)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
