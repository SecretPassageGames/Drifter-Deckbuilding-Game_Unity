﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityIconDisplay : MonoBehaviour
{
    /* ABILITY_SCRIPTABLE_OBJECT */
    private CardAbility abilityScript;

    [SerializeField] private GameObject abilitySprite;
    [SerializeField] private GameObject abilityName;
    [SerializeField] private GameObject abilitySpriteObject;

    public GameObject AbilitySpriteObject { get => abilitySpriteObject; }
    public CardAbility AbilityScript
    {
        get => abilityScript;
        set
        {
            abilityScript = value;
            DisplayAbilityIcon();
        }
    }
    public CardAbility ZoomAbilityScript
    {
        get => abilityScript;
        set
        {
            abilityScript = value;
            DisplayZoomAbilityIcon();
        }
    }

    /******
     * *****
     * ****** DISPLAY_ABILITY_ICON
     * *****
     *****/
    private void DisplayAbilityIcon()
    {
        Sprite sprite;
        if (AbilityScript is StaticAbility) 
            sprite = AbilityScript.AbilitySprite;
        else if (AbilityScript is TriggeredAbility ta)
        {
            AbilityTrigger trigger = ta.AbilityTrigger;
            sprite = trigger.AbilitySprite;
        }
        else
        {
            Debug.LogError("SCRIPT TYPE NOT FOUND!");
            return;
        }
        SetAbilityIcon(sprite);
    }

    /******
     * *****
     * ****** DISPLAY_ZOOM_ABILITY_ICON
     * *****
     *****/
    private void DisplayZoomAbilityIcon()
    {
        DisplayAbilityIcon();
        string abilityName;
        if (AbilityScript is StaticAbility) 
            abilityName = AbilityScript.AbilityName;
        else if (AbilityScript is TriggeredAbility keywordAbility) 
            abilityName = keywordAbility.AbilityDescription;
        else
        {
            Debug.LogError("SCRIPT TYPE NOT FOUND!");
            return;
        }
        SetAbilityName(abilityName);
    }

    /******
     * *****
     * ****** SETTERS
     * *****
     *****/
    private void SetAbilityIcon(Sprite sprite) => 
        abilitySprite.GetComponent<Image>().sprite = sprite;
    private void SetAbilityName(string abilityDescription) => 
        abilityName.GetComponent<TextMeshProUGUI>().SetText(abilityDescription);
}
