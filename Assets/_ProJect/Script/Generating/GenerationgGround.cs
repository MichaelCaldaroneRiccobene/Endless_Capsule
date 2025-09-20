using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationgGround : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanceForSpawnGround = 20;

    [SerializeField] private int distanceForNextLevel = 100;
    
    [SerializeField] private float distanceForEachGround = 10;
    [SerializeField] private int howManyLevels = 3;
    [SerializeField] private int howManyGroundsFirst = 10;

    [SerializeField] private List<TypeGrond> typeBaseGround;
    [SerializeField] private List<TypeGrond> typeEnemyGround;

    [SerializeField] private GameObject enemyBossOne;

    private int incrementDistanceForNextLevel;

    private float incrementDistanceForEachGround;
    private int timeOnSpotBoss = 15;

    private Transform lastGround;
    private int currentLevelIndex = 0;

    private bool isBossSpwaunadet;
    private bool isBossOne;

    private void Start()
    {
        incrementDistanceForNextLevel += distanceForNextLevel;
        GenerationGrounds(howManyGroundsFirst);

        StartCoroutine(EnemyOneTurnRoutine());

        isBossSpwaunadet = false;

        Enemy_BossOne_Animation enemy_BossOne_Animation = enemyBossOne.GetComponentInParent<Enemy_BossOne_Animation>();
        enemy_BossOne_Animation.DisableRoot();
    }

    private void Update()
    {
        if(isBossOne)
        {
            GenerateGroundEnemy();
            return;
        }
        GenerateGround();

        //Debug.Log("Position.Z :" + Player.position.z.ToString("F0"));
        if (player.position.z >= incrementDistanceForNextLevel) NextLevel();
    }

    private void NextLevel()
    {
        Debug.Log("Next Level");
        incrementDistanceForNextLevel += distanceForNextLevel;
        currentLevelIndex++;

        if (currentLevelIndex > howManyLevels) currentLevelIndex = 0;

        Debug.Log("Current level " + currentLevelIndex);
    }

    private IEnumerator EnemyOneTurnRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(20);
            isBossOne = true;

            while (!isBossSpwaunadet) yield return null;

            Debug.Log("Wait For Boss Go Away");
            yield return new WaitForSeconds(timeOnSpotBoss);
            timeOnSpotBoss++;
            isBossOne = false;
            isBossSpwaunadet = false;

            while (enemyBossOne.activeInHierarchy) yield return null;

            Debug.Log("Boss non Nella Hierarchy");
        }
    }

    private void GenerateGround()
    {
        if (lastGround == null) return;

        float distanceToPlayer = Vector3.Distance(lastGround.position, player.position);
        if (distanceForSpawnGround >= distanceToPlayer) GenerationGrounds(2);
    }

    private void GenerationGrounds(int SpawnGrounds)
    {
        for (int i = 0; i < SpawnGrounds; i++)
        {
            incrementDistanceForEachGround += distanceForEachGround;
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementDistanceForEachGround);
            GameObject obj = ManagerPoolRoad.Instance.GetGround(typeBaseGround[currentLevelIndex], spawnPos);

            if(obj != null) lastGround = obj.transform;           
        }
    }

    private void GenerateGroundEnemy()
    {
        if (lastGround == null) return;

        float distanceToPlayer = Vector3.Distance(lastGround.position, player.position);
        if (distanceForSpawnGround >= distanceToPlayer) GenerationGroundsEnemys(2);
    }

    private void GenerationGroundsEnemys(int SpawnGrounds)
    {
        for (int i = 0; i < SpawnGrounds; i++)
        {
            incrementDistanceForEachGround += distanceForEachGround;
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + incrementDistanceForEachGround);
            GameObject obj = ManagerPoolRoad.Instance.GetGround(typeEnemyGround[0], spawnPos);

            if (obj != null) lastGround = obj.transform;

            if(!isBossSpwaunadet) StartCoroutine(TrySpawnBossRoutine());
        }
    }

    private IEnumerator TrySpawnBossRoutine()
    {
        while (!isBossSpwaunadet)
        {
            yield return null;
            Debug.Log("TrySpawnBoss");
            if(!enemyBossOne.activeInHierarchy)
            {
                Enemy_BossOne_Animation enemy_BossOne_Animation = enemyBossOne.GetComponentInParent<Enemy_BossOne_Animation>();
                enemy_BossOne_Animation.EnableRoot();
                isBossSpwaunadet = true;

                Transform parentEnemy = enemyBossOne.transform.parent;
                parentEnemy.position = new Vector3(enemyBossOne.transform.position.x, enemyBossOne.transform.position.y, lastGround.position.z);
            }
            yield return null;
        }
    }
}
