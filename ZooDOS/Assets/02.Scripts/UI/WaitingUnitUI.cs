using UnityEngine;
using System.Collections.Generic;

public class WaitingUnitUI : MonoBehaviour
{
    [Header("Slots")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private RectTransform background;
    [SerializeField] private List<PlayerUnitData> unitDataList;

    [Header("Delegates")]
    [SerializeField] private PreviewSummoner _previewSummoner;   // 인스펙터 연결

    private readonly List<WaitingUnitSlotUI> _slots = new();
    private const float SlotSize = 120f;
    private const float SlotSpacing = 5f;

    void Start()
    {
        foreach (var data in unitDataList)
            AddSlot(data);
    }

    private void AddSlot(PlayerUnitData data)
    {
        GameObject go = Instantiate(slotPrefab, slotContainer);
        var slot = go.GetComponent<WaitingUnitSlotUI>();
        slot.Setup(data, this);

        var rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(SlotSize, SlotSize);
        rt.anchoredPosition = new Vector2(-(SlotSize + SlotSpacing) * _slots.Count, 0);

        _slots.Add(slot);
        UpdateBackgroundSize();
    }

    private void UpdateBackgroundSize()
    {
        float w = _slots.Count * SlotSize + Mathf.Max(0, _slots.Count - 1) * SlotSpacing;
        background.sizeDelta = new Vector2(w, SlotSize);
    }

    // 슬롯이 클릭될 때 호출됨
    public void OnSlotClicked(PlayerUnitData data) => _previewSummoner.StartPreview(data);
}
