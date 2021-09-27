using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroSelectSceneDisplay : MonoBehaviour
{
    [SerializeField] private GameObject selectedHero;
    [SerializeField] private GameObject skillCard_1;
    [SerializeField] private GameObject skillCard_2;
    [SerializeField] private GameObject heroPortrait;
    [SerializeField] private GameObject heroName;
    [SerializeField] private GameObject heroDescription;
    [SerializeField] private GameObject heroPowerImage;
    [SerializeField] private GameObject heroPowerDescription;
    [SerializeField] private GameObject selectedAugment;
    [SerializeField] private GameObject augmentName;
    [SerializeField] private GameObject augmentImage;
    [SerializeField] private GameObject augmentDescription;

    [SerializeField] private GameObject confirmSelection;
    [SerializeField] private GameObject confirmHeroPortrait;
    [SerializeField] private GameObject confirmHeroName;
    [SerializeField] private GameObject confirmAugmentImage;
    [SerializeField] private GameObject confirmAugmentName;

    [SerializeField] private PlayerHero[] playerHeroes;
    [SerializeField] private HeroAugment[] heroAugments;

    private PlayerManager pMan;
    private CombatManager coMan;
    private UIManager uMan;
    private GameObject currentSkill_1;
    private GameObject currentSkill_2;
    private int currentSelection;
    private bool heroSelected;
    private bool augmentSelected;

    private PlayerHero SelectedHero { get => playerHeroes[currentSelection]; }
    private HeroAugment SelectedAugment { get => heroAugments[currentSelection]; }

    private void Start()
    {
        pMan = PlayerManager.Instance;
        coMan = CombatManager.Instance;
        uMan = UIManager.Instance;
        currentSelection = 0;
        heroSelected = false;
        augmentSelected = false;
    }

    private void LoadSelections()
    {
        PlayerHero newPH = ScriptableObject.CreateInstance<PlayerHero>();
        newPH.LoadHero(SelectedHero);
        pMan.PlayerHero = newPH;

        HeroAugment ha = SelectedAugment;
        pMan.HeroAugments.Add(ha);
    }

    public void ConfirmButton_OnClick()
    {
        if (!heroSelected)
        {
            heroSelected = true;
            currentSelection = 0;
            DisplaySelectedAugment();
        }
        else if (!augmentSelected)
        {
            augmentSelected = true;
            DisplayConfirmSelection();
        }
        else
        {
            LoadSelections();
            GameManager.Instance.NextNarrative = pMan.PlayerHero.HeroBackstory;
            SceneLoader.LoadScene(SceneLoader.Scene.NarrativeScene);
        }
    }
    public void BackButton_OnClick()
    {
        if (augmentSelected)
        {
            augmentSelected = false;
            currentSelection = 0;
            DisplaySelectedAugment();
        }
        else if (heroSelected)
        {
            heroSelected = false;
            currentSelection = 0;
            DisplaySelectedHero();
        }
        else GameManager.Instance.EndGame();
    }
    public void RightArrow_OnClick() => NextSelection(RightOrLeft.Right);
    public void LeftArrow_OnClick() => NextSelection(RightOrLeft.Left);
    public enum RightOrLeft { Right, Left }
    private void NextSelection(RightOrLeft rol)
    {
        int lastSelection;
        if (!heroSelected) lastSelection = playerHeroes.Length - 1;
        else lastSelection = heroAugments.Length - 1;
        
        if (rol == RightOrLeft.Right)
        { if (++currentSelection > lastSelection) currentSelection = 0; }
        else
        { if (--currentSelection < 0) currentSelection = lastSelection; }
        
        if (!heroSelected) DisplaySelectedHero();
        else DisplaySelectedAugment();
    }

    private void DisplaySelectedAugment()
    {
        confirmSelection.SetActive(false);
        selectedAugment.SetActive(true);
        selectedHero.SetActive(false);
        skillCard_1.SetActive(false);
        skillCard_2.SetActive(false);

        augmentName.GetComponent<TextMeshProUGUI>().SetText(SelectedAugment.AugmentName);
        augmentImage.GetComponent<Image>().sprite = SelectedAugment.AugmentImage;
        augmentDescription.GetComponent<TextMeshProUGUI>().SetText(SelectedAugment.AugmentDescription);
    }

    public void DisplaySelectedHero()
    {
        confirmSelection.SetActive(false);
        selectedAugment.SetActive(false);
        selectedHero.SetActive(true);
        skillCard_1.SetActive(true);
        skillCard_2.SetActive(true);

        heroName.GetComponent<TextMeshProUGUI>().SetText(SelectedHero.HeroName);
        heroPortrait.GetComponent<Image>().sprite = SelectedHero.HeroPortrait;
        uMan.GetPortraitPosition(SelectedHero.HeroName, 
            out Vector2 position, out Vector2 scale, SceneLoader.Scene.HeroSelectScene);
        heroPortrait.transform.localPosition = position;
        heroPortrait.transform.localScale = scale;

        heroDescription.GetComponent<TextMeshProUGUI>().SetText(SelectedHero.HeroDescription);
        heroPowerImage.GetComponent<Image>().sprite = SelectedHero.HeroPower.PowerSprite;
        heroPowerImage.GetComponentInParent<PowerZoom>().LoadedPower = SelectedHero.HeroPower;

        Sound[] snd = SelectedHero.HeroPower.PowerSounds;
        foreach (Sound s in snd) AudioManager.Instance.StartStopSound(null, s);

        int cost = SelectedHero.HeroPower.PowerCost;
        string actions;
        if (cost > 1) actions = "actions";
        else actions = "action";
        string description = " (" + cost + " " + actions + ", 1/turn): ";

        heroPowerDescription.GetComponent<TextMeshProUGUI>().SetText(SelectedHero.HeroPower.PowerName +
            description + SelectedHero.HeroPower.PowerDescription);

        if (currentSkill_1 != null)
        {
            Destroy(currentSkill_1);
            currentSkill_1 = null;
        }
        if (currentSkill_2 != null)
        {
            Destroy(currentSkill_2);
            currentSkill_2 = null;
        }

        Vector2 vec2 = new Vector2();
        currentSkill_1 = coMan.ShowCard(SelectedHero.HeroStartSkills[0], vec2);
        currentSkill_2 = coMan.ShowCard(SelectedHero.HeroStartSkills[1], vec2);
        currentSkill_1.transform.SetParent(skillCard_1.transform, false);
        currentSkill_2.transform.SetParent(skillCard_2.transform, false);
        Vector2 scaleVec = new Vector2(4, 4);
        currentSkill_1.transform.localScale = scaleVec;
        currentSkill_2.transform.localScale = scaleVec;
    }

    private void DisplayConfirmSelection()
    {
        confirmSelection.SetActive(true);
        selectedAugment.SetActive(false);
        selectedHero.SetActive(false);
        skillCard_1.SetActive(false);
        skillCard_2.SetActive(false);

        confirmHeroName.GetComponent<TextMeshProUGUI>().SetText(SelectedHero.HeroName);
        confirmHeroPortrait.GetComponent<Image>().sprite = SelectedHero.HeroPortrait;
        uMan.GetPortraitPosition(SelectedHero.HeroName,
            out Vector2 position, out Vector2 scale, SceneLoader.Scene.HeroSelectScene);
        confirmHeroPortrait.transform.localPosition = position;
        confirmHeroPortrait.transform.localScale = scale;

        confirmAugmentName.GetComponent<TextMeshProUGUI>().SetText(SelectedAugment.AugmentName);
        confirmAugmentImage.GetComponent<Image>().sprite = SelectedAugment.AugmentImage;
    }
}
