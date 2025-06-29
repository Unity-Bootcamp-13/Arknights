using System;
using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public event Action<int> OnCostChanged;

    private const int MaxCost = 99;
    private const int MinCost = 0;
    private int _currentCost = 0;

    private void Start()
    {
        StartCoroutine(CostRegenLoop());
    }
    private IEnumerator CostRegenLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            AddCost(1);
        }
    }
    public int CurrentCost => _currentCost;
    public void AddCost(int amount)
    {
        int newCost = Mathf.Clamp(_currentCost + amount, MinCost, MaxCost);
        if (newCost != _currentCost)
        {
            _currentCost = newCost;
            OnCostChanged?.Invoke(_currentCost);
        }
    }

    public bool DeleteCost(int amount)
    {
        if (_currentCost >= amount)
        {
            AddCost(-amount);
            return true;
        }
        return false;
    }
}