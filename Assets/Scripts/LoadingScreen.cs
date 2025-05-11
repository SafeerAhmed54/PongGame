using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    [Header("UI References")]
    public GameObject loadingPanel;
    public Slider progressBar;
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI loadingText;

    private bool isLoading = false;

    private float dotTimer = 0f;
    private int dotCount = 0;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadingPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

        if(loadingPanel == null)
        {
            loadingPanel = GameObject.Find("LoadingPanel");
        }
        if (progressBar == null)
        {
            progressBar = loadingPanel.GetComponentInChildren<Slider>();
        }
        if (percentageText == null)
        {
            percentageText = loadingPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
        if (loadingText == null)
        {
            loadingText = loadingPanel.GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;
        loadingPanel.SetActive(true);
        progressBar.value = 0;
        percentageText.text = "0%";
        loadingText.text = "Loading...";

        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";
            yield return null;
        }

        // Fill to 100%
        while (progressBar.value < 1f)
        {
            progressBar.value += Time.deltaTime;
            percentageText.text = Mathf.RoundToInt(progressBar.value * 100f) + "%";
            yield return null;
        }

        operation.allowSceneActivation = true;
        isLoading = false;
        loadingPanel.SetActive(false);
    }

    void Update()
    {
        if (loadingPanel.activeSelf)
        {
            dotTimer += Time.deltaTime;
            if (dotTimer > 0.5f)
            {
                dotCount = (dotCount + 1) % 4;
                loadingText.text = "Loading" + new string('.', dotCount);
                dotTimer = 0f;
            }
        }
    }

}
