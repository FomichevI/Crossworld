using UnityEngine;

public class UiAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _loseSound;
    [SerializeField] private AudioClip _putOn;
    [SerializeField] private AudioClip _cantUse;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayWin()
    {
        _audioSource.PlayOneShot(_winSound);
    }
    public void PlayLose()
    {
        _audioSource.PlayOneShot(_loseSound);
    }
    public void PlaySetOn()
    {
        _audioSource.PlayOneShot(_putOn);
    }
    public void PlayCantUse()
    {
        _audioSource.PlayOneShot(_cantUse);
    }
}
