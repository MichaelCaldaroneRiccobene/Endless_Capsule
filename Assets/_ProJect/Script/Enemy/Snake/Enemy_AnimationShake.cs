using System.Collections;
using UnityEngine;

public class Enemy_AnimationShake : MonoBehaviour
{
    [SerializeField] private Transform lookTarget;
    [SerializeField] private Vector3 offsetRotation;
    [SerializeField] private float speedIdleMovement = 0.5f;

    [SerializeField] private Transform head;
    [SerializeField] private Transform lastTail;

    [Header("Idle")]
    [SerializeField] private Vector3 headIdlePositionOne;
    [SerializeField] private Vector3 headIdleRotationOne;

    [SerializeField] private Vector3 headIdlePositionTwo;
    [SerializeField] private Vector3 headIdleRotationTwo;

    [Header("Attack Ultimate")]
    [SerializeField] private Vector3 headAttackPositionOne;
    [SerializeField] private Vector3 headAttackRotationOne;
                                        
    [SerializeField] private Vector3 headAttackPositionTwo;
    [SerializeField] private Vector3 headAttackRotationTwo;

    [SerializeField] private Vector3 headAttackPositionTree;
    [SerializeField] private Vector3 headAttackRotationTree;


    private Vector3 startLocalPositionHead;
    private Quaternion startLocalRotationHead;

    private Vector3 originalForceTail;

    private bool lookPlayer;

    private void Start()
    {
        startLocalPositionHead = head.localPosition;
        startLocalRotationHead = head.localRotation;

        if (lastTail.TryGetComponent(out ConstantForce constant)) originalForceTail = constant.force;

        StartCoroutine(IdleShakeRoutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) AttackOne();
        if (Input.GetKeyDown(KeyCode.Alpha2)) AttackShake();
        if (Input.GetKeyDown(KeyCode.Alpha3)) AttackTwo();
    }

    private void LateUpdate()
    {
        if(!lookPlayer) return;
        head.LookAt(lookTarget);
        head.rotation *= Quaternion.Euler(offsetRotation);
    }

    private IEnumerator IdleShakeRoutine()
    {
        lookPlayer = true;
        while (true)
        {
            Vector3 currentPosition = head.localPosition;
            Quaternion currentRotation = head.localRotation;

            yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, headIdlePositionOne, Quaternion.Euler(headIdleRotationOne), true));

            currentPosition = head.localPosition;
            currentRotation = head.localRotation;
            yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, headIdlePositionTwo, Quaternion.Euler(headIdleRotationTwo), true));
        }
    }

    [ContextMenu("AttackShake")]
    public void AttackShake()
    {
        StopAllCoroutines();
        StartCoroutine(AttackShankeRoutine());
    }

    private IEnumerator AttackShankeRoutine()
    {
        yield return StartCoroutine(PreparationToIlde(1));

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;

        Vector3 targetForceTail;
        if (lastTail.TryGetComponent(out ConstantForce constant))
        {
            targetForceTail = new Vector3(originalForceTail.x, 500, originalForceTail.z);
            constant.force = targetForceTail;
        }

        //Up
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headAttackPositionOne, Quaternion.Euler(headAttackRotationOne), false));

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //Down
        yield return StartCoroutine(LerpPositionRotationRoutine(head,1.5f, currentPosition, currentRotation, headAttackPositionTwo, Quaternion.Euler(headAttackRotationTwo), false));

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //Attack
        yield return StartCoroutine(LerpPositionRotationRoutine(head,1, currentPosition, currentRotation, headAttackPositionTree, Quaternion.Euler(headAttackRotationTree),false));


        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        //Return Up
        yield return StartCoroutine(LerpPositionRotationRoutine(head,1, currentPosition, currentRotation, headAttackPositionOne, Quaternion.Euler(headAttackRotationOne),true));
        if(constant != null) constant.force = originalForceTail;

        //Start Position
        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        yield return StartCoroutine(LerpPositionRotationRoutine(head, speedIdleMovement, currentPosition, currentRotation, startLocalPositionHead, startLocalRotationHead,true));
        StartCoroutine(IdleShakeRoutine());
    }

    [ContextMenu("AttackOne")]
    private void AttackOne()
    {
        StopAllCoroutines();
        StartCoroutine(AttackOneRoutine(180, -2.5f,0,-300));
    }

    [ContextMenu("AttackTwo")]
    private void AttackTwo()
    {
        StopAllCoroutines();
        StartCoroutine(AttackOneRoutine(-180,2.5f,0,300));
    }

    private IEnumerator AttackOneRoutine(float angle,float posX,float targetX,float constanceForceX)
    {
        yield return StartCoroutine(PreparationToIlde(1));

        Vector3 targetForceTail;

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;

        Vector3 headPosAttackOne = currentPosition + new Vector3(posX, -0.5f, -2);


        if (lastTail.TryGetComponent(out ConstantForce constant))
        {
            targetForceTail = new Vector3(constanceForceX, originalForceTail.y, originalForceTail.z);
            constant.force = targetForceTail;
        }

        //Preparation Attack
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headPosAttackOne, currentRotation, true));


        float flipX = posX > 0 ? -posX * posX : Mathf.Abs(posX) * Mathf.Abs(posX);
        headPosAttackOne = currentPosition + new Vector3(targetX, -0.5f, -4);

        currentPosition = head.localPosition;
        currentRotation = head.localRotation;
        yield return StartCoroutine(LerpPositionRotationRoutine(head, 3, currentPosition, currentRotation, headPosAttackOne, currentRotation, false));


        currentPosition = head.localPosition;
        currentRotation = head.localRotation;

        Vector3 headRotAttackTwo = new Vector3(0, angle, 0);
        
        if (constant != null)
        {
            targetForceTail = new Vector3(constant.force.x, originalForceTail.y, -2000);
            constant.force = targetForceTail;
        }

        Vector3 currentPositionTail = lastTail.localPosition;
        Quaternion currentRotationTail = lastTail.localRotation;

        Vector3 targetPositionTail = new Vector3(0, -2.5f, -15);
        //Rotation And Attack
        yield return StartCoroutine(LerpPositionRotationRoutine(head,1f, currentPosition, currentRotation, currentPosition, Quaternion.Euler(headRotAttackTwo), false));

        yield return new WaitForSeconds(0.5f);

        constant.force = originalForceTail;
        yield return StartCoroutine(PreparationToIlde(2));
        StartCoroutine(IdleShakeRoutine());
    }


    private IEnumerator PreparationToIlde(float velocityToIlde = 2)
    {
        if (lastTail.TryGetComponent(out ConstantForce constant)) constant.force = originalForceTail;

        Vector3 currentPosition = head.localPosition;
        Quaternion currentRotation = head.localRotation;
        lookPlayer = false;

        yield return StartCoroutine(LerpPositionRotationRoutine(head, velocityToIlde, currentPosition, currentRotation, new Vector3(0,-1,0), startLocalRotationHead, true));
    }

    private IEnumerator LerpPositionRotationRoutine(Transform target, float velocity, Vector3 currentPos, Quaternion currentRot, Vector3 targetPos, Quaternion targetRot,bool isSmooth)
    {
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;
            float smooth = Mathf.SmoothStep(0, 1, progress);

            float t = isSmooth? smooth : progress;

            target.localPosition = Vector3.Lerp(currentPos, targetPos, t);
            target.localRotation = Quaternion.Lerp(currentRot, targetRot, t);

            yield return null;
        }
        target.localPosition = targetPos;
        target.localRotation = targetRot;
    }
}
