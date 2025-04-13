using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager pm;
    public PlayerHealth pHealth;
    public PlayerAttack pAttack;

    // Start is called before the first frame update
    void Start()
    {
        if (pm == null)
        {
            pm = this;
        }
        else 
        {
            Destroy(this);
        }

        pHealth = GetComponent<PlayerHealth>();
        pAttack = GetComponent<PlayerAttack>();
    }

    public void PowerUp(int mode, int strength) 
    {
        switch (mode)
        {
            case 0: // Turbo punch
                PlayerUIManager.pum.StartPowerUp(mode, strength * 5);
                pAttack.TurboPunch(strength * 5);
                break;
            case 1: // Invincible
                PlayerUIManager.pum.StartPowerUp(mode, strength * 5);
                pHealth.TurnInvincible(strength * 5);
                break;
            case 2: // Overheal
                pHealth.ResetHealth();
                pHealth.Heal(strength * 2, true);
                break;
            default:
                Debug.Log("Invalid mode");
                break;
        }
    }
}
