using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("Popup & UI")]
    [SerializeField] private GameResultPopup popup;
    [SerializeField] private Sprite characterPortrait;

    [Header("Reward Sprites")]
    [SerializeField] private Sprite itemSprite1;
    [SerializeField] private Sprite itemSprite2;

    public void OnBattleEnd(bool isVictory)
    {
        int starCount = CalculateStarCount(); // 별 개수 계산 로직


        string stageName = "정밀조준";
        string stageCode = "TR-2";

        string victoryDialogue = "박사님의 지시 덕분이네요.";
        string defeatDialogue = "다음엔 꼭 성공하겠습니다.";

        popup.gameObject.SetActive(true); // 팝업 활성화

        popup.InitResultPopup(
            isVictory,
            starCount,
            stageName,
            stageCode,
            characterPortrait,
            victoryDialogue,
            defeatDialogue
        );
    }

    private int CalculateStarCount()
    {
        // 실제 게임 로직에 따라 구현
        return 3;
    }
}
