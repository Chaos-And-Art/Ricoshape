using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    float screenH, screenW;

    protected override void Start()
    {
        base.Start();
        screenH = Camera.main.orthographicSize;
        screenW = screenH * Screen.width / Screen.height;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.transform.position = new Vector3(Screen.width / 2, Screen.height / 8, 90);
        base.OnPointerUp(eventData);
    }
}