using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : BattleEntity, IBattleTickable
{
    public int maxHP = 10;
    private int currentHP;

    public Vector3Int startingCell;

    protected override void Start()
    {
        base.Start();
        currentHP = maxHP;
        SnapToGrid(startingCell);
        BattleManager.Instance.Register(this);
    }

    public override void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Enemy took {amount} damage. HP = {currentHP}");
        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    public void OnBattleTick()
    {
        // This will be called by BattleManager every tick (e.g., every 0.1s)
        // It makes the enemy move, fire, or blink based on behavior
        Debug.Log("Enemy tick update");
    }
}
