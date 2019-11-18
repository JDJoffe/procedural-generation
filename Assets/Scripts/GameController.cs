using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MazeConstructor))]

public class GameController : MonoBehaviour
{
    #region var   
    [SerializeField] private FpsMovement player;
    [SerializeField] private Text timeLabel;
    [SerializeField] private Text scoreLabel;
    private MazeConstructor generator;
    private DateTime startTime;
    private int timeLimit;
    private int reduceLimitBy;
    private int score;
    public int rows = 13;
    public int collumns = 15;
    private bool goalReached;
    #endregion

    void Start()
    {
        // script ref
        generator = GetComponent<MazeConstructor>();
        // cal func
        StartNewGame();
    }

    // set var values
    private void StartNewGame()
    {
        timeLimit = 80;
        reduceLimitBy = 5;
        // for getting the players completion time 
        startTime = DateTime.Now;

        score = 0;
        scoreLabel.text = score.ToString();
        // call func
        StartNewMaze();
    }


    private void StartNewMaze()
    {
        // call func from maze constructor script and input data with parameters from ontrigger
        generator.GenerateNewMaze(rows, collumns, OnStartTrigger, OnGoalTrigger);

        float x = generator.startCol * generator.hallWidth;
        float y = 1;
        float z = generator.startRow * generator.hallWidth;
        player.transform.position = new Vector3(x, y, z);

        goalReached = false;
        player.enabled = true;

        // restart timer
        timeLimit -= reduceLimitBy;
        startTime = DateTime.Now;
    }


    void Update()
    {
        // if player is not enabled dont run the rest
        if (!player.enabled)
        {
            return;
        }

        int timeUsed = (int)(DateTime.Now - startTime).TotalSeconds;
        int timeLeft = timeLimit - timeUsed;
        // if time is above 0 set text to be timeleft.tostring
        if (timeLeft > 0)
        {
            timeLabel.text = timeLeft.ToString();
        }
        // failstate
        else
        {
            timeLabel.text = "TIME UP";
            player.enabled = false;
            // restart game
            Invoke("StartNewGame", 4);
           
        }
    }
    // trigger is the gameobject that is collided with other is the player
    private void OnGoalTrigger(GameObject trigger, GameObject other)
    {
        // goal has been reached
        Debug.Log("Goal!");
        goalReached = true;

        score += 1;
        scoreLabel.text = score.ToString();

        Destroy(trigger);
    }
    // trigger is the gameobject that is collided with other is the player
    private void OnStartTrigger(GameObject trigger, GameObject other)
    {
        // if the goal has been triggered, win when you touch the start trigger again
        if (goalReached)
        {
            Debug.Log("Finish!");
            player.enabled = false;

            Invoke("StartNewMaze", 4);
        }
    }
}
