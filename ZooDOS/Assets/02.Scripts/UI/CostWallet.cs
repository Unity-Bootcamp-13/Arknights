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
    [SerializeField] private Slider progressSlider;   
    [SerializeField] private TMP_Text costText; 

    private Cost _current = Cost.Zero;
    public Cost Current => _current;

    public event Action<Cost> OnCostChanged;

    void Awake()
    {
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
            progressSlider.value = 0f;   // 시작은 0
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

    public void AddCost(Cost amount)
    {
        if (amount.Value <= 0) return;
        _current = _current.Add(amount, maxCost);
        Notify();
    }

    public bool TrySpendCost(Cost price)
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
        AddCost(new Cost(half));
    }

    IEnumerator AutoRegen()
    {
        while (true)
        {
            // 0초 → 1초 동안 진행도를 올린다
            float elapsed = 0f;
            while (elapsed < tickInterval)
            {
                elapsed += Time.deltaTime;
                if (progressSlider)
                    progressSlider.SetValueWithoutNotify(elapsed / tickInterval);
                yield return null;   
            }
            progressSlider.SetValueWithoutNotify(0f);

            AddCost(new Cost(tickAmount));
        }
    }

    void Notify()
    {
        UpdateUI();
        OnCostChanged?.Invoke(_current);
    }

    void UpdateUI()
    {
        if (costText)
        {
            costText.text = _current.Value.ToString();
        }
    }
}