using UnityEngine;

public class MainmenuManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartOneVsOneGame()
    {
        PlayerPrefs.SetInt("GameMode", 0); // 0 = 1v1
        PlayerPrefs.Save();
    }

    public void StartOneVsAIGame()
    {
        PlayerPrefs.SetInt("GameMode", 1); // 1 = 1vAI
        PlayerPrefs.Save();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
