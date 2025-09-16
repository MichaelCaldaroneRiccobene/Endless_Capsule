using System.Collections;
using UnityEngine;

public class Enemy_MeteorAttack : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private int[] lines;

    [SerializeField] private Enemy_WarningMeteor warningPreFab;

    [SerializeField] private float warningDistanceToPlayer = 10;
    [SerializeField] private float timeForEachWarning = 3;

    private void Start() => StartCoroutine(SpawnWarningRoutine());

    private IEnumerator SpawnWarningRoutine()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            int randomX = Random.Range(0, lines.Length);
            int posX = lines[randomX];
            Vector3 zPos = new Vector3(0, 0, player.position.z);
            Vector3 targetPos = zPos + new Vector3(posX, transform.position.y, warningDistanceToPlayer);

            if (ManagerPoolObj.Instance != null)
            {
                GameObject warning = ManagerPoolObj.Instance.GetObjFromPool("Warning");
                if (warning.TryGetComponent(out Enemy_WarningMeteor enemy_Warning))
                {
                    enemy_Warning.Player = player;
                    enemy_Warning.WarningDistanceToPlayer = warningDistanceToPlayer;
                    enemy_Warning.transform.position = targetPos;
                }
            }

            yield return new WaitForSeconds(timeForEachWarning);
        }
    }
}
