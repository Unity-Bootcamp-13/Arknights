using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Unit _target;
    float _speed;
    const float OFFSET = 0.1f;

    float _leftBound;
    float _rightBound;
    float _upBound;
    float _downBound;
    Action _projectileAction;

    // Update is called once per frame
    void Update()
    {
        if (IsInTargetBound())
        {
            _projectileAction?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Targeting();
        }
    }

    public void Targeting()
    {
        transform.position += (_target.transform.position - transform.position).normalized * _speed * Time.deltaTime;
    }

    public void Init(Unit unit, float speed)
    {
        _target = unit;
        _speed = speed;

        _leftBound = _target.transform.position.x - OFFSET;
        _rightBound = _target.transform.position.x + OFFSET;
        _upBound = _target.transform.position.y + OFFSET;
        _downBound = _target.transform.position.y- OFFSET;
    }

    public void SetProjectileAction(Action projectileAction)
    {
        _projectileAction += projectileAction;
    }

    public bool IsInTargetBound()
    {
        if (transform.position.x < _rightBound && transform.position.x > _leftBound && transform.position.y < _upBound && transform.position.y > _downBound)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
