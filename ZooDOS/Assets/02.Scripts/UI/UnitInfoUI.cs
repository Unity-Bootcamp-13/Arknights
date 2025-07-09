using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoUI : MonoBehaviour
{
    [Header("UI Refs")]
    [SerializeField] private TMP_Text _statusLabel;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private Image _standingIllust;
    [SerializeField] private Image _classIcon;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TMP_Text _skillCost;
    [SerializeField] private TMP_Text _skillDescription;
    [SerializeField] private GameObject _infoContainer;
    public void Show(string status, string name, Sprite standSprite, 
        Sprite classSprite, Sprite skillSprite, string skillCost, string skillDesc)
    {
        _statusLabel.text = status;
        _nameLabel.text = name;
        _standingIllust.sprite = standSprite;
        _classIcon.sprite = classSprite;

        _skillIcon.sprite = skillSprite;
        _skillCost.text = $"SP 소모량 : {skillCost}";
        _skillDescription.text = skillDesc;

        _infoContainer.SetActive(true);  
    }

    public void Hide() => _infoContainer.SetActive(false);

   
}