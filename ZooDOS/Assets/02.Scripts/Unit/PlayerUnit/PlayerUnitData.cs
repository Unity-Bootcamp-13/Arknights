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
    [SerializeField] public PlayerUnitType PlayerUnitType;
    [SerializeField] public TileType TileType;
    [SerializeField] public Sprite UnitPortrait;
    [SerializeField] public PlayerUnit UnitPrefab;


    [SerializeField] public int BackwardRange;
    [SerializeField] public int ForwardRange;
    [SerializeField] public int SidewardRange;
}
