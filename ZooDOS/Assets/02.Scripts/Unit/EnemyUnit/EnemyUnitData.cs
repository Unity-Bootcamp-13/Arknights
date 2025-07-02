using UnityEngine;

[CreateAssetMenu(fileName = "EnemyUnitData", menuName = "Scriptable Objects/EnemyUnitData")]
public class EnemyUnitData : ScriptableObject
{
    [SerializeField] public float Hp;
    [SerializeField] public float Def;
    [SerializeField] public float Atk;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public float MoveSpeed;
    [SerializeField] public EnemyUnitType EnemyUnitType;

    [SerializeField] public int RangeRadius;
}
