using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFXSound : MonoBehaviour
{
    [Serializable]
    public class StringAudioClipPair
    {
        public string _soundName;
        public AudioClip _clip;
    }

    [SerializeField] private Dictionary<string, AudioClip> _sfxSounds;
    [SerializeField] private List<StringAudioClipPair> _clips;
    private AudioSource _audioSource;

    public void Init(AudioManager manager)
    {
        _sfxSounds = new Dictionary<string, AudioClip>();
        _audioSource = manager.AudioSource;

        foreach (var clip in _clips)
        {
            _sfxSounds[clip._soundName] = clip._clip;
        }

        if (manager == null)
        {
            Debug.LogWarning("AudioManager를 찾지 못했습니다. 기본 출력 사용.");
            return;
        }
        _audioSource.outputAudioMixerGroup = manager.SFXGroup;
    }

    public void PlaySFXSound(string soundName)
    {
        if (_sfxSounds != null)
        {
            _audioSource.PlayOneShot(_sfxSounds[soundName]);
        }
        else
        {
            Debug.LogWarning("SFX 사운드가 할당되지 않았음.");
        }
    }
}