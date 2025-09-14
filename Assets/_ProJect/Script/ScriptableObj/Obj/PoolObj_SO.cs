using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Obj/Data")]
public class PoolObj_SO : ScriptableObject
{
    public string ID;
    public GameObject PreFab;
    public int StartPool = 5;
}
