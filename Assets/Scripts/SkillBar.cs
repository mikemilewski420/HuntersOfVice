﻿#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillBar : MonoBehaviour
{
    [SerializeField]
    private Character character, Knight, ShadowPriest;

    [SerializeField]
    private Settings settings;

    [SerializeField]
    private PlayerController playerController, KnightController, ShadowPriestController;

    [SerializeField]
    private PlayerAnimations playerAnimations, KnightAnimations, ShadowPriestAnimations;

    private Skills skills;

    private GameObject CastParticle;

    [SerializeField]
    private TextMeshProUGUI SkillName;

    [SerializeField]
    private Image SkillBarImage, SkillImage;

    [SerializeField]
    private bool ParticleExists;

    private float CastTime;

    public Skills GetSkill
    {
        get
        {
            return skills;
        }
        set
        {
            skills = value;
        }
    }

    public Image GetSkillBar
    {
        get
        {
            return SkillBarImage;
        }
        set
        {
            SkillBarImage = value;
        }
    }

    private void OnEnable()
    {
        if(Knight.gameObject.activeInHierarchy)
        {
            character = Knight;
            playerController = KnightController;
            playerAnimations = KnightAnimations;
        }
        else if(ShadowPriest.gameObject.activeInHierarchy)
        {
            character = ShadowPriest;
            playerController = ShadowPriestController;
            playerAnimations = ShadowPriestAnimations;
        }

        if(settings.UseParticleEffects)
        {
            CastParticle = ObjectPooler.Instance.GetPlayerCastParticle();

            CastParticle.SetActive(true);

            CastParticle.transform.position = new Vector3(character.transform.position.x, character.transform.position.y + 0.15f, character.transform.position.z);

            CastParticle.transform.SetParent(character.transform);
        }
    }

    private void OnDisable()
    {
        if(settings.UseParticleEffects || CastParticle.activeInHierarchy)
        ObjectPooler.Instance.ReturnPlayerCastParticleToPool(CastParticle);
    }

    private void Start()
    {
        CastTime = skills.GetCastTime;

        SkillImage.sprite = skills.GetComponent<Image>().sprite;
    }

    private void Update()
    {
        if(playerController.GetMovement == Vector3.zero && character.CurrentHealth > 0 && !SkillsManager.Instance.GetDisruptedSkill && !skills.GetIsBeingDragged)
        {
            playerAnimations.EndAttackAnimation();
            SkillBarImage.fillAmount += Time.deltaTime / skills.GetCastTime;
            CastTime -= Time.deltaTime;
            SkillName.text = skills.GetSkillName + " " + Mathf.Clamp(CastTime, 0, skills.GetCastTime).ToString("F2");
            if (SkillBarImage.fillAmount >= 1)
            {
                ObjectPooler.Instance.ReturnPlayerCastParticleToPool(CastParticle);

                skills.GetButton.onClick.Invoke();
                SkillBarImage.fillAmount = 0;
                CastTime = skills.GetCastTime;
                CastParticle.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        else
        {
            playerAnimations.EndAllSpellcastingBools();

            SkillsManager.Instance.ReactivateSkillButtons();
            SkillBarImage.fillAmount = 0;
            CastTime = skills.GetCastTime;
            SkillsManager.Instance.GetActivatedSkill = false;
            gameObject.SetActive(false);
        }
    }
}
