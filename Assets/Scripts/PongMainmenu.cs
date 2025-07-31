using TMPro;
using UnityEngine;

public class PongMainmenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerName2;
    [SerializeField] private TextMeshProUGUI playerName3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Debug.Log("Awake called: Initializing PongMainmenu");
        Debug.Log("Asalam-o-Alikum Safeer here.");
    }
    void Start()
    {
        score.text = "0";
        highScore.text = "129";
        playerName.text = "Player 1";
        playerName2.text = "Player 2";
        playerName3.text = "Player 3";

        Player player1 = new Player("Player 1");
        Player player2 = new Player("Player 2");
        Player player3 = new Player("Player 3");
        player1.UpdateScore(10);
    }

    enum PlayerType
    {
        Player1,
        Player2,
        Player3
    }

    // Update is called once per frame
    void Update()
    {
        switch(true)
        {
            case true:
                PlayerType playerType1 = PlayerType.Player1;
                break;
            case false:
               PlayerType playerType2 = PlayerType.Player2;
                break;
            default:
                break;
        }
    }

    public void Move()
    {
        decimal speed = 0.5m;
        if (speed > 0)
        {
            Debug.Log("Moving at speed: " + speed);
        }
        else
        {
            Debug.Log("Speed is zero or negative, not moving.");
        }
    }
}

public class Player
{
    public string Name { get; set; }
    public int Score { get; set; }
    public Player(string name)
    {
        Name = name;
        Score = 0;
    }

    public Player(string name, int score)
    {
        Name = name;
        Score = score;
    }

    public void UpdateScore(int points)
    {
        Score += points;
        Debug.Log($"{Name}'s score updated to: {Score}");
    }

    private void Update()
    {
        Debug.Log("Player Update called: " + Name);
    }

    protected void OnDestroy()
    {
        Debug.Log("Player object destroyed: " + Name);
    }
}
