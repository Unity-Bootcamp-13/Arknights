using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBGMOneSound : MonoBehaviour
{
    [SerializeField] private AudioClip _bgmClip;

    [SerializeField] AudioSource _audioSource;

    [SerializeField] private AudioManager _audioManager;

    private void Start()
    {
        _audioSource.outputAudioMixerGroup = _audioManager.BGMGroup;
        _audioSource.playOnAwake = false;   // 씬 시작 시 자동 재생 방지
        _audioSource.loop = false;
    }

    private void OnEnable()
    {
        PlayBGMOneShot();
    }
    public void StopBgm()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();     
    }

    public void PlayBGMOneShot()
    {
        if (_bgmClip != null)
        {
            _audioSource.PlayOneShot(_bgmClip);
        }
        else
        {
            Debug.LogWarning("BGM 사운드가 할당되지 않았습니다.");
        }
    }

}