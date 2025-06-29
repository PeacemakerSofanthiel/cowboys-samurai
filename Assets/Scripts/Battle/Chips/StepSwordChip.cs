using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Linq;

[CreateAssetMenu(fileName = "StepSwordChip", menuName = "BattleChip/StepSword")]
public class StepSwordChip : BattleChip
{
    public GameObject swordHitboxPrefab;
    public float executionDelay = 0.2f;

    public override void Activate(PlayerTilemapMover player)
    {
        player.StartCoroutine(ExecuteStepSword(player));
    }

    private IEnumerator ExecuteStepSword(PlayerTilemapMover player)
    {
        player.movementLocked = true;

        GridManager grid = player.gridManager;
        Tilemap tilemap = player.tilemap;
        Vector3Int startCell = tilemap.WorldToCell(player.transform.position);
        Vector3 originalPosition = player.transform.position;
        Vector3Int forward = Vector3Int.right;

        Vector3Int targetCell = startCell;
        for (int i = 1; i <= range; i++)
        {
            Vector3Int checkCell = startCell + forward * i;
            if (!grid.tileStates.TryGetValue(checkCell, out var state)) break;
            if (state.owner != TileOwner.Enemy && state.owner != TileOwner.Player && state.owner != TileOwner.Neutral) break;

            targetCell = checkCell;
        }

        Vector3 targetPos = tilemap.GetCellCenterWorld(targetCell);
        player.transform.position = targetPos;

        yield return new WaitForSeconds(executionDelay);

        Vector3Int front = targetCell + forward;
        Vector3Int up = front + Vector3Int.up;
        Vector3Int down = front + Vector3Int.down;

        Vector3Int[] hitCells = new Vector3Int[] { front, up, down };
        foreach (var cell in hitCells)
        {
            if (grid.tileStates.TryGetValue(cell, out var state))
            {
                TileBase highlight = TileHighlighter.Instance.highlightTilePlayer;
                if (state.owner == TileOwner.Enemy)
                    highlight = TileHighlighter.Instance.highlightTileEnemy;

                TileHighlighter.Instance.HighlightTile(cell, highlight);
            }
        }
        TileHighlighter.Instance.ClearHighlightsAfterDelay(0.3f);

        if (swordHitboxPrefab != null)
        {
            GameObject hitbox = GameObject.Instantiate(
                swordHitboxPrefab,
                tilemap.GetCellCenterWorld(targetCell),
                Quaternion.identity
            );

            SwordHitbox sword = hitbox.GetComponent<SwordHitbox>();
            if (sword != null)
            {
                sword.damage = damage;

                // Offsets relative to targetCell (center)
                sword.areaOffsets = hitCells
                    .Select(cell => cell - targetCell)
                    .ToList();

                sword.tilemap = tilemap;
            }
        }


        yield return new WaitForSeconds(0.3f);

        player.transform.position = originalPosition;

        player.movementLocked = false;
    }

    
}
