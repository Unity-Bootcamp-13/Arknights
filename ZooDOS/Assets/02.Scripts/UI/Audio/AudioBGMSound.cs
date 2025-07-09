using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioBGMSound : MonoBehaviour
{
    [SerializeField] private AudioClip _bgmClip;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        AudioManager manager = FindAnyObjectByType<AudioManager>();
        if (manager == null)
        {
            Debug.LogWarning("AudioManager를 찾지 못했습니다. 기본 출력 사용.");
            return;
        }
        _audioSource.outputAudioMixerGroup = manager.BGMGroup;
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
}