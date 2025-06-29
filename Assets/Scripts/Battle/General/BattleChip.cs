using UnityEngine;
using UnityEngine.Tilemaps;

public enum ChipType
{
    Hitscan,
    Projectile,
    Sword,
    Trap,
    Summon,
    Support,
    Panel
}

[CreateAssetMenu(fileName = "NewBattleChip", menuName = "BattleChip/New Chip")]
[System.Serializable]
public abstract class BattleChip : ScriptableObject
{
    public string chipName;
    public string chipCode; 
    public ChipType type;
    public int damage;
    public int range;
    public float cooldown; // Seconds or ticks
    public Sprite icon;
    public GameObject chipPrefab; // For projectiles/summons

    public abstract void Activate(PlayerTilemapMover player);
}
