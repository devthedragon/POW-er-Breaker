using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    Image image;

    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SwitchSprite(int targetSprite)
    {
        image.sprite = sprites[targetSprite];
    }

    public void SetAlpha(float alpha)
    {
        Color tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }
}
