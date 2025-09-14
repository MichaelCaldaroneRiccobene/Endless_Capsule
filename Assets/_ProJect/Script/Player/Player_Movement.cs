using System.Collections;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Setting Generic")]
    [SerializeField] private float velocityRestorPosition = 5;

    [Header("Setting Velocity")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float velocitySpeedIncrease = 0.01f;

    [Header("Setting Swap")]
    [SerializeField] private float swapDistance = 4f;
    [SerializeField] private float velocitySwap = 4;

    [Header("Setting Jump")]
    [SerializeField] private float jumpHeight = 2;
    [SerializeField] private float jumpDuration = 1;
    [SerializeField] private AnimationCurve jumpCurve;

    [Header("Setting Slide")]
    [SerializeField] private float slideHeight = -1;
    [SerializeField] private float slideDuration = 0.75f;
    [SerializeField] private Vector3 slideRotation;
    [SerializeField] private AnimationCurve slideCurve;   

    private Rigidbody rb;
    private Player_Input player_Input;

    private float originalY;
    private Quaternion originalRotation;

    private Coroutine jumpCoroutine;
    private Coroutine slideCoroutine;

    private bool isSwapping;
    private bool isJumping;
    private bool isSliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player_Input = GetComponent<Player_Input>();

        SetUpActions();

        originalY = transform.position.y;
        originalRotation = transform.rotation;
    }

    private void Update()
    {
        if (speed >= maxSpeed) speed = maxSpeed;
        else speed += (Time.deltaTime * velocitySpeedIncrease);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector3.forward * (speed * Time.fixedDeltaTime));
    }

    private void SetUpActions()
    {
        if(player_Input != null)
        {
            player_Input.OnGoLeft += GoLeft;
            player_Input.OnGoRight += GoRight;
            player_Input.OnGoDown += GoDown;
            player_Input.OnGoUp += GoUp;
        }
    }

    private void GoLeft()
    {
        if(isSwapping) return;

        StartCoroutine(SpwappingRoutine(-swapDistance));
    }

    private void GoRight()
    {
        if (isSwapping) return;

        StartCoroutine(SpwappingRoutine(swapDistance));
    }

    private IEnumerator SpwappingRoutine(float swapDistance)
    {
        isSwapping = true;

        float startX = rb.position.x;
        float targetX = startX + swapDistance;

        float progress = 0f;
        while (progress < 0.85)
        {
            progress += Time.deltaTime * velocitySwap;

            Vector3 pos = rb.position;
            pos.x = Mathf.Lerp(startX, targetX, progress);

            rb.position = pos;
            yield return null;
        }

        Vector3 finalPos = rb.position;
        finalPos.x = targetX;
        rb.position = finalPos;

        isSwapping = false;
    }

    private void GoUp()
    {
        if (isJumping) return;

        isSliding = true;
        isJumping = true;

        if (slideCoroutine != null) { StopCoroutine(slideCoroutine); slideCoroutine = null; }

        jumpCoroutine = StartCoroutine(JumpRoution());
    }
    private void GoDown()
    {
        if (isSliding) return;

        isSliding = true;
        isJumping = true;

        if (jumpCoroutine != null) StopCoroutine(jumpCoroutine); jumpCoroutine = null;

        slideCoroutine = StartCoroutine(SlideRoution());
    }

    private IEnumerator JumpRoution()
    {
        if (transform.rotation != originalRotation) yield return StartCoroutine(RestorPosAndRotRoutine());
        float startY = originalY;

        float progress = 0f;

        while (progress < jumpDuration)
        {
            progress += Time.deltaTime;
            MovePositionY(progress, jumpDuration, startY,jumpHeight,jumpCurve);

            if (progress >= 0.2f) isSliding = false;
            yield return null;
        }

        OnFinishAction();
    }

    private IEnumerator SlideRoution()
    {
        if (transform.position.y != originalY) yield return StartCoroutine(RestorPosAndRotRoutine());
        float startY = originalY;

        Quaternion startRotation = rb.rotation;
        Quaternion targetRotation = Quaternion.Euler(slideRotation);

        float progress = 0f;

        while (progress < slideDuration)
        {
            progress += Time.deltaTime;
            float t = MovePositionY(progress, slideDuration, startY ,slideHeight, slideCurve);

            rb.rotation = Quaternion.Lerp(startRotation, targetRotation, slideCurve.Evaluate(t));

            if (progress >= 0.2f) isJumping = false;
            yield return null;
        }

        OnFinishAction();
    }

    private float MovePositionY(float progress,float duration,float startY,float Height,AnimationCurve animationCurve)
    {
        float time = Mathf.Clamp01(progress / duration);

        float curveValue = animationCurve.Evaluate(time);
        Vector3 pos = rb.position;
        pos.y = startY + curveValue * Height;
        rb.position = pos;

        return time;
    }

    private void OnFinishAction()
    {
        Vector3 finalPos = rb.position;
        finalPos.y = originalY;

        rb.position = finalPos;
        rb.rotation = originalRotation;

        isJumping = false;
        isSliding = false;
    }

    private IEnumerator RestorPosAndRotRoutine()
    {
        Quaternion currentRot = rb.rotation;
        float currentPosY = rb.position.y;

        float progress = 0f;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocityRestorPosition;

            Quaternion rot = rb.rotation;
            rot = Quaternion.Lerp(currentRot, originalRotation, progress);
            rb.rotation = rot;

            Vector3 posY = rb.position;
            posY.y = Mathf.SmoothStep(currentPosY, originalY, progress);
            rb.position = posY;

            yield return null;
        }

        Vector3 startPosY = rb.position;
        startPosY.y = originalY;

        rb.position = startPosY;
        rb.rotation = originalRotation;
    }

    private void OnDisable()
    {
        if (player_Input != null)
        {
            player_Input.OnGoLeft -= GoLeft;
            player_Input.OnGoRight -= GoRight;
            player_Input.OnGoDown -= GoDown;
            player_Input.OnGoUp -= GoUp;
        }
    }
}
