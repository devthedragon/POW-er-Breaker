using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] GameObject objToSpawn;
    [SerializeField] ItemListScriptableObject dropList;

    protected override void OnDamage()
    {
        base.OnDamage();
    }

    protected override void OnHeal()
    {
        base.OnHeal();
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        if (objToSpawn != null)
        {
            GameObject tempObj = Instantiate(objToSpawn, transform.position, transform.rotation);
            if (dropList != null)
            {
                tempObj.GetComponent<SpawnItem>().Spawn(dropList);
            }
        }
        Destroy(gameObject);
    }
}
