using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewHitscanChip", menuName = "BattleChip/Projectile Chip")]
public class ProjectileChip : BattleChip
{
    public override void Activate(PlayerTilemapMover player)
    {
        if (chipPrefab == null)
        {
            Debug.LogWarning($"Projectile chip {chipName} missing prefab");
            return;
        }

        Vector3Int startCell = player.tilemap.WorldToCell(player.transform.position);
        Vector3Int dir = Vector3Int.right;
        Vector3Int spawnCell = startCell + dir;

        Vector3 spawnPos = player.tilemap.GetCellCenterWorld(spawnCell);

        GameObject proj = GameObject.Instantiate(chipPrefab, spawnPos, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.direction = Vector3.right; 
            p.damage = damage;
            //p.range = range;
        }

        Debug.Log($"[ProjectileChip] Fired: {chipName}");
    }

}
