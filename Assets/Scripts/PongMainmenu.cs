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
