using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameSpeedController _gameSpeedController;  
    private Button _blockerButton;


    private bool _visible = false;
    private void Start()
    {
        if (_audioManager == null) 
        { 
            Debug.LogError("AudioManager 못 찾음"); 
            return; 
        }

        InitSlider(_masterSlider, _masterText, _audioManager.MasterVolume);  
        InitSlider(_bgmSlider, _bgmText, _audioManager.BgmVolume);
        InitSlider(_sfxSlider, _sfxText, _audioManager.SfxVolume);


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
        CreateBlocker();
        _blockerButton.gameObject.SetActive(false);
        _soundSettingPanel.SetActive(false);
        
        
    }

    private void InitSlider(Slider slider, TMP_Text volume, float init)
    {
        slider.SetValueWithoutNotify(init);
        volume.text = ToPercent(init);
    }


    public void TogglePanel()
    {
        _visible = !_visible;
        _soundSettingPanel.SetActive(_visible);
        _blockerButton.gameObject.SetActive(_visible);
        if (_visible)
        {
            Time.timeScale = 0f;
        }
        else
        {
            _gameSpeedController.UpdateTimeScale();
        }
    }

    static string ToPercent(float v)
    {
        return $"{Mathf.RoundToInt(v * 100)} %";
    }

    private void CreateBlocker()
    {
        // 1) 빈 오브젝트 + 컴포넌트
        GameObject go = new("UnitInfoBlocker",
                            typeof(RectTransform),
                            typeof(CanvasRenderer),
                            typeof(Image),
                            typeof(Button));

        // 2) Canvas 하위(Panel과 같은 부모)에 붙이기
        go.transform.SetParent(_canvas.transform, false);

        // 3) 풀스크린으로 Stretch
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        // 4) 이미지 → 완전 투명
        Image img = go.GetComponent<Image>();
        img.color = new Color(0, 0, 0, 0);
        img.raycastTarget = true;

        // 5) 버튼 클릭 → ClosePanel
        _blockerButton = go.GetComponent<Button>();
        _blockerButton.onClick.AddListener(TogglePanel);

        // 6) 패널보다 뒤(인덱스가 작아야 뒤)로 이동
        int panelIndex = _soundSettingPanel.transform.GetSiblingIndex();
        go.transform.SetSiblingIndex(panelIndex);
        _soundSettingPanel.transform.SetSiblingIndex(panelIndex + 1);

    }

    public void GotoMainLobby()
    {
        SceneManager.LoadScene("MainLobbyScene");
    }
}