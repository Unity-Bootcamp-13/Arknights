using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitData", menuName = "Scriptable Objects/PlayerUnitData")]
public class PlayerUnitData : ScriptableObject
{
    [SerializeField] public float Hp;
    [SerializeField] public float Def;
    [SerializeField] public float Atk;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public int ResistCapacity;
    [SerializeField] public int PlaceCost;
    [SerializeField] public float ReplaceTime;
    [SerializeField] public float AtkRange;
    [SerializeField] public PlayerUnitType PlayerUnitType;
    [SerializeField] public TileType TileType;
    [SerializeField] public PlayerUnit UnitPrefab;

}
