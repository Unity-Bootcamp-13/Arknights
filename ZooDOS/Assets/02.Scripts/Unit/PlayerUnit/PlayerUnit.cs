using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] private Animator _animator;


    public Func<Vector3, GameObject> GetSkillEffect;
    private GameObject _skillActivateEffect;

    private List<Maptile> _attackRange;
    private PlayerUnitStatus _status;
    
    private int _resistCapacity;
    private TileType _tileType;

    private Sp _sp;
    private PlayerUnitSpUI _unitSpUI;

    private PlayerUnitSKill _basicAttack;
    private PlayerUnitSKill _skillAttack;

    float _remainSkillDuration;
    float _skillDuration;
    bool _isSkillActivated;
    bool _isSpCharged;

    public TileType TileType => _tileType;
    public Sp Sp => _sp;
    

    void Update()
    {
        if (_remainSkillDuration>=0)
        {
            _remainSkillDuration -= Time.deltaTime;
            Attack(_skillAttack);

            if(_remainSkillDuration < 0)
            {
                _skillAttack.UnBlockTargets();
                FinishSkillEffect();
                _isSkillActivated = false;
            }

            return;
        }

        if (_isSpCharged == false)
        {
            _sp.ChargeSp(Time.deltaTime);
        }

        Attack(_basicAttack);
       
    }


    public void Attack(PlayerUnitSKill skill)
    {
        skill.AddTarget();

        skill.Attack();
    }

    public void StartSkillEffect()
    {
        _skillActivateEffect = GetSkillEffect?.Invoke(transform.position);
    }  
    
    public void FinishSkillEffect()
    {
        if(_skillActivateEffect== null)
        {
            return;
        }

        _skillActivateEffect.SetActive(false);
    }

    public void ActivateSkill()
    {
        if (_isSkillActivated||_isSpCharged== false)
        {
            return;
        }

        _isSkillActivated = true;
        _isSpCharged = false;
        StartSkillEffect();
        _sp.ResetSp();
        _basicAttack.UnBlockTargets();
        _skillAttack.Init();
        _remainSkillDuration = _skillDuration;
    }

    public void SetSpCharged()
    {
        _isSpCharged = true;
    }

    /// <summary>
    /// 배치 시 실행되는 메서드
    /// 공격 딜레이 초기화
    /// 이번 배치에서 지정된 사거리와 위치 지정
    /// 배치되는 타일에 참조 전달
    /// 체력 초기화
    /// 배치 애니메이션 재생
    /// </summary>
    /// <param name="attackRange"></param>
    /// <param name="placedTile"></param>
    public virtual void OnPlace(List<Maptile> attackRange)
    {
        _attackRange = attackRange;
        _hp.ResetHp();
        _sp.ResetSp();


        _basicAttack.SetRange(_attackRange);
        _skillAttack.SetRange(_attackRange);
        _remainSkillDuration = -1;

        _unitHpUI.SetUIPosition(transform.position);
        _unitSpUI.SetUIPosition(transform.position);

        transform.position += Vector3.up * Constants.FALLING_POS;
        StartCoroutine(C_FallingCoroutine());
    }


    /// <summary>
    /// 유닛 데이터 최초 초기화 함수
    /// 
    /// </summary>
    /// <param name="playerUnitData"> Spawner에게 전달받은 유닛 데이터 </param>
    /// <param name="enemyUnitSpawner"> Spawner에게 전달받은 enemyUnitSpawner 참조 </param>
    public void Init(PlayerUnitData playerUnitData)
    {
        gameObject.SetActive(false);

        _status = new PlayerUnitStatus(playerUnitData.UnitPortrait, playerUnitData.StandingIllust, playerUnitData.Name, playerUnitData.PlaceCost);
        _tileType = playerUnitData.TileType;
        _hp = new Hp(this, playerUnitData.Hp);
        _def = playerUnitData.Def;
        _atk = playerUnitData.Atk;
        _atkSpeed = playerUnitData.AtkSpeed;
        _projectileSpeed = playerUnitData.ProjectileSpeed;
        _resistCapacity = playerUnitData.ResistCapacity;
        _skillDuration = playerUnitData.SkillAttackData[0].Duration;

        _basicAttack = new PlayerUnitSKill(this, _atk, _atkSpeed, playerUnitData.BasicAttackData);
        _skillAttack = new PlayerUnitSKill(this, _atk, _atkSpeed, playerUnitData.SkillAttackData[0]);
        _sp = new Sp(this, playerUnitData.SkillAttackData[0].SkillCost);
    }

    public PlayerUnitStatus GetStatus()
    {
        _status.SetChangableStatus(_atk, _def, _resistCapacity, _hp.HP, _hp.MaxHP);

        return _status;
    }

    public void SetSpUI(PlayerUnitSpUI ui)
    {
        _unitSpUI = ui;
    }

    public void ShootDamageProjectile(Unit target, float value, Projectile projectile)
    {
        
        _animator.SetTrigger("Attack_t");
        

        float damage = Math.Max(1, value - target.Def);

        if (projectile != null)
        {
            Projectile proj = Instantiate(projectile);
            proj.Init(target, _projectileSpeed);
            proj.transform.position = transform.position + Vector3.up;
            proj.SetProjectileAction(() => target.Hp.GetDamage(damage));
        }
        else
        {
            target.Hp.GetDamage(damage);
        }

    }


    public void ShootHealProjectile(Unit target, float value, Projectile projectile)
    {
        _animator.SetTrigger("Attack_t");


        if (projectile != null)
        {
            Projectile proj = Instantiate(projectile);
            proj.Init(target, _projectileSpeed);
            proj.transform.position = transform.position + Vector3.up;
            proj.SetProjectileAction(() => target.Hp.GetHeal(value));
        }
        else
        {
            target.Hp.GetHeal(value);
        }

    }


    IEnumerator C_FallingCoroutine()
    {
        _animator.SetTrigger("Place_t");
        float targetY = transform.position.y - Constants.FALLING_POS;

        while (transform.position.y > targetY)
        {
            transform.position += Vector3.down * Time.deltaTime * Constants.FALLING_SPEED;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }


    public override void OnDeath()
    {
        base.OnDeath();
        _unitSpUI.DisableUI();


        _basicAttack.UnBlockTargets();
        _skillAttack.UnBlockTargets();

        Die?.Invoke(this);
        gameObject.SetActive(false);
    }
}
