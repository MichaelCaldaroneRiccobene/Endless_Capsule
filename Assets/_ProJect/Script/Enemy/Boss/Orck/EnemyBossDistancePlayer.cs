using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossDistancePlayer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform root;

    [SerializeField] private float distanceToMainteinPlayer = 15;

    [SerializeField] private BossTypeAttack[] listAttack;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;


    private Transform parent;
    private Enemy_BossOne_Animation anim;
    private Enemy_BossOneLogicAttack logic;

    private bool animationGoStart;
    private bool isGrounded;
    private bool canAttack;

    private void Awake()
    {
        parent = root.transform.parent;
        anim = GetComponentInParent<Enemy_BossOne_Animation>();
        logic = GetComponentInParent <Enemy_BossOneLogicAttack>();
    }

    private void FixedUpdate()
    {
        if(player != null)
        {
            float distanceToPlayer = parent.position.z - player.position.z;
            if (distanceToPlayer < distanceToMainteinPlayer)
            {
                Vector3 zPos = new Vector3(0, 0, player.position.z);
                Vector3 targetPos = zPos + new Vector3(parent.position.x, parent.position.y, distanceToMainteinPlayer);

                parent.position = targetPos;

                if(!canAttack) logic.Attack();
                canAttack = true;
            }
        }

        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundCheckRadius, groundLayer);

        if (!isGrounded && !animationGoStart)
        {
            animationGoStart = true;
            anim.GoToStarAnimation();
        }
    }

    private void OnDisable()
    {
        animationGoStart = false;
        canAttack = false;

        logic.StopCoroutine();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}
