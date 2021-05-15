﻿using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    /* CARD_MANAGER_DATA */
    private const string PLAYER_HAND = CardManagerData.PLAYER_HAND;
    private const string PLAYER_ZONE = CardManagerData.PLAYER_ZONE;
    private const string ENEMY_HAND = CardManagerData.ENEMY_HAND;
    private const string ENEMY_ZONE = CardManagerData.ENEMY_ZONE;
    private const string PLAYER_CARD = CardManagerData.PLAYER_CARD;
    private const string ENEMY_CARD = CardManagerData.ENEMY_CARD;

    /* CHANGE_LAYER_DATA */
    private const string ZOOM_LAYER = ChangeLayerData.ZOOM_LAYER;

    /* ZOOMCARD_DATA */
    private const float ZOOM_BUFFER = ZoomCardData.ZOOM_BUFFER;
    private const float ZOOM_SCALE_VALUE = ZoomCardData.ZOOM_SCALE_VALUE;
    private const float CENTER_SCALE_VALUE = ZoomCardData.CENTER_SCALE_VALUE;
    private const float POPUP_SCALE_VALUE = ZoomCardData.POPUP_SCALE_VALUE;
    private const float POPUP_X_VALUE = ZoomCardData.POPUP_X_VALUE;

    /* MANAGERS */
    private UIManager UIManager;

    /* PREFABS */
    public GameObject HeroZoomCard;
    //public GameObject ZoomActionCard;

        // ABILITY_PREFABS
    [SerializeField] private GameObject abilityBoxPrefab;
    [SerializeField] private GameObject abilityPopupPrefab;
    
        // NEXT_LEVEL_PREFABS
    [SerializeField] private GameObject nextLevelBox;
    [SerializeField] private GameObject level2Popup;

        // LORE_PREFAB
    [SerializeField] private GameObject lorePopup;

    /* ZONES */
    private GameObject background;
    private GameObject playerHand;
    private GameObject playerZone;
    private GameObject enemyHand;
    private GameObject enemyZone;

    /* STATIC_CLASS_VARIABLES */
    public static bool ZoomCardIsCentered = false;
    public static GameObject CurrentZoomCard { get; set; }
    public static GameObject NextLevelPopup { get; set; }
    public static GameObject LorePopup { get; set; }
    
    /* CLASS_VARIABLES */
    private CardDisplay cardDisplay;
    
    /******
     * *****
     * ****** AWAKE
     * *****
     *****/
    public void Awake()
    {
        UIManager = UIManager.Instance;
        background = GameObject.Find("Background");
        playerHand = GameObject.Find(PLAYER_HAND);
        playerZone = GameObject.Find(PLAYER_ZONE);
        enemyHand = GameObject.Find(ENEMY_HAND);
        enemyZone = GameObject.Find(ENEMY_ZONE);
        cardDisplay = gameObject.GetComponent<CardDisplay>();
    }

    /******
     * *****
     * ****** ON_CLICK
     * *****
     *****/
    public void OnClick()
    {
        if (transform.parent.gameObject == enemyHand) return; // HIDE THE ENEMY HAND
        if (DragDrop.CardIsDragging || ZoomCardIsCentered) return;
        
        UIManager.SetScreenDimmer(true);
        ZoomCardIsCentered = true;

        CreateZoomCard(new Vector3(0, 50), CENTER_SCALE_VALUE);
        HeroCardDisplay heroCardDisplay = (HeroCardDisplay)cardDisplay;
        HeroCard heroCard = (HeroCard)heroCardDisplay.CardScript;

        CreateNextLevelPopup(new Vector2(POPUP_X_VALUE, 0), POPUP_SCALE_VALUE, heroCard.Level2Abiliites);
        CreateLorePopup(new Vector2(-600, 0), POPUP_SCALE_VALUE);
    }

    /******
     * *****
     * ****** ON_POINTER_ENTER
     * *****
     *****/
    public void OnPointerEnter()
    {
        //if (UIManager.CardIsDragging || UIManager.ZoomCardIsCentered) return;
        if (DragDrop.CardIsDragging || ZoomCardIsCentered) return;

        float yPos;
        RectTransform rect;
        if (transform.parent.gameObject == playerHand)
        {
            rect = playerHand.GetComponent<RectTransform>();
            yPos = rect.position.y + ZOOM_BUFFER;
        }
        else if (transform.parent.gameObject == playerZone)
        {
            rect = playerZone.GetComponent<RectTransform>();
            yPos = rect.position.y + ZOOM_BUFFER;
        }
        else if (transform.parent.gameObject == enemyHand) return; // HIDE THE ENEMY HAND
        else if (transform.parent.gameObject == enemyZone)
        {
            rect = enemyZone.GetComponent<RectTransform>();
            yPos = (int)rect.position.y - ZOOM_BUFFER;
        }
        else return;
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CreateZoomCard(new Vector3(vec3.x, yPos), ZOOM_SCALE_VALUE);
    }

    /******
     * *****
     * ****** ON_POINTER_EXIT
     * *****
     *****/
    public void OnPointerExit()
    {
        if (ZoomCardIsCentered) return;
        UIManager.DestroyAllZoomObjects();
    }

    /******
     * *****
     * ****** CREATE_ZOOM_OBJECT
     * *****
     *****/
    private GameObject CreateZoomObject(GameObject prefab, Vector3 vec3, Transform parentTransform, float scaleValue)
    {
        GameObject zoomObject = Instantiate(prefab, vec3, Quaternion.identity);
        Transform popTran = zoomObject.transform;
        popTran.SetParent(parentTransform, true);
        popTran.position = new Vector3(popTran.position.x, popTran.position.y, vec3.z);
        popTran.localScale = new Vector2(scaleValue, scaleValue);
        return zoomObject;
    }

    /******
     * *****
     * ****** CREATE_ZOOM_CARD
     * *****
     *****/
    private void CreateZoomCard(Vector2 vec2, float scaleValue)
    {
        if (CurrentZoomCard != null) Destroy(CurrentZoomCard);
        GameObject cardPrefab = null;
        if (gameObject.GetComponent<CardDisplay>() is HeroCardDisplay) cardPrefab = HeroZoomCard;
        //else if (gameObject.GetComponent<CardDisplay>() is ActionCardDisplay) cardPrefab = ActionZoomCard;
        else Debug.Log("[CreateZoomCard() in CardZoom] CardDisplay TYPE NOT FOUND!");

        CurrentZoomCard = CreateZoomObject(cardPrefab, new Vector3(vec2.x, vec2.y, -4), background.transform, scaleValue);
        CurrentZoomCard.GetComponent<CardDisplay>().DisplayZoomCard(gameObject);
    }

    /******
     * *****
     * ****** CREATE_ZOOM_ABILLITY_ICON
     * *****
     *****/
    public void CreateZoomAbilityIcon(CardAbility cardAbility, Transform parentTransform, float scaleValue)
    {
        GameObject abilityIconPrefab = gameObject.GetComponent<HeroCardDisplay>().AbilityIconPrefab;
        //GameObject abilityIcon = CreateZoomObject(abilityIconPrefab, new Vector3(0, 0, 0), parentTransform, scaleValue);

        GameObject abilityIcon = Instantiate(abilityIconPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Transform popTran = abilityIcon.transform;
        popTran.SetParent(parentTransform, true);
        popTran.localScale = new Vector2(scaleValue, scaleValue);

        abilityIcon.GetComponent<ChangeLayer>().ZoomLayer();
        abilityIcon.layer = LayerMask.NameToLayer(ZOOM_LAYER);
        foreach (Transform child in abilityIcon.transform) child.gameObject.layer = LayerMask.NameToLayer(ZOOM_LAYER);

        abilityIcon.GetComponent<AbilityIconEvents>().IsZoomIcon = true;
        abilityIcon.GetComponent<AbilityIconDisplay>().AbilityScript = cardAbility;
    }

    /******
     * *****
     * ****** CREATE_NEXT_LEVEL_POPUP
     * *****
     *****/
    private void CreateNextLevelPopup(Vector2 vec2, float scaleValue, List<CardAbility> level2Abilities)
    {
        NextLevelPopup = CreateZoomObject(nextLevelBox, new Vector3(vec2.x, vec2.y, -4), background.transform, scaleValue);
        CreateZoomObject(level2Popup, new Vector2(0, 0), NextLevelPopup.transform, scaleValue / 3);
        foreach (CardAbility cardAbility in level2Abilities)
        {
            if (cardAbility == null) continue;
            CreateZoomAbilityIcon(cardAbility, NextLevelPopup.transform, scaleValue);
        }
    }

    /******
     * *****
     * ****** CREATE_LORE_POPUP
     * *****
     *****/
    private void CreateLorePopup(Vector2 vec2, float scaleValue)
    {
        LorePopup = CreateZoomObject(lorePopup, new Vector3(vec2.x, vec2.y, 0), background.transform, scaleValue);
        HeroCardDisplay heroCardDisplay = (HeroCardDisplay)cardDisplay;
        HeroCard heroCard = (HeroCard)heroCardDisplay.CardScript;
        LorePopup.GetComponent<LorePopupDisplay>().DisplayLorePopup(heroCard.HeroLore);
    }
}
