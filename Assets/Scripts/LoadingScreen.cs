using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string sceneToLoad = "GameScene";
    [SerializeField] private float artificialMinLoadTime = 1f;

    [Header("UI References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Tween Settings")]
    [SerializeField] private float sliderTweenDuration = 0.3f;
    [SerializeField] private float textAnimationInterval = 0.5f;

    private Coroutine loadRoutine;
    private Tween loadingDotsTween;
    private bool isLoading = false;

    private void OnEnable()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            StartLoading();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        loadingDotsTween?.Kill();
    }

    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }

    public void StartLoading()
    {
        if (isLoading || string.IsNullOrEmpty(sceneToLoad)) return;

        isLoading = true;
        progressSlider.value = 0f;
        percentageText.text = "0%";

        AnimateLoadingText();
        loadRoutine = StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);
        async.allowSceneActivation = false;

        float elapsed = 0f;
        float targetProgress = 0f;

        while (async.progress < 0.9f)
        {
            elapsed += Time.deltaTime;
            targetProgress = Mathf.Clamp01(async.progress / 0.9f);

            UpdateProgressBar(targetProgress);
            yield return null;
        }

        // Wait for artificial minimum time
        while (elapsed < artificialMinLoadTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fill to 100% before activating
        yield return FillToCompletion();

        async.allowSceneActivation = true;
        isLoading = false;
    }

    private void UpdateProgressBar(float value)
    {
        float percent = Mathf.Clamp01(value) * 100f;
        progressSlider.DOValue(value, sliderTweenDuration);
        percentageText.text = Mathf.RoundToInt(percent) + "%";
    }

    private IEnumerator FillToCompletion()
    {
        float fill = progressSlider.value;
        while (fill < 1f)
        {
            fill = Mathf.MoveTowards(fill, 1f, Time.deltaTime);
            progressSlider.value = fill;
            percentageText.text = Mathf.RoundToInt(fill * 100f) + "%";
            yield return null;
        }
    }

    private void AnimateLoadingText()
    {
        string[] loadingDots = { "Loading.", "Loading..", "Loading..." };
        int index = 0;

        loadingDotsTween = DOTween.Sequence()
            .AppendCallback(() => loadingText.text = loadingDots[index])
            .AppendInterval(textAnimationInterval)
            .SetLoops(-1)
            .OnStepComplete(() =>
            {
                index = (index + 1) % loadingDots.Length;
            });
    }
}
