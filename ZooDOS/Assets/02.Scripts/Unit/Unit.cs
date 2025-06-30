using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected bool _isDead;



    public float HpRatio => _hp.HP / _hp.MaxHP;
    public bool IsDead => _isDead;


    public void GetDamage(float value)
    {
        if (_isDead) return;

        float damage = Math.Min(-1, _def - value);

        _hp.HP +=  damage;


        if (_hp.HP <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        _isDead = true;
        Destroy(gameObject);
    }
}
