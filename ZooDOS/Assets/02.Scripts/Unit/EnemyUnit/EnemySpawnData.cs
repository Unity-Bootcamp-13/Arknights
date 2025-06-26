using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab; // 유닛프리팹
    public float spawnTime;        // 이 시간에 생성
    public List<Vector2> path;          // 경로
}
