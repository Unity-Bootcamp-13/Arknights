using UnityEngine;

[CreateAssetMenu(fileName = "PlayerUnitData", menuName = "Scriptable Objects/PlayerUnitData")]
public class PlayerUnitData : ScriptableObject
{
    [SerializeField] public string Name;
    [SerializeField] public int Id;
    [SerializeField] public float Hp;
    [SerializeField] public float Def;
    [SerializeField] public float Atk;
    [SerializeField] public float AtkSpeed;
    [SerializeField] public float ProjectileSpeed;
    [SerializeField] public int ResistCapacity;
    [SerializeField] public int PlaceCost;
    [SerializeField] public float ReplaceTime;
    [SerializeField] public int BackwardRange;
    [SerializeField] public int ForwardRange;
    [SerializeField] public int SidewardRange;



    [SerializeField] public PlayerUnitType PlayerUnitType;
    [SerializeField] public TileType TileType;
    [SerializeField] public AttackType AttackType;


    [SerializeField] public Sprite StandingIllust;
    [SerializeField] public Sprite UnitPortrait;
    [SerializeField] public PlayerUnit UnitPrefab;
    [SerializeField] public GameObject UnitTposePrefab;
    [SerializeField] public GameObject SkillActivateEffectPrefab;


    [SerializeField] public SkillData BasicAttackData;
    [SerializeField] public SkillData[] SkillAttackData;



}
