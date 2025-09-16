using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDistancePool : MonoBehaviour
{
    [SerializeField] private string id;

    public Transform Player { get; set; }

    private void OnEnable() => StartCoroutine(CheckDistanceRoutione());

    private IEnumerator CheckDistanceRoutione()
    {
        yield return null;
        if (Player == null) yield break;
        while (true)
        {
            yield return new WaitForSeconds(3);

            Vector3 distanceToPlayer = transform.position - Player.position;
            if (distanceToPlayer.z < -35)
            {
                if(ManagerPoolObj.Instance !=null) ManagerPoolObj.Instance.ReturnToPool(id, gameObject);
            }
        }
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}
