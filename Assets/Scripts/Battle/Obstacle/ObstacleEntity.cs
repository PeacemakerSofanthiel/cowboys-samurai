using UnityEngine;

public class ObstacleEntity : BattleEntity
{
    public bool isDestructible = true;

    protected override void Start()
    {
        base.Start();
        if (isDestructible)
            currentHP = maxHP;
    }

    public override void TakeDamage(int amount)
    {
        if (!isDestructible) return;
        base.TakeDamage(amount);
        Debug.Log($"{gameObject.name} took {amount} damage. HP = {currentHP}");
    }

    protected override void Die()
    {
        Debug.Log($"{gameObject.name} destroyed.");
        Destroy(gameObject);
    }
}
