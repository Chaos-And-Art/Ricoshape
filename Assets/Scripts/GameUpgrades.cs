using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUpgrades : MonoBehaviour
{
    public TextMeshProUGUI totalPoints;
    [Space(10)]
    public TextMeshProUGUI healthCost;
    public TextMeshProUGUI damageCost;
    public TextMeshProUGUI speedCost;
    public TextMeshProUGUI hitsCost;
    [Space(10)]
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerSpeed;
    public TextMeshProUGUI bulletHits;
    [Space(10)]
    public GameObject confirmHealthPurchase;
    public TextMeshProUGUI healthDesc;
    public GameObject confirmDamagePurchase;
    public TextMeshProUGUI damageDesc;
    public GameObject confirmSpeedPurchase;
    public TextMeshProUGUI speedDesc;
    public GameObject confirmHitsPurchase;
    public TextMeshProUGUI hitsDesc;
    public GameObject noPoints;


    public int pointsTotal, pointsAfterCost;
    int costAfterHealth, costAfterDamage, costAfterSpeed, costAfterHits;

    int costHealth, costDamage, costSpeed, costHits;
    int healthUpgrades, damageUpgrades, speedUpgrades, hitsUpgrades;
    float pHealth, pDamage, pSpeed, bHits;

    string valueH, outputH, valueD, outputD, valueS, outputS, valueHits, outputHits;

    PlayerControl playerControl;
    ShapeSpawner shapeSpawner;

    #region BasicSingleton
    public static GameUpgrades Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        playerControl = PlayerControl.Instance;
        shapeSpawner = ShapeSpawner.Instance;

        pointsTotal = PlayerPrefs.GetInt("Points", 0);

        healthCost.SetText(PlayerPrefs.GetInt("HealthCost", 25).ToString());
        damageCost.SetText(PlayerPrefs.GetInt("DamageCost", 25).ToString());
        speedCost.SetText(PlayerPrefs.GetInt("SpeedCost", 25).ToString());
        hitsCost.SetText(PlayerPrefs.GetInt("HitsCost", 100).ToString());

        playerHealth.SetText(PlayerPrefs.GetFloat("MaxHealth", 10).ToString());
        playerDamage.SetText(PlayerPrefs.GetFloat("Damage", 1).ToString());
        if (PlayerPrefs.GetFloat("AttackSpeed") == 1.33f)
        {
            playerSpeed.SetText(2.ToString());
        }
        else if (PlayerPrefs.GetFloat("AttackSpeed") == 1.66f)
        {
            playerSpeed.SetText(3.ToString());
        }
        else
        {
            playerSpeed.SetText(PlayerPrefs.GetFloat("pSpeed", 1).ToString());
        }
        bulletHits.SetText(PlayerPrefs.GetFloat("BulletHits", 2).ToString());

        pHealth = PlayerPrefs.GetFloat("MaxHealth", 10);
        pDamage = PlayerPrefs.GetFloat("Damage", 1);
        pSpeed = PlayerPrefs.GetFloat("AttackSpeed", 1);
        bHits = PlayerPrefs.GetFloat("BulletHits", 2);

        valueH = healthDesc.text;
        outputH = valueH.Replace("-24", PlayerPrefs.GetInt("HealthCost", 25).ToString());
        healthDesc.SetText(outputH);

        valueD = damageDesc.text;
        outputD = valueD.Replace("-24", PlayerPrefs.GetInt("DamageCost", 25).ToString());
        damageDesc.SetText(outputD);

        valueS = speedDesc.text;
        outputS = valueS.Replace("-24", PlayerPrefs.GetInt("SpeedCost", 25).ToString());
        speedDesc.SetText(outputS);

        valueHits = hitsDesc.text;
        outputHits = valueHits.Replace("-24", PlayerPrefs.GetInt("HitsCost", 100).ToString());
        hitsDesc.SetText(outputHits);
    }

    public void gameOver()
    {
        // Keep Upgrades if gamemode 1, reset if Gamemode 2
        pointsTotal = playerControl.totalPoints;
        totalPoints.SetText(pointsTotal.ToString());
    }

    private IEnumerator notEnoughPoints()
    {
        noPoints.SetActive(true);
        yield return new WaitForSeconds(2);
        noPoints.SetActive(false);
    }

    public void upgradeHealth()
    {
        healthUpgrades = PlayerPrefs.GetInt("healthUpgrades", 1);
        costHealth = healthUpgrades * 25;
        if (pointsTotal >= costHealth)
        {
            confirmHealthPurchase.SetActive(true);
        }
        else
        {
            StartCoroutine(notEnoughPoints());
        }
    }

    public void upgradeDamage()
    {
        damageUpgrades = PlayerPrefs.GetInt("damageUpgrades", 1);
        costDamage = damageUpgrades * 25;
        if (pointsTotal >= costDamage)
        {
            confirmDamagePurchase.SetActive(true);
        }
        else
        {
            StartCoroutine(notEnoughPoints());
        }
    }

    public void upgradeSpeed()
    {
        speedUpgrades = PlayerPrefs.GetInt("speedUpgrades", 1);
        costSpeed = speedUpgrades * 25;
        if (pointsTotal >= costSpeed)
        {
            confirmSpeedPurchase.SetActive(true);
        }
        else
        {
            StartCoroutine(notEnoughPoints());
        }
    }

    public void upgradeHits()
    {
        hitsUpgrades = PlayerPrefs.GetInt("hitsUpgrades", 2);
        costHits = hitsUpgrades * 50;
        if (pointsTotal >= costHits)
        {
            confirmHitsPurchase.SetActive(true);
        }
        else
        {
            StartCoroutine(notEnoughPoints());
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////

    private IEnumerator subtractPoints()
    {
        for (int i = pointsTotal; i + 1 > pointsAfterCost; i--)
        {
            totalPoints.SetText(i.ToString());
            yield return new WaitForSeconds(.05f / i);
        }
    }

    public void healthPurchase()
    {
        pointsAfterCost = pointsTotal - costHealth;
        PlayerPrefs.SetInt("Points", pointsAfterCost);
        StartCoroutine(subtractPoints());

        pHealth = (healthUpgrades + 1) * 10;
        PlayerPrefs.SetFloat("MaxHealth", pHealth);
        playerHealth.SetText(pHealth.ToString());

        healthUpgrades++;
        PlayerPrefs.SetInt("healthUpgrades", healthUpgrades);
        costAfterHealth = healthUpgrades * 25;
        PlayerPrefs.SetInt("HealthCost", costAfterHealth);
        StartCoroutine(increaseHealthCost());

        outputH = valueH.Replace("-24", costAfterHealth.ToString());
        healthDesc.SetText(outputH);

        confirmHealthPurchase.SetActive(false);
        pointsTotal = PlayerPrefs.GetInt("Points", 0);
    }

    private IEnumerator increaseHealthCost()
    {
        for (int i = costHealth; i < costAfterHealth + 1; i++)
        {
            healthCost.SetText(i.ToString());
            yield return new WaitForSeconds(.05f / i);
        }
    }

    public void damagePurchase()
    {
        pointsAfterCost = pointsTotal - costDamage;
        PlayerPrefs.SetInt("Points", pointsAfterCost);
        StartCoroutine(subtractPoints());

        pDamage = (damageUpgrades + 1) * 1;
        PlayerPrefs.SetFloat("Damage", pDamage);
        playerDamage.SetText(pDamage.ToString());

        damageUpgrades++;
        PlayerPrefs.SetInt("damageUpgrades", damageUpgrades);
        costAfterDamage = damageUpgrades * 25;
        PlayerPrefs.SetInt("DamageCost", costAfterDamage);
        StartCoroutine(increaseDamageCost());

        outputD = valueD.Replace("-24", costAfterDamage.ToString());
        damageDesc.SetText(outputD);

        confirmDamagePurchase.SetActive(false);
        pointsTotal = PlayerPrefs.GetInt("Points", 0);
    }

    private IEnumerator increaseDamageCost()
    {
        for (int i = costDamage; i < costAfterDamage + 1; i++)
        {
            damageCost.SetText(i.ToString());
            yield return new WaitForSeconds(.05f / i);
        }
    }

    public void speedPurchase()
    {
        pointsAfterCost = pointsTotal - costSpeed;
        PlayerPrefs.SetInt("Points", pointsAfterCost);
        StartCoroutine(subtractPoints());

        pSpeed = (speedUpgrades + 1) * 1;
        float trueSpeed;
        if (pSpeed == 2)
        {
            trueSpeed = 1.33f;
        }
        else if (pSpeed == 3)
        {
            trueSpeed = 1.66f;
        }
        else
        {
            trueSpeed = pSpeed / 2;
        }
        PlayerPrefs.SetFloat("AttackSpeed", trueSpeed);
        PlayerPrefs.SetFloat("pSpeed", pSpeed);
        playerSpeed.SetText(pSpeed.ToString());

        speedUpgrades++;
        PlayerPrefs.SetInt("speedUpgrades", speedUpgrades);
        costAfterSpeed = speedUpgrades * 25;
        PlayerPrefs.SetInt("SpeedCost", costAfterSpeed);
        StartCoroutine(increaseSpeedCost());

        outputS = valueS.Replace("-24", costAfterSpeed.ToString());
        speedDesc.SetText(outputS);

        confirmSpeedPurchase.SetActive(false);
        pointsTotal = PlayerPrefs.GetInt("Points", 0);
    }

    private IEnumerator increaseSpeedCost()
    {
        for (int i = costSpeed; i < costAfterSpeed + 1; i++)
        {
            speedCost.SetText(i.ToString());
            yield return new WaitForSeconds(.05f / i);
        }
    }

    public void hitsPurchase()
    {
        pointsAfterCost = pointsTotal - costHits;
        PlayerPrefs.SetInt("Points", pointsAfterCost);
        StartCoroutine(subtractPoints());

        bHits = (hitsUpgrades + 1) * 1;
        PlayerPrefs.SetFloat("BulletHits", bHits);
        bulletHits.SetText(bHits.ToString());

        hitsUpgrades++;
        PlayerPrefs.SetInt("hitsUpgrades", hitsUpgrades);
        costAfterHits = hitsUpgrades * 50;
        PlayerPrefs.SetInt("HitsCost", costAfterHits);
        StartCoroutine(increaseHitsCost());

        outputHits = valueHits.Replace("-24", costAfterHits.ToString());
        hitsDesc.SetText(outputHits);

        confirmHitsPurchase.SetActive(false);
        pointsTotal = PlayerPrefs.GetInt("Points", 0);
    }

    private IEnumerator increaseHitsCost()
    {
        for (int i = costHits; i < costAfterHits + 1; i++)
        {
            hitsCost.SetText(i.ToString());
            yield return new WaitForSeconds(.05f / i);
        }
    }

    public void noPurchase()
    {
        confirmHealthPurchase.SetActive(false);
        confirmDamagePurchase.SetActive(false);
        confirmSpeedPurchase.SetActive(false);
        confirmHitsPurchase.SetActive(false);
    }
}
