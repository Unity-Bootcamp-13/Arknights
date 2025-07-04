﻿using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Map _map;

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
    public void SetPlaybackSpeed(float gameSpeed, float playbackSpeed)
    {
        Time.timeScale = gameSpeed * playbackSpeed;
    }

    public void OnEnemyEnterDefensePoint()
    {
        _leftLifeCount--;
        _leftEnemyCount--;                          
        NotifyHud();
    }

    public void OnEnemyDeath()
    {
        _leftEnemyCount--;
        NotifyHud();
    }

    public void SetEnemyCountOfThisStage(int EnemyCount)
    {
        _totalEnemyCount = EnemyCount;
        _leftEnemyCount = _totalEnemyCount;
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
