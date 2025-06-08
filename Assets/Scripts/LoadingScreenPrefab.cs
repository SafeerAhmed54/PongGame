using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Helper script to create a loading screen prefab.
/// This script should be attached to a Canvas object.
/// </summary>
public class LoadingScreenPrefab : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI percentageText;

    [Header("Optional Components")]
    [SerializeField] private RectTransform loadingIcon;

    private void Reset()
    {
        // Check if this component is attached to a Canvas
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("LoadingScreenPrefab should be attached to a Canvas object! Please add this script to a Canvas instead.");
            return;
        }

        // Configure the canvas properly
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Ensure it's on top of everything

        // Add canvas scaler if it doesn't exist
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
        }
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Add GraphicRaycaster if it doesn't exist
        if (GetComponent<GraphicRaycaster>() == null)
        {
            gameObject.AddComponent<GraphicRaycaster>();
        }
    }

#if UNITY_EDITOR
    // This is just a helper method for editor setup
    [ContextMenu("Create Loading Screen Elements")]
    private void CreateLoadingScreenElements()
    {
        // Create background panel
        GameObject panel = new GameObject("BackgroundPanel");
        panel.transform.SetParent(transform, false);
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f); // Semi-transparent black
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;

        // Create loading text
        GameObject loadingTextObj = new GameObject("LoadingText");
        loadingTextObj.transform.SetParent(panel.transform, false);
        loadingText = loadingTextObj.AddComponent<TextMeshProUGUI>();
        loadingText.text = "Loading...";
        loadingText.fontSize = 36;
        loadingText.alignment = TextAlignmentOptions.Center;
        RectTransform loadingTextRect = loadingTextObj.GetComponent<RectTransform>();
        loadingTextRect.anchoredPosition = new Vector2(0, 50);
        loadingTextRect.sizeDelta = new Vector2(400, 50);

        // Create percentage text
        GameObject percentageTextObj = new GameObject("PercentageText");
        percentageTextObj.transform.SetParent(panel.transform, false);
        percentageText = percentageTextObj.AddComponent<TextMeshProUGUI>();
        percentageText.text = "0%";
        percentageText.fontSize = 24;
        percentageText.alignment = TextAlignmentOptions.Center;
        RectTransform percentageTextRect = percentageTextObj.GetComponent<RectTransform>();
        percentageTextRect.anchoredPosition = new Vector2(0, -50);
        percentageTextRect.sizeDelta = new Vector2(100, 30);

        // Create progress slider
        GameObject sliderObj = new GameObject("ProgressSlider");
        sliderObj.transform.SetParent(panel.transform, false);
        progressSlider = sliderObj.AddComponent<Slider>();
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchoredPosition = new Vector2(0, 0);
        sliderRect.sizeDelta = new Vector2(500, 20);

        // Add background to slider
        GameObject background = new GameObject("Background");
        background.transform.SetParent(sliderObj.transform, false);
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);
        RectTransform bgRect = background.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;

        // Add fill area and fill to slider
        GameObject fillArea = new GameObject("Fill Area");
        fillArea.transform.SetParent(sliderObj.transform, false);
        RectTransform fillAreaRect = fillArea.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(5, 0);
        fillAreaRect.offsetMax = new Vector2(-5, 0);

        GameObject fill = new GameObject("Fill");
        fill.transform.SetParent(fillArea.transform, false);
        Image fillImage = fill.AddComponent<Image>();
        fillImage.color = new Color(0, 0.7f, 1f);
        RectTransform fillRect = fill.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = Vector2.zero;

        // Setup slider
        progressSlider.fillRect = fillRect;
        progressSlider.targetGraphic = fillImage;
        progressSlider.direction = Slider.Direction.LeftToRight;

        // Create loading icon (optional)
        GameObject loadingIconObj = new GameObject("LoadingIcon");
        loadingIconObj.transform.SetParent(panel.transform, false);
        Image iconImage = loadingIconObj.AddComponent<Image>();
        iconImage.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        loadingIcon = loadingIconObj.GetComponent<RectTransform>();
        loadingIcon.anchoredPosition = new Vector2(0, 120);
        loadingIcon.sizeDelta = new Vector2(50, 50);

        Debug.Log("Loading screen elements created! Adjust positions as needed in the inspector.");
    }
#endif
}