using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Unit _target;
    float _speed;
    Action _projectileAction;

    // Update is called once per frame
    void Update()
    {
        if(_target == null)
        {
            Destroy(gameObject);
            return;
        }

        if(_target.Hp.IsDead == false)
        {
            
            Targeting();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Targeting()
    {
        transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime;
        transform.LookAt(_target.gameObject.transform);
    }

    public void Init(Unit unit, float speed)
    {
        _target = unit;
        _speed = speed;
    }

    public void SetProjectileAction(Action projectileAction)
    {
        _projectileAction += projectileAction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Unit Component))
        {
            if (_target == Component)
            {
                _projectileAction?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
