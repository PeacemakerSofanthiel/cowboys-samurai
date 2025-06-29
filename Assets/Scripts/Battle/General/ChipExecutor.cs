using UnityEngine;

public static class ChipExecutor
{
    public static void Execute(BattleChip chip, PlayerTilemapMover player)
    {
        if (chip == null || player == null)
        {
            Debug.LogWarning("ChipExecutor: Chip or player is null");
            return;
        }

        switch (chip.type)
        {
            case ChipType.Projectile:
                ExecuteProjectile(chip, player);
                break;
            case ChipType.Hitscan:
                ExecuteHitscan(chip, player);
                break;
            case ChipType.Sword:
                ExecuteSword(chip, player);
                break;
            default:
                Debug.LogWarning($"ChipExecutor: No executor defined for chip type {chip.type}");
                break;
        }
    }

    static void ExecuteProjectile(BattleChip chip, PlayerTilemapMover player)
    {
        if (chip.chipPrefab == null)
        {
            Debug.LogWarning($"Projectile chip {chip.chipName} missing prefab");
            return;
        }

        Vector3 spawnPos = player.transform.position;
        Vector3 shootDir = Vector3.right;

        GameObject proj = GameObject.Instantiate(chip.chipPrefab, spawnPos, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();

        if (p != null)
        {
            p.direction = shootDir;
            p.damage = chip.damage;
            //p.speed = chip.speed;
            p.maxDistance = chip.range;
        }


        Debug.Log($"Fired projectile chip: {chip.chipName}");
    }


    static void ExecuteHitscan(BattleChip chip, PlayerTilemapMover player)
    {
        Vector3Int currentCell = player.tilemap.WorldToCell(player.transform.position);

        for (int i = 1; i <= chip.range; i++)
        {
            Vector3Int checkCell = currentCell + Vector3Int.right * i;
            Vector3 worldPos = player.tilemap.GetCellCenterWorld(checkCell);

            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(chip.damage);
                    Debug.Log($"Hitscan chip hit enemy at {checkCell}");
                    return;
                }
            }
        }

        Debug.Log($"Hitscan chip {chip.chipName} hit no enemies.");
    }

    static void ExecuteSword(BattleChip chip, PlayerTilemapMover player)
    {
        Debug.Log($"Sword chip {chip.chipName} activated.");
    }
}
