using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SwordHitbox : MonoBehaviour
{
    public int damage;
    public float lifetime = 0.1f;
    public Tilemap tilemap;

    [Tooltip("Offsets from the hitbox's center cell (0,0 is center). Use to define AoE pattern.")]
    public List<Vector3Int> areaOffsets = new List<Vector3Int> { Vector3Int.zero };  // default: center only

    void Start()
    {
        ApplyDamage();
        Destroy(gameObject, lifetime);
    }

    void ApplyDamage()
    {
        if (tilemap == null)
        {
            Debug.LogWarning("SwordHitbox has no tilemap assigned!");
            return;
        }

        Vector3Int centerCell = tilemap.WorldToCell(transform.position);
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        foreach (var offset in areaOffsets)
        {
            Vector3Int targetCell = centerCell + offset;
            Vector3 worldPos = tilemap.GetCellCenterWorld(targetCell);

            Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos, 0.2f);

            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null && !damagedEnemies.Contains(enemy))
                {
                    damagedEnemies.Add(enemy);
                    enemy.TakeDamage(damage);
                    Debug.Log($"Applying {damage} damage to {enemy.name}");
                }
            }
        }
    }



    void OnDrawGizmosSelected()
    {
        if (tilemap == null || areaOffsets == null) return;

        Gizmos.color = Color.red;
        Vector3Int centerCell = tilemap.WorldToCell(transform.position);

        foreach (var offset in areaOffsets)
        {
            Vector3Int targetCell = centerCell + offset;
            Vector3 worldPos = tilemap.GetCellCenterWorld(targetCell);
            Gizmos.DrawWireCube(worldPos, Vector3.one * 0.9f);
        }
    }

    void OnDrawGizmos()
    {
        if (tilemap == null || areaOffsets == null) return;

        Gizmos.color = Color.yellow;

        Vector3Int centerCell = tilemap.WorldToCell(transform.position);

        foreach (var offset in areaOffsets)
        {
            Vector3Int cell = centerCell + offset;
            Vector3 worldPos = tilemap.GetCellCenterWorld(cell);
            Gizmos.DrawWireSphere(worldPos, 0.2f); // visual circle for detection
        }
    }

}
