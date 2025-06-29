using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Tagged Tile")]
public class TaggedTile : Tile
{
    public bool isWalkable = true;
    public TileOwner owner = TileOwner.Neutral;
}
