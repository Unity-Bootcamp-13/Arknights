using TMPro;
using UnityEngine;

public class GameStatusUI : MonoBehaviour
{
    [Header("의존성 주입")]
    [SerializeField] private GameManager _gameManager;    //추후 슬라이더 등 UI 요소로 변경 예정
    [SerializeField] private TextMeshProUGUI _nowCostText;
    private void Start()
    {
        Initialize(_gameManager);
    }
    public void Initialize(GameManager manager)
    {
        _gameManager = manager;
        _gameManager.OnCostChanged += UpdateCostUI;
    }

    private void UpdateCostUI(int cost)
    {
        _nowCostText.text = cost.ToString();
    }
}