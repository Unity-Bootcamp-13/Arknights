using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSFXSound : MonoBehaviour
{
    [SerializeField] private AudioClip _sfxSound;
    
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
        _audioSource.outputAudioMixerGroup = manager.SFXGroup;
        
    }

    public void PlaySFXSound()
    {
        if (_sfxSound != null)
        {
            _audioSource.PlayOneShot(_sfxSound);
        }
        else
        {
            Debug.LogWarning("SFX 사운드가 할당되지 않았음.");
        }
    }
}