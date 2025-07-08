using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
[Serializable]
public class SkillData : ScriptableObject
{
    [SerializeField] public float AtkCoefficient;
    [SerializeField] public float AtkSpeedCoefficient;
    [SerializeField] public AttackType AttackType;
    [SerializeField] public float Duration;
    [SerializeField] public int TargetCapacity;
    [SerializeField] public Projectile ProjectilePrefab;
    [SerializeField] public float SkillCost;
    [Multiline(3)] public string SkillDescription;
}
