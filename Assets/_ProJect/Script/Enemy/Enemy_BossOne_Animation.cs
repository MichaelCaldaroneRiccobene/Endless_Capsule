using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BossTypeAttack { One,Two,Ultimate }
[System.Serializable]
public class AnimationSettingBossOne
{
    public GameObject preFabBullet;
    public Transform[] pointShootPos;
    public int howManyTimeShoot = 5;
    public float velocityForShootTarget = 0.2f;

    public float velocityForGoOnPosition = 2;

    public Vector3 positionForShoot;
    public Vector3 rotationForShoot;

    public Transform shoulderPrimary;
    public Vector3 rotationShoulderPrimary;
    public Transform shoulderSecondary;
    public Vector3 rotationShoulderSecondary;


    public BossTypeAttack typeAttack;
}

public class Enemy_BossOne_Animation : MonoBehaviour
{
    [SerializeField] private Transform root;
    [SerializeField] private Transform body;
    [SerializeField] private Transform arti;

    [SerializeField] private Transform shoulderR;
    [SerializeField] private Transform shoulderL;

    [SerializeField] private Vector3 positionIdleFirst;
    [SerializeField] private Vector3 rotationIdleFirst;

    [SerializeField] private Vector3 positionIdleSecond;
    [SerializeField] private Vector3 rotationIdleSecond;

    [SerializeField] private float velocityMovementIdle = 1.5f;
    [SerializeField] private float velocityToIdle = 1.5f;

    [SerializeField] private float forceForAttack = 500;

    [SerializeField] private List<AnimationSettingBossOne> typeOfAnimationList;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;

    private bool isOnAnimationStar;

    private void Awake() => SavePosRot();

    private void Start() => StartCoroutine(AnimationIdleRoutine());

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ShootArmL();
        if (Input.GetKeyDown(KeyCode.Alpha2)) ShootTwoArm();
        if (Input.GetKeyDown(KeyCode.Alpha3)) ShootArmR();
    }

    public void SavePosRot()
    {
        startLocalPosition = body.localPosition;
        startLocalRotation = body.localRotation;
    }

    [ContextMenu("EnableRoot")]
    public void EnableRoot() => StartAnimationIdle();

    private void StartAnimationIdle()
    {
        StopAllCoroutines();
        root.gameObject.SetActive(true);
        StartCoroutine(AnimationIdleRoutine());
    }

    [ContextMenu("DisableRoot")]
    public void DisableRoot()
    {
        StopAllCoroutines();
        StartCoroutine(DisableRoutine());
    }

    #region Attack

    [ContextMenu("ShootArmL")]
    public void ShootArmL()
    {
        if(isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(ShootArmRoutine(BossTypeAttack.One));
    }

    [ContextMenu("ShootArmR")]
    public void ShootArmR()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(ShootArmRoutine(BossTypeAttack.Two));
    }
    [ContextMenu("ShootTwoArm")]
    public void ShootTwoArm()
    {
        if (isOnAnimationStar) return;
        StopAllCoroutines();
        StartCoroutine(ShootArmRoutine(BossTypeAttack.Ultimate));
    }

    private IEnumerator ShootArmRoutine(BossTypeAttack attack)
    {
        Vector3 currentPosition = body.localPosition;
        Quaternion currentRotation = body.localRotation;

        yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityToIdle, currentPosition,currentRotation,startLocalPosition,startLocalRotation));
        yield return null;

        AnimationSettingBossOne typeAttack = typeOfAnimationList.Find(x => x.typeAttack == attack);
        if(typeAttack == null) yield break;

        Vector3 targetPosAttack = typeAttack.positionForShoot + body.localPosition;
        Quaternion targetRotAttack = Quaternion.Euler(typeAttack.rotationForShoot);

        yield return StartCoroutine(LerpPositionRotationRoutine(body,typeAttack.velocityForGoOnPosition, startLocalPosition, startLocalRotation, targetPosAttack, targetRotAttack));

        yield return StartCoroutine(ShottingInSequenceRoutine(typeAttack));

        currentPosition = body.localPosition;
        currentRotation = body.localRotation;
        yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityToIdle, currentPosition, currentRotation, startLocalPosition, startLocalRotation));

        StartCoroutine(AnimationIdleRoutine());
    }

    private IEnumerator ShottingInSequenceRoutine(AnimationSettingBossOne typeAttack)
    {
        Quaternion targetRotSecondary = Quaternion.Euler(typeAttack.rotationShoulderSecondary);
        StartCoroutine(LerpRotationRoutine(typeAttack.shoulderSecondary, 2, typeAttack.shoulderSecondary.localRotation, targetRotSecondary));



        for (int i = 0; i < typeAttack.howManyTimeShoot; i++)
        {
            Quaternion targetRotPrimary = Quaternion.Euler(typeAttack.rotationShoulderPrimary);
            yield return StartCoroutine(LerpRotationRoutine(typeAttack.shoulderPrimary,2.5f, typeAttack.shoulderPrimary.localRotation, targetRotPrimary));

            Vector3 posSpawn = typeAttack.pointShootPos[Random.Range(0, typeAttack.pointShootPos.Length)].position;
            if (posSpawn == null) yield break;

            GameObject bullet = ManagerPoolObj.Instance.GetObjFromPool("BulletOne");
            bullet.transform.position = posSpawn;

            targetRotPrimary = Quaternion.Euler(0f, 0f, 0f);
            StartCoroutine(LerpRotationRoutine(typeAttack.shoulderPrimary, 2, typeAttack.shoulderPrimary.localRotation, targetRotPrimary));

            if (bullet.TryGetComponent(out Rigidbody rb)) rb.AddForce(body.forward * 10, ForceMode.VelocityChange);
            yield return new WaitForSeconds(typeAttack.velocityForShootTarget);
        }

        Quaternion targetRotPrimarys = Quaternion.Euler(0f, 0f, 0f);
        targetRotSecondary = Quaternion.Euler(0f, 0f, 0f);

        StartCoroutine(LerpRotationRoutine(typeAttack.shoulderSecondary, 2, typeAttack.shoulderSecondary.localRotation, targetRotSecondary));
        StartCoroutine(LerpRotationRoutine(typeAttack.shoulderPrimary, 2, typeAttack.shoulderPrimary.localRotation, targetRotPrimarys));

        yield return null;
    }
    #endregion

    private IEnumerator AnimationIdleRoutine()
    {
        while (true)
        {
            Vector3 currentPosition = body.localPosition;
            Quaternion currentRotation = body.localRotation;

            yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityToIdle, currentPosition, currentRotation, startLocalPosition, startLocalRotation));

            Vector3 targetPosition = body.localPosition + positionIdleFirst;
            Quaternion targetRotation = Quaternion.Euler(rotationIdleFirst);

            yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityMovementIdle, startLocalPosition, startLocalRotation, targetPosition, targetRotation));

            yield return new WaitForSeconds(velocityToIdle);

            currentRotation = body.localRotation;
            currentPosition = body.localPosition;

            yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityToIdle, currentPosition, currentRotation, startLocalPosition, startLocalRotation));

            targetPosition = body.localPosition + positionIdleSecond;
            targetRotation = Quaternion.Euler(rotationIdleSecond);

            yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityMovementIdle, startLocalPosition, startLocalRotation, targetPosition, targetRotation));
            yield return new WaitForSeconds(velocityToIdle);
        }
    }

    public void GoToStarAnimation()
    {
        StopAllCoroutines();
        isOnAnimationStar = true;
        StartCoroutine(GoToStarAnimationRoutione());
    }

    private IEnumerator GoToStarAnimationRoutione()
    {
        Vector3 currentPosition = body.localPosition;
        Quaternion currentRotation = body.localRotation;

        //
        Quaternion targetRotPrimary = Quaternion.Euler(0f, 0f, 0f);
        Quaternion targetRotSecondary = Quaternion.Euler(0f, 0f, 0f);

        StartCoroutine(LerpRotationRoutine(typeOfAnimationList[0].shoulderSecondary, 2, typeOfAnimationList[0].shoulderSecondary.localRotation, targetRotSecondary));
        StartCoroutine(LerpRotationRoutine(typeOfAnimationList[0].shoulderPrimary, 2, typeOfAnimationList[0].shoulderPrimary.localRotation, targetRotPrimary));
        //

        Debug.Log("GoToStarAnimation");
        yield return StartCoroutine(LerpPositionRotationRoutine(body, velocityToIdle, currentPosition, currentRotation, startLocalPosition, startLocalRotation));
        Vector3 targetPosition = root.localPosition + new Vector3(0,20,0);
        yield return StartCoroutine(LerpPositionRotationRoutine(root, 1, startLocalPosition, startLocalRotation, targetPosition, startLocalRotation));

        root.gameObject.SetActive(false);
        root.localPosition = new Vector3 (0,0,0);

        DisableRoot();
        isOnAnimationStar = false;
    }

    private IEnumerator DisableRoutine()
    {
        yield return null;
        body.localPosition = startLocalPosition;
        body.localRotation = startLocalRotation;

        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        yield return new WaitForSeconds(1f);

        root.gameObject.SetActive(false);
    }

    private IEnumerator LerpPositionRotationRoutine(Transform target, float velocity, Vector3 currentPos, Quaternion currentRot, Vector3 targetPos, Quaternion targetRot)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;

            target.localPosition = Vector3.Lerp(currentPos, targetPos, progress);
            target.localRotation = Quaternion.Lerp(currentRot, targetRot, progress);

            yield return null;
        }
        yield return null;

        target.localPosition = targetPos;
        target.localRotation = targetRot;
    }

    private IEnumerator LerpRotationRoutine(Transform target, float velocity, Quaternion currentRot, Quaternion targetRot)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;

            target.localRotation = Quaternion.Lerp(currentRot, targetRot, progress);

            yield return null;
        }
        yield return null;

        target.localRotation = targetRot;
    }
}
