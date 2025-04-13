using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float range = 5;
    [SerializeField] float chargeTime = 0.5f;
    [SerializeField] float resetTime = 1f;
    float chargeTimer = 0;
    float resetTimer = 0;
    int chargeLevel = 0;
    int maxCharge = 3;
    AudioPlayer audioPlayer;

    KeyCode attackKey = KeyCode.Space;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(attackKey) && resetTimer <= 0)
        {
            chargeTimer += Time.deltaTime;
            if (chargeTimer > chargeTime * chargeLevel && chargeLevel < maxCharge)
            {
                chargeLevel++;
                audioPlayer.PlayAudio(chargeLevel+3);
                PlayerUIManager.pum.ChargeStatus(chargeLevel);
            }
        }
        else if (chargeLevel > 0)
        {
            Attack(chargeLevel);
            chargeLevel = 0;
            resetTimer = resetTime;
            PlayerUIManager.pum.ChargeStatus(chargeLevel);
        }
        else if (resetTimer > 0)
        {
            chargeTimer = 0;
            resetTimer -= Time.deltaTime;
        }
    }

    void Attack(int chargeLvl)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<Health>().TakeDamage(damage * chargeLvl);
                return;
            }
        }

        audioPlayer.PlayAudio(3);
    }

    public void TurboPunch(float time) 
    {
        StartCoroutine(PunchDuration(time));
    }

    IEnumerator PunchDuration(float time) 
    {
        float tempSpeed = chargeTime;
        float tempReset = resetTime;
        chargeTime = 0.1f;
        resetTime = 0.1f;
        yield return new WaitForSeconds(time);
        chargeTime = tempSpeed;
        resetTime = tempReset;
    }
}
