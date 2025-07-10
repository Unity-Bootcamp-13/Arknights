using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSFXSound : MonoBehaviour
{
    [Serializable]
    public class StringAudioClipPair
    {
        public string _soundName;
        public AudioClip _clip;
    }

    [SerializeField] private Dictionary<string, AudioClip> _sfxSounds;
    [SerializeField] private AudioSource _audioSource; 
    [SerializeField] private List<StringAudioClipPair> _clips;

    public void Init(AudioManager manager)
    {
        _clips = new List<StringAudioClipPair>();

        _sfxSounds = new Dictionary<string, AudioClip>();

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