using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int damage = 1;
    [SerializeField] float range = 2.5f;
    [SerializeField] float walkSpeed = 0.25f;
    [SerializeField] float chargeTime = 0.75f;
    [SerializeField] float resetTime = 1f;

    AudioPlayer audioPlayer;
    NavMeshAgent agent;
    GameObject target;
    SpriteSwitcher ss;
    bool isAwake = false;
    bool isAttacking = false;
    bool isWalking = false;
    Coroutine walkLoop;

    //Bad Implementation
    [SerializeField] GameObject footstepSource;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        ss = GetComponentInChildren<SpriteSwitcher>();
        audioPlayer = GetComponent<AudioPlayer>();
        footstepSource.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isAwake == false)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit);
            if (hit.transform.gameObject == target)
            {
                isAwake = true;
            }
        }
        else if (isAttacking == false)
        {
            if ((transform.position - target.transform.position).magnitude > range - 0.5f)
            {
                if (isWalking == false)
                {
                    walkLoop = StartCoroutine(WalkingLoop());
                    isWalking = true;
                    footstepSource.SetActive(true);
                }

                agent.SetDestination(target.transform.position);
            }
            else
            {
                if (isWalking)
                {
                    StopCoroutine(walkLoop);
                    isWalking = false;
                    footstepSource.SetActive(false);
                }

                agent.SetDestination(transform.position);
                StartCoroutine(AttackSequence());
            }
        }
    }

    IEnumerator WalkingLoop()
    {
        while (true)
        {
            ss.SwitchSprite(1);
            yield return new WaitForSeconds(walkSpeed);
            ss.SwitchSprite(0);
            yield return new WaitForSeconds(walkSpeed);
            ss.SwitchSprite(2);
            yield return new WaitForSeconds(walkSpeed);
            ss.SwitchSprite(0);
            yield return new WaitForSeconds(walkSpeed);
        }
    }

    IEnumerator AttackSequence() 
    {
        isAttacking = true;
        ss.SwitchSprite(3);
        yield return new WaitForSeconds(chargeTime);
        ss.SwitchSprite(4);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.gameObject.GetComponent<Health>())
            {
                hit.transform.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
            else
            {
                audioPlayer.PlayAudio(3);
            }
        }
        else 
        {
            audioPlayer.PlayAudio(3);
        }

        yield return new WaitForSeconds(0.25f);
        ss.SwitchSprite(0);
        yield return new WaitForSeconds(resetTime);
        isAttacking = false;
    }
}
