using UnityEngine;

public class PlayerChipHandler : MonoBehaviour
{
    public BattleChip equippedChip;
    private float chipCooldownTimer = 0f;
    private PlayerTilemapMover tileMover;

    void Awake()
    {
        tileMover = GetComponent<PlayerTilemapMover>();
        if (tileMover == null)
            Debug.LogError("PlayerChipHandler needs PlayerTilemapMover on the same GameObject.");
    }

    void Update()
    {
        if (chipCooldownTimer > 0f)
            chipCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X) && equippedChip != null && chipCooldownTimer <= 0f)
        {
            UseChip();
        }
    }

    void UseChip()
    {
        Debug.Log("Using chip: " + equippedChip.chipName);
        equippedChip.Activate(tileMover);
        chipCooldownTimer = equippedChip.cooldown;
    }

}
