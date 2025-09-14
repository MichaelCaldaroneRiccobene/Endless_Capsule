using System.Collections.Generic;
using UnityEngine;

public class ManagerPoolObj : GenericSingleton<ManagerPoolObj>
{
    [Header("Setting")]
    [SerializeField] private List<PoolObj_SO> poolObjs;

    private Dictionary<string,Queue<GameObject>> poolDictionaryObj = new Dictionary<string,Queue<GameObject>>();

    private void Start() => GenerationPoolObj();

    private void GenerationPoolObj()
    {
        foreach (PoolObj_SO poolObj in poolObjs)
        {
            if (poolObj == null) continue;

            Queue<GameObject> objPool = new Queue<GameObject>();

            for(int i = 0; i < poolObj.StartPool; i++)
            {
                GameObject obj = Instantiate(poolObj.PreFab, transform);
                objPool.Enqueue(obj);
                obj.SetActive(false);
            }
            poolDictionaryObj.Add(poolObj.ID, objPool);
        }
    }

    public GameObject GetObjFromPool(string tag)
    {
        if(!poolDictionaryObj.ContainsKey(tag)) return null;

        if (poolDictionaryObj[tag].Count > 0)
        {
            GameObject objToSpawn = poolDictionaryObj[tag].Dequeue();
            objToSpawn.SetActive(true);
            return objToSpawn;
        }
        else return SpawnForPool(tag);
    }

    private GameObject SpawnForPool(string tag)
    {
        PoolObj_SO poolList = poolObjs.Find(x => x.ID == tag);
        GameObject objToSpawn = Instantiate(poolList.PreFab, transform);

        return objToSpawn;
    }

    public void ReturnToPool(string tag,GameObject obj)
    {
        if (!poolDictionaryObj.ContainsKey(tag)) return;

        obj.gameObject.SetActive(false);
        poolDictionaryObj[tag].Enqueue(obj);
    }
}
