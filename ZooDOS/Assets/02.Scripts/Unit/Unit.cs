using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected float _projectileSpeed;

    public Hp Hp => _hp;
    public float Def => _def;





    public void HpEventSubscribe(Action action)
    {
        _hp.OnHpChanged += action;
    }


    public Action<Unit> Die;
    public virtual void OnDeath() { }
}
