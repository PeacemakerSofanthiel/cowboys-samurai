using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlayerHitscanShooter : MonoBehaviour
{
    public GridManager gridManager;
    public Tilemap tilemap;

    public Vector3Int shootDirection = Vector3Int.right;
    public int maxRange = 100;
    public int damage = 10;

    private Vector3Int currentCell;
    private bool canShoot = true;

    public float travelSpeedTilesPerSecond = 10f;

    public float delayModifier = 1f;

    void Start()
    {
        currentCell = tilemap.WorldToCell(transform.position);
    }

    void Update()
    {
        currentCell = tilemap.WorldToCell(transform.position);

        if (Input.GetKeyDown(KeyCode.C) && canShoot)
        {
            StartCoroutine(ShootHitscanWithDelay());
        }
    }


    IEnumerator ShootHitscanWithDelay()
    {
        canShoot = false;

        Vector3Int checkCell = currentCell;

        for (int i = 1; i <= maxRange; i++)
        {
            checkCell += shootDirection;

            if (!gridManager.tileStates.ContainsKey(checkCell))
                break;

            var tileState = gridManager.tileStates[checkCell];
            if (tileState.owner == TileOwner.Enemy)
            {
                float baseDelay = i / travelSpeedTilesPerSecond;
                float totalDelay = baseDelay * delayModifier;

                Debug.Log($"Shooting delay for distance {i} tiles: {totalDelay} seconds");

                yield return new WaitForSeconds(totalDelay);

                Vector3 worldPos = tilemap.GetCellCenterWorld(checkCell);
                Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);
                foreach (var hit in hits)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                        Debug.Log("Hitscan hit enemy at " + checkCell);
                        break;
                    }
                }

                canShoot = true;
                yield break;
            }
        }

        Debug.Log("Hitscan found no enemies in path.");
        canShoot = true;
    }
}
