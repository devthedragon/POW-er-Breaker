using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    bool invincible = false;

    protected override void OnHeal()
    {
        PlayerUIManager.pum.UpdateHealth((float)currentHealth / maxHealth * 100);
        base.OnHeal();
    }

    protected override void OnDamage()
    {
        if (invincible == false)
        {
            PlayerUIManager.pum.UpdateHealth((float)currentHealth / maxHealth * 100);
            base.OnDamage();
        }
    }

    protected override void OnDeath()
    {
        PlayerUIManager.pum.UpdateHealth(0);
        audioPlayer.SwitchMusic(-1);
        base.OnDeath();
        OverlayManager.om.DeathScreen();
    }

    protected override void OnHealthChange()
    {
        PlayerUIManager.pum.UpdateFace(Mathf.Clamp(Mathf.CeilToInt(currentHealth/3.34f), 0, 3));
        base.OnHealthChange();
    }

    public void TurnInvincible(float duration) 
    {
        StartCoroutine(InvincibleTimer(duration));
    }

    IEnumerator InvincibleTimer(float duration) 
    {
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
    }
}