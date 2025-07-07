using System.Collections.Generic;
using UnityEngine;

public class UnitHpSpUIManager:MonoBehaviour
{
    [SerializeField] UnitHpUI _playerUnitHpUI;
    [SerializeField] PlayerUnitSpUI _playerUnitSpUI;
    [SerializeField] UnitHpUI _enemyUnitHpUI;


    List<UnitHpUI> _playerUnitHpUIs;
    List<PlayerUnitSpUI> _playerUnitSpUIs;
    List<UnitHpUI> _enemyUnitHpUIs;

    const int PLAYERUNIT_UI_COUNT = 10;
    const int ENEMYUNIT_HP_UI_COUNT = 100;


    private void Awake()
    {
        _playerUnitHpUIs = new List<UnitHpUI>();
        _playerUnitSpUIs = new List<PlayerUnitSpUI>();
        _enemyUnitHpUIs = new List<UnitHpUI>();

        for (int i =0; i <PLAYERUNIT_UI_COUNT; i++)
        {
            UnitHpUI ui = Instantiate(_playerUnitHpUI);
            _playerUnitHpUIs.Add(ui);
            ui.transform.parent = transform;
            ui.gameObject.SetActive(false);
        }

        for (int i = 0; i < PLAYERUNIT_UI_COUNT; i++)
        {
            PlayerUnitSpUI ui = Instantiate(_playerUnitSpUI);
            _playerUnitSpUIs.Add(ui);
            ui.transform.parent = transform;
            ui.gameObject.SetActive(false);
        }

        for (int i = 0; i < ENEMYUNIT_HP_UI_COUNT; i++)
        {
            UnitHpUI ui = Instantiate(_enemyUnitHpUI);
            _enemyUnitHpUIs.Add(ui);
            ui.transform.parent = transform;
            ui.gameObject.SetActive(false);
        }
    }

    public UnitHpUI GetPlayerUnitHpUI()
    {

        foreach (UnitHpUI ui in _playerUnitHpUIs)
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

    public PlayerUnitSpUI GetPlayerUnitSpUI()
    {

        foreach (PlayerUnitSpUI ui in _playerUnitSpUIs)
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

    public UnitHpUI GetEnemyUnitHpUI()
    {
        foreach (UnitHpUI ui in _enemyUnitHpUIs)
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