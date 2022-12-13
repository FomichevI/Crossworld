using UnityEngine;

public class PlayerAudio : MobAudio
{
    private AudioClip _stepAc;

    private void Start()
    {
        _stepAc = Resources.Load<AudioClip>("Audio/Player/step");
    }
    public void PlayStep()
    {
        _audioSource.PlayOneShot(_stepAc);
    }
}
