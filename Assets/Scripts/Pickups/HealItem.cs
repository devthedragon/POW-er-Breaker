using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Pickup
{
    [SerializeField] int healAmount = 1;

    protected override void OnPickup(GameObject obj)
    {
        obj.GetComponent<PlayerHealth>().Heal(healAmount, false);
        base.OnPickup(obj);
    }
}
