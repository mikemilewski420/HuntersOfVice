﻿#pragma warning disable 0414, 0649
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Materials : MonoBehaviour
{
    [SerializeField]
    private MaterialData materialData;

    [SerializeField]
    private Image FadedImage;

    [SerializeField]
    private Materials InstancedObject, ParentMaterial;

    private string MaterialDescription;

    [SerializeField]
    private string MaterialName;

    [SerializeField]
    private int ShopPoints, Quantity, CommittedQuantity;

    public MaterialData GetMaterialData
    {
        get
        {
            return materialData;
        }
        set
        {
            materialData = value;
        }
    }

    public string GetMaterialName
    {
        get
        {
            return MaterialName;
        }
        set
        {
            MaterialName = value;
        }
    }

    public int GetShopPoints
    {
        get
        {
            return ShopPoints;
        }
        set
        {
            ShopPoints = value;
        }
    }

    public int GetQuantity
    {
        get
        {
            return Quantity;
        }
        set
        {
            Quantity = value;
        }
    }

    public int GetCommittedQuantity
    {
        get
        {
            return CommittedQuantity;
        }
        set
        {
            CommittedQuantity = value;
        }
    }

    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = materialData.MaterialSprite;

        MaterialName = materialData.MaterialName;

        MaterialDescription = materialData.MaterialDescription;

        ShopPoints = materialData.ShopPoints;
    }

    public void OpenInformationPanel()
    {
        if (gameObject.transform.parent == GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetShopMaterialTransform)
        {
            GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetItemDescriptionPanel.SetActive(true);

            GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetItemDescriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text =
                                                                   "<size=12>" + "<u>" + MaterialName + "</u>" + "</size>" + "\n\n" +
                                                                   MaterialDescription + "\n\n" + "Shop Points: " + ShopPoints + "\n\n" + "Quantity: " + Quantity;
        }
        else
        {
            GameManager.Instance.GetItemDescriptionPanel.SetActive(true);

            GameManager.Instance.GetItemDescriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text =
                                                                   "<size=12>" + "<u>" + MaterialName + "</u>" + "</size>" + "\n\n" +
                                                                   MaterialDescription + "\n\n" + "Shop Points: " + ShopPoints + "\n\n" + "Committed Amount: "
                                                                   + CommittedQuantity;
        }
    }

    public void CloseInformationPanel()
    {
        if (gameObject.transform.parent == GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetShopMaterialTransform)
        {
            GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetItemDescriptionPanel.SetActive(false);
        }
        else
        {
            GameManager.Instance.GetItemDescriptionPanel.SetActive(false);
        }
    }

    private void UpdatePanel()
    {
        SoundManager.Instance.ShopMaterials();

        if (gameObject.transform.parent == GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetShopMaterialTransform)
        {
            GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetItemDescriptionPanel.SetActive(true);

            GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetItemDescriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text =
                                                                   "<size=12>" + "<u>" + MaterialName + "</u>" + "</size>" + "\n\n" +
                                                                   MaterialDescription + "\n\n" + "Shop Points: " + ShopPoints + "\n\n" + "Quantity: " + Quantity;
        }
        else
        {
            GameManager.Instance.GetItemDescriptionPanel.SetActive(true);

            GameManager.Instance.GetItemDescriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text =
                                                                   "<size=12>" + "<u>" + MaterialName + "</u>" + "</size>" + "\n\n" +
                                                                   MaterialDescription + "\n\n" + "Shop Points: " + ShopPoints + "\n\n" + "Committed Amount: "
                                                                   + CommittedQuantity;
        }
    }

    public void SetMaterialParent()
    {
        if(GameManager.Instance.GetShopupgrade.GetCanUpgrade)
        {
            if (gameObject.transform.parent == GameManager.Instance.GetInventoryPanel.GetComponent<Inventory>().GetShopMaterialTransform)
            {
                if(Quantity > 0)
                {
                    Quantity--;
                    //FadedImage.enabled = false;
                    if(!CheckForSameMaterialName())
                    {
                        Materials mat = Instantiate(InstancedObject, GameManager.Instance.GetShopUpgradePanel.transform);
                        mat.CommittedQuantity++;
                    }
                    AddExperience();
                    UpdatePanel();
                }
                else
                {
                    //FadedImage.enabled = true;
                }
            }
            else
            {
                CheckForSameMaterialNameInInventory();
                SubtractExperience();
            }
        }
    }

    private bool CheckForSameMaterialName()
    {
        bool SameName = false;

        foreach(Materials m in GameManager.Instance.GetShopUpgradePanel.transform.GetComponentsInChildren<Materials>())
        {
            if(m.GetMaterialData.MaterialName == this.GetMaterialData.MaterialName)
            {
                SameName = true;
                m.CommittedQuantity++;
            }
        }

        return SameName;
    }

    private bool CheckForSameMaterialNameInInventory()
    {
        bool SameName = false;

        UpdatePanel();
        foreach (Materials m in GameManager.Instance.GetInventoryMaterialTransform.GetComponentsInChildren<Materials>())
        {
            if (m.GetMaterialData.MaterialName == this.GetMaterialData.MaterialName)
            {
                SameName = true;
                m.Quantity++;
                this.CommittedQuantity--;
                if(this.CommittedQuantity <= 0)
                {
                    CloseInformationPanel();
                    this.gameObject.transform.SetParent(GameManager.Instance.transform);
                    Destroy(this.gameObject);
                }
            }
        }
        return SameName;
    }

    private void AddExperience()
    {
        GameManager.Instance.GetShop.GetExperiencePoints += ShopPoints;
        GameManager.Instance.GetShop.GetNTL -= ShopPoints;
        GameManager.Instance.GetShop.ShowPreviewExperience();
    }

    private void SubtractExperience()
    {
        GameManager.Instance.GetShop.GetExperiencePoints -= ShopPoints;
        GameManager.Instance.GetShop.GetNTL += ShopPoints;
        GameManager.Instance.GetShop.ShowPreviewExperience();
    }
}
