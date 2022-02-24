using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Transform upgradeButton;
    Vector3 uBStartScale;

    void Start()
    {
        uBStartScale = upgradeButton.localScale;
    }

    void Update()
    {
        upgradeButton.localScale = uBStartScale * (1 + (0.08f * Mathf.Sin(Time.time * 2)));
    }
}
