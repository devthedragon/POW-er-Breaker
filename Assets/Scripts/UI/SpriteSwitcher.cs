using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwitcher : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        if (sr == null)
        {
            Initialize();
        }
    }

    public void Initialize() 
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }
    }

    public void SwitchSprite(int targetSprite) 
    {
        sr.sprite = sprites[targetSprite];
    }

    public void FlipDir(bool isFlipped) 
    {
        sr.flipX = isFlipped;
    }

    public void SetAlpha(float alpha) 
    {
        Color tempColor = sr.color;
        tempColor.a = alpha * 255;
        sr.color = tempColor;
    }
}
