using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Map _map;
    [SerializeField] GameResultPopup _gameResultPopup;
    [SerializeField] private Sprite characterPortrait;
    
    private int _leftLifeCount;
    private int _totalEnemyCount;
    private int _leftEnemyCount;

    public int LeftLifeCount => _leftLifeCount;
    public int TotalEnemyCount => _totalEnemyCount;
    public int LeftEnemyCount => _leftEnemyCount;
    public Map Map => _map;

    public event Action OnHudDataChanged;            


    private void Awake()
    {
        _leftLifeCount = Constants.LIFE_OF_PLAYER;

    }

    private void Update()
    {
        EvaluateGameResult();
    }

    private void EvaluateGameResult()
    {
        if (_leftLifeCount <= 0)
        {
            //패배
            OnBattleEnd(false);
        } 
        else if(_leftEnemyCount <= 0)
        {
            //승리
            OnBattleEnd(true);
        }
    }
    public void OnBattleEnd(bool isVictory)
    {
        int starCount = CalculateStarCount(); // 별 개수 계산 로직


        string stageName = "정밀조준";
        string stageCode = "TR-2";

        string victoryDialogue = "박사님의 지시 덕분이네요.";
        string defeatDialogue = "다음엔 꼭 성공하겠습니다.";

        _gameResultPopup.gameObject.SetActive(true); // 팝업 활성화

        _gameResultPopup.InitResultPopup(
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
        return _leftLifeCount;
    }
    
    
    public void SetPlaybackSpeed(float gameSpeed, float playbackSpeed)
    {
        Time.timeScale = gameSpeed * playbackSpeed;
    }

    public void OnEnemyEnterDefensePoint()
    {
        _leftLifeCount--;
        NotifyHud();
    }

    public void OnEnemyDeath(Unit unit)
    {
        _leftEnemyCount--;
        NotifyHud();
    }

    public void SetEnemyCountOfThisStage(int EnemyCount)
    {
        _totalEnemyCount = EnemyCount;
        _leftEnemyCount = EnemyCount;
        NotifyHud();
    }

    private void NotifyHud()
    {
      
        if (OnHudDataChanged != null)
        {
            OnHudDataChanged.Invoke();               
        }
    }
}
