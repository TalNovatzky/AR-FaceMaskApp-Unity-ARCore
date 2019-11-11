using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundInteraction : MonoBehaviour, IInteraction
{
    private AudioSource _audioSource;
    public AudioClip AudioClip;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        if (AudioClip == null)
        {
            Debug.LogError("AudioClip reference is missing in the prefab! ", this);
        }
        else
        {
            _audioSource.clip = AudioClip;
        }

    }

    public void Interact()
    {
        _audioSource.Play();
    }

    public void ResetInteraction()
    {
        _audioSource.Stop();
    }

    private void OnDisable()
    {
        ResetInteraction();
    }
}
