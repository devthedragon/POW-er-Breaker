using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveableHealth : Health
{
    [SerializeField] Vector3 targetOffset;
    [SerializeField] int moveTime;
    [SerializeField] bool canClose;

    bool isMoving = false;
    bool isOpen = false;
    bool waitingForClose = false;
    Vector3 startLoc;

    private void Awake()
    {
        startLoc = transform.position;
    }

    private void Update()
    {
        if (waitingForClose && isOpen == true && isMoving == false) 
        {
            StartCoroutine(CloseOverTime());
            waitingForClose = false;
        }
    }

    protected override void OnDamage()
    {
        if (isMoving == false && isOpen == false)
        {
            base.OnDamage();
            Heal(10);
            StartCoroutine(MoveOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() && canClose == true) { 
            waitingForClose = true;
        }
    }

    IEnumerator MoveOverTime()
    {
        Vector3 targetLoc = startLoc + targetOffset;
        float timer = 0;
        isMoving = true;

        while (timer < moveTime)
        {
            transform.position = Vector3.Lerp(startLoc, targetLoc, timer/moveTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetLoc;
        isMoving = false;
        isOpen = true;
    }

    IEnumerator CloseOverTime() 
    {
        Vector3 targetLoc = startLoc + targetOffset;
        float timer = 0;
        isMoving = true;

        Heal(1);

        while (timer < moveTime)
        {
            transform.position = Vector3.Lerp(targetLoc, startLoc, timer / moveTime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = startLoc;
        isMoving = false;
        isOpen = false;
    }
}
