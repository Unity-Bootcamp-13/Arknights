using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundSettingUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _soundSettingButton;
    [SerializeField] private GameObject _soundSettingPanel;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;

    [SerializeField] private TMP_Text _masterText;
    [SerializeField] private TMP_Text _bgmText;
    [SerializeField] private TMP_Text _sfxText;

    private AudioManager _audioManager;
    private bool _visible;
    void Awake()
    {
        _audioManager = FindAnyObjectByType<AudioManager>(FindObjectsInactive.Include);
        if (_audioManager == null) 
        { 
            Debug.LogError("AudioManager 못 찾음"); 
            return; 
        }

        InitSlider(_masterSlider, _masterText, 0.3f);  
        InitSlider(_bgmSlider, _bgmText, 0.3f);
        InitSlider(_sfxSlider, _sfxText, 0.3f);


        _soundSettingButton.onClick.AddListener(TogglePanel);

        _masterSlider.onValueChanged.AddListener(v =>
        {
            _audioManager.SetVolume("Master", v);
            _masterText.text = ToPercent(v);
        });
        _bgmSlider.onValueChanged.AddListener(v =>
        {
            _audioManager.SetVolume("BGM", v);
            _bgmText.text = ToPercent(v);
        });
        _sfxSlider.onValueChanged.AddListener(v =>
        {
            _audioManager.SetVolume("SFX", v);
            _sfxText.text = ToPercent(v);
        });

        _soundSettingPanel.SetActive(false);
    }

    void InitSlider(Slider slider, TMP_Text volume, float init)
    {
        slider.SetValueWithoutNotify(init);
        volume.text = ToPercent(init);
    }


    void TogglePanel()
    {
        _visible = !_visible;
        _soundSettingPanel.SetActive(_visible);
    }

    static string ToPercent(float v)
    {
        return $"{Mathf.RoundToInt(v * 100)} %";
    }
}