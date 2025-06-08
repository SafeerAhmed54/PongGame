using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private int scoreToWin = 11;

    [Header("Game Objects")]
    [SerializeField] private GameObject player2Object;
    [SerializeField] private GameObject aiObject;
    [SerializeField] private GameObject settingsPanel;

    private int player1Score = 0;
    private int player2Score = 0;
    private bool gameStarted = false;

    void Start()
    {
        // Hide game over panel at start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Show settings panel at start
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }

        // Set game mode based on player preference
        if (GetGameModeInt() == 1) // AI Mode
        {
            player2Object.SetActive(false);
            aiObject.SetActive(true);
        }
        else // 1v1 Mode
        {
            player2Object.SetActive(true);
            aiObject.SetActive(false);
        }

        // Initialize score text
        UpdateScoreUI();
    }

    public void Mainmenu()
    {
        // Load the main menu scene
        LoadingScreenManager.Instance.LoadScene(0);
    }

    public void StartGame()
    {
        // Called when settings are confirmed
        gameStarted = true;

        // Make sure the game is running at normal speed
        Time.timeScale = 1;
    }

    public void SetScoreToWin(int score)
    {
        scoreToWin = score;
    }

    public void AddScore(bool isPlayer1)
    {
        if (isPlayer1)
        {
            player1Score++;
        }
        else
        {
            player2Score++;
        }
        UpdateScoreUI();

        // Check if game is over
        CheckForGameOver();
    }

    void UpdateScoreUI()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = player1Score.ToString();
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = player2Score.ToString();
        }
    }

    void CheckForGameOver()
    {
        if (player1Score >= scoreToWin || player2Score >= scoreToWin)
        {
            // Determine winner
            string winner = player1Score > player2Score ? "Player 1" : "Player 2";

            // Show game over UI
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                if (winnerText != null)
                {
                    winnerText.text = winner + " Wins!";
                }
            }

            // Pause the game
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        // Reset time scale
        Time.timeScale = 1;

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int GetGameModeInt()
    {
        if (PlayerPrefs.HasKey("GameMode"))
        {
            int gameMode = PlayerPrefs.GetInt("GameMode", 0);
            return gameMode;
        }
        else
        {
            int gameMode = 1;
            return gameMode;
        }
    }

    public void QuitGame()
    {
        // In editor, this just stops play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}