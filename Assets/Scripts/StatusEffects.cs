﻿using UnityEngine;
using UnityEngine.UI;

public class StatusEffects : MonoBehaviour
{
    [SerializeField]
    private StatusEffect effects;

    [SerializeField]
    private Character character;

    [SerializeField]
    private Image StatusIcon;

    [SerializeField]
    private Transform StatusEffectIconTrans = null;

    [SerializeField]
    private string StatusEffectName, StatusDescription;

    [SerializeField] [Tooltip("The duration of the status effect. Set this to -1 to make the effect permanent.")]
    private float StatusDuration;

    public Character GetCharacter
    {
        get
        {
            return character;
        }
        set
        {
            character = value;
        }
    }

    public Image GetStatusIcon
    {
        get
        {
            return StatusIcon;
        }
        set
        {
            StatusIcon = value;
        }
    }

    public Transform GetStatusEffectIconTrans
    {
        get
        {
            return StatusEffectIconTrans;
        }
        set
        {
            StatusEffectIconTrans = value;
        }
    }

    public string GetStatusEffectName
    {
        get
        {
            return StatusEffectName;
        }
        set
        {
            StatusEffectName = value;
        }
    }

    public string GetStatusDescription
    {
        get
        {
            return StatusDescription;
        }
        set
        {
            StatusDescription = value;
        }
    }

    public float GetStatusDuration
    {
        get
        {
            return StatusDuration;
        }
        set
        {
            StatusDuration = value;
        }
    }

    public StatusEffect GetStatusEffects
    {
        get
        {
            return effects;
        }
        set
        {
            effects = value;
        }
    }

    public void StrengthUP(Character chara, int value, float duration)
    {
        duration = StatusDuration;

        float Percentage = (float)value / 100;

        float TempStrength = (float)chara.CharacterStrength;

        Mathf.FloorToInt(TempStrength);

        TempStrength += (float)chara.CharacterStrength * Percentage;

        chara.CharacterStrength = (int)TempStrength;

        StatusIcon.sprite = SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].GetComponent<Button>().GetComponent<Image>().sprite;
    }

    public void DefenseUP(Character chara, int value, float duration)
    {
        duration = StatusDuration;

        float Percentage = (float)value / 100;

        float TempDefense = (float)chara.CharacterDefense;

        Mathf.FloorToInt(TempDefense);

        TempDefense += (float)chara.CharacterDefense * Percentage;

        chara.CharacterDefense = (int)TempDefense;

        StatusIcon.sprite = SkillsManager.Instance.GetSkills[SkillsManager.Instance.GetKeyInput].GetComponent<Button>().GetComponent<Image>().sprite;
    }
    /*
    public void RemoveStatusAffix(Character chara)
    {
        switch(effects)
        {
            case (statuseffects.StrengthUP):
                chara.CharacterStrength = chara.GetCharacterData.Strength;
                break;
            case (statuseffects.StrengthDOWN):
                chara.CharacterStrength = chara.GetCharacterData.Strength;
                break;
            case (statuseffects.DefenseUP):
                chara.CharacterDefense = chara.GetCharacterData.Defense;
                break;
            case (statuseffects.DefenseDOWN):
                chara.CharacterDefense = chara.GetCharacterData.Defense;
                break;
            case (statuseffects.IntelliegenceUP):
                chara.CharacterIntelligence = chara.GetCharacterData.Intelligence;
                break;
            case (statuseffects.IntelligenceDOWN):
                chara.CharacterIntelligence = chara.GetCharacterData.Intelligence;
                break;
        }
    }
    */
}
