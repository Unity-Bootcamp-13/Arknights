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
    [SerializeField] Image buttonImage;  


    [Header("A 상태")]
    [SerializeField] Sprite spriteA;
  
    [Header("B 상태")]
    [SerializeField] Sprite spriteB;


    bool isA = true;





    public void OnClick_TogglePause()
    {
        if (isGamePaused)
        {
            Time.timeScale = 1f;
            isGamePaused = false;

         
            fastSpeedButton.interactable = true;
        }
        else
        {
            Time.timeScale = 0f;
            isGamePaused = true;

     
            fastSpeedButton.interactable = false;

        
            if (isFastSpeedEnabled)
            {
                isFastSpeedEnabled = false;
            }
        }
    }

 
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
        isA = !isA;           
        ApplyVisual();
    }



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
