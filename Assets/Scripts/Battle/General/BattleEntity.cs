using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class BattleEntity : DamageableEntity
{
    public Tilemap tilemap;
    public Vector3Int currentCell;

    protected virtual void Start()
    {
        SnapToGrid(currentCell);
    }

    protected void SnapToGrid(Vector3Int cell)
    {
        if (tilemap != null)
        {
            currentCell = cell;
            transform.position = tilemap.GetCellCenterWorld(cell);
        }
        else
        {
            Debug.LogWarning($"{name} has no tilemap assigned!");
        }
    }

    public void MoveTo(Vector3Int newCell)
    {
        currentCell = newCell;
        SnapToGrid(currentCell);
    }

    public virtual void TakeDamage(int amount) { }
}
