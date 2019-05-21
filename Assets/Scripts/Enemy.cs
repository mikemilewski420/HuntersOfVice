﻿using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Character character;

    [SerializeField]
    private EnemyAI enemyAI;

    [SerializeField]
    private Health health;

    [SerializeField]
    private EnemySkills enemySkills;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private EnemySkillBar enemySkillBar;

    [SerializeField]
    private Experience EXP;

    [SerializeField]
    private GameObject LocalHealth;

    [SerializeField]
    private Transform UI, DebuffTransform;

    [SerializeField]
    private Image LocalHealthBar;

    [SerializeField]
    private Text EnemyInfo, LocalEnemyInfo;

    [SerializeField]
    private int ExperiencePoints;

    public int GetExperiencePoints
    {
        get
        {
            return ExperiencePoints;
        }
        set
        {
            ExperiencePoints = value;
        }
    }

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

    public EnemyAI GetAI
    {
        get
        {
            return enemyAI;
        }
        set
        {
            enemyAI = value;
        }
    }

    public EnemySkills GetSkills
    {
        get
        {
            return enemySkills;
        }
        set
        {
            enemySkills = value;
        }
    }

    public EnemySkillBar GetEnemySkillBar
    {
        get
        {
            return enemySkillBar;
        }
        set
        {
            enemySkillBar = value;
        }
    }

    public Health GetHealth
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public GameObject GetLocalHealth
    {
        get
        {
            return LocalHealth;
        }
        set
        {
            LocalHealth = value;
        }
    }

    public Transform GetUI
    {
        get
        {
            return UI;
        }
        set
        {
            UI = value;
        }
    }

    public Transform GetDebuffTransform
    {
        get
        {
            return DebuffTransform;
        }
        set
        {
            DebuffTransform = value;
        }
    }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        GetEnemyInfo();
    }

    private void LateUpdate()
    {
        ToggleHealthBar();
    }

    public void GetFilledBar()
    {
        health.GetHealthBar.fillAmount = (float)character.CurrentHealth / (float)character.MaxHealth;
        health.GetFillBarTwo.fillAmount = (float)character.CurrentHealth / (float)character.MaxHealth;
    }

    public void ToggleHealthBar()
    {
        if (GameManager.Instance.GetLastObject == enemy.gameObject)
        {
            foreach(Image i in health.GetComponentsInChildren<Image>())
            {
                i.enabled = true;
            }
            health.GetComponentInChildren<Text>().enabled = true;
        }
        else
        {
            foreach (Image i in health.GetComponentsInChildren<Image>())
            {
                i.enabled = false;
            }
            health.GetComponentInChildren<Text>().enabled = false;
        }
    }

    public void GetLocalHealthInfo()
    {
        LocalHealthBar.fillAmount = (float)character.CurrentHealth / (float)character.MaxHealth;
    }

    public void GetEnemyInfo()
    {
        EnemyInfo.text = "LV: " + character.Level + " " + character.characterName;
        LocalEnemyInfo.text = "LV: " + character.Level + " " + character.characterName;
    }

    public void ReturnExperience()
    {
        EXP.GainEXP(ExperiencePoints);
        EXP.GetShowExperienceText().text = ExperiencePoints + " EXP";
    }
}