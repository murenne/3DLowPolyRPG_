using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    [SerializeField] private float _fadeInDuration;
    [SerializeField] private float _fadeOutDuration;
    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator FadeOutAndIn()
    {
        yield return FadeOut(_fadeOutDuration);
        yield return FadeIn(_fadeInDuration);
    }

    public IEnumerator FadeOut(float time)
    {
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (_canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        _canvasGroup.alpha = 0;
        Destroy(gameObject);
    }
}
