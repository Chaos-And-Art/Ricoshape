using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity, IPooledObject
{
    ShapeSpawner shapeSpawner;
    
    void Awake()
    {   shapeSpawner = ShapeSpawner.Instance;
        currentHealth = maxHealth;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Shape")
        {
            this.TakeDamage(new DamageReport(this.damage, this));
        }
    }

    protected override void Death(Entity killer)
    {
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        Vector2 force = new Vector2(0, 0);
        GetComponent<Rigidbody2D>().velocity = force;
        maxHealth = shapeSpawner.bulletHealth;
        currentHealth = maxHealth;

        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(30);

        gameObject.SetActive(false);
    }

    public void devControl(float value)
    {
        maxHealth = value;
    }
}
