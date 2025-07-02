using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    [Header("UI Buttons")]
    [SerializeField] private Button _pauseButton; 
    [SerializeField] private Button _fastSpeedButton;


    [Header("필수 참조")]
    [SerializeField] private Image _pauseButtonImage;  
    [SerializeField] private Image _playbackButtonImage;  


    [Header("Play 상태")]
    [SerializeField] private Sprite _pauseSprite;
  
    [Header("Pause 상태")]
    [SerializeField] private Sprite _playSprite;

    [Header("2배속 상태")]
    [SerializeField] private Sprite _playbackSpeed1Sprite;
  
    [Header("1배속 상태")]
    [SerializeField] private Sprite _playbackSpeed2Sprite;


    private bool _isPauseButtonPushed;
    private bool _isPlayBackButtonPushed;


    private int _playBackSpeed;
    private int _gameSpeed;


    private void Awake()
    {
        _isPauseButtonPushed = false;
        _isPlayBackButtonPushed = false;
        _playBackSpeed = 1;
        _gameSpeed = 1;
    }


    public void OnClickPauseButton()
    {
        _isPauseButtonPushed = !_isPauseButtonPushed;

        PauseButtonImageChange();

        if (_gameSpeed == 0)
        {
            _gameSpeed = 1;
            UpdateTimeScale();
        }
        else
        {
            _gameSpeed = 0;
            UpdateTimeScale();
        }
    }

    public void OnClickPlaybackSpeedButton()
    {
        _isPlayBackButtonPushed = !_isPlayBackButtonPushed;

        PlayBackSpeedButtonImageChange();

        if (_playBackSpeed == 2)
        {
            _playBackSpeed = 1;
            UpdateTimeScale();
        }
        else
        {
            _playBackSpeed = 2;
            UpdateTimeScale();
        }
    }


    public void UpdateTimeScale()
    {
        Time.timeScale = _playBackSpeed * _gameSpeed;
    }


    public void PauseButtonImageChange()
    {

        if (_isPauseButtonPushed)
        {
            _pauseButtonImage.sprite = _playSprite;
        }
        else
        {
            _pauseButtonImage.sprite = _pauseSprite;
        }
    }

    public void PlayBackSpeedButtonImageChange()
    {

        if (_isPlayBackButtonPushed)
        {
            _playbackButtonImage.sprite = _playbackSpeed1Sprite;
        }
        else
        {
            _playbackButtonImage.sprite = _playbackSpeed2Sprite;
        }
    }



}
