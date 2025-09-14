using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5;
    [SerializeField] private PoolObj_SO poolInfo;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        StartCoroutine(DisableRoutine());
        rb.isKinematic = false;
    }

    private IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(lifeTime);

        ManagerPoolObj.Instance.ReturnToPool(poolInfo.ID, gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        ManagerPoolObj.Instance.ReturnToPool(poolInfo.ID, gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
