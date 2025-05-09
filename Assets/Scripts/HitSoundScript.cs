using UnityEngine;

public class HitSoundScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            audioSource.Play();
        }
    }
}
