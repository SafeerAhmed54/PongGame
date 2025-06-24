using TMPro;
using UnityEngine;

public class PongMainmenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private TextMeshProUGUI playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score.text = "0";
        highScore.text = "129";
        playerName.text = "Player 1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
