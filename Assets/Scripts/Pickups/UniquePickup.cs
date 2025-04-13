using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniquePickup : Pickup
{
    [SerializeField] int powerType; // 0 = Turbo Punch, 1 = Invincible, 2 = Overheal

    protected override void OnPickup(GameObject obj)
    {
        PipeManager.pm.powerUpType = powerType;
        PipeManager.pm.StartPipes();
        base.OnPickup(obj);
    }
}
