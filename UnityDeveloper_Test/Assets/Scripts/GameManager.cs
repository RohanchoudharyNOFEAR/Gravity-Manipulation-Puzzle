using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//SINGLETON THAT MANAGES GAME STATE AND UI
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text TimerText;
    public TMP_Text ScoreText;
    public GameObject WinText;
    public GameObject Instructions;
    public int Score { get { return score; } set { score = value; CheckScore(); } }

    [SerializeField] private float timeRemaining =120; //in Seconds
    private bool timerIsRunning = true;
    private int score = 0;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CountdownTimer();
        TurnInstructionOff();
    }

    // Action called when score changes
    void CheckScore()
    {
        ScoreText.text = "SCORE : " + Score + "/ 5";
        if (Score == 5)
        {
            WinText.SetActive(true);
            Invoke("RestartGame", 2);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void CountdownTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
              
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                RestartGame();
            }
        }
        DisplayTime(timeRemaining);


    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimerText.text = string.Format("Timer : {0:00}:{1:00}", minutes, seconds);
    }

     void TurnInstructionOff()
    {
        if(Input.GetKeyDown(KeyCode.Tab)) 
        {
            Instructions.SetActive(false);
        }
    }

}
