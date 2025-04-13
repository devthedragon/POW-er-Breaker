using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/PipeMapScriptableObject", order = 1)]
public class PipeMapScriptableObject : ScriptableObject
{
    public int[] tileType = new int[49] {4,3,3,3,3,3,4,
                                         3,1,1,1,1,1,3,
                                         3,1,1,1,1,1,3,
                                         3,1,1,1,1,1,3,
                                         3,1,1,1,1,1,3,
                                         3,1,1,1,1,1,3,
                                         4,3,3,3,3,3,4}; // 0 = Start/End, 1 = Straight, 2 = Curve, 3 = Border, 4 = Corner of map

    public int[] startRot = new int[49] {0,1,1,1,1,1,0,
                                         0,-1,-1,-1,-1,-1,2,
                                         0,-1,-1,-1,-1,-1,2,
                                         0,-1,-1,-1,-1,-1,2,
                                         0,-1,-1,-1,-1,-1,2,
                                         0,-1,-1,-1,-1,-1,2,
                                         0,3,3,3,3,3,0};
    public int startTile = 0;
}