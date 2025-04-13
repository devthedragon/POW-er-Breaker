using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PipeManager : MonoBehaviour
{
    public static PipeManager pm;

    [SerializeField] GameObject mainAssembly;
    [SerializeField] GameObject frontPanel;
    [SerializeField] PipeTile[] tiles = new PipeTile[49];
    [SerializeField] ImageSwitcher[] fuses = new ImageSwitcher[3];
    [SerializeField] PipeMapScriptableObject[] maps;
    [SerializeField] float speedMult = 5;

    int currentMapIndex = 0;
    int currentTileIndex = 14;
    int currentEndPoint = -1;
    int lives = 3;
    [HideInInspector] public int powerUpType = 0; // 0 = Turbo Punch, 1 = Invincible, 2 = Overheal
    bool isRunning = false;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            if (tiles[currentTileIndex].finishedFill)
            {
                if (CheckValid(currentEndPoint) == false)
                {
                    Reset();
                    if (lives > 0)
                    {
                        currentEndPoint = tiles[currentTileIndex].StartFill(-1);
                    }
                }
            }
        }
    }

    void Initialize()
    {
        lives = 3;
        frontPanel.GetComponentInChildren<ImageSwitcher>().SwitchSprite(powerUpType);

        foreach (ImageSwitcher fuse in fuses)
        {
            fuse.SwitchSprite(0);
        }

        currentMapIndex = Random.Range(0, maps.Length);

        Reset();

        for (int i = 0, n = tiles.Length; i < n; i++)
        {
            tiles[i].Initialize(maps[currentMapIndex].tileType[i]);
            tiles[i].SetRotate(maps[currentMapIndex].startRot[i]);
            if (i == maps[currentMapIndex].startTile) 
            {
                tiles[i].currentSpeed = 0.5f;
            }
        }
    }

    bool CheckValid(int endPoint)
    {
        PipeTile nextTile = tiles[currentTileIndex + CalcIndexChange(endPoint)];
        foreach (var point in nextTile.endPoints)
        {
            if (point >= 0 && point % 2 == endPoint % 2 && point != endPoint)
            {
                currentTileIndex = currentTileIndex + CalcIndexChange(endPoint);
                currentEndPoint = nextTile.StartFill(point);
                return true;
            }
        }

        if (endPoint < 0 || lives < 2)
        {
            if (lives < 2 && endPoint >= 0)
            {
                GetComponent<AudioPlayer>().PlayAudio(0, GameObject.Find("Player").transform.position);
                fuses[3 - lives].SwitchSprite(1);
                lives--;
            }
            isRunning = false;
            StartCoroutine(EndingSequence());
            return true;
        }
        GetComponent<AudioPlayer>().PlayAudio(0, GameObject.Find("Player").transform.position);
        fuses[3 - lives].SwitchSprite(1);
        lives--;
        return false;
    }

    void Reset()
    {
        currentTileIndex = maps[currentMapIndex].startTile;
        foreach (PipeTile tile in tiles)
        {
            tile.Reset();
        }
    }

    public void StartPipes()
    {
        StartCoroutine(StartUpSequence());
    }

    public void FastForward()
    {
        foreach (PipeTile tile in tiles)
        {
            tile.currentSpeed = tile.baseSpeed * speedMult;
        }
    }

    public void Pause() 
    {
        foreach (PipeTile tile in tiles)
        {
            tile.Pause();
        }
    }

    public void Unpause()
    {
        foreach (PipeTile tile in tiles)
        {
            tile.Unpause();
        }
    }

    int CalcIndexChange(int endPoint)
    {
        switch (endPoint)
        {
            case 0:
                return -7;
            case 1:
                return 1;
            case 2:
                return 7;
            case 3:
                return -1;
            default:
                return 0;
        }
    }

    IEnumerator StartUpSequence()
    {
        Time.timeScale = 0;
        isRunning = true;

        float timer = 0;
        float dropTime = 0.5f;
        float openOutTime = 0.5f;
        float openSlideTime = 1;
        RectTransform rt = GetComponent<RectTransform>();
        RectTransform panelRT = frontPanel.GetComponent<RectTransform>();
        Vector2 targetScale = new Vector2(0.8f, 0.8f);
        Vector2 pnlStartScale = new Vector2(1, 1);
        Vector2 pnlTargetScale = new Vector2(1.1f, 1.1f);
        Vector2 pnlStartLoc = new Vector2(-80, 87);
        Vector2 pnlTargetLoc = new Vector2(-80, 850);

        rt.localScale = targetScale;
        panelRT.localPosition = pnlStartLoc;
        panelRT.localScale = pnlStartScale;

        mainAssembly.SetActive(true);
        Initialize();

        while (timer < dropTime)
        {
            timer += Time.unscaledDeltaTime;
            rt.localScale = Vector2.Lerp(targetScale, Vector2.one, timer / dropTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.25f);

        timer = 0;

        while (timer < openOutTime)
        {
            timer += Time.unscaledDeltaTime;
            panelRT.localScale = Vector2.Lerp(pnlStartScale, pnlTargetScale, timer / openOutTime);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.25f);

        timer = 0;

        while (timer < openSlideTime)
        {
            timer += Time.unscaledDeltaTime;
            panelRT.localPosition = Vector2.Lerp(pnlStartLoc, pnlTargetLoc, timer / openSlideTime);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(0.25f);

        currentEndPoint = tiles[currentTileIndex].StartFill(-1);
    }

    IEnumerator EndingSequence()
    {
        Time.timeScale = 0;
        isRunning = false;

        float timer = 0;
        float dropTime = 1;
        float openOutTime = 0.5f;
        float openSlideTime = 1;
        RectTransform rt = GetComponent<RectTransform>();
        RectTransform panelRT = frontPanel.GetComponent<RectTransform>();
        Vector2 targetScale = new Vector2(0.05f, 0.05f);
        Vector2 pnlStartScale = new Vector2(1.1f, 1.1f);
        Vector2 pnlTargetScale = new Vector2(1, 1);
        Vector2 pnlStartLoc = new Vector2(-80, 850);
        Vector2 pnlTargetLoc = new Vector2(-80, 87);

        rt.localScale = Vector2.one;
        panelRT.localPosition = pnlStartLoc;
        panelRT.localScale = pnlStartScale;

        while (timer < openSlideTime)
        {
            timer += Time.unscaledDeltaTime;
            panelRT.localPosition = Vector2.Lerp(pnlStartLoc, pnlTargetLoc, timer / openSlideTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.25f);

        timer = 0;

        while (timer < openOutTime)
        {
            timer += Time.unscaledDeltaTime;
            panelRT.localScale = Vector2.Lerp(pnlStartScale, pnlTargetScale, timer / openOutTime);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.25f);

        timer = 0;

        while (timer < dropTime)
        {
            timer += Time.unscaledDeltaTime;
            rt.localScale = Vector2.Lerp(Vector2.one, targetScale, timer / dropTime);
            yield return new WaitForEndOfFrame();
        }

        mainAssembly.SetActive(false);

        yield return new WaitForSecondsRealtime(0.25f);

        Time.timeScale = 1;
        PlayerManager.pm.PowerUp(powerUpType, lives + 1);
    }
}
