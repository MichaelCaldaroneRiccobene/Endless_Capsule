using System.Collections;
using UnityEngine;

public class DisableDistance : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        if (player == null) return  ;
        player  = FindAnyObjectByType<Player_Movement>().transform;
    }

    private void OnEnable() => StartCoroutine(CheckDistanceRoutione());

    private IEnumerator CheckDistanceRoutione()
    {
        if(player == null) yield break;
        while (true)
        {
            yield return new WaitForSeconds(3);

            Vector3 distanceToPlayer = transform.position - player.position;

            if (distanceToPlayer.z < -35) gameObject.SetActive(false);
        }
    }

    private void OnDisable() => StopAllCoroutines();
}
