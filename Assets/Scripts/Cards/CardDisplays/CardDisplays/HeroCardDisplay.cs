﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeroCardDisplay : CardDisplay
{
    /* HERO_CARD_SCRIPTABLE_OBJECT */
    private HeroCard heroCard;

    /* ABILITY_ICON_PREFAB */
    public GameObject AbilityIconPrefab;

    /* HERO_CARD_DATA */
    /* XP */
    [SerializeField] private GameObject LevelUpCondition;
    /* ATTACK_SCORE */
    [SerializeField] private GameObject attackScore;
    [SerializeField] private GameObject attackScoreModifier;
    /* DEFENSE_SCORE */
    [SerializeField] private GameObject defenseScore;
    [SerializeField] private GameObject defenseScoreModifier;
    [SerializeField] private GameObject maxDefenseScoreDisplay;

    private int currentDefenseScore;
    public int CurrentDefenseScore
    {
        get => currentDefenseScore;
        set
        {
            currentDefenseScore = value;
            TextMeshPro txtPro = defenseScore.GetComponent<TextMeshPro>();
            txtPro.SetText(CurrentDefenseScore.ToString());
        }
    }
    private int maxDefenseScore;
    public int MaxDefenseScore
    {
        get => maxDefenseScore;
        set
        {
            maxDefenseScore = value;
            if (maxDefenseScoreDisplay == null) return;
            TextMeshPro txtPro = maxDefenseScoreDisplay.GetComponent<TextMeshPro>();
            txtPro.SetText(MaxDefenseScore.ToString());
        }
    }

    /* ABILITIES */
    public List<CardAbility> CurrentAbilities;
    public List<GameObject> AbilityIcons;
    [SerializeField] private GameObject CurrentAbilitiesDisplay;
    /* EXHAUSTED_ICON */
    [SerializeField] private GameObject ExhaustedIcon;

    private bool canAttack = false;
    public bool CanAttack
    {
        get => canAttack;
        set
        {
            canAttack = value;
            gameObject.GetComponent<Animator>().SetBool("Exhausted", !CanAttack);
        }
    }

    /******
     * *****
     * ****** DISPLAY_CARD
     * *****
     *****/
    protected override void DisplayCard()
    {
        base.DisplayCard();
        heroCard = (HeroCard)CardScript;
        SetLevelUpCondition("Gain XP: " + heroCard.XPCondition);
        SetAttackScore(heroCard.AttackScore);

        MaxDefenseScore = heroCard.DefenseScore; // TESTING
        CurrentDefenseScore = MaxDefenseScore; // TESTING
        
        foreach (CardAbility cardAbility in heroCard.Level1Abilities)
        {
            if (cardAbility == null) continue; // Skip empty abilities
            AddCurrentAbility(cardAbility);
        }
    }

    /******
     * *****
     * ****** DISPLAY_ZOOM_CARD
     * *****
     *****/
    public override void DisplayZoomCard(GameObject parentCard)
    {
        base.DisplayZoomCard(parentCard);
        CardDisplay parentCardDisplay = parentCard.GetComponent<CardDisplay>();
        HeroCardDisplay heroParentCardDisplay = (HeroCardDisplay)parentCardDisplay;
        SetLevelUpCondition(heroParentCardDisplay.GetLevelUpCondition());
        SetAttackScore(heroParentCardDisplay.GetAttackScore());

        CurrentDefenseScore = heroParentCardDisplay.CurrentDefenseScore; // TESTING
        MaxDefenseScore = heroParentCardDisplay.MaxDefenseScore; // TESTING

        foreach (CardAbility cardAbility in heroParentCardDisplay.CurrentAbilities)
        {
            if (cardAbility == null) continue; // Skip empty abilities
            gameObject.GetComponent<CardZoom>().CreateZoomAbilityIcon(cardAbility, CurrentAbilitiesDisplay.transform, 1);
        }
    }

    /******
     * *****
     * ****** ADD_CURRENT_ABILITY
     * *****
     *****/
    public void AddCurrentAbility(CardAbility cardAbility)
    {
        CurrentAbilities.Add(cardAbility);        
        AbilityIcons.Add(CreateAbilityIcon(cardAbility));
    }

    /******
     * *****
     * ****** REMOVE_CURRENT_ABILITY
     * *****
     *****/
    public void RemoveCurrentAbility(CardAbility cardAbility) // IMPLEMENT!
    {
        int abilityIndex = CurrentAbilities.FindIndex(x => x.AbilityName == cardAbility.AbilityName);
        Destroy(AbilityIcons[abilityIndex]);
        AbilityIcons.RemoveAt(abilityIndex);
        CurrentAbilities.RemoveAt(abilityIndex);
    }

    /******
     * *****
     * ****** CREATE_ABILITY_ICON
     * *****
     *****/
    public GameObject CreateAbilityIcon(CardAbility cardAbility)
    {
        GameObject abilityIcon = Instantiate(AbilityIconPrefab, new Vector2(0, 0), Quaternion.identity);
        abilityIcon.GetComponent<AbilityIconDisplay>().AbilityScript = cardAbility;
        abilityIcon.transform.SetParent(CurrentAbilitiesDisplay.transform, false);
        return abilityIcon;
    }

    /******
     * *****
     * ****** GETTERS/SETTERS
     * *****
     *****/
    public int GetAttackScore() => System.Convert.ToInt32(attackScore.GetComponent<TextMeshPro>().text);
    public void SetAttackScore(int newAttackScore)
    {
        TextMeshPro txtPro = attackScore.GetComponent<TextMeshPro>();
        txtPro.SetText(newAttackScore.ToString());
    }
    public void SetAttackScoreModifier(int newAttackScoreModifier)
    {
        TextMeshPro txtPro = attackScoreModifier.GetComponent<TextMeshPro>();
        txtPro.SetText(newAttackScoreModifier.ToString());
    }
    public void SetDefenseScoreModifier(int newDefenseScoreModifier)
    {
        TextMeshPro txtPro = defenseScoreModifier.GetComponent<TextMeshPro>();
        txtPro.SetText(newDefenseScoreModifier.ToString());
    }
    public string GetLevelUpCondition() => LevelUpCondition.GetComponent<TextMeshPro>().text;
    public void SetLevelUpCondition(string newLevelUpCondition)
    {
        TextMeshPro txtPro = LevelUpCondition.GetComponent<TextMeshPro>();
        txtPro.SetText(newLevelUpCondition);
    }
}