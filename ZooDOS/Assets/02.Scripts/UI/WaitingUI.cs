using UnityEngine;
using System.Collections.Generic;

public class WaitingUI : MonoBehaviour
{
    private const float SlotSize = 120f;
    private const float SlotSpacing = 5f;

    [Header("대기석 슬롯")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform background;
    [SerializeField] private List<PlayerUnitData> unitDataList;

    [Header("의존성 주입")]
    [SerializeField] private PreviewSummoner _previewSummoner;   // 인스펙터 연결

    private readonly List<WaitingSlotUI> _slots = new();
    
    void Start()
    {
        foreach (var data in unitDataList)
            AddSlot(data);
    }

    private void AddSlot(PlayerUnitData data)
    {
        GameObject gameObject = Instantiate(slotPrefab, slotContainer);

        WaitingSlotUI slot = gameObject.GetComponent<WaitingSlotUI>();
        slot.SetupSlot(data, this);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(SlotSize, SlotSize);
        rectTransform.anchoredPosition = new Vector2(-(SlotSize + SlotSpacing) * _slots.Count, 0);

        _slots.Add(slot);
        UpdateBackgroundSize();
    }

    private void UpdateBackgroundSize()
    {
        float width = _slots.Count * SlotSize + Mathf.Max(0, _slots.Count - 1) * SlotSpacing;
        background.sizeDelta = new Vector2(width, SlotSize);
    }

    public void OnSlotClicked(PlayerUnitData data)
    {
        _previewSummoner.StartPreview(data); 
    }
}
