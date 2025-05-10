using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings Panel")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TMP_InputField scoreInputField;
    [SerializeField] private TMP_Dropdown player1ColorDropdown;
    [SerializeField] private TMP_Dropdown player2ColorDropdown;
    [SerializeField] private Button confirmButton;

    [Header("Game References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject player1Paddle;
    [SerializeField] private GameObject player2Paddle;

    // Color options
    private List<Color> paddleColors = new List<Color>
    {
        Color.white,     // Default
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        Color.cyan,
        Color.magenta
    };

    private void Start()
    {
        // Initialize settings panel
        InitializeSettingsPanel();

        // Pause the game while settings are shown
        Time.timeScale = 0;

        // Add listener for confirm button
        if (confirmButton != null)
        {
            confirmButton.onClick.AddListener(ConfirmSettings);
        }
    }

    private void InitializeSettingsPanel()
    {
        // Initialize score input field with default value
        if (scoreInputField != null)
        {
            scoreInputField.text = "11";
        }

        // Initialize color dropdowns
        if (player1ColorDropdown != null)
        {
            SetupColorDropdown(player1ColorDropdown);
        }

        if (player2ColorDropdown != null)
        {
            SetupColorDropdown(player2ColorDropdown);
        }
    }

    private void SetupColorDropdown(TMP_Dropdown dropdown)
    {
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("White"));
        options.Add(new TMP_Dropdown.OptionData("Red"));
        options.Add(new TMP_Dropdown.OptionData("Blue"));
        options.Add(new TMP_Dropdown.OptionData("Green"));
        options.Add(new TMP_Dropdown.OptionData("Yellow"));
        options.Add(new TMP_Dropdown.OptionData("Cyan"));
        options.Add(new TMP_Dropdown.OptionData("Magenta"));

        dropdown.AddOptions(options);
    }

    public void ConfirmSettings()
    {
        // Apply score setting
        if (scoreInputField != null && gameManager != null)
        {
            int scoreToWin;
            if (int.TryParse(scoreInputField.text, out scoreToWin) && scoreToWin > 0)
            {
                gameManager.SetScoreToWin(scoreToWin);
            }
            else
            {
                // Default to 11 if invalid input
                gameManager.SetScoreToWin(11);
            }
        }

        // Apply paddle colors
        if (player1Paddle != null && player1ColorDropdown != null)
        {
            ApplyPaddleColor(player1Paddle, player1ColorDropdown.value);
        }

        if (player2Paddle != null && player2ColorDropdown != null)
        {
            ApplyPaddleColor(player2Paddle, player2ColorDropdown.value);
        }

        // Hide settings panel and resume game
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void ApplyPaddleColor(GameObject paddle, int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < paddleColors.Count)
        {
            // Apply color to paddle renderer
            Renderer paddleRenderer = paddle.GetComponent<Renderer>();
            if (paddleRenderer != null)
            {
                paddleRenderer.material.color = paddleColors[colorIndex];
            }

            // Also check for sprite renderer for 2D games
            SpriteRenderer spriteRenderer = paddle.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = paddleColors[colorIndex];
            }
        }
    }
}