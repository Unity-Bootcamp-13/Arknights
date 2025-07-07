using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected float _projectileSpeed;
    [SerializeField] protected UnitHealthUI _unitHealthUI;

    public Hp Hp => _hp;
    public float Def => _def;


    public void OnGetDamage()
    {

    }


    public void HpEventSubscribe(Action action)
    {
        _hp.OnHpChanged += action;
    }

    public void SetHealthUI(UnitHealthUI ui)
    {
        _unitHealthUI = ui;
    }

    public Action<Unit> Die;

    public virtual void OnDeath() 
    {
        _unitHealthUI.DisableUI();
    }
}
