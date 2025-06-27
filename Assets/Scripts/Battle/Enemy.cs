using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public int maxHP = 10;
    private int currentHP;

    public Vector3Int startingCell;
    public Tilemap tilemap;

    void Start()
    {
        currentHP = maxHP;

        if (tilemap != null)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(startingCell);
            transform.position = worldPos;
        }
        else
        {
            Debug.LogWarning("Tilemap reference is missing on Enemy.");
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log("Enemy took " + amount + "damage || HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
