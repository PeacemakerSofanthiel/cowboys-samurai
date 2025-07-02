using UnityEngine;

public abstract class DamageableEntity : MonoBehaviour
{
    public int maxHP = 10;
    public int currentHP;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"{name} took {amount} damage. HP = {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        Debug.Log($"{name} died.");
        Destroy(gameObject);
    }
}
