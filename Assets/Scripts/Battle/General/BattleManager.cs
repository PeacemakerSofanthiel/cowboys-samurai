using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    public float tickRate = 0.1f; 
    private float tickTimer;

    private List<IBattleTickable> tickables = new List<IBattleTickable>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer -= tickRate;
            Tick();
        }
    }

    void Tick()
    {
        foreach (var obj in tickables)
        {
            obj.OnBattleTick();
        }
    }

    public void Register(IBattleTickable tickable)
    {
        if (!tickables.Contains(tickable))
            tickables.Add(tickable);
    }

    public void Unregister(IBattleTickable tickable)
    {
        tickables.Remove(tickable);
    }
}
