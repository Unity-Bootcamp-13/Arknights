using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Button _goToStageButton;

    private void Awake()
    {
        _goToStageButton.onClick.AddListener(OnStageButtonClicked);
    }

    public void OnStageButtonClicked()
    {
        SceneManager.LoadScene("StageSelect");
    }
}
