using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Map _map;
    [SerializeField] GameResultPopup _gameResultPopup;
    [SerializeField] private Sprite characterPortrait;
    
    private int _leftLifeCount;
    private int _totalEnemyCount;
    private int _leftEnemyCount;
    private int resultStarCnt = 3;
    public int LeftLifeCount => _leftLifeCount;
    public int TotalEnemyCount => _totalEnemyCount;
    public int LeftEnemyCount => _leftEnemyCount;
    public Map Map => _map;

    public event Action OnHudDataChanged;            

    private bool isGameEnded = false;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    private void Update()
    {
        if (!isGameEnded)
        {
            EvaluateGameResult();
        }
    }

    public void SetStageLife(int life)
    {
        _leftLifeCount = life;
    }

    private void EvaluateGameResult()
    {
        if (_leftLifeCount <= 0)
        {
            //패배
            isGameEnded = true;
            OnBattleEnd(false);
        } 
        else if(_leftEnemyCount <= 0)
        {
            //승리
            isGameEnded = true;
            OnBattleEnd(true);
        }
    }
    public void OnBattleEnd(bool isVictory)
    {
        int starCount = CalculateStarCount(); // 별 개수 계산 로직


        string stageName = "정밀조준";
        string stageCode = "TR-2";

        string victoryDialogue = "대장님의 지시 덕분이네요.";
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
        Time.timeScale = 0;
        //스테이지 별 저장
        string currentSceneName = SceneManager.GetActiveScene().name;
        int stageNum = Convert.ToInt32(currentSceneName.Substring(6));
        GetComponent<StageManager>().SetStageStarCnt(stageNum - 1, starCount);

    }
    
    private int CalculateStarCount()
    {
        int result = Mathf.Max(0, resultStarCnt);
        
        return result;
    }
    
    
    public void SetPlaybackSpeed(float gameSpeed, float playbackSpeed)
    {
        Time.timeScale = gameSpeed * playbackSpeed;
    }

    public void OnEnemyEnterDefensePoint()
    {
        _leftLifeCount--;
        resultStarCnt--;
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
