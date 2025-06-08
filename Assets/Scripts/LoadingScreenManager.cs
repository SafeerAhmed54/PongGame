using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading Screen Prefab")]
    [SerializeField] private GameObject loadingScreenPrefab;

    // These will be assigned at runtime from the instantiated prefab
    private GameObject loadingScreenCanvas;
    private Slider progressSlider;
    private TextMeshProUGUI loadingText;
    private TextMeshProUGUI percentageText;
    private RectTransform loadingIcon;

    [Header("Animation Settings")]
    [SerializeField] private float dotAnimationSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 200f;

    // Track active coroutines so we can stop them when needed
    private List<Coroutine> activeCoroutines = new List<Coroutine>();

    // Flag to track if we're currently loading a scene
    private bool isLoadingScene = false;

    // Singleton pattern
    public static LoadingScreenManager Instance { get; private set; }

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Create canvas for loading screen that will persist between scenes
            if (loadingScreenPrefab == null)
            {
                Debug.LogError("Loading Screen Prefab is not assigned! Please assign a Canvas prefab in the inspector.");
                return;
            }

            // Instantiate loading screen prefab as a child of this object
            loadingScreenCanvas = Instantiate(loadingScreenPrefab, transform);

            // Find all required components in the prefab
            progressSlider = loadingScreenCanvas.GetComponentInChildren<Slider>();

            // Find TextMeshPro components
            TextMeshProUGUI[] textComponents = loadingScreenCanvas.GetComponentsInChildren<TextMeshProUGUI>();
            if (textComponents.Length >= 2)
            {
                loadingText = textComponents[0];  // First TextMeshPro is loading text
                percentageText = textComponents[1]; // Second TextMeshPro is percentage text
            }
            else
            {
                Debug.LogError("Loading Screen Prefab must contain at least two TextMeshPro components!");
            }

            // Try to find the loading icon if it exists
            Transform iconTransform = loadingScreenCanvas.transform.Find("LoadingIcon");
            if (iconTransform != null)
            {
                loadingIcon = iconTransform.GetComponent<RectTransform>();
            }

            // Ensure loading screen is hidden at start
            loadingScreenCanvas.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Load a scene by name with a loading screen
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        // If already loading, don't start another load
        if (isLoadingScene)
        {
            Debug.LogWarning("Already loading a scene. Ignoring request to load: " + sceneName);
            return;
        }

        // Stop any existing coroutines
        StopAllCoroutines();
        activeCoroutines.Clear();

        // Start new loading process
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    /// <summary>
    /// Load a scene by build index with a loading screen
    /// </summary>
    /// <param name="sceneBuildIndex">The build index of the scene to load</param>
    public void LoadScene(int sceneBuildIndex)
    {
        // If already loading, don't start another load
        if (isLoadingScene)
        {
            Debug.LogWarning("Already loading a scene. Ignoring request to load scene index: " + sceneBuildIndex);
            return;
        }

        // Stop any existing coroutines
        StopAllCoroutines();
        activeCoroutines.Clear();

        // Start new loading process
        StartCoroutine(LoadSceneAsync(sceneBuildIndex));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("Starting to load scene: " + sceneName);
        isLoadingScene = true;

        // Show loading screen
        loadingScreenCanvas.SetActive(true);

        // Start loading text animation and track coroutine
        Coroutine textAnimCoroutine = StartCoroutine(AnimateLoadingText());
        activeCoroutines.Add(textAnimCoroutine);

        // Start loading icon animation if assigned
        if (loadingIcon != null)
        {
            Coroutine iconAnimCoroutine = StartCoroutine(AnimateLoadingIcon());
            activeCoroutines.Add(iconAnimCoroutine);
        }

        // Reset progress
        progressSlider.value = 0;
        percentageText.text = "0%";

        // Wait one frame to allow the loading screen to be visible
        yield return null;

        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // Don't let the scene activate until we allow it to
        asyncOperation.allowSceneActivation = false;

        // Track loading progress
        float progress = 0;

        // While the scene loads
        while (!asyncOperation.isDone)
        {
            // Calculate progress (AsyncOperation goes from 0 to 0.9 when it's done loading)
            // We convert to a 0-1 scale for our UI
            Debug.Log("Current Progress " + asyncOperation.progress);
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            Debug.Log("New Progress " + progress);

            // Update UI elements
            progressSlider.value = progress;
            percentageText.text = Mathf.Round(progress * 100) + "%";

            Debug.Log("Complete? " + (asyncOperation.progress >= 0.9f));

            // If loading is complete (progress reaches 90% which is 100% in AsyncOperation)
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Loading complete, waiting before activation");

                // Wait a bit to ensure user sees 100%
                float waitStartTime = Time.time;
                float waitDuration = 0.5f;

                while (Time.time < waitStartTime + waitDuration)
                {
                    Debug.Log("Waiting: " + (Time.time - waitStartTime) + " / " + waitDuration);
                    yield return null;
                }

                Debug.Log("Hello We meet after 0.5 seconds : " + (asyncOperation.progress >= 0.9f));

                // Activate the scene
                asyncOperation.allowSceneActivation = true;
                Debug.Log("Called asyncOperation.allowSceneActivation = true");
            }

            yield return null;
        }

        Debug.Log("Scene load completed, cleaning up");

        // Clean up coroutines
        foreach (Coroutine coroutine in activeCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();

        // Hide loading screen
        loadingScreenCanvas.SetActive(false);
        isLoadingScene = false;
    }

    private IEnumerator LoadSceneAsync(int sceneBuildIndex)
    {
        Debug.Log("Starting to load scene index: " + sceneBuildIndex);
        isLoadingScene = true;

        // Show loading screen
        loadingScreenCanvas.SetActive(true);

        // Start loading text animation
        Coroutine textAnimCoroutine = StartCoroutine(AnimateLoadingText());
        activeCoroutines.Add(textAnimCoroutine);

        // Start loading icon animation if assigned
        if (loadingIcon != null)
        {
            Coroutine iconAnimCoroutine = StartCoroutine(AnimateLoadingIcon());
            activeCoroutines.Add(iconAnimCoroutine);
        }

        // Reset progress
        progressSlider.value = 0;
        percentageText.text = "0%";

        // Wait one frame for the loading screen to be visible
        yield return null;

        // Start loading the scene
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);

        // Don't let the scene activate until we allow it to
        asyncOperation.allowSceneActivation = false;

        // Track loading progress
        float progress = 0;

        // While the scene is still loading
        while (!asyncOperation.isDone)
        {
            // Calculate progress (AsyncOperation goes from 0 to 0.9 when it's done loading)
            progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            // Update UI elements
            progressSlider.value = progress;
            percentageText.text = Mathf.Round(progress * 100) + "%";

            // If loading is complete (progress reaches 90% which is 100% in AsyncOperation)
            if (asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Loading complete, waiting before activation");

                // Wait a bit to ensure user sees 100%
                float waitStartTime = Time.time;
                float waitDuration = 0.5f;

                while (Time.time < waitStartTime + waitDuration)
                {
                    Debug.Log("Waiting: " + (Time.time - waitStartTime) + " / " + waitDuration);
                    yield return null;
                }

                Debug.Log("Hello We meet after 0.5 seconds : " + (asyncOperation.progress >= 0.9f));

                // Activate the scene
                asyncOperation.allowSceneActivation = true;
                Debug.Log("Called asyncOperation.allowSceneActivation = true");
            }

            yield return null;
        }

        Debug.Log("Scene load completed, cleaning up");

        // Clean up coroutines
        foreach (Coroutine coroutine in activeCoroutines)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        activeCoroutines.Clear();

        // Hide loading screen
        loadingScreenCanvas.SetActive(false);
        isLoadingScene = false;
    }

    private IEnumerator AnimateLoadingText()
    {
        string[] loadingVariations = { "Loading", "Loading.", "Loading..", "Loading..." };
        int currentIndex = 0;

        while (loadingScreenCanvas.activeInHierarchy)
        {
            if (loadingText != null)
            {
                loadingText.text = loadingVariations[currentIndex];
                currentIndex = (currentIndex + 1) % loadingVariations.Length;
            }
            yield return new WaitForSeconds(dotAnimationSpeed);
        }
    }

    private IEnumerator AnimateLoadingIcon()
    {
        while (loadingScreenCanvas.activeInHierarchy)
        {
            if (loadingIcon != null)
            {
                loadingIcon.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        // Clean up coroutines
        StopAllCoroutines();
    }
}