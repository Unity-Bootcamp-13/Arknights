using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitData", menuName = "Scriptable Objects/PlayerUnitData")]
public class PlayerUnitData : ScriptableObject
{
    [field: SerializeField] public float Hp { get; }
    [field: SerializeField] public float Def { get; }
    [field: SerializeField] public float Atk { get; }
    [field: SerializeField] public float AtkSpeed { get; }
    [field: SerializeField] public int ResistCapacity { get; }
    [field: SerializeField] public int PlaceCost { get; }
    [field: SerializeField] public float ReplaceTime { get; }
    [field: SerializeField] public float AtkRange { get; }
    [field :SerializeField] public PlayerUnitType PlayerUnitType { get; }
    [field: SerializeField] public TileType TileType { get; }
    [field: SerializeField] public PlayerUnit UnitPrefab { get; }

}
