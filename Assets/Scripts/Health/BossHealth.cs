using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [SerializeField] GameObject objToDestroy;

    protected override void OnDeath()
    {
        Vector3 playerLoc = GameObject.Find("Player").transform.position;

        Destroy(objToDestroy);
        audioPlayer.PlayAudio(4, playerLoc + (Vector3.Normalize(objToDestroy.transform.position - playerLoc)) * 2);
        base.OnDeath();
        Destroy(gameObject);
    }
}
