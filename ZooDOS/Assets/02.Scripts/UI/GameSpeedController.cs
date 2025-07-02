using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private bool isGamePaused = false;
    private bool isFastSpeedEnabled = false;

    [Header("UI Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button fastSpeedButton;


    [Header("�ʼ� ����")]
    [SerializeField] Image buttonImage;   // ��ư�� Image ������Ʈ


    [Header("A ����")]
    [SerializeField] Sprite spriteA;
  
    [Header("B ����")]
    [SerializeField] Sprite spriteB;


    bool isA = true;





    // �� �Ͻ����� ���¸� ���
    public void OnClick_TogglePause()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            isGamePaused = false;

            // �� 2��� ��ư �ٽ� Ȱ��ȭ
            fastSpeedButton.interactable = true;
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;

            // �� 2��� ��ư ��Ȱ��ȭ
            fastSpeedButton.interactable = false;

            // 2��� ���¿��ٸ� ���� �ӵ��� �ǵ���
            if (isFastSpeedEnabled)
            {
                isFastSpeedEnabled = false;
            }
        }
    }

    // �� 2��� ���¸� ��� (�Ͻ����� ���¿����� ���� �Ұ�)
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
        isA = !isA;            // ���� ����
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
