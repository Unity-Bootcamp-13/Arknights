using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 부모 오브젝트가 활성화되면 곧바로 시작하는
/// Radial(도넛) 형태 쿨타임 UI.
/// </summary>
public class CooltimeUI : MonoBehaviour
{
    [Header("필수 참조")]
    [SerializeField] private Image radialImage;   

    [Header("쿨타임(초)")]
    [SerializeField] private float cooldownSeconds = 10f;    // 나중에 받아올거지만 테스트를 위해 우선 수동입력으로 해둠

    private Coroutine routine;

   
    private void Reset()                       // 컴포넌트 추가 시 자동 연결
    {
        radialImage = GetComponent<Image>();
    }

    private void Awake()                       // 최초 1회 설정
    {
        if (radialImage == null) radialImage = GetComponent<Image>();
        InitImageSettings();
    }

    private void OnEnable()                    // 부모가 SetActive(true) 되는 순간 호출
    {
        InitImageSettings();                   // 항상 처음 상태로 리셋
        routine = StartCoroutine(FillDown());
    }

    private void OnDisable()                   // 비활성화 시 코루틴 정리
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }
 



    public void SetCooldown(float seconds)
    {
        cooldownSeconds = Mathf.Max(0.1f, seconds);
        OnEnable();      // 재시작 (Enable 상태일 때도 호출 가능)
    }

    private void InitImageSettings()
    {
        radialImage.type = Image.Type.Filled;
        radialImage.fillMethod = Image.FillMethod.Radial360;

        radialImage.fillOrigin = 2;   // 0:Top, 1:Right, 2:Bottom, 3:Left
        radialImage.fillClockwise = false;
        radialImage.fillAmount = 1f;
    }


    private IEnumerator FillDown()             // 쿨타임 카운트다운
    {
        float t = cooldownSeconds;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            radialImage.fillAmount = Mathf.Clamp01(t / cooldownSeconds);
            yield return null;
        }
        radialImage.fillAmount = 0f;           // 완전히 사라진 뒤 종료
    }
  
}
