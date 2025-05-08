using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager Instance { get; private set; }
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            Debug.Log("AudioManager instance created." + gameObject.name);
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this instance persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
}
