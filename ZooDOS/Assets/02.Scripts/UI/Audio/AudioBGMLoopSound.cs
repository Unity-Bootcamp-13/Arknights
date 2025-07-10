using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBGMLoopSound : MonoBehaviour
{
    [SerializeField] private AudioClip _bgmClip;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] private AudioManager _audioManager;

    private void Start()
    {
        _audioSource.outputAudioMixerGroup = _audioManager.BGMGroup;
        PlayBGMSound();
    }
    public void PlayBGMSound()
    {
        if (_bgmClip != null)
        {
            _audioSource.clip = _bgmClip;
            _audioSource.loop = true;   
            _audioSource.playOnAwake = false;
            _audioSource.Play();
        }
        else
        {
            Debug.LogWarning("BGM Clip이 할당되지 않았습니다.");
        }
    }

    public void StopBgm()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();     
    }


}