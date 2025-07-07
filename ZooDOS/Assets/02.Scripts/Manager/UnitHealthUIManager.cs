using System.Collections.Generic;
using UnityEngine;

public class UnitHealthUIManager:MonoBehaviour
{
    [SerializeField] UnitHealthUI _playerUnitHealthUI;
    [SerializeField] UnitHealthUI _enemyUnitHealthUI;


    List<UnitHealthUI> _playerUnitHealthUIs;
    List<UnitHealthUI> _enemyUnitHealthUIs;

    const int PLAYERUNIT_HEALTHUI_COUNT = 10;
    const int ENEMYUNIT_HEALTHUI_COUNT = 100;


    private void Awake()
    {
        _playerUnitHealthUIs = new List<UnitHealthUI>();
        _enemyUnitHealthUIs = new List<UnitHealthUI>();

        for (int i =0; i <PLAYERUNIT_HEALTHUI_COUNT; i++)
        {
            UnitHealthUI ui = Instantiate(_playerUnitHealthUI);
            _playerUnitHealthUIs.Add(ui);
            ui.transform.parent = transform;
            ui.gameObject.SetActive(false);
        }

        for (int i = 0; i < ENEMYUNIT_HEALTHUI_COUNT; i++)
        {
            UnitHealthUI ui = Instantiate(_enemyUnitHealthUI);
            _enemyUnitHealthUIs.Add(ui);
            ui.transform.parent = transform;
            ui.gameObject.SetActive(false);
        }
    }

    public UnitHealthUI GetPlayerUnitHealthUI()
    {

        foreach (UnitHealthUI ui in _playerUnitHealthUIs)
        {
            if (ui.gameObject.activeSelf)
            {
                continue;
            }

            ui.gameObject.SetActive(true);
            return ui;
        }

        return null;
    }

    public UnitHealthUI GetEnemyUnitHealthUI()
    {
        foreach (UnitHealthUI ui in _enemyUnitHealthUIs)
        {
            if (ui.gameObject.activeSelf)
            {
                continue;
            }

            ui.gameObject.SetActive(true);
            return ui;
        }

        return null;
    }

  
    
}