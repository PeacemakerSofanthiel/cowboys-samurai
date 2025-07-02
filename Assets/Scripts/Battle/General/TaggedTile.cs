using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Custom/Tagged Tile")]
public class TaggedTile : Tile
{
    public TileOwner owner;
    public TileType type = TileType.Normal;
}
