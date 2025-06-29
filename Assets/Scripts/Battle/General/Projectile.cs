using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public float speed = 5f;
    public int damage = 1;
    public float maxDistance = 10f;

    private Vector3 startPosition;
    private Tilemap tilemap;
    private GridManager gridManager;
    private Vector3Int lastHighlightedCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);

    void Start()
    {
        startPosition = transform.position;

        tilemap = FindObjectOfType<Tilemap>();
        gridManager = FindObjectOfType<GridManager>();

        HighlightCurrentCell();
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        HighlightCurrentCell();

        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            ClearHighlight();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            ClearHighlight();
            Destroy(gameObject);
        }
    }

    private void HighlightCurrentCell()
    {
        if (tilemap == null || gridManager == null)
            return;

        Vector3Int currentCell = tilemap.WorldToCell(transform.position);

        if (currentCell != lastHighlightedCell)
        {
            // Clear previous tile
            TileHighlighter.Instance.ClearTile(lastHighlightedCell);

            TileBase highlightTile = TileHighlighter.Instance.highlightTilePlayer; // default

            if (gridManager.tileStates.TryGetValue(currentCell, out var tileState))
            {
                switch (tileState.owner)
                {
                    case TileOwner.Player:
                        highlightTile = TileHighlighter.Instance.highlightTilePlayer;
                        break;
                    case TileOwner.Enemy:
                        highlightTile = TileHighlighter.Instance.highlightTileEnemy;
                        break;
                    case TileOwner.Neutral:
                        highlightTile = TileHighlighter.Instance.highlightTilePlayer;
                        break;
                }
            }

            TileHighlighter.Instance.HighlightTile(currentCell, highlightTile);

            lastHighlightedCell = currentCell;
        }
    }

    private void ClearHighlight()
    {
        TileHighlighter.Instance.ClearTile(lastHighlightedCell);
        lastHighlightedCell = new Vector3Int(int.MinValue, int.MinValue, int.MinValue); // reset invalid cell
    }
}
