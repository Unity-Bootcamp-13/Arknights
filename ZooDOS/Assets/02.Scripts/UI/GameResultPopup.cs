using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameResultPopup : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Image[] starImages; // Star1~Star3
    [SerializeField] private Button fullScreenButton;

    [Header("Stage Info UI")]
    [SerializeField] private TextMeshProUGUI stageNameText;
    [SerializeField] private TextMeshProUGUI stageCodeText;

    [Header("Character UI")]
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private TextMeshProUGUI characterDialogueText;

    [Header("Star Colors")]
    [SerializeField] private Color activeStarColor = Color.white;
    [SerializeField] private Color inactiveStarColor = Color.gray;

    public void Init()
    {
        fullScreenButton.onClick.AddListener(() => SceneManager.LoadScene("StageSelect"));
        gameObject.SetActive(false); 
    }

    /// <summary>
    /// 결과 팝업 초기화
    /// </summary>
    public void InitResultPopup(
        bool isVictory,
        int starCount,
        string stageName,
        string stageCode,
        Sprite characterPortrait,
        string victoryDialogue,
        string defeatDialogue)
    {
        // 결과 텍스트
        if (isVictory)
        {
            resultText.text = "작전 성공";
        }
        else
        {
            resultText.text = "작전 실패";
        }

        // 별 색상
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < starCount)
            {
                starImages[i].color = activeStarColor;
            }
            else
            {
                starImages[i].color = inactiveStarColor;
            }
        }


        // 스테이지 이름, 번호 출력
        stageNameText.text = stageName;
        stageCodeText.text = stageCode;

        // 캐릭터 초상화 출력
        characterPortraitImage.sprite = characterPortrait;

        // 대사 출력
        if (isVictory)
        {
            characterDialogueText.text = victoryDialogue;
        }
        else
        {
            characterDialogueText.text = defeatDialogue;
        }
    }



}
