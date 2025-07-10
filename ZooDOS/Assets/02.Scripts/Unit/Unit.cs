using System;
using UnityEngine;

public class Unit : MonoBehaviour, IDamagable
{
    public Action<Vector3> PlayHitEffect;
    public Action<Vector3> PlayHealEffect;
    public Action<Unit> Die;

    protected Hp _hp;
    protected float _def;
    protected float _atk;
    protected float _atkSpeed;
    protected float _projectileSpeed;
    [SerializeField] protected UnitHpUI _unitHpUI;
    [SerializeField] protected AudioSFXSound _sfxSound;


    public Hp Hp => _hp;
    public float Def => _def;


    public void OnGetDamage()
    {
        PlayHitEffect?.Invoke(transform.position);
    }

    public void OnGetHeal()
    {
        PlayHealEffect?.Invoke(transform.position);
    }


    public void SetSFXSound(AudioManager manager)
    {
        _sfxSound.Init(manager);
    }

    public void SetHpUI(UnitHpUI ui)
    {
        _unitHpUI = ui;
    }



    public virtual void OnDeath() 
    {
        _unitHpUI.DisableUI();
    }
}
