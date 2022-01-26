﻿using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    /* SINGELTON_PATTERN */
    public static CardManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private PlayerManager pMan;
    private EnemyManager enMan;
    private AudioManager auMan;

    [Header("PREFABS")]
    [SerializeField] private GameObject unitCardPrefab;
    [SerializeField] private GameObject actionCardPrefab;

    [Header("PLAYER UNITS")]
    [SerializeField] private UnitCard[] playerStartUnits;
    [Header("RECRUIT UNITS")]
    [SerializeField] private List<UnitCard> playerRecruitMages;
    [SerializeField] private List<UnitCard> playerRecruitRogues;
    [SerializeField] private List<UnitCard> playerRecruitTechs;
    [SerializeField] private List<UnitCard> playerRecruitWarriors;
    
    [Header("TRIGGER")]
    [SerializeField] private CardAbility triggerKeyword;

    // Static Abilities
    public const string ABILITY_BLITZ = "Blitz";
    public const string ABILITY_FORCEFIELD = "Forcefield";
    public const string ABILITY_RANGED = "Ranged";
    public const string ABILITY_STEALTH = "Stealth";
    // Keyword Abilities
    public const string ABILITY_MARKED = "Marked";
    // Ability Triggers
    public const string TRIGGER_DEATHBLOW = "Deathblow";
    public const string TRIGGER_INFILTRATE = "Infiltrate";
    public const string TRIGGER_PLAY = "Play";
    public const string TRIGGER_RESEARCH = "Research";
    public const string TRIGGER_REVENGE = "Revenge";
    public const string TRIGGER_SPARK = "Spark";

    public static List<string> EvergreenTriggers = new List<string>
    {
        TRIGGER_DEATHBLOW,
        TRIGGER_INFILTRATE,
        TRIGGER_RESEARCH,
        TRIGGER_REVENGE,
        TRIGGER_SPARK,
    };

    public GameObject UnitCardPrefab { get => unitCardPrefab; }
    public GameObject ActionCardPrefab { get => actionCardPrefab; }
    public UnitCard[] PlayerStartUnits { get => playerStartUnits; }

    public List<UnitCard> PlayerRecruitMages { get => playerRecruitMages; }
    public List<UnitCard> PlayerRecruitRogues { get => playerRecruitRogues; }
    public List<UnitCard> PlayerRecruitTechs { get => playerRecruitTechs; }
    public List<UnitCard> PlayerRecruitWarriors { get => playerRecruitWarriors; }
    public List<UnitCard> PlayerRecruitUnits
    {
        get
        {
            List<UnitCard> returnList = new List<UnitCard>();
            List<List<UnitCard>> recruitLists = new List<List<UnitCard>>
            {
                playerRecruitMages,
                playerRecruitRogues,
                playerRecruitTechs,
                playerRecruitWarriors,
            };

            foreach (List<UnitCard> list in recruitLists)
            {
                foreach (UnitCard uc in list)
                {
                    if (uc == null) break;
                    int index = pMan.PlayerDeckList.FindIndex(x => x.CardName == uc.CardName);
                    if (index == -1)
                    {
                        returnList.Add(uc);
                        break;
                    }
                }
            }
            return returnList;
        }
    }

    public Card[] ChooseCards()
    {
        Card[] allChooseCards = Resources.LoadAll<Card>("Combat Rewards");
        if (allChooseCards.Length < 1)
        {
            Debug.LogError("NO CARDS FOUND!");
            return null;
        }

        allChooseCards.Shuffle();
        Card[] chooseCards = new Card[3];
        int index = 0;
        foreach (Card card in allChooseCards)
        {
            if (pMan.PlayerDeckList.FindIndex(x => x.CardName == card.CardName) == -1)
            {
                chooseCards[index++] = card;
                if (index == 3) break;
            }
        }
        return chooseCards;
    }

    public CardAbility TriggerKeyword { get => triggerKeyword; }

    private void Start()
    {
        pMan = PlayerManager.Instance;
        enMan = EnemyManager.Instance;
        auMan = AudioManager.Instance;
    }

    public void ShuffleRecruits()
    {
        List<List<UnitCard>> recruitLists = new List<List<UnitCard>>
        {
            playerRecruitMages,
            playerRecruitRogues,
            playerRecruitTechs,
            playerRecruitWarriors,
        };

        foreach (List<UnitCard> list in recruitLists)
            list.Shuffle();
    }

    /******
     * *****
     * ****** ADD/REMOVE_CARD
     * *****
     *****/
    public void AddCard(Card card, string hero)
    {
        List<Card> deck;
        Card cardInstance;
        if (hero == GameManager.PLAYER) deck = PlayerManager.Instance.PlayerDeckList;
        else if (hero == GameManager.ENEMY) deck = EnemyManager.Instance.EnemyDeckList;
        else
        {
            Debug.LogError("HERO NOT FOUND!");
            return;
        }
        if (card is UnitCard) cardInstance = ScriptableObject.CreateInstance<UnitCard>();
        else if (card is ActionCard) cardInstance = ScriptableObject.CreateInstance<ActionCard>();
        else
        {
            Debug.LogError("CARD TYPE NOT FOUND!");
            return;
        }
        cardInstance.LoadCard(card);
        deck.Add(cardInstance);
    }
    public void RemovePlayerCard(Card card) =>
        PlayerManager.Instance.PlayerDeckList.Remove(card);
    
    /******
     * *****
     * ****** SHUFFLE_DECK
     * *****
     *****/
    public void ShuffleDeck(string hero, bool playSound = true)
    {
        Debug.LogWarning("SHUFFLE <" + hero + "> DECK!");
        List<Card> deck;
        if (hero == GameManager.PLAYER)
            deck = pMan.CurrentPlayerDeck;
        else if (hero == GameManager.ENEMY)
        {
            deck = enMan.CurrentEnemyDeck;
            playSound = false;
        }
        else
        {
            Debug.LogError("INVALID HERO!");
            return;
        }
        deck.Shuffle();

        if (playSound)
            auMan.StartStopSound("SFX_ShuffleDeck");
    }

    /******
     * *****
     * ****** UPDATE_DECK
     * *****
     *****/
    public void UpdateDeck(string hero)
    {
        List<Card> deckList;
        List<Card> currentDeck;

        if (hero == GameManager.PLAYER)
        {
            deckList = pMan.PlayerDeckList;
            currentDeck = pMan.CurrentPlayerDeck;
        }
        else if (hero == GameManager.ENEMY)
        {
            deckList = enMan.EnemyDeckList;
            currentDeck = enMan.CurrentEnemyDeck;
        }
        else
        {
            Debug.LogError("HERO NOT FOUND!");
            return;
        }

        currentDeck.Clear();
        foreach (Card card in deckList)
            currentDeck.Add(card);
        currentDeck.Shuffle();
    }

    /******
     * *****
     * ****** CARD_ABILITIES
     * *****
     *****/
    public static bool GetAbility(GameObject unitCard, string ability)
    {
        if (unitCard == null)
        {
            Debug.LogError("CARD IS NULL!");
            return false;
        }

        if (!unitCard.TryGetComponent(out UnitCardDisplay ucd))
        {
            Debug.LogError("TARGET IS NOT UNIT CARD!");
            return false;
        }

        int abilityIndex = ucd.CurrentAbilities.FindIndex(x => x.AbilityName == ability);
        if (abilityIndex == -1) return false;
        else return true;
    }
    public static bool GetTrigger(GameObject card, string triggerName)
    {
        if (card == null)
        {
            Debug.LogError("CARD IS NULL!");
            return false;
        }

        foreach (CardAbility ca in card.GetComponent<UnitCardDisplay>().CurrentAbilities)
            if (ca is TriggeredAbility tra)
                if (tra.AbilityTrigger.AbilityName == triggerName) return true;
        return false;
    }
    public bool TriggerUnitAbility(GameObject unitCard, string triggerName)
    {
        if (unitCard == null)
        {
            Debug.LogError("CARD IS NULL!");
            return false;
        }

        if (!unitCard.TryGetComponent(out UnitCardDisplay ucd))
        {
            Debug.LogError("TARGET IS NOT UNIT CARD!");
            return false;
        }

        bool effectFound = false;
        foreach (CardAbility ca in ucd.CurrentAbilities)
            if (ca is TriggeredAbility tra)
                if (tra.AbilityTrigger.AbilityName == triggerName)
                {
                    Debug.Log("TRIGGER! <" + triggerName + ">");
                    EventManager.Instance.NewDelayedAction(() =>
                    EffectManager.Instance.StartEffectGroupList(tra.EffectGroupList, unitCard, triggerName), 0.5f, true);
                    effectFound = true;
                }
        return effectFound;
    }
    public bool TriggerPlayedUnits(string triggerName)
    {
        bool triggerFound = false;
        foreach (GameObject unit in CombatManager.Instance.PlayerZoneCards)
        {
            if (TriggerUnitAbility(unit, triggerName))
                triggerFound = true;
        }
        return triggerFound;
    }
}