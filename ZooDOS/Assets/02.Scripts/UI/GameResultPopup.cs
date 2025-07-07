using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameResultPopup : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Text resultText;
    [SerializeField] private Image[] starImages; // Star1~Star3
    [SerializeField] private Button fullScreenButton;

    [Header("Stage Info UI")]
    [SerializeField] private Text stageNameText;
    [SerializeField] private Text stageCodeText;

    [Header("Character UI")]
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private Text characterDialogueText;

    [Header("Star Colors")]
    [SerializeField] private Color activeStarColor = Color.white;
    [SerializeField] private Color inactiveStarColor = Color.gray;

    private void Start()
    {
        fullScreenButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
    }

    /// <summary>
    /// 결과 팝업 초기화
    /// </summary>
    public void InitResultPopup(
        bool isVictory,
        int starCount,
        List<RewardData> rewards,
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
