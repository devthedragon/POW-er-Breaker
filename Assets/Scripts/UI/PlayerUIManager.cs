using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager pum;

    [SerializeField] GameObject healthText;
    [SerializeField] ImageSwitcher chargeIndicator;
    [SerializeField] ImageSwitcher powerUpIcon;
    [SerializeField] ImageSwitcher healthFaceIcon;
    [SerializeField] float flashTime = 5;
    RectTransform arm;
    bool isCharging;
    Vector2 startPos = new Vector2(550, -300);
    Vector2 chargeOffset = new Vector2(30, -20);

    [HideInInspector] public float playerSpeed;

    private void Awake()
    {
        if (pum == null)
        {
            pum = this;
        }
        else 
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        arm = GameObject.Find("Arm").GetComponent<RectTransform>();
        powerUpIcon.SetAlpha(0);
        ChargeStatus(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging == false) 
        {
            arm.localPosition = startPos + new Vector2 ((Mathf.Sin(Time.time * Mathf.PI) - 1) * 10, (Mathf.Cos((2 * Time.time + Mathf.PI) * Mathf.PI) - 1) * 5) * playerSpeed;
        }
    }

    void ArmVisual(int chargeLevel)
    {
        arm.localPosition = startPos + chargeOffset * chargeLevel;
        arm.GetComponent<ImageSwitcher>().SwitchSprite(chargeLevel);
        chargeIndicator.SwitchSprite(chargeLevel);
        if (chargeLevel > 0)
        {
            isCharging = true;
        }
        else 
        {
            isCharging = false;
        }
    }

    public void UpdateFace(int targetFace) 
    {
        healthFaceIcon.SwitchSprite(targetFace);
    }

    public void ChargeStatus(int chargeLevel) 
    {
        ArmVisual(chargeLevel);
    }

    public void UpdateHealth(float health)
    {
        healthText.GetComponent<TMP_Text>().text = health.ToString("0") + "%";
    }

    public void StartPowerUp(int power, float time)
    {
        StopAllCoroutines();
        powerUpIcon.gameObject.SetActive(true);
        powerUpIcon.SwitchSprite(power);
        StartCoroutine(PowerUpFade(time));
    }

    IEnumerator PowerUpFade(float time)
    {
        float timer = 0;
        powerUpIcon.SetAlpha(1);
        if (time > flashTime)
        {
            yield return new WaitForSeconds(time - flashTime);
        }
        else
        {
            yield return new WaitForEndOfFrame();
        }

        while (timer < flashTime)
        {
            timer += Time.deltaTime;
            powerUpIcon.SetAlpha((Mathf.Lerp(0.25f, 1, (Mathf.Sin(timer / flashTime * Mathf.PI * 10)) + 1)/2));
            yield return new WaitForEndOfFrame();
        }

        powerUpIcon.gameObject.SetActive(false);
    }
}
