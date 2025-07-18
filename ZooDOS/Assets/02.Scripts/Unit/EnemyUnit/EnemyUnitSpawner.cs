﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Map map;
    [SerializeField] private UnitHpSpUIManager _unitHpSpUIManager;
    [SerializeField] private EffectManager _effectManager;
    [SerializeField] private StageData _stageData;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private GameManager _gameManager;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager.SetStageLife(_stageData.StageLife);
        StartCoroutine(CallStage(_stageData));
        _gameManager.SetEnemyCountOfThisStage(_stageData.StageCount);
    }


    IEnumerator CallStage(StageData stageData)
    {
        foreach (var waveData in stageData.Stage)
        {
            //정해놓은 스폰시간마다 적스폰, spawnTime 간격 대기 
            yield return new WaitForSeconds(waveData.StartTime);
            StartCoroutine(CallWave(waveData));
        }

    }


    /// <summary>
    /// 웨이브호출
    /// </summary>
    IEnumerator CallWave (WaveData waveData)
    {
        foreach (var enemy in waveData.Wave)
        {
            //정해놓은 스폰시간마다 적스폰, spawnTime 간격 대기 
            yield return new WaitForSeconds(enemy.spawnTime); 
            SpawnEnemy(enemy);
        }
        
    }
    
    
    /// <summary>
    /// 적 스폰
    /// </summary>
    /// <param name="enemyData">유닛 데이터</param>
    // EnemySpawnData : 적유닛프리펩, 스폰시간, 경로
    public void SpawnEnemy(EnemySpawnData enemyData)
    {
        var enemy = Instantiate(enemyData.enemyPrefab).GetComponent<EnemyUnit>();
        List<Vector3> path = new List<Vector3>();
        foreach (var waypoint in enemyData.path)
        {
            path.Add(map.CoordToVector3(waypoint));
        }
        enemy.Init(path);
        //적유닛이 아니면 아래 로직 호출 x
        if (enemy.EnemyUnitData.EnemyUnitType == EnemyUnitType.Nothing) return;

        enemy.SetSFXSound(_audioManager);
        enemy.Die += _gameManager.OnEnemyDeath;
        enemy.OnArrived += _gameManager.OnEnemyEnterDefensePoint;

        enemy.PlayHitEffect += _effectManager.PlayHitEffect;
        
        UnitHpUI ui = _unitHpSpUIManager.GetEnemyUnitHpUI();
        enemy.SetHpUI(ui);
        ui.Init(enemy);

        
    }
    
    
}
