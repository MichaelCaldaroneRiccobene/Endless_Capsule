using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObstacle : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private float upgradeDistanze = 300;

    [Header("Setting How Many Obj")]
    [SerializeField] private int coinInTheGround = 10;
    [SerializeField] private int obstacleInTheGroundJump = 2;
    [SerializeField] private int obstacleInTheGroundSlide = 1;
    [SerializeField] private int obstacleEnemy = 1;

    [Header("Setting For Max How Many Obj")]
    [SerializeField] private int maxCoinInTheGround = 20;
    [SerializeField] private int maxObstacleInTheGround = 5;
    [SerializeField] private int maxObstacleInTheGroundSlide = 3;
    [SerializeField] private int maxObstacleEnemy = 5;

    [Header("Setting List OF Obj")]
    [SerializeField] private Transform[] coins;
    [SerializeField] private Transform[] obstacleTargetsJump;
    [SerializeField] private Transform[] obstacleTargetsSlide;
    [SerializeField] private Transform[] obstacleTargetsEnemy;

    private float lastUpgradeDistance;

    private void Start()
    {
        lastUpgradeDistance = upgradeDistanze;
    }

    private void OnEnable()
    {
        if(coins != null) foreach (Transform coin in coins) { coin.gameObject.SetActive(true); StartCoroutine(SetUpCoinsRoutine()); }

        SpawnObstacles(obstacleTargetsJump, obstacleInTheGroundJump);
        SpawnObstacles(obstacleTargetsSlide, obstacleInTheGroundSlide);
        SpawnObstacles(obstacleTargetsEnemy, obstacleEnemy);

        if(transform.position.z > lastUpgradeDistance)
        {
            Debug.Log("Upgrade");
            lastUpgradeDistance += upgradeDistanze;
            Upgrade();
        }
    }

    private IEnumerator SetUpCoinsRoutine()
    {
        yield return null;
        SpawnObstacles(coins, coinInTheGround);
    }

    public void Upgrade()
    {
        coinInTheGround = Mathf.Clamp(++coinInTheGround, 0, maxCoinInTheGround);
        obstacleInTheGroundJump = Mathf.Clamp(++obstacleInTheGroundJump, 0, maxObstacleInTheGround);
        obstacleInTheGroundSlide = Mathf.Clamp(++obstacleInTheGroundSlide, 0, maxObstacleInTheGroundSlide);
        obstacleEnemy = Mathf.Clamp(++obstacleEnemy, 0, maxObstacleEnemy);
    }

    private void SpawnObstacles(Transform[] typeList,int howMany)
    {
        if (typeList == null || typeList.Length == 0) return;
        foreach (Transform obj in typeList)  obj.gameObject.SetActive(false); 

        howMany = Mathf.Min(howMany, typeList.Length);
        List<Transform> activeList = new List<Transform>();
        for (int i = 0; i < howMany; i++)
        {
            GameObject obj = typeList[Random.Range(0, typeList.Length)].gameObject;

            while (activeList.Contains(obj.transform))
            {
                obj = typeList[Random.Range(0, typeList.Length)].gameObject;
            }

            activeList.Add(obj.transform);
            obj.SetActive(true);
        }
    }

    private void DisableObstacles(Transform[] typeList) { if (typeList != null) foreach (Transform obj in typeList) {if(obj.gameObject.activeInHierarchy) obj.gameObject.SetActive(false); } }

    private void OnDisable() 
    {
        DisableObstacles(coins);
        DisableObstacles(obstacleTargetsJump);
        DisableObstacles(obstacleTargetsSlide);
        DisableObstacles(obstacleTargetsEnemy);
    }
}
