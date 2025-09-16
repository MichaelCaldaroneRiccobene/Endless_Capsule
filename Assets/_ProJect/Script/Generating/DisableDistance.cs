using System.Collections;
using UnityEngine;

public class DisableDistance : MonoBehaviour
{
    private Transform player;

    private IEnumerator Start()
    {
        yield return null;
        player  = FindAnyObjectByType<Player_Movement>().transform;
    }

    private void OnEnable() => StartCoroutine(CheckDistanceRoutione());

    private IEnumerator CheckDistanceRoutione()
    {
        yield return new WaitForSeconds(1);
        if(player == null) yield break;
        while (true)
        {
            yield return new WaitForSeconds(3);

            Vector3 distanceToPlayer = transform.position - player.position;

            if (distanceToPlayer.z < -35) gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}
