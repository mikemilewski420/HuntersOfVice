﻿#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType { HpHeal, MpHeal }

public class Items : MonoBehaviour
{
    [SerializeField]
    private Character Knight, ShadowPriest;

    [SerializeField]
    private Settings settings;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Transform HealTextTransform;

    [SerializeField]
    private Transform TextHolder = null;

    [SerializeField]
    private TextMeshProUGUI ItemText, CoolDownText;

    [SerializeField]
    private ItemType itemType;

    [SerializeField]
    private Image CooldownImage;

    [SerializeField]
    private string ItemName;

    [SerializeField]
    private int HealAmount;

    [SerializeField]
    private float ApplyItemUse, Cooldown;

    [SerializeField]
    private bool UnlockedPassive;

    private float cooldown;

    public ItemType GetItemType
    {
        get
        {
            return itemType;
        }
        set
        {
            itemType = value;
        }
    }

    public Button GetButton
    {
        get
        {
            return button;
        }
        set
        {
            button = value;
        }
    }

    public Image GetCoolDownImage
    {
        get
        {
            return CooldownImage;
        }
        set
        {
            CooldownImage = value;
        }
    }

    public float GetCoolDown
    {
        get
        {
            return Cooldown;
        }
        set
        {
            Cooldown = value;
        }
    }

    public int GetHealAmount
    {
        get
        {
            return HealAmount;
        }
        set
        {
            HealAmount = value;
        }
    }

    public bool GetUnlockedPassive
    {
        get
        {
            return UnlockedPassive;
        }
        set
        {
            UnlockedPassive = value;
        }
    }

    private void Awake()
    {
        cooldown = Cooldown;
    }

    private void Update()
    {
        if(Knight != null || ShadowPriest != null)
        CheckCoolDownStatus();
    }

    private void CheckCoolDownStatus()
    {
        if(CooldownImage.fillAmount <= 0 && !SkillsManager.Instance.GetDisruptedSkill && !GameManager.Instance.GetIsDead && !SkillsManager.Instance.GetActivatedSkill)
        {
            button.interactable = true;

            CoolDownText.enabled = false;

            cooldown = Cooldown;

            return;
        }
        else
        {
            CooldownImage.fillAmount -= Time.deltaTime / Cooldown;
            button.interactable = false;
        }

        if(CooldownImage.fillAmount > 0)
        {
            cooldown -= Time.deltaTime;
            CoolDownText.text = Mathf.Clamp(cooldown, 0, Cooldown).ToString("F1");
        }
    }

    public void Use()
    {
        switch (itemType)
        {
            case (ItemType.HpHeal):
                ReadyHpHealing();
                break;
            case (ItemType.MpHeal):
                ReadyMpHealing();
                ManaPulsePassive();
                break;
        }
    }

    private void ReadyHpHealing()
    {
        if(settings.UseParticleEffects)
        {
            if(Knight.gameObject.activeInHierarchy)
            {
                HpParticleKnight();
            }
            else if(ShadowPriest.gameObject.activeInHierarchy)
            {
                HpParticleShadowPriest();
            }
        }

        SoundManager.Instance.ItemBottle();

        Invoke("HpHealing", ApplyItemUse);
        CooldownImage.fillAmount = 1;
    }

    private void ReadyMpHealing()
    {
        if(settings.UseParticleEffects)
        {
            if(Knight.gameObject.activeInHierarchy)
            {
                MpParticleKnight();
            }
            else if(ShadowPriest.gameObject.activeInHierarchy)
            {
                MpParticleShadowPriest();
            }
        }

        SoundManager.Instance.ItemBottle();

        Invoke("MpHealing", ApplyItemUse);
        CooldownImage.fillAmount = 1;
    }

    private void HpHealing()
    {
        if(Knight.CurrentHealth > 0 || ShadowPriest.CurrentHealth > 0)
        {
            SoundManager.Instance.ItemHeal();

            if(Knight.gameObject.activeInHierarchy)
            {
                Knight.GetComponent<Health>().IncreaseHealth(HpHeal(HealAmount));
            }
            else if(ShadowPriest.gameObject.activeInHierarchy)
            {
                ShadowPriest.GetComponent<Health>().IncreaseHealth(HpHeal(HealAmount));
            }

            HealText();
        }
    }

    private void MpHealing()
    {
        if(Knight.CurrentHealth > 0 || ShadowPriest.CurrentHealth > 0)
        {
            SoundManager.Instance.ItemHeal();

            if(Knight.gameObject.activeInHierarchy)
            {
                Knight.GetComponent<Mana>().IncreaseMana(MpHeal(HealAmount));
            }
            else if(ShadowPriest.gameObject.activeInHierarchy)
            {
                ShadowPriest.GetComponent<Mana>().IncreaseMana(MpHeal(HealAmount));
            }

            HealText();
        }
    }

    private int HpHeal(float value)
    {
        float Percentage = value / 100;

        float HealthAmt = Knight.MaxHealth * Percentage;

        Mathf.Round(HealthAmt);

        return (int)HealthAmt;
    }

    private int MpHeal(float value)
    {
        float Percentage = value / 100;

        float ManaAmt = Knight.MaxMana * Percentage;

        Mathf.Round(ManaAmt);

        return (int)ManaAmt;
    }

    private TextMeshProUGUI HealText()
    {
        var HealingText = ObjectPooler.Instance.GetPlayerHealText();

        HealingText.SetActive(true);

        HealingText.transform.SetParent(HealTextTransform, false);

        if(itemType == ItemType.HpHeal)
        {
            HealingText.GetComponentInChildren<TextMeshProUGUI>().text = "<size=25>" + HpHeal(HealAmount).ToString();
        }
        else
        {
            HealingText.GetComponentInChildren<TextMeshProUGUI>().text = "<size=25>" + MpHeal(HealAmount) + "</size>" + "<size=20>" + " MP";
        }

        return HealingText.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowItemDescriptionPanel(GameObject Panel)
    {
        Panel.SetActive(true);

        if(itemType == ItemType.HpHeal)
        {
            ItemText.text = "<size=12>" + "<u>" + ItemName + "</u>" + "</size>" + "\n\n" + "Recovers HP by " + HealAmount + "%" + "\n\n" + "Cooldown: " + Cooldown + "s";
        }
        else
        {
            ItemText.text = "<size=12>" + "<u>" + ItemName + "</u>" + "</size>" + "\n\n" + "Recovers MP by " + HealAmount + "%" + "\n\n" + "Cooldown: " + Cooldown + "s";
        }
    }

    private void HpParticleKnight()
    {
        var HpParticles = ObjectPooler.Instance.GetHpItemParticle();

        HpParticles.SetActive(true);

        HpParticles.transform.position = new Vector3(Knight.transform.position.x, Knight.transform.position.y + 2f, Knight.transform.position.z);

        HpParticles.transform.SetParent(Knight.transform);
    }

    private void HpParticleShadowPriest()
    {
        var HpParticles = ObjectPooler.Instance.GetHpItemParticle();

        HpParticles.SetActive(true);

        HpParticles.transform.position = new Vector3(ShadowPriest.transform.position.x, ShadowPriest.transform.position.y + 2f, ShadowPriest.transform.position.z);

        HpParticles.transform.SetParent(ShadowPriest.transform);
    }

    private void MpParticleKnight()
    {
        var MpParticles = ObjectPooler.Instance.GetMpItemParticle();

        MpParticles.SetActive(true);

        MpParticles.transform.position = new Vector3(Knight.transform.position.x, Knight.transform.position.y + 2f, Knight.transform.position.z);

        MpParticles.transform.SetParent(Knight.transform);
    }

    private void MpParticleShadowPriest()
    {
        var HpParticles = ObjectPooler.Instance.GetMpItemParticle();

        HpParticles.SetActive(true);

        HpParticles.transform.position = new Vector3(ShadowPriest.transform.position.x, ShadowPriest.transform.position.y + 2f, ShadowPriest.transform.position.z);

        HpParticles.transform.SetParent(ShadowPriest.transform);
    }

    public void ShowCoolDownText()
    {
        if(CooldownImage.fillAmount > 0)
        {
            CoolDownText.enabled = true;
        }
        else
        {
            CoolDownText.enabled = false;
        }
    }

    private void ManaPulsePassive()
    {
        if (UnlockedPassive)
        {
            if(Knight.gameObject.activeInHierarchy)
            {
                SetUpDamagePerimiter(Knight.gameObject.transform.position, 15f);
            }
            else if(ShadowPriest.gameObject.activeInHierarchy)
            {
                SetUpDamagePerimiter(ShadowPriest.gameObject.transform.position, 5f);
            }
            ManaPulseParticle();
        }
        else return;
    }

    private void ManaPulseParticle()
    {
        var ManaPulse = ObjectPooler.Instance.GetManaPulseParticle();

        ManaPulse.SetActive(true);

        ManaPulse.transform.position = new Vector3(ShadowPriest.transform.position.x, ShadowPriest.transform.position.y, ShadowPriest.transform.position.z);
    }

    private void SetUpDamagePerimiter(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<Enemy>())
            {
                if (settings.UseParticleEffects)
                {
                    var HitParticle = ObjectPooler.Instance.GetHitParticle();

                    HitParticle.SetActive(true);

                    HitParticle.transform.position = new Vector3(hitColliders[i].transform.position.x, hitColliders[i].transform.position.y + 0.5f,
                                                                 hitColliders[i].transform.position.z);

                    HitParticle.transform.SetParent(hitColliders[i].transform);
                }
                DamageSkillText(hitColliders[i].GetComponent<Enemy>());
            }
        }
    }

    private TextMeshProUGUI DamageSkillText(Enemy Target)
    {
        var DamageTxt = ObjectPooler.Instance.GetEnemyDamageText();

        if(Target != null)
        {
            DamageTxt.SetActive(true);

            TextHolder = Target.GetUI;

            DamageTxt.transform.SetParent(TextHolder.transform, false);

            Target.GetHealth.ModifyHealth(-((60 + ShadowPriest.CharacterIntelligence) - Target.GetCharacter.CharacterIntelligence));

            DamageTxt.GetComponentInChildren<TextMeshProUGUI>().text = "<size=15>" + "Mana Pulse" + " " + ((60 + ShadowPriest.CharacterIntelligence) - 
                                                                                                            Target.GetCharacter.CharacterIntelligence);
        }

        if (Target.GetAI != null)
        {
            if (Target.GetAI.GetPlayerTarget == null)
            {
                Target.GetAI.GetPlayerTarget = SkillsManager.Instance.GetCharacter;
            }

            if (Target.GetAI.GetStates != States.Skill && Target.GetAI.GetStates != States.ApplyingAttack && Target.GetAI.GetStates != States.SkillAnimation)
            {
                Target.GetAI.GetStates = States.Damaged;
            }
        }
        if (Target.GetPuckAI != null)
        {
            if (Target.GetPuckAI.GetPlayerTarget == null)
            {
                Target.GetPuckAI.GetPlayerTarget = SkillsManager.Instance.GetCharacter;
                Target.GetPuckAI.GetSphereTrigger.gameObject.SetActive(false);
                Target.GetPuckAI.GetStates = BossStates.Chase;
                Target.GetPuckAI.EnableSpeech();
            }

            Target.GetPuckAI.CheckHP();
        }
        if (Target.GetRuneGolemAI != null)
        {
            if (Target.GetRuneGolemAI.GetPlayerTarget == null)
            {
                Target.GetRuneGolemAI.GetPlayerTarget = SkillsManager.Instance.GetCharacter;
                Target.GetRuneGolemAI.GetSphereTrigger.gameObject.SetActive(false);
                Target.GetRuneGolemAI.GetStates = RuneGolemStates.Chase;
            }

            Target.GetRuneGolemAI.CheckHP();
        }
        return DamageTxt.GetComponentInChildren<TextMeshProUGUI>();
    }
}
