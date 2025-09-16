using UnityEngine;

public class Enemy_PingPong : MonoBehaviour
{
    [SerializeField] private Vector3 newLocation;
    [SerializeField] private float speedMoving = 5;

    [SerializeField] private bool hasRandomSpeed;

    [SerializeField] private float minRandomSpeed = 1.5f;
    [SerializeField] private float maxRandomSpeed = 5;

    private Vector3 originallocation;
    private Vector3 targetPos;

    private void OnEnable()
    {
        originallocation = transform.position;
        targetPos = originallocation + newLocation;

        if (!hasRandomSpeed) return;
        speedMoving = Random.Range(minRandomSpeed, maxRandomSpeed);
    }

    private void FixedUpdate()
    {
        float progress = Mathf.PingPong(Time.time * speedMoving, 1);
        float smooth = Mathf.SmoothStep(0, 1, progress);

        Vector3 pos = Vector3.Lerp(originallocation, targetPos, smooth);
        transform.position = pos;
    }

    private void OnDisable()
    {
        transform.position = originallocation;
        targetPos = Vector3.zero;
    }
}
