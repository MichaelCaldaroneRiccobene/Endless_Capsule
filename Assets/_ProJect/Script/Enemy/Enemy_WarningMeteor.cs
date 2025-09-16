using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_WarningMeteor : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private GameObject meteorPreFab;

    [SerializeField] private float timeForMeteorToReachWarning = 5;
    [SerializeField] private AnimationCurve meteorCurve;
    [SerializeField] private Vector3 positionSpawnMeteorFromPlayer;

    public Transform Player { get; set; }
    public float WarningDistanceToPlayer { get; set; }

    private void OnEnable() => StartCoroutine(SpawnMeteorRoutione());

    private void Update() => UpdatePositionZ();

    private void UpdatePositionZ()
    {
        Vector3 zPos = new Vector3(0, 0, Player.position.z);
        Vector3 targetPos = zPos + new Vector3(transform.position.x, transform.position.y, WarningDistanceToPlayer);
        transform.position = targetPos;
    }

    private IEnumerator SpawnMeteorRoutione()
    {
        yield return null;

        if(ManagerPoolObj.Instance == null) yield break;
        GameObject meteor = ManagerPoolObj.Instance.GetObjFromPool("Meteor");
        float multiplicatorZ = 0;

        if (Player.transform.TryGetComponent(out Rigidbody rigidbody)) multiplicatorZ += rigidbody.velocity.magnitude;
        if(meteor.TryGetComponent(out DisableDistancePool pool)) pool.Player = Player;

        Vector3 targetPosSpawn = new Vector3(0, 0, Player.transform.position.z);
        targetPosSpawn += new Vector3(0, 0, positionSpawnMeteorFromPlayer.z * multiplicatorZ);
        meteor.transform.position = positionSpawnMeteorFromPlayer + targetPosSpawn;

        Vector3 startPos = meteor.transform.position;

        Quaternion startRotation = meteor.transform.rotation;
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0, 50), Random.Range(0, 360), Random.Range(0, 50));

        float progress = 0;
        while (progress < timeForMeteorToReachWarning)
        {
            progress += Time.deltaTime;

            float t = Mathf.Clamp01(progress / timeForMeteorToReachWarning);
            float curveValue = meteorCurve.Evaluate(t);

            meteor.transform.position = Vector3.Lerp(startPos, transform.position, curveValue);
            meteor.transform.rotation = Quaternion.Lerp(startRotation,randomRotation,curveValue);

            yield return null;
        }

        meteor.transform.position = transform.position;
        meteor.transform.rotation = randomRotation;

        if (ManagerPoolObj.Instance != null) ManagerPoolObj.Instance.ReturnToPool(id, gameObject);
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        StopAllCoroutines();
    }
}
