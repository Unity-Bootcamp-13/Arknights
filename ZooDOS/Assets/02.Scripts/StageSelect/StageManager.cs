using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    private StageModelList _stageData;

    [SerializeField]private GameObject[] stageStarObjects;
    [SerializeField]private Sprite[] starSprite;
    private string SavePath => Path.Combine(Application.persistentDataPath, "StageState.json");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         LoadStageData();
         if (SceneManager.GetActiveScene().name == "StageSelect")
         {
            InitStageStarCnt();
         }
    }


    public void InitStageStarCnt()
    {
        
        for(int i = 0 ; i<_stageData.data.Length ; i++)
        {
            int cnt = GetStageStarCnt(i);
            SetStageStarImg(i, cnt);
        }

    }
    

    public int GetStageStarCnt(int stageIndex)
    {
        return _stageData.data[stageIndex].stage_clear_count;
    }
    
    public void SetStageStarCnt(int stageIndex, int star)
    {
        if (_stageData.data[stageIndex].stage_clear_count < star)
        {
            _stageData.data[stageIndex].stage_clear_count = star;
            
        }
        
        SaveStageData();

    }

    public void SetStageStarImg(int stageIndex, int star)
    {
        Image img =stageStarObjects[stageIndex].GetComponent<Image>();
        img.sprite = starSprite[star];
    }
    
    public void LoadStageData()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            _stageData = JsonUtility.FromJson<StageModelList>(json);
            Debug.Log("📥 유저 저장 스테이지 데이터 불러옴");
        }
        else
        {
            TextAsset defaultJson = Resources.Load<TextAsset>("Data/StageState");
            if (defaultJson == null)
            {
                Debug.LogError("Resources/Data/StageState.json 파일이 없음!");
                return;
            }

            _stageData = JsonUtility.FromJson<StageModelList>(defaultJson.text);
            Debug.Log("📥 기본 데이터 로드 (처음 실행)");

            // 처음 실행 시 기본 데이터도 저장해둠
            SaveStageData();
        }
    }
    
    void SaveStageData()
    {
        string json = JsonUtility.ToJson(_stageData, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"💾 스테이지 저장 완료: {SavePath}");
    }

    public void ClickStageBtn(int stageNum)
    {
        SceneManager.LoadScene($"Stage_{stageNum}");
    }
}
