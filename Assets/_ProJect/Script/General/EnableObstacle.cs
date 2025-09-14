using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObstacle : MonoBehaviour
{
    [SerializeField] private int coinInTheGround = 10;
    [SerializeField] private int obstacleInTheGroundJump = 4;
    [SerializeField] private int obstacleInTheGroundSlide = 2;
    [SerializeField] private int obstacleEnemy = 4;

    [SerializeField] private Transform[] coins;

    [SerializeField] private Transform[] obstacleTargetsJump;
    [SerializeField] private Transform[] obstacleTargetsSlide;
    [SerializeField] private Transform[] obstacleTargetsEnemy;


    private void OnEnable()
    {
        if(coins != null) foreach (Transform coin in coins) { coin.gameObject.SetActive(true); StartCoroutine(SetUpCoinsRoutine()); }

        //
        if (obstacleTargetsJump != null) for (int i = 0; i < obstacleTargetsJump.Length; i++) obstacleTargetsJump[i].gameObject.SetActive(false);
        if (obstacleTargetsJump != null) for (int i = 0;i < obstacleInTheGroundJump; i++) obstacleTargetsJump[Random.Range(0,obstacleTargetsJump.Length)].gameObject.SetActive(true);

        ///
        if (obstacleTargetsSlide != null) for (int i = 0; i < obstacleTargetsSlide.Length; i++) obstacleTargetsSlide[i].gameObject.SetActive(false);
        if (obstacleTargetsSlide != null) for (int i = 0; i < obstacleInTheGroundSlide; i++) obstacleTargetsSlide[Random.Range(0, obstacleTargetsSlide.Length)].gameObject.SetActive(true);

        ///

        if (obstacleTargetsEnemy != null) for (int i = 0; i < obstacleTargetsEnemy.Length; i++) obstacleTargetsEnemy[i].gameObject.SetActive(false);
        if (obstacleTargetsEnemy != null) for (int i = 0; i < obstacleEnemy; i++) obstacleTargetsEnemy[Random.Range(0, obstacleTargetsEnemy.Length)].gameObject.SetActive(true);

    }

    private IEnumerator SetUpCoinsRoutine()
    {
        yield return null;
        List<Transform> coinActiveList = new List<Transform>();
        foreach (Transform coin in coins) { if (coin.gameObject.activeInHierarchy) coinActiveList.Add(coin); }

        for (int i = 0; i < coinActiveList.Count; i++) coinActiveList[i].gameObject.SetActive(false);
        for (int i = 0; i < coinInTheGround; i++) coinActiveList[Random.Range(0, coinActiveList.Count)].gameObject.SetActive(true);
    }

    private void OnDisable() 
    {
        if (coins != null) foreach (Transform coin in coins) coin.gameObject.SetActive(false);
        //
        if (obstacleTargetsJump != null) for (int i = 0; i < obstacleTargetsJump.Length; i++) obstacleTargetsJump[i].gameObject.SetActive(false);
        ///
        if (obstacleTargetsSlide != null) for (int i = 0; i < obstacleTargetsSlide.Length; i++) obstacleTargetsSlide[i].gameObject.SetActive(false);
        ///
        if (obstacleTargetsEnemy != null) for (int i = 0; i < obstacleTargetsEnemy.Length; i++) obstacleTargetsEnemy[i].gameObject.SetActive(false);
    }
}
