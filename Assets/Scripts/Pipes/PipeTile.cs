using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PipeTile : MonoBehaviour
{
    [SerializeField] Image topSprite;
    [SerializeField] Image bottomSprite;
    [SerializeField] Sprite[] litTiles;
    [SerializeField] Sprite[] unlitTiles;
    [SerializeField] int tileMode = 0; // 0 = Start/End, 1 = Straight, 2 = Curve, 3 = Border
    [SerializeField] float _baseSpeed = 1;
    public float baseSpeed { get { return _baseSpeed; } }
    float prevSpeed = 0;

    bool _finishedFill = false;
    public bool finishedFill { get { return _finishedFill; } }
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public int[] endPoints; // 0 = top, 1 = right, 2 = bottom, 3 = left

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(int targetMode) 
    {
        tileMode = targetMode;
        if (tileMode >= 0)
        {
            topSprite.sprite = litTiles[tileMode];
            bottomSprite.sprite = unlitTiles[tileMode];
        }
        else 
        {
            topSprite.color = new Vector4 (0,0,0,0);
            bottomSprite.color = new Vector4(0, 0, 0, 0);
        }
        switch (tileMode)
        {
            case 0:
                topSprite.fillMethod = Image.FillMethod.Horizontal;
                endPoints = new int[2] { -1, 1 };
                currentSpeed = baseSpeed / 2; 
                break;
            case 1:
                topSprite.fillMethod = Image.FillMethod.Vertical;
                endPoints = new int[2] { 0, 2 };

                break;
            case 2:
                topSprite.fillMethod = Image.FillMethod.Radial90;
                topSprite.fillOrigin = (int)Image.Origin90.TopRight;
                endPoints = new int[2] { 0, 1 };

                break;
            default:
                // Border tile
                break;
        }

        Reset();
    }

    public void Rotate(int input) 
    {
        int num = 0;

        if (input >= 0)
        {
            num = input;
        }
        else 
        {
            num = Random.Range(0, 4);
        }

        for (int i = 0, n = endPoints.Length; i < n; i++)
        {
            if (endPoints[i] >= 0)
            {
                endPoints[i] = (endPoints[i] + num) % 4;
            }
        }

        bottomSprite.transform.rotation = Quaternion.Euler(bottomSprite.transform.eulerAngles + new Vector3(0, 0, -90 * num));
        topSprite.transform.rotation = Quaternion.Euler(topSprite.transform.eulerAngles + new Vector3(0, 0, -90 * num));
    }

    public void SetRotate(int input)
    {
        int num = 0;

        if (input >= 0)
        {
            num = input;
        }
        else
        {
            num = Random.Range(0, 4);
        }

        for (int i = 0, n = endPoints.Length; i < n; i++)
        {
            if (endPoints[i] >= 0)
            {
                endPoints[i] = (endPoints[i] + num) % 4;
            }
        }

        bottomSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90 * num));
        topSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90 * num));
    }

    public int StartFill(int startPoint) 
    {
        SetTileLock(true);
        GetComponent<AudioPlayer>().PlayAudio(0, GameObject.Find("Player").transform.position);
        StartCoroutine(FillPipe());
        return FillDirection(startPoint);
    }

    int FillDirection(int startPoint) 
    {
        foreach (var point in endPoints) 
        {
            if (point != startPoint)
            {
                switch (tileMode)
                {
                    case 0:
                        if (startPoint >= 0)
                        {
                            topSprite.fillOrigin = (int)Image.OriginHorizontal.Right;
                        }
                        else
                        {
                            topSprite.fillOrigin = (int)Image.OriginHorizontal.Left;
                        }
                        break;
                    case 1:
                        if (((360 - topSprite.transform.rotation.eulerAngles.z) / 90) % 4 < 2)
                        {
                            if (point < startPoint)
                            {
                                topSprite.fillOrigin = (int)Image.OriginVertical.Bottom;
                            }
                            else
                            {
                                topSprite.fillOrigin = (int)Image.OriginVertical.Top;
                            }
                        }
                        else 
                        {
                            if (point < startPoint)
                            {
                                topSprite.fillOrigin = (int)Image.OriginVertical.Top;
                            }
                            else
                            {
                                topSprite.fillOrigin = (int)Image.OriginVertical.Bottom;
                            }
                        }

                        break;
                    case 2:
                        if ((startPoint + 1) % 4 == point)
                        {
                            topSprite.fillClockwise = false;
                        }
                        else
                        {
                            topSprite.fillClockwise = true;
                        }

                        break;
                }
                return point;
            }
        }
        return -100;
    }

    public void Reset()
    {
        StopAllCoroutines();
        currentSpeed = _baseSpeed;
        topSprite.fillAmount = 0;
        _finishedFill = false;
        if (tileMode == 1 || tileMode == 2)
        {
            SetTileLock(false);
        }
        else 
        {
            SetTileLock(true);
        }
    }

    public void Pause()
    {
        prevSpeed = currentSpeed;
        currentSpeed = 0;
    }

    public void Unpause() 
    {
        currentSpeed = prevSpeed;
    }

    void SetTileLock(bool state) 
    {
        GetComponent<Button>().enabled = !state;
    }



    IEnumerator FillPipe()
    {
        float timer = 0;
        while (timer < 1)
        {
            topSprite.fillAmount = timer;
            timer += Time.unscaledDeltaTime * currentSpeed;
            yield return new WaitForEndOfFrame();
        }
        topSprite.fillAmount = 1;
        _finishedFill = true;
    }
}
