using UnityEngine;
using TMPro;

public class Shapes : Entity, IPooledObject
{
    public TextMeshProUGUI shapeHealthText;
    PlayerControl touchingPlayer;

    float _lastUpdateTime;
    Color startColor;
    Color finalColor;
    PlayerControl playerControl;
    ShapeSpawner shapeSpawner;

    Vector3 startScale;
    Vector3 finalScale;

    void Awake()
    {
        shapeSpawner = ShapeSpawner.Instance;
        playerControl = PlayerControl.Instance;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<PlayerControl>();
            touchingPlayer = player;
            _lastUpdateTime = Time.time;
            player.TakeDamage(new DamageReport(damage, this));
        }

        if (collision.gameObject.tag == "Bullet")
        {
            this.TakeDamage(new DamageReport(playerControl.Damage, this));
            this.GetComponentInChildren<TextMeshProUGUI>().SetText(currentHealth.ToString());
            float healthValue = currentHealth / maxHealth;

            finalColor = Color.Lerp(Color.white, startColor, healthValue);
            this.GetComponent<SpriteRenderer>().color = finalColor;
        }
    }

    void Update()
    {
        if (touchingPlayer != null)
        {
            if (Time.time - _lastUpdateTime >= 1)
            {
                _lastUpdateTime = Time.time;
                touchingPlayer.TakeDamage(new DamageReport(damage, this));
            }
        }
    }

    protected override void Death(Entity killer)
    {
        playerControl.AddPoints();
        touchingPlayer = null;
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        Vector2 force = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().velocity = force;
        damage = shapeSpawner.Damage;
        maxHealth = shapeSpawner.MaxHealth;
        currentHealth = maxHealth;
        shapeHealthText.SetText(currentHealth.ToString());

        startColor = new Color(1, 1 / maxHealth, 1 / maxHealth, 1);
        this.GetComponent<SpriteRenderer>().color = startColor;
    }
}
