using System;

public class Sp
{
    float _maxSp;
    float _sp;
    PlayerUnit _unit;

    public Action OnSpChanged;



    public Sp(PlayerUnit unit, float sp)
    {
        _unit = unit;
        _maxSp = sp;
        _sp = 0;

    }

    public float SP
    {

        get
        {
            return _sp;
        }

        set
        {
            _sp = Math.Clamp(value, 0, _maxSp);
            OnSpChanged?.Invoke();

            if (_sp >= _maxSp)
            {
                _unit.SetSpCharged();
            }

        }
    }


    public float SpRatio => _sp / _maxSp;
    public float MaxSP => _maxSp;


    public void ResetSp()
    {
        SP = 0;
    }
    
    public void ChargeSp(float value)
    {
        SP += value;
    }

    public void SubscribeSpEvent(Action action)
    {
        OnSpChanged += action;
    }


}