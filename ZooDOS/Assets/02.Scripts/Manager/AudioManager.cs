using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("External Refs")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerGroup _bgmGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    
    [Range(0f, 1f)] private float masterVolume = 0.3f;
    [Range(0f, 1f)] private float bgmVolume = 0.3f;
    [Range(0f, 1f)] private float sfxVolume = 0.3f;

    private readonly Dictionary<string, float> _lastVolumes = new();

    public AudioMixerGroup BGMGroup => _bgmGroup;
    public AudioMixerGroup SFXGroup => _sfxGroup;

   
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //1. playerprefs 값셋팅
        //2. saveValue(   ) ;\
    }

    private void Start()
    {
        ApplyInitialVolumes();
    }
    private void ApplyInitialVolumes()
    {
        SetVolume("Master", masterVolume);
        SetVolume("BGM", bgmVolume);
        SetVolume("SFX", sfxVolume);
    }

    public void SetVolume(string parameter, float normalizedValue)
    {
        float dB = Mathf.Log10(Mathf.Clamp(normalizedValue, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat(parameter, dB);

        switch (parameter)
        {
            case "Master":
                masterVolume = normalizedValue;
                break;
            case "BGM":
                bgmVolume = normalizedValue;
                break;
            case "SFX":
                sfxVolume = normalizedValue;
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
