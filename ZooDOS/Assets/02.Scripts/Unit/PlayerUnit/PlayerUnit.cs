using System.Collections;
using UnityEngine;

public enum PlayerUnitType
{
    Nothing,
    Defender,
    Sniper,
    Medic,
}

public class PlayerUnit : Unit
{
    const float FALLING_POS = 5f;
    const float FALLING_SPEED = 15f;


    protected float _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected int _resistCapacity;
    protected int _placeCost;
    protected float _replaceTime;
    protected float _atkRange;
    protected PlayerUnitType _playerUnitType;
    protected TileType _tileType;

    void Start()
    {
        transform.position += Vector3.up * FALLING_POS;
        StartCoroutine(C_FallingCoroutine());
    }


    /// <summary>
    /// 유닛 데이터 초기화 함수
    /// </summary>
    /// <param name="playerUnitData"> Spawner에게 전달받은 유닛 데이터 </param>
    public void Init(PlayerUnitData playerUnitData)
    {
        _playerUnitType = playerUnitData.PlayerUnitType;
        _tileType = playerUnitData.TileType;
        _hp = playerUnitData.Hp;
        _def = playerUnitData.Def;
        _atk = playerUnitData.Atk;
        _atkSpeed = playerUnitData.AtkSpeed;
        _resistCapacity = playerUnitData.ResistCapacity;
        _placeCost = playerUnitData.PlaceCost;
        _replaceTime = playerUnitData.ReplaceTime;
        _atkRange = playerUnitData.AtkRange;
    }

    IEnumerator C_FallingCoroutine()
    {
        float targetY = transform.position.y - FALLING_POS;

        while (transform.position.y > targetY)
        {
            transform.position += Vector3.down * Time.deltaTime * FALLING_SPEED;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}
