using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public enum Controllers
    {
        player,mainMenu,subMenu,skillTargetting,gameLoading
    }

    public GameInput gameInput;
    public PlayerCharacter hero;
    public List<Enemy> enemyList;
    public Camera mainCamera;
    public Controllers currentController;


    //UI
    public Image[] heartContainers;
    public ItemSlot[] itemSlots;
    public GameObject deathPanel;
    public GameObject loadingScreen;
    public TextMeshProUGUI goldAmnt;
    public TextMeshProUGUI enemyKilledAmnt;
    public ScreenEffects screenEffects;
    public GameObject debugLogPanel;
    public TextMeshProUGUI yourScore;
    public TextMeshProUGUI highScore;

    private Map currentMap;
    private bool isHeroAlife;
    private bool hasTurnEnded;
    private DebugLogManager debugLogManager;

    private bool playAfterInit = false;
    // Start is called before the first frame update
    void Start()
    {
        enemyList.Clear();
        GameManager._instance.OnGoldChange+= UpdateHeroGoldGUI;
        GameManager._instance.OnKillsChange += UpdateHeroKillsGUI;
        isHeroAlife = true;
        hasTurnEnded = true;
        loadingScreen.SetActive(true);
        debugLogManager = GetComponent<DebugLogManager>();
        currentController = Controllers.gameLoading;
        StartCoroutine(GenerateRandom(GameManager._instance.CurrentLevel));
        if (GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled == 0)
        {
            debugLogPanel.SetActive(false);
        }
    }

    public void PlayInMap()
    {
        if (isHeroAlife)
        {
            if (ResolvePlayerMovement())
            {
                ResolveTurn();
                currentMap.FOV(hero);
            }
        }
    }


    void ResolveTurn()
    {
        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].MovementBehaviour();
        }
    }

        private bool ResolvePlayerMovement()
    {
        if (currentController == Controllers.player)
        {
            switch (gameInput.TakeInput())
            {
                case null:return false;
                case "ArrowUp":
                    {
                        hero.Move(Character.Directions.up);
                    }
                    return true;
                case "ArrowDown":
                    {
                        hero.Move(Character.Directions.down);
                    }
                    return true;
                case "ArrowRight":
                    {
                        hero.Move(Character.Directions.right);
                    }
                    return true;
                case "ArrowLeft":
                    {
                        hero.Move(Character.Directions.left);
                    }
                    return true;

                case "Space":
                    hero.RefreshVision();
                    return true;

                case "1":
                    {
                        hero.UseItem(1);
                        return true;
                    }
                case "2":
                    {
                        hero.UseItem(2);
                        return true;
                    }
                case "3":
                    {
                        hero.UseItem(3);
                        return true;
                    }
                case "4":
                    {
                        hero.UseItem(4);
                        return true;
                    }
                case "5":
                    {
                        hero.UseItem(5);
                        return true;
                    }
                case "6":
                    {
                        hero.UseItem(6);
                        return true;
                    }
                    
                case "Escape":
                case "Q":
                    {
                        currentController = Controllers.mainMenu;
                    }
                    return false;
                default: return false;
            }
        }
        
        if (currentController == Controllers.subMenu)
        {
            switch (gameInput.TakeInput())
            {
                case "R":
                    {
                        GameManager._instance.ResetGame();
                        break;
                    }
                case "Escape":
                default:
                    break;
            }
            return false;
        }
        
        else return false;
    }


    public IEnumerator GenerateRandom(int floorNumber)
    {
        Stopwatch stopWatch = Stopwatch.StartNew();
        System.Random rnd = new System.Random();
        DungeonStruct dungeonStructure = new DungeonStruct(300,150);
        yield return null;
        dungeonStructure = SetDungeonSize(floorNumber);
        Map newMap = new Map(dungeonStructure, dungeonStructure.dungeon.Length, dungeonStructure.dungeon[0].Length,this);
        currentMap = newMap;
        mainCamera.transform.position = new Vector3(hero.transform.position.x, hero.transform.position.y, -10);
        mainCamera.GetComponent<SmoothCamera>().target = hero.gameObject.transform.GetChild(0);
        UpdateHeroHealthGUI();
        UpdateHeroGoldGUI();
        UpdateHeroKillsGUI();
        UpdateHeroItemSlots();
        PlayInMap();
        loadingScreen.GetComponent<LoadingScreen>().FadeOut();
        currentController = Controllers.player;
        stopWatch.Stop();
        debugLogManager.AddLog("World generated in: " + stopWatch.Elapsed.TotalMilliseconds.ToString() + "ms");
        yield return null;
    }

    private DungeonStruct SetDungeonSize(int floorNumber)
    {
        LevelGenerator mapGenerator = new LevelGenerator(100, 50);
        int rowAmmount = 0;
        int columnAmmount = 0;
        switch (floorNumber)
        {
            case 1:
                rowAmmount = 35;
                columnAmmount = 70;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 12);
            case 2:
                rowAmmount = 50;
                columnAmmount = 100;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 18);
            case 3:
                rowAmmount = 70;
                columnAmmount = 120;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 24);
            case 4:
                rowAmmount = 80;
                columnAmmount = 100;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 30);
        }
        rowAmmount = 80;
        columnAmmount = 100;
        return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 20); ;
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        GameManager._instance.EnemiesKilled++;
        enemyList.Remove(enemy);
    }

    public void UpdateHeroHealthGUI()
    {
        for(int i = 0; i < hero.HealthPoints; i++)
        {
            heartContainers[i].GetComponent<HeartContainer>().Activate();
        }
        for(int i = hero.HealthPoints; i < heartContainers.Length; i++)
        {
            heartContainers[i].GetComponent<HeartContainer>().Deactivate();
        }
        if (hero.HealthPoints < 2)
        {
            screenEffects.StartHeroLowHealthEffect();
        }
        else
        {
            screenEffects.StopHeroLowHealthEffect();
        }
    }

    public void HeroHitEffectGUI()
    {
        screenEffects.PlayHitEffect();
    }

    public void HeroHealEffectGUI()
    {
        if (!playAfterInit) playAfterInit=true;
        else screenEffects.PlayHealEffect();

    }

    public void UpdateHeroDeathGUI()
    {
        if(hero.IsDead == true)
        {
            deathPanel.SetActive(true);
            currentController = Controllers.subMenu;
            int score = (GameManager._instance.EnemiesKilled + GameManager._instance.CollectedGold * 10);
            yourScore.SetText("Your Score: " + score.ToString());
            if (score > GameManager._instance.GetComponent<GameDataManager>().BestScore)
            {
                GameManager._instance.GetComponent<GameDataManager>().BestScore = score;
            }
            highScore.SetText("Best Score: " + GameManager._instance.GetComponent<GameDataManager>().BestScore.ToString());
        }
    }

    public void UpdateHeroGoldGUI()
    {
        goldAmnt.text = "x" + GameManager._instance.CollectedGold;
    }

    public void UpdateHeroKillsGUI()
    {
        enemyKilledAmnt.text = "x" + GameManager._instance.EnemiesKilled;
    }

    public void UpdateHeroItemSlots()
    {
        for(int i = 0; i < 6; i++)
        {
            if(hero.equipment[i] is null)
            {
                itemSlots[i].DisableItem();
            }
            else
            {
                itemSlots[i].EnableItem(hero.equipment[i].GetComponent<SpriteRenderer>().sprite);
            }
        }
    }

    public void AddLog(string message,string fileMessage="")
    {
        if (GameManager._instance.GetComponent<GameDataManager>().DebugConsoleEnabled == 1)
            debugLogManager.AddLog(message,fileMessage);
    }
}
