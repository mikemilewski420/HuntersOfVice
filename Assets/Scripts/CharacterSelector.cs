﻿#pragma warning disable 0649
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour
{
    [SerializeField]
    private SelectedCharacter selectedCharacter;

    [SerializeField]
    private Animator CharacterInfoPanel, StartButton;

    [SerializeField]
    private GameObject[] SkillExamples;

    [SerializeField]
    private Sprite[] SkillImages;

    [SerializeField]
    private TextMeshProUGUI CharacterInformationText, CharacterNameText;

    [SerializeField][TextArea]
    private string CharacterInformaion;

    [SerializeField]
    private string CharacterClass;

    private void OnEnable()
    {
        SelectedCharacter SC = FindObjectOfType<SelectedCharacter>();

        selectedCharacter = SC;
    }

    public string GetCharacterClass
    {
        get
        {
            return CharacterClass;
        }
        set
        {
            CharacterClass = value;
        }
    }

    public SelectedCharacter GetSelectedCharacter
    {
        get
        {
            return selectedCharacter;
        }
        set
        {
            selectedCharacter = value;
        }
    }

    public void EndCharacterSelectionAnimation()
    {
        gameObject.GetComponent<Animator>().SetBool("CharacterSelection", false);
    }

    public void PlayPanelAndButtonAnimations()
    {
        CharacterInfoPanel.SetBool("OpenMenu", true);
        StartButton.SetBool("ShowButton", true);
    }

    public void ShowCharacterInformation()
    {
        CharacterInformationText.text = CharacterInformaion;
        CharacterNameText.text = CharacterClass;
    }

    public void ShowCharacterSkills()
    {
        for(int i = 0; i < SkillExamples.Length; i++)
        {
            SkillExamples[i].GetComponent<Image>().sprite = SkillImages[i];
        }
    }
}
