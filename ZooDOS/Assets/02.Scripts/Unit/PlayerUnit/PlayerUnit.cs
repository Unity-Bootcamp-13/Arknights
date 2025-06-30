using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] protected Animator _animator;

    protected List<Maptile> _attackRange;
    protected Maptile _placeTile;

    protected float _leftAttackTime;
    protected int _resistCapacity;
    protected int _placeCost;
    protected float _replaceTime;
    protected PlayerUnitType _playerUnitType;
    protected TileType _tileType;
    protected PlayerUnitAttackRange _playerUnitAttackRange;

    

    /// <summary>
    /// 배치 시 실행되는 메서드
    /// 이번 배치에서 지정된 사거리와 위치 지정
    /// 체력 초기화
    /// </summary>
    /// <param name="attackRange"></param>
    /// <param name="placedTile"></param>
    public virtual void OnPlace(List<Maptile> attackRange, Maptile placedTile)
    {
        _leftAttackTime = 0;
        _attackRange = attackRange;
        _placeTile = placedTile;
        _hp.Replace();
        transform.position += Vector3.up * Constants.FALLING_POS;
        StartCoroutine(C_FallingCoroutine());
    }


    /// <summary>
    /// 유닛 데이터 최초 초기화 함수
    /// </summary>
    /// <param name="playerUnitData"> Spawner에게 전달받은 유닛 데이터 </param>
    /// <param name="enemyUnitSpawner"> Spawner에게 전달받은 enemyUnitSpawner 참조 </param>
    public void Init(PlayerUnitData playerUnitData)
    {
        gameObject.SetActive(false);
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
            (playerUnitData.BackwardRange * Constants.MAPTILE_LENGTH + Constants.MAPTILE_LENGTH/2,
             playerUnitData.ForwardRange * Constants.MAPTILE_LENGTH + Constants.MAPTILE_LENGTH / 2,
             playerUnitData.SidewardRange * Constants.MAPTILE_LENGTH + Constants.MAPTILE_LENGTH / 2
            );
    }


    IEnumerator C_FallingCoroutine()
    {
        _animator.SetTrigger("Place_t");
        float targetY = transform.position.y - Constants.FALLING_POS;

        while (transform.position.y > targetY)
        {
            transform.position += Vector3.down * Time.deltaTime * Constants.FALLING_SPEED;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }



    public override void OnDeath()
    {
        base.OnDeath();
        _placeTile.PlayerUnit = null;
    }
}
