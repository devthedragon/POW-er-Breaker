using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] GameObject spawnLoc;
    [SerializeField] SpriteSwitcher itemHolder;

    int weightTotal = 0;
    GameObject spawnedItem;
    bool hasSwitched = false;

    // Update is called once per frame
    void Update()
    {
        if (hasSwitched == false)
        {
            if (itemHolder != null)
            {
                if (spawnedItem == null)
                {
                    itemHolder.SwitchSprite(0);
                    hasSwitched = true;
                }
            }
            else 
            {
                hasSwitched = true;
            }
        }
    }

    public void Spawn(ItemListScriptableObject itemList) 
    {
        foreach (var weight in itemList.itemWeights)
        {
            weightTotal += weight;
        }

        int randNum = Random.Range(0, weightTotal);
        int tempTotal = 0;
        for (int i = 0, n = itemList.itemWeights.Length; i < n; i++)
        {
            tempTotal += itemList.itemWeights[i];
            if (randNum < tempTotal)
            {
                if (spawnLoc != null)
                {
                    spawnedItem = Instantiate(itemList.itemsToSpawn[i], spawnLoc.transform.position, spawnLoc.transform.rotation, spawnLoc.transform);
                }
                else 
                {
                    spawnedItem = Instantiate(itemList.itemsToSpawn[i], transform.position, transform.rotation, spawnLoc.transform);
                }
                break;
            }
        }
        if (itemHolder != null)
        {
            if (spawnedItem == null)
            {
                itemHolder.Initialize();
                itemHolder.SwitchSprite(0);
            }
            else
            {
                itemHolder.Initialize();
                itemHolder.SwitchSprite(1);
            }
        }
    }

    public void Spawn(ItemListScriptableObject itemList,  int index) 
    {
        Instantiate(itemList.itemsToSpawn[index], transform.position, transform.rotation);
    }
}
