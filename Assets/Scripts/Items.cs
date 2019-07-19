﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ItemType { HpHeal, MpHeal }

public class Items : MonoBehaviour
{
    [SerializeField]
    private Character character;

    [SerializeField]
    private Button button;

    [SerializeField]
    private Transform HealTextTransform;

    [SerializeField]
    private TextMeshProUGUI ItemText;

    [SerializeField]
    private ItemType itemType;

    [SerializeField]
    private Image CooldownImage;

    [SerializeField]
    private string ItemName;
    
    [SerializeField] [TextArea]
    private string ItemDescription;

    [SerializeField]
    private int HealAmount;

    [SerializeField]
    private float ApplyItemUse, Cooldown;

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

    private void Update()
    {
        if(character != null)
        CheckCoolDownStatus();
    }

    private void CheckCoolDownStatus()
    {
        if(CooldownImage.fillAmount <= 0)
        {
            button.interactable = true;

            return;
        }
        else
        {
            CooldownImage.fillAmount -= Time.deltaTime / Cooldown;
            button.interactable = false;
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
                break;
        }
    }

    private void ReadyHpHealing()
    {
        if(CooldownImage.fillAmount <= 0)
        Invoke("HpHealing", ApplyItemUse);
        CooldownImage.fillAmount = 1;
    }

    private void ReadyMpHealing()
    {
        if(CooldownImage.fillAmount <= 0)
        Invoke("MpHealing", ApplyItemUse);
        CooldownImage.fillAmount = 1;
    }

    private void HpHealing()
    {
        if(character.CurrentHealth > 0)
        {
            character.GetComponent<Health>().IncreaseHealth(HpHeal(HealAmount));

            HealText();
        }
    }

    private void MpHealing()
    {
        if(character.CurrentHealth > 0)
        {
            character.GetComponent<Mana>().IncreaseMana(MpHeal(HealAmount));

            HealText();
        }
    }

    private int HpHeal(float value)
    {
        float Percentage = value / 100;

        float HealthAmt = character.MaxHealth * Percentage;

        Mathf.Round(HealthAmt);

        return (int)HealthAmt;
    }

    private int MpHeal(float value)
    {
        float Percentage = value / 100;

        float ManaAmt = character.MaxMana * Percentage;

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
            HealingText.GetComponentInChildren<TextMeshProUGUI>().text = HpHeal(HealAmount).ToString();
        }
        else
        {
            HealingText.GetComponentInChildren<TextMeshProUGUI>().text = MpHeal(HealAmount) + "<size=11>" + " MP";
        }

        return HealingText.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowItemDescriptionPanel(GameObject Panel)
    {
        Panel.SetActive(true);

        ItemText.text = "<size=12>" + "<u>"+ ItemName + "</u>" + "</size>" + "\n\n" + ItemDescription + "\n\n" + "Cooldown: " + Cooldown + "s";
    }
}
