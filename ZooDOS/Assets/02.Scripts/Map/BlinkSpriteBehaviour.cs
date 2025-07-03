using System.Collections;
using UnityEngine;

public class BlinkSpriteBehaviour : MonoBehaviour
{
    private SpriteRenderer _spr;
    private Color _originColor;
    private Coroutine _blinkCoroutine;

    [Header("배치 깜박임")]
    [SerializeField] private Color _placement0 = new(0.4f, 0.5f, 1f, 0.5f);
    [SerializeField] private Color _placement1 = new(0.4f, 0.6f, 1f, 0.5f);

    [Header("공격범위 깜박임")]
    [SerializeField] private Color _attack0 = new(0.9f, 0f, 0f, 0.5f);
    [SerializeField] private Color _attack1 = new(0.85f, 0f, 0f, 0.5f);

    private void Awake()
    {
        _spr = GetComponent<SpriteRenderer>();
        _originColor = _spr.color;              
    }

    public void StartBlink(Color c0, Color c1, float interval = 0.5f)
    {
        StopBlink();                          
        _blinkCoroutine = StartCoroutine(BlinkRoutine(c0, c1, interval));
    }

    public void StopBlink()
    {
        if (_blinkCoroutine == null)
        {
            return;
        }
        StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;
        _spr.color = _originColor;              
    }

    public void StartPlacementBlink()
    {
        StartBlink(_placement0, _placement1);
    }

    public void StartAttackRangeBlink()
    {
        StartBlink(_attack0, _attack1);
    }

    private IEnumerator BlinkRoutine(Color c0, Color c1, float interval)
    {
        bool toggle = false;
        while (true)
        {
            _spr.color = toggle ? c1 : c0;
            toggle = !toggle;
            yield return new WaitForSeconds(interval);
        }
    }
}