using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : Entity
{
    float lastSpawnTime;
    public float spawnsPerSecond;
    public float bulletHealth;
    public bool okayToSpawn;

    [Space(10)]
    public Transform player;
    bool activeObjects;

    GameObject square, diamond, hexagon, triangle;
    ObjectPooler objectPooler;
    Shapes shapesControl;
    GameLogic gameLogic;
    List<GameObject> ShapesList = new List<GameObject>();

    #region BasicSingleton
    public static ShapeSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        gameLogic = GameLogic.Instance;
        okayToSpawn = true;

        //if Gamemode 1, start with these values, other wise reset them to default
        bulletHealth = PlayerPrefs.GetFloat("BulletHits", 2);
    }

    void FixedUpdate()
    {
        if (okayToSpawn)
        {
            if (Time.time - lastSpawnTime >= (1 / spawnsPerSecond))
            {
                lastSpawnTime = Time.time;
                int randomShape = Random.Range(1, 5);
                SpawnObject(randomShape);
            }
        }

        GameObject[] shapeAmount = GameObject.FindGameObjectsWithTag("Shape");


        foreach (GameObject shapes in ShapesList)
        {
            Vector2 direction = (new Vector2(shapes.transform.position.x - player.position.x, shapes.transform.position.y - player.position.y)).normalized;
            Rigidbody2D rb2d = shapes.GetComponent<Rigidbody2D>();

            rb2d.AddForce(-direction * attackSpeed, ForceMode2D.Impulse);

            if (shapes.activeInHierarchy)
            {
                gameLogic.objectsInactive = false;
            }
            else if (!shapes.activeInHierarchy && shapeAmount.Length == 0)
            {
                gameLogic.objectsInactive = true;
            }


            // float angle = Mathf.Atan2(player.position.y - shapes.transform.position.y, player.position.x - shapes.transform.position.x) * Mathf.Rad2Deg + 90;
            // Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            // shapes.transform.rotation = Quaternion.RotateTowards(shapes.transform.rotation, targetRotation, 1);
        }
    }

    void SpawnObject(int randShape)
    {
        float screenH = Camera.main.orthographicSize;
        float screenW = screenH * Screen.width / Screen.height;
        Vector3 position = Random.insideUnitCircle.normalized * Random.Range(4, 4);

        if (position.x > 2.8f)
        {
            position.x = 2.8f;
        }
        else if (position.x > -2.8f)
        {
            position.x = -2.8f;
        }

        switch (randShape)
        {
            case 1:
                square = objectPooler.SpawnFromPool("Square", position, Quaternion.identity);
                break;
            case 2:
                diamond = objectPooler.SpawnFromPool("Diamond", position, Quaternion.identity);
                break;
            case 3:
                hexagon = objectPooler.SpawnFromPool("Hexagon", position, Quaternion.identity);
                break;
            case 4:
                triangle = objectPooler.SpawnFromPool("Triangle", position, Quaternion.identity);
                break;
            default:
                square = objectPooler.SpawnFromPool("Square", position, Quaternion.identity);
                break;
        }

        if (!ShapesList.Contains(square) && square != null)
        {
            ShapesList.Add(square);
        }
        if (!ShapesList.Contains(diamond) && diamond != null)
        {
            ShapesList.Add(diamond);
        }
        if (!ShapesList.Contains(hexagon) && hexagon != null)
        {
            ShapesList.Add(hexagon);
        }
        if (!ShapesList.Contains(triangle) && triangle != null)
        {
            ShapesList.Add(triangle);
        }

    }

    protected override void Death(Entity killer)
    {
        //Not Applicable
    }

    public void Reset()
    {
        foreach (GameObject shapes in ShapesList)
        {
            shapes.SetActive(false);
        }
        bulletHealth = PlayerPrefs.GetFloat("BulletHits", 3);
    }

    public void setShapeValues(float value)
    {
        maxHealth = value;
        damage = Mathf.Sqrt(value) % Mathf.Sqrt(value + 1);
        attackSpeed = (Mathf.Sqrt(value) % Mathf.Sqrt(value + 1) + 3);
        // if (value <= 2)
        // {
        //     attackSpeed = 4f;
        // }
        // else
        // {
        //     attackSpeed = (Mathf.Sqrt(value) % Mathf.Sqrt(value + 4)) / 2.8f;
        // }
        spawnsPerSecond = (Mathf.Sqrt(value) % Mathf.Sqrt(value + 1)) / 3;
    }



    public bool devActivated;


    public void devControlBulletHealth(float value)
    {
        devActivated = true;
        bulletHealth = value;
    }

    public void devControlHealth(float value)
    {
        devActivated = true;
        // setShapeValues(value);
        maxHealth = value;
    }

    public void devControlDamage(float value)
    {
        devActivated = true;
        damage = value;
    }

    public void devControlAttackSpeed(float value)
    {
        devActivated = true;
        attackSpeed = value;
    }

    public void devControlSpawnRate(float value)
    {
        devActivated = true;
        spawnsPerSecond = value;
    }
}
