using TMPro;
using UnityEngine;

public class GameStatusUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

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