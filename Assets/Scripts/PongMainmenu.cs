using TMPro;
using UnityEngine;

public class PongMainmenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
