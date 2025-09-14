using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distanzeToPlayer = -10;

    private void LateUpdate()
    {
        Vector3 zPos = new Vector3(0, 0, player.position.z);
        Vector3 targetPos = zPos + new Vector3(transform.position.x, transform.position.y, distanzeToPlayer);

        transform.position = targetPos;
    }
}

