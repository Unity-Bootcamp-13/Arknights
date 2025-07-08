using System.Collections;
using UnityEngine;

public class BlinkSpriteBehaviour : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color _originColor;
    private Coroutine _blinkCoroutine;
    MaterialPropertyBlock _materialPropertyBlock;

    [SerializeField] float _blinkInterval = 0.50f;

    [Header("배치 깜박임")]
    [SerializeField] private Color _placement0 = new(0.4f, 0.5f, 1f, 0.5f);
    [SerializeField] private Color _placement1 = new(0.4f, 0.6f, 1f, 0.5f);

    [Header("공격범위 깜박임")]
    [SerializeField] private Color _attack0 = new(0.95f, 0.8f, 0.95f, 0.3f);
    [SerializeField] private Color _attack1 = new(0.9f, 0.8f, 0.9f, 0.3f);

    static readonly int ShaderID_BaseColor = Shader.PropertyToID("_BaseColor");
    static readonly int ShaderID_Color = Shader.PropertyToID("_Color");

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        // 시작 시점 머티리얼 색상을 저장
        _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        _originColor = ReadColor(_materialPropertyBlock);
    }

    public void StartBlink(Color c0, Color c1, float interval = 0.5f)
    {
        StopBlink();                          
        _blinkCoroutine = StartCoroutine(BlinkRoutine(c0, c1, _blinkInterval));
    }

    public void StopBlink()
    {
        if (_blinkCoroutine == null)
        {
            return;
        }
        StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;
        WriteColor(_originColor);
    }

    public void StartPlacementBlink()
    {
        StartBlink(_placement0, _placement1, _blinkInterval);
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
            WriteColor(toggle ? c0 : c1);
            toggle = !toggle;
            yield return new WaitForSeconds(interval);
        }
    }
    private Color ReadColor(MaterialPropertyBlock block)
        => block.HasColor(ShaderID_BaseColor) ? block.GetColor(ShaderID_BaseColor) :
           block.HasColor(ShaderID_Color) ? block.GetColor(ShaderID_Color) :
           Color.white;
    private void WriteColor(Color c)
    {
        _meshRenderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetColor(ShaderID_BaseColor, c);
        _materialPropertyBlock.SetColor(ShaderID_Color, c);
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}