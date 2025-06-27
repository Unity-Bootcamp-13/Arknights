using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WaitingUnitUI : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotPrefab2;
    [SerializeField] private GameObject slotPrefab3;
    
    private List<WaitingUnitSlotUI> slotList = new List<WaitingUnitSlotUI>(); //

    [SerializeField] private Map _map;
    [SerializeField] private PlayerUnitSpawner _playerUnitSpawner;

    private const float SlotSize = 120f;
    private const float SlotSpacing = 5f;

    private void Start()
    {
        AddUnitSlot(slotPrefab);
        AddUnitSlot(slotPrefab2);
        AddUnitSlot(slotPrefab3);
    }

    public void AddUnitSlot(GameObject slotPrefab)
    {
        GameObject slotObj = Instantiate(slotPrefab, slotContainer);
        WaitingUnitSlotUI slot = slotObj.GetComponent<WaitingUnitSlotUI>();
        
        PlayerUnitData data = slot?.GetPlayerUnitData();
        if (data == null)
        {
            Debug.LogError("PlayerUnitData not set on slot prefab.");
            return;
        }

        slot.Initialize(data, _playerUnitSpawner, _map);
        RectTransform rt = slotObj.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(SlotSize, SlotSize);
        float xPos = -(SlotSize + SlotSpacing) * slotList.Count;
        rt.anchoredPosition = new Vector2(xPos, 0); 
        slotList.Add(slot);

        UpdateBackgroundSize();
    }

    private void UpdateBackgroundSize()
    {
        float totalWidth = SlotListWidth();
        background.sizeDelta = new Vector2(totalWidth, SlotSize);
    }

    private float SlotListWidth()
    {
        int count = slotList.Count;
        return count * SlotSize + Mathf.Max(0, count - 1) * SlotSpacing;
    }
}
