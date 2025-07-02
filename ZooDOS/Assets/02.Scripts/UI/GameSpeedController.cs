using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private bool isGamePaused = false;
    private bool isFastSpeedEnabled = false;

    [Header("UI Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button fastSpeedButton;


    [Header("필수 참조")]
    [SerializeField] Image buttonImage;   // 버튼의 Image 컴포넌트


    [Header("A 상태")]
    [SerializeField] Sprite spriteA;
  
    [Header("B 상태")]
    [SerializeField] Sprite spriteB;


    bool isA = true;





    // ▶ 일시정지 상태를 토글
    public void OnClick_TogglePause()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            isGamePaused = false;

            // ▶ 2배속 버튼 다시 활성화
            fastSpeedButton.interactable = true;
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;

            // ▶ 2배속 버튼 비활성화
            fastSpeedButton.interactable = false;

            // 2배속 상태였다면 원래 속도로 되돌림
            if (isFastSpeedEnabled)
            {
                isFastSpeedEnabled = false;
            }
        }
    }

    // ▶ 2배속 상태를 토글 (일시정지 상태에서는 실행 불가)
    public void OnClick_ToggleFastSpeed()
    {
        if (isGamePaused)
            return;

        if (isFastSpeedEnabled)
        {
            Time.timeScale = 1f;
            isFastSpeedEnabled = false;
        }
        else
        {
            Time.timeScale = 2f;
            isFastSpeedEnabled = true;
        }
    }


    public void ToggleVisual()
    {
        isA = !isA;            // 상태 반전
        ApplyVisual();
    }


    //void Start() => ApplyVisual();

    void ApplyVisual()
    {
        if (isA)
        {
            buttonImage.sprite = spriteA;
           
        }
        else
        {
            buttonImage.sprite = spriteB;
          
        }
    }



}
