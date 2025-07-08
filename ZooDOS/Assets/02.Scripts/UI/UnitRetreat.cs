using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitRetreat : MonoBehaviour
{
    [Header("Deps")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Map _map;
    [SerializeField] private CostWallet _costWallet;
    [SerializeField] private PreviewSummoner _previewSummoner;

    [Header("UI")]
    [SerializeField] private GameObject _unitDiamondPanel;
    [SerializeField] private Button _retreatButton;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _statusLabel;
    [SerializeField] private TMP_Text _nameLabel;
    [SerializeField] private Image _standingIllustPanel;
    [SerializeField] private Image _ClassIcon;
    
    private Button _blockerButton;
    private LayerMask _playerUnitMask;   // PlayerUnit 레이어
    private PlayerUnit _selectedUnit;

    void Awake()
    {
        _retreatButton.onClick.AddListener(OnRetreatClicked);
        _playerUnitMask = LayerMask.GetMask("PlayerUnit");
        CreateBlocker();
        _unitDiamondPanel.SetActive(false);
        _standingIllustPanel.gameObject.SetActive(false);
    }

    void CreateBlocker()
    {
        // 1) 빈 오브젝트 + 컴포넌트
        GameObject go = new("UnitInfoBlocker",
                            typeof(RectTransform),
                            typeof(CanvasRenderer),
                            typeof(Image),
                            typeof(Button));

        // 2) Canvas 하위(Panel과 같은 부모)에 붙이기
        go.transform.SetParent(_canvas.transform, false);

        // 3) 풀스크린으로 Stretch
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;

        // 4) 이미지 → 완전 투명
        Image img = go.GetComponent<Image>();
        img.color = new Color(0, 0, 0, 0);   
        img.raycastTarget = true;

        // 5) 버튼 클릭 → ClosePanel
        _blockerButton = go.GetComponent<Button>();
        _blockerButton.onClick.AddListener(ClosePanel);

        // 6) 패널보다 뒤(인덱스가 작아야 뒤)로 이동
        int panelIndex = _unitDiamondPanel.transform.GetSiblingIndex();
        go.transform.SetSiblingIndex(panelIndex);
        _unitDiamondPanel.transform.SetSiblingIndex(panelIndex + 1);

        go.SetActive(false);          
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && 
            _previewSummoner._previewSummonerIsNull() &&
            !_unitDiamondPanel.activeSelf)
        {
            TrySelectUnitAtPointer();
        }
    }

    void TrySelectUnitAtPointer()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100f, _playerUnitMask))
        {
            
            return;
        }

        PlayerUnit unit = hit.collider.GetComponent<PlayerUnit>();
        if (unit == null)
        {
            return;
        }
        Debug.Log(unit.name);
        OpenUnitPanel(unit);
    }

    void OpenUnitPanel(PlayerUnit unit)
    {
        _selectedUnit = unit;
        _selectedUnit.Die += HandleUnitDie;
        // 기본 정보 표시 
        PlayerUnitStatus stat = _selectedUnit.GetStatus();
        _statusLabel.text = BuildStatusString(stat);
        _nameLabel.text = stat.Name;
        _standingIllustPanel.sprite = stat.StandingIllust;
        _ClassIcon.sprite = stat.ClassIcon;
        //AttackRange
        Position pos = _map.Vector3ToCoord(unit.transform.position);
        Vector3 dir = unit.transform.forward.normalized;
        _previewSummoner.ShowAttackRange(pos, dir);

        int top = _canvas.transform.childCount - 1;   // 최상위 인덱스
       
        _blockerButton.gameObject.SetActive(true);
        _blockerButton.transform.SetSiblingIndex(top - 1);
        _unitDiamondPanel.SetActive(true);
        _unitDiamondPanel.transform.SetAsLastSibling();
        _standingIllustPanel.gameObject.SetActive(true);
        PositionPanelAtWorld(unit.transform.position);
    }

    void PositionPanelAtWorld(Vector3 worldPos)
    {
        // 1) 월드 → 스크린
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(worldPos);       

        // 2) 스크린 → 캔버스 로컬
        RectTransform canvasRect = _canvas.transform as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPos,
            _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _mainCamera,
            out Vector2 localPos);

        // 3) 패널 이동
        RectTransform panelRect = _unitDiamondPanel.transform as RectTransform;
        panelRect.anchoredPosition = localPos;
    }

    void OnRetreatClicked()
    {
        if (_selectedUnit == null)
        {
            return;
        }
        // 1) 코스트 환급 => 추후 PlayerUnit이 전달해주는 정보로 수정
        _costWallet.RefundHalf(new Cost(10));

        // 2) 유닛 제거
        _selectedUnit.Hp.OnDeath();

        // 3) UI 정리
        _previewSummoner.HideAttackRange();
        _selectedUnit = null;
        ClosePanel();
    }
    void ClosePanel()
    {
        _unitDiamondPanel.SetActive(false);
        _standingIllustPanel.gameObject.SetActive(false);
        _blockerButton.gameObject.SetActive(false);

        _previewSummoner.HideAttackRange();     

        if (_selectedUnit != null)
            _selectedUnit.Die -= HandleUnitDie; 

        _selectedUnit = null;
    }

    void HandleUnitDie(Unit unit)
    {
        ClosePanel();
    }


    private string BuildStatusString(PlayerUnitStatus stat)
    {
        //추후 한글로 변경
        return $"Cost : {stat.Cost}\nATK  : {stat.Atk}\nDEF  : {stat.Def}\nMax Target : {stat.MaxTarget}";
    }
}

