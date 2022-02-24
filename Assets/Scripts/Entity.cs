using UnityEngine;

public abstract class Entity : MonoBehaviour
{  
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;

    public float Health
    {
        get { return currentHealth; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
    }
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }
    public float Damage
    {
        get { return damage; }
    }

    public bool TakeDamage(DamageReport damageReport)
    {
        currentHealth -= damageReport.damage;
        if (currentHealth <= 0)
        {
            Death(damageReport.attacker);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Execute on Entity death
    /// </summary>
    /// <param name="killer">Reference to killer</param>
    protected abstract void Death(Entity killer);
}
