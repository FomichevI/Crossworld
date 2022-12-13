using UnityEngine;

public class MobAudio : MonoBehaviour
{
    protected AudioSource _audioSource;
    [SerializeField] private AudioClip _getDamage;
    [SerializeField] private AudioClip _dodge;
    [SerializeField] private AudioClip _block;
    [SerializeField] private AudioClip _hit;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGetDamage()
    {
        _audioSource.PlayOneShot(_getDamage);
    }
    public void PlayDodge()
    {
        _audioSource.PlayOneShot(_dodge);
    }
    public void PlayBlock()
    {
        _audioSource.PlayOneShot(_block);
    }
    public void PlayHit()
    {
        _audioSource.PlayOneShot(_hit);
    }
}
