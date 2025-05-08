using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading Settings")]
    [SerializeField] private string sceneToLoad = "GameScene";
    [SerializeField] private float artificialMinLoadTime = 1.5f; // Minimum time to show loading screen

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private Slider progressSlider;

    [Header("Animation Settings")]
    [SerializeField] private float animationSpeed = 0.5f; // Speed of loading text animation
    [SerializeField] private float progressBarSpeed = 0.5f; // Speed of progress bar movement (lower = slower)

    private bool isLoading = false;
    private float currentProgress = 0f;
    private float targetProgress = 0f;
    private float smoothVelocity = 0.0f;

    private void OnEnable()
    {
        // Start loading when this screen is enabled
        StartCoroutine(LoadSceneAsync());
        StartCoroutine(AnimateLoadingText());
    }

    private IEnumerator LoadSceneAsync()
    {
        if (isLoading)
            yield break;

        isLoading = true;

        // Initialize UI
        progressSlider.value = 0;
        percentageText.text = "0%";

        // Start loading the scene in background
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        // Don't let the scene activate until we allow it
        asyncOperation.allowSceneActivation = false;

        // Track the elapsed time for artificial minimum load time
        float elapsedTime = 0f;

        // While the scene loads
        while (!asyncOperation.isDone)
        {
            elapsedTime += Time.deltaTime;

            // Progress goes from 0 to 0.9 when scene is fully loaded
            // We scale it to go from 0 to 1
            targetProgress = asyncOperation.progress / 0.9f;

            // Smooth the progress bar (higher value = slower movement)
            currentProgress = Mathf.SmoothDamp(currentProgress, targetProgress, ref smoothVelocity, progressBarSpeed);

            // Update UI
            progressSlider.value = currentProgress;
            percentageText.text = Mathf.RoundToInt(currentProgress * 100) + "%";

            // If scene is loaded AND we've hit our minimum time
            if (asyncOperation.progress >= 0.9f && elapsedTime >= artificialMinLoadTime)
            {
                // Make sure progress is at 100% visually
                StartCoroutine(FillProgressBarToCompletion());

                // Allow scene to activate
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        isLoading = false;
    }

    private IEnumerator FillProgressBarToCompletion()
    {
        // Smoothly fill progress bar to 100%
        float finalFillSpeed = 0.3f; // Lower = slower final fill

        while (currentProgress < 1f)
        {
            currentProgress = Mathf.MoveTowards(currentProgress, 1f, Time.deltaTime * finalFillSpeed);
            progressSlider.value = currentProgress;
            percentageText.text = Mathf.RoundToInt(currentProgress * 100) + "%";
            yield return null;
        }
    }

    private IEnumerator AnimateLoadingText()
    {
        string[] loadingStates = { "Loading.", "Loading..", "Loading..." };
        int currentState = 0;

        while (isLoading || currentProgress < 1f)
        {
            loadingText.text = loadingStates[currentState];
            currentState = (currentState + 1) % loadingStates.Length;

            yield return new WaitForSeconds(animationSpeed);
        }
    }

    // Public method to load a specific scene
    public void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        StartCoroutine(LoadSceneAsync());
    }
}