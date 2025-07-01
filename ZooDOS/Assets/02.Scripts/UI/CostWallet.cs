using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CostWallet : MonoBehaviour
{
    private const int tickAmount = 1;
    private const float tickInterval = 1f;
    private const int maxCost = 99;

    
    [Header("UI")]
    [SerializeField] private Slider costSlider;   
    [SerializeField] private TMP_Text costText; 

    
    private Cost _current = Cost.Zero;
    public Cost Current => _current;

    public event Action<Cost> OnCostChanged;

    void Awake()
    {
        if (costSlider != null)
        {
            costSlider.minValue = 0;
            costSlider.maxValue = maxCost;
            costSlider.value = _current.Value;
        }
        UpdateUI();
    }

    void OnEnable()
    {
        StartCoroutine(AutoRegen());
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Add(Cost amount)
    {
        if (amount.Value <= 0) return;
        _current = _current.Add(amount, maxCost);
        Notify();
    }

    public bool TrySpend(Cost price)
    {
        if (!_current.IsEnoughFor(price))
        {
            return false;
        }
        _current = _current.Subtract(price, 0);
        Notify();
        return true;
    }

    public void RefundHalf(Cost originalPrice)
    {
        int half = Mathf.CeilToInt(originalPrice.Value * 0.5f);
        Add(new Cost(half));
    }

    IEnumerator AutoRegen()
    {
        var wait = new WaitForSeconds(tickInterval);
        while (true)
        {
            Add(new Cost(tickAmount));
            yield return wait;
        }
    }

    void Notify()
    {
        UpdateUI();
        OnCostChanged?.Invoke(_current);
    }

    void UpdateUI()
    {
        if (costSlider)
        {
            costSlider.SetValueWithoutNotify(_current.Value);
        }
        if (costText)
        {
            costText.text = _current.Value.ToString();
        }
    }
}