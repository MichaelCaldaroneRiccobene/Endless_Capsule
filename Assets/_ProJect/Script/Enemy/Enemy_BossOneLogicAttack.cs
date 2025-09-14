using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BossOneLogicAttack : MonoBehaviour
{
    private Transform player;
    private Enemy_BossOne_Animation anim;

    private void Start()
    {
        player = FindAnyObjectByType<Player_Movement>().transform;
        anim = GetComponent<Enemy_BossOne_Animation>();
    }

    public void Attack()
    {
        if (player != null) StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (Mathf.Abs(player.position.x) < 1) anim.ShootTwoArm();
            else if (player.position.x < 1) anim.ShootArmR();
            else if (player.position.x > -1) anim.ShootArmL();

            yield return new WaitForSeconds(4);
        }
    }

    public void StopCoroutine() => StopAllCoroutines();
}
