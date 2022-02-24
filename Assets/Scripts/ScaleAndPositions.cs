using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAndPositions : MonoBehaviour
{
    public Camera cam;
    public Transform RightWall;
    public Transform LeftWall;
    public Transform TopWall;
    public Transform BottomWall;
    [Space(10)]
    public Transform Level;
    public Transform Points;
    public Transform Pause;
    public Transform JoystickUI;
    public Transform JoystickBack;
    public Transform JoystickHand;
    [Space(10)]
    public Transform PauseText;
    public Transform ButtonContinue;
    public Transform ButtonQuit;
    public Transform ButtonSettings;
    public Transform ButtonStats;
    [Space(10)]
    public Transform GameTitle;
    public Transform PlayGame;
    public Transform Settings;
    [Space(10)]
    public Transform PlayerHealth;
    public Transform TopCover;

    #region BasicSingleton
    public static ScaleAndPositions Instance;
    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        Instance = this;
    }
    #endregion


    void Start()
    {
        positionElements();
    }

    public void positionElements()
    {
        float screenH = Camera.main.orthographicSize;
        float screenW = screenH * Screen.width / Screen.height;

        RightWall.position = new Vector3(screenW, 0, 0);
        LeftWall.position = new Vector3(-screenW, 0, 0);
        TopWall.position = new Vector3(0, screenH - 0.8f, 0);
        BottomWall.position = new Vector3(0, -screenH, 0);

        Level.position = new Vector3(-screenW + 0.7f, screenH - 0.3f, 90);
        Points.position = new Vector3(0, screenH - 0.55f, 90);
        Pause.position = new Vector3(screenW - 0.4f, screenH - 0.4f, 90);

        RectTransform rtJoystickUI = (RectTransform)JoystickUI.transform;
        rtJoystickUI.sizeDelta = new Vector2(Screen.width, Screen.height - 200);

        float value = PlayerPrefs.GetFloat("ScaleJS", 50);
        RectTransform rtJoystickBack = (RectTransform)JoystickBack.transform;
        rtJoystickBack.sizeDelta = new Vector2((value * (Screen.width * 2)) / 400, (value * Screen.height) / 420);
        JoystickBack.position = new Vector3(Screen.width / 2, Screen.height / 8, 90);
        /////////////////////////////////////////////////////////////////////////////////////
        RectTransform rtJoystickHand = (RectTransform)JoystickHand.transform;
        rtJoystickHand.sizeDelta = new Vector2((value * (Screen.width * 2)) / 1100, (value * Screen.height) / 1150);
        JoystickHand.position = new Vector3(Screen.width / 2, Screen.height / 8, 90);

        PauseText.position = new Vector3(0, screenH - 3, 90);
        ButtonContinue.position = new Vector3(0, screenH - 5, 90);
        ButtonStats.position = new Vector3(0, screenH - 6, 90);
        ButtonQuit.position = new Vector3(ButtonStats.position.x - 1.5f, screenH - 6, 90);
        ButtonSettings.position = new Vector3(ButtonStats.position.x + 1.5f, screenH - 6, 90);

        GameTitle.position = new Vector3(0, screenH - 2, 90);
        PlayGame.position = new Vector3(0, screenH - 5, 90);
        Settings.position = new Vector3(0, screenH - 6, 90);

        PlayerHealth.position = new Vector3(0, screenH - .92f, 90);
        TopCover.position = new Vector3(0, screenH, 90);
        RectTransform rtTopCover = (RectTransform)TopCover.transform;
        rtTopCover.sizeDelta = new Vector2(Screen.width * 2, 215);

    }

}
