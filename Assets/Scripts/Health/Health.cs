using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected int currentHealth = 3;
    [SerializeField] protected int maxHealth = 3;
    protected AudioPlayer audioPlayer; // [0] On hit; [1] On heal; [2] On death

    // Start is called before the first frame update
    protected void Start()
    {
        ResetHealth();
        audioPlayer = GetComponent<AudioPlayer>();
        if (audioPlayer == null) 
        {
            Debug.Log(gameObject.name + "is missing an audio component.");
        }
    }

    public void ResetHealth() 
    {
        currentHealth = maxHealth;
        OnHealthChange();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChange();
        if (currentHealth <= 0)
        {
            OnDeath();
        }
        else
        {
            OnDamage();
        }
    }

    public void Heal(int health) 
    {
        currentHealth += health;
        OnHeal();
        OnHealthChange();
    }

    public void Heal(int health, bool canOverheal) 
    {
        currentHealth += health;
        if (currentHealth > maxHealth && canOverheal == false)
        {
            ResetHealth();
        }
        else if (currentHealth > maxHealth * 2)
        {
            currentHealth = maxHealth * 2;
        }
        OnHeal();
        OnHealthChange();
    }

    protected virtual void OnDamage() 
    {
        audioPlayer.PlayAudio(0);
    }

    protected virtual void OnHeal() 
    {
        audioPlayer.PlayAudio(1);
    }

    protected virtual void OnDeath() 
    {
        audioPlayer.PlayAudio(2);
    }

    protected virtual void OnHealthChange() 
    {

    }
}
