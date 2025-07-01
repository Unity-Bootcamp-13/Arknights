using System.Collections;
using UnityEngine;

public class BlinkSpriteBehaviour : MonoBehaviour
{
    private SpriteRenderer _spr;
    private Color[] _colors;

    private void Awake()
    {
        _spr = transform.Find("Spr").GetComponent<SpriteRenderer>();
    }

    public void Init(bool canPlace)
    {
        _colors = canPlace
            ? new[] { new Color(0.2f, 0.2f, 1f, 0.5f), new Color(0f, 0.6f, 1f, 0.5f) }
            : new[] { new Color(1f, 0.1f, 0.1f, 0.5f), new Color(1f, 0.4f, 0f, 0.5f) };

        StartCoroutine(Blink());
    }

    private IEnumerator Blink()
    {
        bool toggle = false;
        while (true)
        {
            _spr.color = toggle ? _colors[1] : _colors[0];
            toggle = !toggle;
            yield return new WaitForSeconds(0.15f);
        }
    }
}