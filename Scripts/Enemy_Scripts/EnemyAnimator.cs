using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Walk(bool walk)
    {
        anim.SetBool(AnimationTags.WALK_PARAM, walk);
    } // animation for patroll and go_to_location

    public void Run(bool run)
    {
        anim.SetBool(AnimationTags.RUN_PARAM, run);
    } // animation for chase

    public void Attack()
    {
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    } // settect for attack trigger

    public void SetAttack(bool atk)
    {
        anim.SetBool(AnimationTags.ATTACK_TRIGGER, atk);
    } // setter for attack bool

    public void Dead()
    {
        anim.SetTrigger(AnimationTags.DEAD_TRIGGER);
    } // dead animation
}
