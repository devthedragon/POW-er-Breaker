using UnityEngine;

[CreateAssetMenu(fileName = "DropList", menuName = "ScriptableObjects/Drop List", order = 2)]
public class ItemListScriptableObject : ScriptableObject
{
    public GameObject[] itemsToSpawn;
    public int[] itemWeights = new int[1] { 1 };
}
