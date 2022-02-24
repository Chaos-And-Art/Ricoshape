using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameNav : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject gameUpgradesMenu;
    public GameObject devMenu;
    public GameObject settingsMenu;
    public TextMeshProUGUI inputText;
    public GameObject scaleJoystickMenu;
    public GameObject statsMenu;
    [Space(10)]
    public GameObject gameModeInfo1;
    public GameObject gameModeInfo2;
    public GameObject playGame;
    public GameObject JoyStickUI;
    public GameObject ScaleJoystick;
    public Transform JoystickBack;
    public Transform JoystickHand;
    [Space(10)]
    public GameObject player;
    public GameObject shapeSpawner;
    [Space(10)]
    public GameObject upgradesMenu;
    public GameObject confirmReset;

    ShapeSpawner shapeSpawn;
    PlayerControl playerControl;
    GameLogic gameLogic;
    GameUpgrades gameUpgrades;
    ScaleAndPositions scaleAndPositions;
    bool mainWasActive = false;

    #region BasicSingleton
    public static GameNav Instance;
    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        Instance = this;
    }
    #endregion

    void Start()
    {
        shapeSpawn = ShapeSpawner.Instance;
        playerControl = PlayerControl.Instance;
        gameLogic = GameLogic.Instance;
        gameUpgrades = GameUpgrades.Instance;
        scaleAndPositions = ScaleAndPositions.Instance;
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        playGame.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameUpgradesMenu.SetActive(false);
        devMenu.SetActive(false);
        JoyStickUI.SetActive(false);
        ScaleJoystick.SetActive(false);
        player.SetActive(false);
        shapeSpawner.SetActive(false);
    }

    public void startGame()
    {
        gameLogic.Restart();
        shapeSpawn.Reset();
        playerControl.Reset();
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        playGame.SetActive(true);
        shapeSpawner.SetActive(true);
        if (PlayerPrefs.GetString("Input", "Joy") == "Touch")
        {
            JoyStickUI.SetActive(false);
        }
        else if (PlayerPrefs.GetString("Input", "Joy") == "Joy")
        {
            JoyStickUI.SetActive(true);
        }
        player.SetActive(true);
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        gameLogic.isPaused = true;
        pauseMenu.SetActive(true);
        shapeSpawner.SetActive(false);
        JoyStickUI.SetActive(false);
    }

    public void continueGame()
    {
        Time.timeScale = 1;
        gameLogic.isPaused = false;
        gameLogic.StartCoroutine(gameLogic.StartPlaying());
        pauseMenu.SetActive(false);
        shapeSpawner.SetActive(true);
        if (PlayerPrefs.GetString("Input", "Joy") == "Touch")
        {
            JoyStickUI.SetActive(false);
        }
        else if (PlayerPrefs.GetString("Input", "Joy") == "Joy")
        {
            JoyStickUI.SetActive(true);
        }
    }

    public void gameOver()
    {
        // Time.timeScale = 0;
        player.SetActive(false);
        gameOverMenu.SetActive(true);
        JoyStickUI.SetActive(false);
    }

    public void playAgain()
    {
        gameLogic.Restart();
        shapeSpawn.Reset();
        playerControl.Reset();
        mainMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        gameMenu.SetActive(true);
        playGame.SetActive(true);
        JoyStickUI.SetActive(true);
        player.SetActive(true);
        shapeSpawner.SetActive(true);
        Time.timeScale = 1;
    }

    public void quitGame()
    {
        shapeSpawn.Reset();
        playerControl.Reset();
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        playGame.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        devMenu.SetActive(false);
        JoyStickUI.SetActive(false);
        player.SetActive(false);
        shapeSpawner.SetActive(false);
        Time.timeScale = 1;
    }

    public void devOptionsMenu()
    {
        shapeSpawn.Reset();
        playerControl.Reset();
        devMenu.SetActive(true);
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
        playGame.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        JoyStickUI.SetActive(false);
        player.SetActive(false);
        Time.timeScale = 1;
    }

    public void openGameUpgrades()
    {
        upgradesMenu.SetActive(true);
        gameUpgrades.pointsTotal = PlayerPrefs.GetInt("Points", 0);
        gameUpgrades.totalPoints.SetText(gameUpgrades.pointsTotal.ToString());
    }

    public void exitGameUpgrades()
    {
        upgradesMenu.SetActive(false);
        gameLogic.updatePoints();
        playerControl.totalPoints = PlayerPrefs.GetInt("Points", 0);
        playerControl.pointsText.SetText(playerControl.totalPoints.ToString());
    }

    public void resetUpgrades()
    {
        confirmReset.SetActive(true);
    }

    public void yesReset()
    {
        PlayerPrefs.DeleteAll();
        confirmReset.SetActive(false);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void noReset()
    {
        confirmReset.SetActive(false);
    }

    public void Settings()
    {
        settingsMenu.SetActive(true);
        if(mainMenu.activeInHierarchy){
            mainMenu.SetActive(false);
            mainWasActive = true;
        }
    }

    public void exitSettings()
    {
        settingsMenu.SetActive(false);
        if(mainWasActive){
            mainMenu.SetActive(true);
            mainWasActive = false;
        }
    }

    public void Input()
    {
        if (PlayerPrefs.GetString("Input", "Joy") == "Joy")
        {
            PlayerPrefs.SetString("Input", "Touch");
            inputText.SetText("Touch");
        }
        else if (PlayerPrefs.GetString("Input", "Joy") == "Touch")
        {
            PlayerPrefs.SetString("Input", "Joy");
            inputText.SetText("Joystick");
        }
    }

    public void scaleJoystick()
    {
        float value = PlayerPrefs.GetFloat("ScaleJS", 50);
        scaleJoystickMenu.SetActive(true);
        JoystickBack.position = new Vector3(Screen.width / 2, Screen.height / 4, 90);
        JoystickHand.position = new Vector3(Screen.width / 2, Screen.height / 4, 90);
        ScaleJoystick.SetActive(true);
    }

    public void exitScaling()
    {
        scaleAndPositions.positionElements();
        scaleJoystickMenu.SetActive(false);
        ScaleJoystick.SetActive(false);

    }

    public void Scaling(float value)
    {
        RectTransform rtJoystickBack = (RectTransform)JoystickBack.transform;
        rtJoystickBack.sizeDelta = new Vector2((value * (Screen.width * 2)) / 400, (value * Screen.height) / 420);
        /////////////////////////////////////////////////////////////////////////////////////
        RectTransform rtJoystickHand = (RectTransform)JoystickHand.transform;
        rtJoystickHand.sizeDelta = new Vector2((value * (Screen.width * 2)) / 1100, (value * Screen.height) / 1150);

        PlayerPrefs.SetFloat("ScaleJS", value);
    }

    ////////////////////////////////////////////////////////////////////////////

    public void Stats()
    {
        playerControl.playerHealth.SetText(PlayerPrefs.GetFloat("MaxHealth", 10).ToString());
        playerControl.playerDamage.SetText(PlayerPrefs.GetFloat("Damage", 1).ToString());
        if (PlayerPrefs.GetFloat("AttackSpeed") == 1.33f)
        {
            playerControl.playerSpeed.SetText(2.ToString());
        }
        else if (PlayerPrefs.GetFloat("AttackSpeed") == 1.66f)
        {
            playerControl.playerSpeed.SetText(3.ToString());
        }
        else
        {
            playerControl.playerSpeed.SetText(PlayerPrefs.GetFloat("pSpeed", 1).ToString());
        }
        playerControl.bulletHits.SetText(PlayerPrefs.GetFloat("BulletHits", 2).ToString());
        statsMenu.SetActive(true);
    }

    public void exitStats()
    {
        statsMenu.SetActive(false);
    }

    /////////////////////////////

    public void gameMode1Info()
    {
        gameModeInfo1.SetActive(true);
        StartCoroutine(infoInactive());
    }

    public void gameMode2Info()
    {
        gameModeInfo2.SetActive(true);
        StartCoroutine(infoInactive());
    }

    IEnumerator infoInactive()
    {
        yield return new WaitForSeconds(4);

        gameModeInfo1.SetActive(false);
        gameModeInfo2.SetActive(false);
    }

}
