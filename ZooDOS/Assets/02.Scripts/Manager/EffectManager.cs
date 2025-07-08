using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private GameObject _hitEffectPrefab;
    [SerializeField] private GameObject _healEffectPrefab;
    [SerializeField] private GameObject _playerSkillEffectPrefab;

    List<GameObject> _hitEffects;
    List<GameObject> _healEffects;
    List<GameObject> _playerSkillEffects;

    const int HIT_EFFECT_COUNT = 50;
    const int HEAL_EFFECT_COUNT = 50;
    const int PLAYER_SKILL_EFFECT_COUNT = 10;

    private Vector3 _offset;

    private void Awake()
    {
        _offset = new Vector3(0, 1.5f, 0);
        _hitEffects = new List<GameObject>();
        _healEffects = new List<GameObject>();
        _playerSkillEffects = new List<GameObject>();

        for (int i = 0; i < HIT_EFFECT_COUNT; i++)
        {
            GameObject effect = Instantiate(_hitEffectPrefab);
            _hitEffects.Add(effect);
            effect.transform.parent = transform;
            effect.gameObject.SetActive(false);
        }

        for (int i = 0; i < HEAL_EFFECT_COUNT; i++)
        {
            GameObject effect = Instantiate(_healEffectPrefab);
            _healEffects.Add(effect);
            effect.transform.parent = transform;
            effect.gameObject.SetActive(false);
        }

        for (int i = 0; i < PLAYER_SKILL_EFFECT_COUNT; i++)
        {
            GameObject effect = Instantiate(_playerSkillEffectPrefab);
            _playerSkillEffects.Add(effect);
            effect.transform.parent = transform;
            effect.gameObject.SetActive(false);
        }
    }

    public void PlayHitEffect(Vector3 position)
    {
        foreach (GameObject effect in _hitEffects)
        {
            if (effect.activeSelf)
            {
                continue;
            }

            effect.SetActive(true);
            effect.transform.position = position + _offset;

            StartCoroutine(C_EffectRoutine(effect));
            return;
        }
    }

    public void PlayHealEffect(Vector3 position)
    {
        foreach (GameObject effect in _healEffects)
        {
            if (effect.activeSelf)
            {
                continue;
            }

            effect.SetActive(true);
            effect.transform.position = position + _offset;

            StartCoroutine(C_EffectRoutine(effect));
            return;
        }

    }

    public GameObject GetPlayerSkillEffect(Vector3 position)
    {
        foreach (GameObject effect in _playerSkillEffects)
        {
            if (effect.activeSelf)
            {
                continue;
            }

            effect.SetActive(true);
            effect.transform.position = position + Vector3.up;
            return effect;
        }
        return null;
    }

    IEnumerator C_EffectRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(1);
        effect.SetActive(false);
    }

}