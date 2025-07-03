using TMPro;
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
    [SerializeField] TextMeshProUGUI label;

    [Header("Play 상태")]
    [SerializeField] private Image _pauseImage;
  
    [Header("Pause 상태")]
    [SerializeField] private Image _playImage;

    [Header("2배속 상태")]
    [SerializeField] private Image _playbackSpeed1Image;
    [SerializeField] string textA = "x1";

    [Header("1배속 상태")]
    [SerializeField] private Image _playbackSpeed2ImageL;
    [SerializeField] private Image _playbackSpeed2ImageR;
    [SerializeField] string textB = "x2";


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
            _playImage.enabled = true;
            _pauseImage.enabled = false;
     
        }
        else
        {
            _playImage.enabled = false;
            _pauseImage.enabled = true;
          
        }
    }

    public void PlayBackSpeedButtonImageChange()
    {

        if (_isPlayBackButtonPushed)
        {
            _playbackSpeed1Image.enabled = false;
            _playbackSpeed2ImageL.enabled = true;
            _playbackSpeed2ImageR.enabled = true;
            label.text = textB;
        
        }
        else
        {
            _playbackSpeed1Image.enabled = true;
            _playbackSpeed2ImageL.enabled = false;
            _playbackSpeed2ImageR.enabled = false;
            label.text = textA;
        }
    }



}
