using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
public class LoadingScreen : MonoBehaviour
{

    public Slider loadingSlider; // Reference to the slider
    public TextMeshProUGUI loadingText; // Reference to the loading text
    public TextMeshProUGUI percentageText; // Reference to the percentage text
    public float duration = 4f; // Duration of the loading animation (increased for slower animation)
    public Ease easeType = Ease.Linear; // Ease type for the loading animation
    public string targetSceneName; // Name of the scene to open after loading

    private AsyncOperation asyncLoad;

    private void Start()
    {
        //AnimateLoadingScreen();
    }

    public void AnimateLoadingScreen(int index)
    {
        Debug.Log("Loading screen started 1");
        float targetSliderValue = 1f; // Target value for the slider
        string loadingTextPrefix = "Loading"; // Text prefix for the loading text

        Debug.Log("Loading screen started 2");
        loadingSlider.value = 0f; // Set the initial slider value to zero
        loadingText.text = loadingTextPrefix; // Set the initial loading text
        percentageText.text = "0%"; // Set the initial percentage text
        Debug.Log(index);
        asyncLoad = SceneManager.LoadSceneAsync(index);
        asyncLoad.allowSceneActivation = false;
        // Animate the slider value
        loadingSlider.DOValue(targetSliderValue, duration).SetEase(easeType);

        Debug.Log("Loading screen started 3");
        // Animate the loading text
        loadingText.DOText(loadingTextPrefix + "...", 5)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Restart); // Loop the animation

        Debug.Log("Loading screen started 4");
        // Animate the percentage text
        int startValue = 0;
        try
        {
            startValue = int.Parse(percentageText.text.Replace("%", ""));
        }
        catch
        {
            Debug.LogWarning("Failed to parse percentageText, defaulting to 0");
        }

        DOTween.To(() => startValue, x => percentageText.text = x + "%", 100, duration)
            .SetEase(easeType)
            .OnComplete(() => LoadTargetScene());
        Debug.Log("Loading screen started 5");
    }

    private void LoadTargetScene()
    {
        Debug.Log("Completed");
        asyncLoad.allowSceneActivation = true;
    }

    public void ReloadScene()
    {
        // Get the currently loaded scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene by loading it again
        SceneManager.LoadScene(currentSceneName);
    }

}