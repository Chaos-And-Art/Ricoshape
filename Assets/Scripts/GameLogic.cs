using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic : MonoBehaviour
{
    public TextMeshProUGUI playingLevelText;
    public TextMeshProUGUI overLevelText;
    public TextMeshProUGUI overHighestLevelText;
    public TextMeshProUGUI overPointsText;
    [Space(10)]
    public GameObject countdownBackground;
    public TextMeshProUGUI countdown;

    int gameLevel = 1;
    int increaseTime = 0;
    public int levelTime;
    public bool objectsInactive, isPaused = false;
    bool timeMet;

    PlayerControl playerControl;
    ShapeSpawner shapeSpawner;

    #region BasicSingleton
    public static GameLogic Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        playerControl = PlayerControl.Instance;
        shapeSpawner = ShapeSpawner.Instance;
        playingLevelText.SetText(gameLevel.ToString());
    }


    public IEnumerator StartPlaying()
    {
        if (gameLevel <= 5)
        {
            levelTime = 10;
        }
        else if (gameLevel > 5 && gameLevel <= 10)
        {
            levelTime = 15;
        }
        else if (gameLevel > 10)
        {
            levelTime = 25;
        }

        if (increaseTime < levelTime && !isPaused)
        {
            yield return new WaitForSecondsRealtime(1);
            increaseTime++;
            // print(increaseTime);
            StartCoroutine(StartPlaying());
        }
        else if (increaseTime <= levelTime && isPaused)
        {
            //Stop Courtine
        }
        else
        {
            shapeSpawner.okayToSpawn = false;
            timeMet = true;
        }
    }


    void Update()
    {
        if (objectsInactive == true && timeMet == true)
        {
            objectsInactive = false;
            timeMet = false;
            setUpNextLevel();
        }
        // when the player reaches a certain amount of upgrades, create buy option to skip first few levels
    }

    void setUpNextLevel()
    {
        StartCoroutine(StartCountdown());
        gameLevel += 1;
    }

    public void gameIsOver()
    {
        objectsInactive = false;
        timeMet = false;
        overLevelText.SetText(gameLevel.ToString());
        PlayerPrefs.GetInt("HighLevel", 1);
        if (PlayerPrefs.GetInt("HighLevel") < gameLevel)
        {
            overHighestLevelText.SetText(gameLevel.ToString());
            PlayerPrefs.SetInt("HighLevel", gameLevel);
        }
        else
        {
            overHighestLevelText.SetText(PlayerPrefs.GetInt("HighLevel").ToString());
        }
        if (playerControl.totalPoints != 0)
        {
            overPointsText.SetText(playerControl.totalPoints.ToString());
            PlayerPrefs.SetInt("Points", playerControl.totalPoints);
        }
        else
        {
            overPointsText.SetText(PlayerPrefs.GetInt("Points", 0).ToString());
        }

    }

    public void updatePoints()
    {
        overPointsText.SetText(PlayerPrefs.GetInt("Points", 0).ToString());
    }


    private IEnumerator StartCountdown()
    {
        countdownBackground.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdown.SetText(i.ToString());
            yield return new WaitForSecondsRealtime(1);
        }
        countdownBackground.SetActive(false);
        playingLevelText.SetText(gameLevel.ToString());
        shapeSpawner.setShapeValues(gameLevel);
        shapeSpawner.okayToSpawn = true;
        increaseTime = 0;
        StartCoroutine(StartPlaying());
    }

    public void Restart()
    {
        objectsInactive = false;
        shapeSpawner.okayToSpawn = true;
        timeMet = false;
        gameLevel = 1;
        playingLevelText.SetText(gameLevel.ToString());
        if (shapeSpawner.devActivated == false)
        {
            shapeSpawner.setShapeValues(gameLevel);
        }
        increaseTime = 0;
        StartCoroutine(StartPlaying());
    }
}
