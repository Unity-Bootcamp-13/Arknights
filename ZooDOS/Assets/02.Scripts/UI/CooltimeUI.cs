using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// 부모 오브젝트가 활성화되면 곧바로 시작하는
/// Radial(도넛) 형태 쿨타임 UI.
/// </summary>
public class CooltimeUI : MonoBehaviour
{
    [Header("필수 참조")]
    [SerializeField] private Image _radialImage;
    [SerializeField] private TMP_Text _timeLabel;
    
    private float _cooldownSeconds;
    private Coroutine _coroutine;

    public void CooldownSetting(PlayerUnitData data)
    {
        _cooldownSeconds = data.ReplaceTime;
    }
    private void Reset()                       
    {
        _radialImage = GetComponent<Image>();
        _timeLabel = GetComponentInChildren<TMP_Text>();
    }

    private void Awake()                      
    {
        if (_radialImage == null) _radialImage = GetComponent<Image>(); 
        if (_timeLabel == null) _timeLabel = GetComponentInChildren<TMP_Text>();
        InitImageSettings();
    }

    private void OnEnable()                    
    {
        if (_coroutine == null)
        {
            InitImageSettings();
            _coroutine = StartCoroutine(FillDown());
        }
    }

    private void OnDisable()                   
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    public void SetCooldown(float seconds)
    {
        _cooldownSeconds = Mathf.Max(0.1f, seconds);
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        gameObject.SetActive(true);        // 비활성 상태였다면 켠다
        InitImageSettings();               // 처음 상태로 리셋
        _coroutine = StartCoroutine(FillDown());
    }

    private void InitImageSettings()
    {
        _radialImage.type = Image.Type.Filled;
        _radialImage.fillMethod = Image.FillMethod.Radial360;

        _radialImage.fillOrigin = 2;   // 0:Top, 1:Right, 2:Bottom, 3:Left
        _radialImage.fillClockwise = false;
        _radialImage.fillAmount = 1f;

        if (_timeLabel != null)
            _timeLabel.text = Mathf.CeilToInt(_cooldownSeconds).ToString();

    }


    private IEnumerator FillDown()             // 쿨타임 카운트다운
    {
        float t = _cooldownSeconds;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            _radialImage.fillAmount = Mathf.Clamp01(t / _cooldownSeconds);

            if (_timeLabel != null)
                _timeLabel.text = Mathf.CeilToInt(t).ToString();

            yield return null;
        }
        _radialImage.fillAmount = 0f;           // 완전히 사라진 뒤 종료
        if (_timeLabel != null)
            _timeLabel.text = "0";
        gameObject.SetActive(false);
    }

}
