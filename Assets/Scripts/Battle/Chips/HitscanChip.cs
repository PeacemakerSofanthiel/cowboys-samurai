using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewHitscanChip", menuName = "BattleChip/Hitscan Chip")]
public class HitscanChip : BattleChip
{
    public override void Activate(PlayerTilemapMover player)
    {
        Vector3Int currentCell = player.tilemap.WorldToCell(player.transform.position);
        Vector3Int shootDirection = Vector3Int.right; 

        for (int i = 1; i <= range; i++)
        {
            Vector3Int checkCell = currentCell + shootDirection * i;

            if (!player.gridManager.tileStates.ContainsKey(checkCell))
                break;

            Vector3 worldPos = player.tilemap.GetCellCenterWorld(checkCell);
            Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    TileBase highlightTile = TileHighlighter.Instance.highlightTileEnemy;
                    TileHighlighter.Instance.HighlightTile(checkCell, highlightTile);
                    
                    TileHighlighter.Instance.ClearHighlightsAfterDelay(0.5f);

                    Debug.Log($"Hitscan chip {chipName} hit enemy at {checkCell}");
                    return; 
                }
            }
        }

        Debug.Log($"Hitscan chip {chipName} hit no enemies.");
    }

}
