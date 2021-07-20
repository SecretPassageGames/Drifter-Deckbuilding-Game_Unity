﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    /* SINGELTON_PATTERN */
    public static PlayerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        PlayerDeckList = new List<Card>();
        CurrentPlayerDeck = new List<Card>();
        HeroPowerUsed = false;
    }

    /* PLAYER_HERO */
    public Hero PlayerHero
    {
        get => playerHero;
        set
        {
            playerHero = value;
            for (int i = 0; i < GameManager.PLAYER_START_FOLLOWERS; i++)
            {
                CardManager.Instance.AddCard(CardManager.Instance.StartPlayerUnit_1, GameManager.PLAYER);
                CardManager.Instance.AddCard(CardManager.Instance.StartPlayerUnit_2, GameManager.PLAYER);
            }
            foreach (SkillCard skill in PlayerHero.HeroSkills)
            {
                for (int i = 0; i < GameManager.PLAYER_START_SKILLS; i++)
                {
                    CardManager.Instance.AddCard(skill, GameManager.PLAYER);
                }
            }
        }
    }
    private Hero playerHero;

    /* PLAYER_DECK */
    public List<Card> PlayerDeckList { get; private set; }
    public List<Card> CurrentPlayerDeck { get; private set; }

    /* IS_MY_TURN */
    public bool IsMyTurn { get; set; }

    /* ACTIONS_PER_TURN */
    public int ActionsPerTurn { get; set; }

    /* HEALTH */
    private int playerHealth;
    public int PlayerHealth
    {
        get => playerHealth;
        set
        {
            playerHealth = value;
            UIManager.Instance.UpdatePlayerHealth(PlayerHealth);
        }
    }

    /* ACTIONS_LEFT */
    private int playerActionsLeft;
    public int PlayerActionsLeft
    {
        get => playerActionsLeft;
        set
        {
            playerActionsLeft = value;
            if (playerActionsLeft > GameManager.MAXIMUM_ACTIONS) playerActionsLeft = GameManager.MAXIMUM_ACTIONS;
            UIManager.Instance.UpdatePlayerActionsLeft(PlayerActionsLeft);
        }
    }

    /* HERO_POWER */
    public bool HeroPowerUsed { get; set; }
    public void UseHeroPower()
    {
        Debug.Log("USE HERO POWER: " + PlayerHero.HeroPower.PowerName);
        if (HeroPowerUsed == true)
        {
            Debug.Log("HERO POWER ALREADY USED THIS TURN!");
            return;
        }
        else if (PlayerActionsLeft < 1)
        {
            Debug.Log("NOT ENOUGH ACTIONS!");
            return;
        }
        else
        {
            PlayerActionsLeft -= 1;
            HeroPowerUsed = true;
            EffectManager.Instance.StartEffectGroupList(PlayerHero.HeroPower.EffectGroupList, CardManager.Instance.PlayerHero);
        }
    }
}
