<h1 align="center">
  💎Drifter Deckbuilding Game💎
</h1>

<h2 align="center">
  A deckbuilding adventure game in corporate dystopia.
  <br>
  Created with <a href="https://www.unity.com">Unity</a>.
  <br><br>
  <a href="https://drifterthegame.com/" target="_blank">💠Play Drifter Now!💠</a>
</h2>

<h5 align="center">
  Wesley Coburn (Concept and Programming) => <a href="https://github.com/weslex555" target="_blank">GitHub</a>
  <br><br>
  Joe Rouverol (Art and Design) => <a href="https://www.instagram.com/dragonswordart/" target="_blank">Instagram</a>
  <br><br>
  Alden Muller (Soundtrack) => <a href="https://soundcloud.com/little_fields" target="_blank">SoundCloud</a>
  <br><br>
  <img src="https://i.imgur.com/YwqUa7z.jpg" alt="Drifter Deckbuilding Game" width="250" height="200">
</h5>

<h2 align="center">
  🚩Noteable Files🚩
</h2>

<h3 align="center">
  Managers
</h3>
  
<p align="center">  
  🔴
  <a href="Assets/Scripts/Managers/CardManager.cs" target="_blank">CardManager</a>
  🔴
  <a href="Assets/Scripts/Managers/CombatManager.cs" target="_blank">CombatManager</a>
  🔴
  <a href="Assets/Scripts/Managers/EffectManager.cs" target="_blank">EffectManager</a>
  🔴
</p>

> Manager classes are attached to gameObjects in *ManagerScene* and follow the **singleton** pattern:
```
  // Singleton Pattern
  
  public static CombatManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
  ```
    
<h3 align="center">
  Components
</h3>

> Display classes are attached to prefabs, which are loaded from a manager class.

<p align="center">
  🔴
  <a href="Assets/Scripts/Displays/Card Displays/CardPageDisplay.cs" target="_blank">CardPageDisplay</a>
  🔴
  <a href="Assets/Scripts/Cards/Card Displays/Card Displays/UnitCardDisplay.cs" target="_blank">UnitCardDisplay</a>
  🔴
  <a href="Assets/Scripts/Displays/Card Displays/CardPageDisplay.cs" target="_blank">CardPageDisplay</a>
  🔴
</p>
