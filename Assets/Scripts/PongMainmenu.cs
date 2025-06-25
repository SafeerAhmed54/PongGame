using TMPro;
using UnityEngine;

public class PongMainmenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerName2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score.text = "0";
        highScore.text = "129";
        playerName.text = "Player 1";
        playerName2.text = "Player 2";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
