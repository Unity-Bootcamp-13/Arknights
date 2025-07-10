using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("그룹 연결")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _bgmGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField] private AudioSource _audioSource;
    
    [Range(0f, 1f)] private float _masterVolume;
    [Range(0f, 1f)] private float _bgmVolume;
    [Range(0f, 1f)] private float _sfxVolume;

    public float MasterVolume => _masterVolume;
    public float BgmVolume => _bgmVolume;
    public float SfxVolume => _sfxVolume;


    private readonly Dictionary<string, float> _lastVolumes = new();

    public AudioSource AudioSource => _audioSource;

    public AudioMixerGroup BGMGroup => _bgmGroup;
    public AudioMixerGroup SFXGroup => _sfxGroup;

    private void Start()
    {
        _masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0.3f);
        _bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0.3f);
        _sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
        ApplyInitialVolumes();

    }
    private void ApplyInitialVolumes()
    {
        SetVolume("Master", _masterVolume);
        SetVolume("BGM", _bgmVolume);
        SetVolume("SFX", _sfxVolume);
    }

    public void SetVolume(string parameter, float normalizedValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(normalizedValue, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat(parameter, dB);
        switch (parameter)
        {
            case "Master":
                _masterVolume = normalizedValue;
                PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
                PlayerPrefs.Save();
                break;
            case "BGM":
                _bgmVolume = normalizedValue;
                PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
                PlayerPrefs.Save();
                break;
            case "SFX":
                _sfxVolume = normalizedValue;
                PlayerPrefs.SetFloat("SFXVolume", _sfxVolume);
                PlayerPrefs.Save();
                break;
            default:
                break;
        }
        
    }

    public void ToggleMuteSingle(string paramName)
    {
        bool currentlyMuted =
            _audioMixer.GetFloat(paramName, out float currentDb) && currentDb <= -79f;

        if (currentlyMuted)
        {
            float restoredDb = _lastVolumes.TryGetValue(paramName, out float prevDb) ? prevDb : 0f;
            _audioMixer.SetFloat(paramName, restoredDb);
        }
        else
        {
            _audioMixer.GetFloat(paramName, out currentDb);
            _lastVolumes[paramName] = currentDb;
            _audioMixer.SetFloat(paramName, -80f);
        }
    }

}
