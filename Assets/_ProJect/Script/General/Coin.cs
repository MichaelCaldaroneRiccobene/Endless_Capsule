using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float velocityRotation = 2;

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(Vector3.up * (velocityRotation * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.TryGetComponent(out Obstacle obstacle)) gameObject.SetActive(false);
       if(other.TryGetComponent(out Player_Controller controller))
        {
            ManagerGame.Instance.AddCoin();
            gameObject.SetActive(false);
        }
    }
}
