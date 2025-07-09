using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonCooldown : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private Image _fillImage;
    private Image _skillImage;
    private Button _skillButton;
    private PlayerUnit _nowPlayerUnit;
    private bool _cooling;

    public event Action Clicked;
    private void Awake()
    {
        _skillButton = GetComponent<Button>();
        _skillButton.onClick.AddListener(OnClick_Skill);
        _skillImage = GetComponent<Image>();
    }

    public void InitSkillData(PlayerUnit unit, Sprite sprite)
    {
        _nowPlayerUnit = unit;
        _skillImage.sprite = sprite;

        
    }
    public void DeleteUnit()
    {
        _nowPlayerUnit = null;
    }
    private void Update()
    {
        UpdateSkillRatio(_nowPlayerUnit);
    }
    public void UpdateSkillRatio(PlayerUnit unit)
    {
        _fillImage.fillAmount = unit.Sp.SpRatio;
        if (_fillImage.fillAmount == 1)
        {
            _fillImage.enabled = false;
        }
        else
        {
            _fillImage.enabled = true;
        }
    }
    public void OnClick_Skill()
    {
        Clicked?.Invoke();
    }
}