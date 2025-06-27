using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    protected IReadOnlyList<EnemyUnit> _enemyUnits;

    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected int _resistCapacity;
    protected int _placeCost;
    protected float _replaceTime;
    protected PlayerUnitType _playerUnitType;
    protected TileType _tileType;
    protected PlayerUnitAttackRange _playerUnitAttackRange;

    void Start()
    {
        transform.position += Vector3.up * Constants.FALLING_POS;
        StartCoroutine(C_FallingCoroutine());
    }


    /// <summary>
    /// 유닛 데이터 초기화 함수
    /// </summary>
    /// <param name="playerUnitData"> Spawner에게 전달받은 유닛 데이터 </param>
    public void Init(PlayerUnitData playerUnitData, EnemyUnitSpawner enemyUnitSpawner)
    {
        _enemyUnits = enemyUnitSpawner.EnemyUnitList;
        _playerUnitType = playerUnitData.PlayerUnitType;
        _tileType = playerUnitData.TileType;
        _hp = new Hp(playerUnitData.Hp);
        _def = playerUnitData.Def;
        _atk = playerUnitData.Atk;
        _atkSpeed = playerUnitData.AtkSpeed;
        _resistCapacity = playerUnitData.ResistCapacity;
        _placeCost = playerUnitData.PlaceCost;
        _replaceTime = playerUnitData.ReplaceTime;

        _playerUnitAttackRange = new PlayerUnitAttackRange
            (playerUnitData.BackwardRange * Constants.TILE_LENGTH,
             playerUnitData.ForwardRange * Constants.TILE_LENGTH,
             playerUnitData.SidewardRange * Constants.TILE_LENGTH
            );

    }


    IEnumerator C_FallingCoroutine()
    {
        float targetY = transform.position.y - Constants.FALLING_POS;

        while (transform.position.y > targetY)
        {
            transform.position += Vector3.down * Time.deltaTime * Constants.FALLING_SPEED;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
