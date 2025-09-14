using System.Collections.Generic;
using UnityEngine;

public enum TypeGrond { BaseGround1, BaseGround2, BaseGround3, EnemyGround1 };

[System.Serializable]
public class PreFabsForGrounds
{
    public TypeGrond type;
    public GameObject[] grounds;
}

public class ManagerPoolRoad : GenericSingleton<ManagerPoolRoad>
{
    [Header("Setting")]
    [SerializeField] private PreFabsForGrounds[] preFabGrounds;
    [SerializeField] private int poolSize = 10;

    private Dictionary<TypeGrond, Dictionary<GameObject, List<GameObject>>> poolGrounds = new Dictionary<TypeGrond, Dictionary<GameObject, List<GameObject>>>();

    protected override void Awake()
    {
        base.Awake();
        SetUpPoolGrounds();
    }

    private void SetUpPoolGrounds()
    {
        foreach (PreFabsForGrounds groundSet in preFabGrounds)
        {
            Dictionary<GameObject, List<GameObject>> levelPool = new Dictionary<GameObject, List<GameObject>>();

            foreach(GameObject preFab in groundSet.grounds)
            {
                List<GameObject> listObjPool = new List<GameObject>();
                for (int j = 0; j < poolSize; j++)
                {
                    GameObject obj = Instantiate(preFab);
                    obj.SetActive(false);
                    listObjPool.Add(obj);
                }
                levelPool[preFab] = listObjPool;
            }
            poolGrounds[groundSet.type] = levelPool;
        }
    }

    public GameObject GetGround(TypeGrond type,Vector3 pos)
    {
        if(!poolGrounds.ContainsKey(type)) return null;

        Dictionary<GameObject, List<GameObject>> levelPool = poolGrounds[type];

        PreFabsForGrounds groundSet = System.Array.Find(preFabGrounds,g => g.type == type);

        GameObject randomPrefab = groundSet.grounds[Random.Range(0, groundSet.grounds.Length)];

        List<GameObject> preFabPool = levelPool[randomPrefab];
        foreach(GameObject obj in preFabPool)
        {
            if(!obj.activeInHierarchy)
            {
                obj.transform.position = pos; obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(randomPrefab, pos, Quaternion.identity);
        newObj.SetActive(true);
        preFabPool.Add(newObj);
        return newObj;
    }
}
