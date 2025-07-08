using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Unity.VisualScripting;

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
    [SerializeField] private PlayerUnitSpawner _spawner;
    [SerializeField] private PreviewSummoner _previewSummoner;   // 인스펙터 연결
    [SerializeField] private DirectionSelectUI _dirSelectUI;
    [SerializeField] private CostWallet _costWallet;
    [SerializeField] private GameSpeedController _gameSpeedController;
    //private readonly List<WaitingSlotUI> _slots = new();
    private readonly Dictionary<int, WaitingSlotUI> _slotMap = new();
    void Start()
    {
        foreach (var data in unitDataList)
        {
            AddSlot(data);
        }
        RegisterExistingUnits();
        _dirSelectUI.Spawn += HandleUnitSpawned;
    }

    void OnDestroy()
    {
        _dirSelectUI.Spawn -= HandleUnitSpawned;
    }

    private void HandleUnitSpawned(int id)
    {
        var data = unitDataList.FirstOrDefault(data => data.Id == id);
        if (data == null) return;

        // 코스트 재계산 (현재 소환 횟수 기반)
        int spawnCount = _dirSelectUI.SpawnCounts.TryGetValue(id, out var count) ? count : 0;
        int cost = Mathf.RoundToInt(data.PlaceCost * Mathf.Pow(1.5f, spawnCount));

        if (_slotMap.TryGetValue(id, out var slot))
        {
            slot.UpdateSpawnCost(cost);
            slot.gameObject.SetActive(false);
        }
    }
  

    private void HandleUnitDie(int id)
    {
        if (_slotMap.TryGetValue(id, out var slot))
        {
            slot.gameObject.SetActive(true);    // 다시 보이기
            slot.StartCooldown();               // 쿨타임 시작
            return;
        }
    }
    private void AddSlot(PlayerUnitData data)
    {
        GameObject go = Instantiate(slotPrefab, slotContainer);
        WaitingSlotUI slot = go.GetComponent<WaitingSlotUI>();
        slot.SetupSlot(data, this, _costWallet);

        _slotMap[data.Id] = slot;
    }

    /*private void UpdateBackgroundSize()
    {
        int count = _slotMap.Count;
        float width = _slotMap.Count * SlotSize + Mathf.Max(0, _slotMap.Count - 1) * SlotSpacing;
        background.sizeDelta = new Vector2(width, SlotSize);
    }*/

    public void OnSlotClicked(PlayerUnitData data)
    {
        if (_gameSpeedController.IsPause == true)
        {
            OnInfoOnlyClicked(data);
        }
        else
        {
            _previewSummoner.StartPreview(data);
        }
           
    }

    public void OnInfoOnlyClicked(PlayerUnitData data)
    {
        _previewSummoner.ShowInfoOnly(data);   // 프리뷰 없이 정보만
    }
    private void RegisterExistingUnits()
    {
        foreach (var pair in _spawner.PlayerUnits)   // pair.Key = id
        {
            int id = pair.Key;
            PlayerUnit unit = pair.Value;

            if (_slotMap.TryGetValue(id, out var slot))
            {
                /* (1) 슬롯 자체 쿨타임과 연결 */
                unit.Die -= slot.StartCooldownProxy;      // 중복 방지
                unit.Die += slot.StartCooldownProxy;

                /* (2) WaitingUI가 ID만 받도록 람다 캡처 */
                int capturedId = id;                 // foreach 캡처 주의
                unit.Die -= _ => HandleUnitDie(capturedId);
                unit.Die += _ => HandleUnitDie(capturedId);
            }
        }
    }
}
