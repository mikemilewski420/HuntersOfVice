﻿#pragma warning disable 0649
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField]
    private Character Knight, ShadowPriest;

    [SerializeField]
    private Animator animator;

    public Animator GetAnimator
    {
        get
        {
            return animator;
        }
        set
        {
            animator = value;
        }
    }

    public void IdleAnimation()
    {
        animator.SetFloat("Speed", 0);
        animator.SetBool("Attacking", false);
    }

    public void RunAnimation()
    {
        animator.SetFloat("Speed", 1);
    }

    public void AttackAnimation()
    {
        animator.SetBool("Attacking", true);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void EndAttackAnimation()
    {
        this.GetComponent<BasicAttack>().GetAutoAttackTime = 0;

        animator.ResetTrigger("Attacking");
        animator.SetBool("Attacking", false);
    }

    public void DamagedAnimation()
    {
        animator.SetBool("Damaged", true);
    }

    //Animation event placed at the end of the Damaged animation.
    public void EndDamagedAnimation()
    {
        animator.SetBool("Damaged", false);
    }

    public void DeathAnimation()
    {
        animator.SetBool("Attacking", false);
        animator.SetBool("SpellCasting", false);
        animator.SetBool("ContinueCasting", false);

        if(Knight.gameObject.activeInHierarchy)
        {
            animator.SetBool("StormThrust", false);
            animator.SetBool("WhirlwindSlash", false);
            animator.SetBool("EvilsEnd", false);
        }

        animator.SetBool("Skill", false);
        animator.SetBool("SkillCast", false);

        if(ShadowPriest.gameObject.activeInHierarchy)
        {
            animator.SetBool("SpellFinish", false);
        }

        animator.SetBool("Damaged", false);
        animator.SetBool("Dead", true);

        SkillsManager.Instance.GetWhirlwind = false;
    }

    public void PlayResurrectAnimation()
    {
        animator.SetBool("Dead", false);
        animator.SetBool("Ressurecting", true);
    }

    //Animation event placed at the end of the Resurrection animation.
    public void EndRessurectAnimation()
    {
        animator.SetBool("Ressurecting", false);
        this.GetComponent<PlayerController>().enabled = true;
        GameManager.Instance.GetIsDead = false;
        SkillsManager.Instance.ReactivateSkillButtons();
    }

    public void PlaySpellCastAnimation()
    {
        animator.SetBool("SpellCasting", true);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void PlayContinueCastAnimation()
    {
        if(this.gameObject.GetComponent<PlayerController>().GetMovement == Vector3.zero)
        {
            animator.SetBool("SpellCasting", false);
            animator.SetBool("ContinueCasting", true);

            animator.ResetTrigger("Damaged");
            animator.SetBool("Damaged", false);
        }
        else
        {
            animator.SetBool("SpellCasting", false);
            animator.SetBool("ContinueCasting", false);

            animator.ResetTrigger("Damaged");
            animator.SetBool("Damaged", false);
        }
    }

    public void SpellCast()
    {
        animator.SetBool("SpellFinish", true);
    }

    public void EndSpellCast()
    {
        animator.SetBool("SpellFinish", false);
    }

    public void EndSpellCastingAnimation()
    {
        animator.SetBool("ContinueCasting", false);
        animator.SetBool("SpellCasting", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void EndAllSpellcastingBools()
    {
        animator.ResetTrigger("SpellCasting");
        animator.ResetTrigger("ContinueCasting");

        animator.SetBool("SpellCasting", false);
        animator.SetBool("ContinueCasting", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void PlaySkillAnimation()
    {
        animator.SetBool("Skill", true);
        animator.SetBool("Attacking", false);
        animator.SetBool("SkillCast", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void StormThrustAnimation()
    {
        animator.SetBool("StormThrust", true);
        animator.SetBool("Attacking", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void WhirlwindSlashAnimation()
    {
        animator.SetBool("WhirlwindSlash", true);
        animator.SetBool("Attacking", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void DealSkillDamage()
    {
        SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].DamageSkillText(SkillsManager.Instance.GetCharacter.GetComponent<BasicAttack>().GetTarget);
    }

    public void SkillCastAnimation()
    {
        animator.SetBool("SkillCast", true);
        animator.SetBool("Skill", false);

        if(Knight.gameObject.activeInHierarchy)
        {
            animator.SetBool("StormThrust", false);
            animator.SetBool("WhirlwindSlash", false);
        }

        animator.SetBool("SpellCasting", false);
        animator.SetBool("ContinueCasting", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void EndSkillCast()
    {
        animator.SetBool("SkillCast", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);

        SkillsManager.Instance.GetActivatedSkill = false;
    }

    public void EndSkillAnimation()
    {
        animator.SetBool("Skill", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);

        SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].GetFacingEnemy = false;

        SkillsManager.Instance.GetActivatedSkill = false;
    }

    public void EndStormThrustAnimation()
    {
        animator.SetBool("StormThrust", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void EndWhirlwindSlashAnimation()
    {
        animator.SetBool("WhirlwindSlash", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);

        SkillsManager.Instance.GetWhirlwind = false;
        SkillsManager.Instance.GetActivatedSkill = false;

        SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].SetUpDamagePerimiter(SkillsManager.Instance.GetCharacter.transform.position, 2);
    }

    public void EvilsEndAnimation()
    {
        animator.SetBool("EvilsEnd", true);
        animator.SetBool("Attacking", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);
    }

    public void EndEvilsEndAnimation()
    {
        animator.SetBool("EvilsEnd", false);

        animator.ResetTrigger("Damaged");
        animator.SetBool("Damaged", false);

        SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].GetFacingEnemy = false;

        SkillsManager.Instance.GetActivatedSkill = false;
    }

    public void SetWhirlwindOn()
    {
        SkillsManager.Instance.GetWhirlwind = true;
    }

    public void PlayFallSoundEffect()
    {
        SoundManager.Instance.FallSE();
    }

    public void PlayRightFootSE()
    {
        SoundManager.Instance.RightFootStep();
    }

    public void PlayLeftFootSE()
    {
        SoundManager.Instance.LeftFootStep();
    }

    public void PlaySwordHit()
    {
        SoundManager.Instance.SwordHit();
    }

    public void PlayStaffHit()
    {
        SoundManager.Instance.StaffHit();
    }
}
