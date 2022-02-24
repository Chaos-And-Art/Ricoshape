using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : Entity
{
    [Space(10)]
    public Transform player;
    public Transform firepoint;
    public GameObject bulletPrefab;
    public Joystick joystick;
    [Space(10)]
    public TextMeshProUGUI playerHealth;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerSpeed;
    public TextMeshProUGUI bulletHits;
    public TextMeshProUGUI pointsText;

    int deathStop = 0;
    public int totalPoints;
    List<GameObject> BulletList = new List<GameObject>();
    float lastShootTime;
    float angle;
    ObjectPooler objectPooler;
    GameNav gameNav;
    GameLogic gameLogic;
    GameUpgrades gameUpgrades;

    #region BasicSingleton
    public static PlayerControl Instance;
    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
    }
    #endregion

    void Start()
    {
        player.transform.rotation = Quaternion.identity;
        objectPooler = ObjectPooler.Instance;
        gameNav = GameNav.Instance;
        gameLogic = GameLogic.Instance;
        gameUpgrades = GameUpgrades.Instance;

        pointsText.SetText(PlayerPrefs.GetInt("Points", 0).ToString());

        //if Gamemode 1, start with these values, other wise reset them to default
        maxHealth = PlayerPrefs.GetFloat("MaxHealth", 10);
        damage = PlayerPrefs.GetFloat("Damage", 1);
        attackSpeed = PlayerPrefs.GetFloat("AttackSpeed", 1);
    }

    protected override void Death(Entity killer)
    {
        deathStop++;
        if (deathStop == 1)
        {
            gameNav.gameOver();
            gameLogic.gameIsOver();
            gameUpgrades.gameOver();
        }
    }

    void Update()
    {
        if (PlayerPrefs.GetString("Input", "Joy") == "Touch")
        {
            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = mousePos - transform.position;
                var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

                if (Time.time - lastShootTime >= (1 / attackSpeed))
                {
                    lastShootTime = Time.time;
                    Shoot(new DamageReport(damage, this));
                }

            }
        }

        ///////////////////////////////////////////////

        if (PlayerPrefs.GetString("Input", "Joy") == "Joy")
        {
            if (joystick.pressed)
            {
                angle = (Mathf.Atan2(joystick.Vertical, joystick.Horizontal) * Mathf.Rad2Deg) - 90;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                if (Time.time - lastShootTime >= (1 / attackSpeed))
                {
                    lastShootTime = Time.time;
                    Shoot(new DamageReport(damage, this));
                }
            }
            else
            {
                player.transform.rotation = transform.rotation;
            }
        }

    }

    void Shoot(DamageReport damageReport)
    {
        GameObject bullet = objectPooler.SpawnFromPool("Bullet", firepoint.position, Quaternion.identity);
        Rigidbody2D rb2d = bullet.GetComponent<Rigidbody2D>();

        rb2d.AddForce(firepoint.up * attackSpeed, ForceMode2D.Impulse);

        if (!BulletList.Contains(bullet))
            BulletList.Add(bullet);
    }

    public void Reset()
    { //when player dies or goes to the next level, they will have a chance to spend their points on upgrades
        //points will minus when they spend them, and will be set accordinging. FOr Now I just set it back to 0 when player dies
        joystick.pressed = false;
        joystick.background.position = new Vector2(Screen.width / 2, Screen.height / 8);
        joystick.handle.position = new Vector2(Screen.width / 2, Screen.height / 8);
        player.transform.rotation = Quaternion.identity;
        deathStop = 0;
        pointsText.SetText(PlayerPrefs.GetInt("Points", 0).ToString());
        maxHealth = PlayerPrefs.GetFloat("MaxHealth", 10);
        damage = PlayerPrefs.GetFloat("Damage", 1);
        attackSpeed = PlayerPrefs.GetFloat("AttackSpeed", 1);
        currentHealth = maxHealth;
        foreach (GameObject bullets in BulletList)
        {
            bullets.SetActive(false);
        }
    }

    public void AddPoints()
    {
        totalPoints = PlayerPrefs.GetInt("Points", 0) + 1;
        pointsText.SetText(totalPoints.ToString());
        PlayerPrefs.SetInt("Points", totalPoints);
    }

    public void devControlHealth(float value)
    {
        maxHealth = value;
    }

    public void devControlDamage(float value)
    {
        damage = value;
    }

    public void devControlAttackSpeed(float value)
    {
        attackSpeed = value;
    }

}
