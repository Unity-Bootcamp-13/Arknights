using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Image _scenePanel;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Sprite[] _icons;
    private Sprite _icon;

    const float FADE_IN_COEFFICIENT = 2f;

    private void Awake()
    {
        _icon = _icons[Random.Range(0, _icons.Length - 1)];
        _iconImage.sprite = _icon;
        Time.timeScale = 1;
        _scenePanel.gameObject.SetActive(true);
        FadeIn();


    }


    public void OnSceneChageButtonClicked(string sceneName)
    {
        FadeOut(sceneName);
    }

    public void FadeIn()
    {
        StartCoroutine(C_FadeIn());
    }

    public void FadeOut(string sceneName)
    {
        StartCoroutine(C_FadeOut(sceneName));
    }

    IEnumerator C_FadeIn()
    {
        
        float alpha = 1;
        _scenePanel.color = new Color(0, 0, 0, alpha);
        _iconImage.color = new Color(255, 255, 255, alpha);

        while (alpha > 0)
        {
            Debug.Log("11111133");
            alpha -= Time.deltaTime * FADE_IN_COEFFICIENT;
            _scenePanel.color = new Color(0, 0, 0, alpha);
            _iconImage.color = new Color(255, 255, 255, alpha);
            yield return null;
        }

        _scenePanel.color = new Color(0, 0, 0, 0);
        _iconImage.color = new Color(255, 255, 255, 0);

        _scenePanel.gameObject.SetActive(false);


    }

    IEnumerator C_FadeOut(string sceneName)
    {
        _scenePanel.gameObject.SetActive(true);
        float alpha = 0;
        _scenePanel.color = new Color(0, 0, 0, alpha);
        _iconImage.color = new Color(255, 255, 255, alpha);

        while (alpha <1)
        {
            alpha += Time.deltaTime * FADE_IN_COEFFICIENT;
            _scenePanel.color = new Color(0, 0, 0, alpha);
            _iconImage.color = new Color(255, 255, 255, alpha);
            yield return null;
        }

        _scenePanel.color = new Color(0, 0, 0, 1);
        _iconImage.color = new Color(255, 255, 255, 1);
        SceneManager.LoadScene($"{sceneName}");


    }

}
