using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour
{
    public AudioClip hitByEnemy;
    public AudioClip docked;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnHit()
    {
        audioSource.PlayOneShot(hitByEnemy);
    }

    public void OnDock()
    {
        audioSource.PlayOneShot(docked);
    }
}
