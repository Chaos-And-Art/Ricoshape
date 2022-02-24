using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStats : MonoBehaviour
{

    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerSpeed;
    public TextMeshProUGUI bulletHits;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth.SetText(PlayerPrefs.GetFloat("MaxHealth", 10).ToString());
        playerDamage.SetText(PlayerPrefs.GetFloat("Damage", 1).ToString());
        playerSpeed.SetText(PlayerPrefs.GetFloat("AttackSpeed", 1).ToString());
        bulletHits.SetText(PlayerPrefs.GetFloat("BulletHits", 2).ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
