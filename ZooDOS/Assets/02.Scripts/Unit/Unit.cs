using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected float _projectileSpeed;
    [SerializeField] protected UnitHpUI _unitHpUI;

    public Hp Hp => _hp;
    public float Def => _def;


    public void OnGetDamage()
    {

    }


   

    public void SetHpUI(UnitHpUI ui)
    {
        _unitHpUI = ui;
    }


    public Action<Unit> Die;

    public virtual void OnDeath() 
    {
        _unitHpUI.DisableUI();
    }
}
