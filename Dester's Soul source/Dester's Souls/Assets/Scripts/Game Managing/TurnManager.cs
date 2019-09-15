using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Controllers
    {
        player,mainMenu,subMenu,skillTargetting
    }

    public GameInput gameInput;
    public PlayerCharacter hero;
    public List<Enemy> enemyList;
    public Camera mainCamera;

    private Map currentMap;
    private bool isHeroAlife;
    private bool hasTurnEnded;
    private Controllers currentController;
    // Start is called before the first frame update
    void Start()
    {
        isHeroAlife = true;
        hasTurnEnded = true;
        GenerateRandom(GameManager._instance.CurrentLevel);
        mainCamera.transform.position = new Vector3(hero.transform.position.x, hero.transform.position.y, -10);
        mainCamera.GetComponent<SmoothCamera>().target = hero.gameObject.transform;
        PlayInMap();
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
        foreach(Enemy enemy in enemyList)
        {
             enemy.MovementBehaviour();
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
                    /*
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
                    */
                case "Escape":
                case "Q":
                    {
                        currentController = Controllers.mainMenu;
                    }
                    return false;
                default: return false;
            }
        }
        /*
        if (currentController == Controllers.subMenu)
        {
            switch (inputCommand)
            {
                case "E":
                    {
                        Environment.Exit(0);
                        break;
                    }
                case "Escape":
                case "C":
                    {
                        hero.currentMap.MoveFocus(hero);
                        whatInControl = 0;
                        break;
                    }
                default:
                    break;
            }
            return false;
        }
        */
        else return false;
    }


    public Map GenerateRandom(int floorNumber)
    {
        System.Random rnd = new System.Random();
        DungeonStruct dungeonStructure = new DungeonStruct(300,150);
        dungeonStructure = SetDungeonSize(floorNumber);
        Map newMap = new Map(dungeonStructure, dungeonStructure.dungeon.Length, dungeonStructure.dungeon[0].Length,this);
        currentMap = newMap;
        /*
        display.SetStatUI(1, hero.name);
        display.SetStatUI(2, hero.hp.ToString());
        display.SetStatUI(6, gold.ToString());
        display.SetStatUI(7, enemiesKilled.ToString());
        */
        bool displayed = false;
        /*
        for (int i = 0; i < 6; i++)
            if (hero.equipment[i] != null)
            {
                display.RefreshItem(i, hero.equipment[i].name);
                displayed = true;
                break;
            }
        if (!displayed)
        {
            display.RefreshItem(-1, "Whatever");
        }
        */
        currentController = Controllers.player;
        return newMap;
    }

    private DungeonStruct SetDungeonSize(int floorNumber)
    {
        LevelGenerator mapGenerator = new LevelGenerator(100, 50);
        int rowAmmount = 0;
        int columnAmmount = 0;
        switch (floorNumber)
        {
            case 1:
                rowAmmount = 25;
                columnAmmount = 50;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 8);
            case 2:
                rowAmmount = 35;
                columnAmmount = 70;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 13);
            case 3:
                rowAmmount = 45;
                columnAmmount = 85;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 16);
            case 4:
                rowAmmount = 150;
                columnAmmount = 300;
                return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 130);
        }
        return mapGenerator.CreateDungeon(columnAmmount, rowAmmount, 20); ;
    }

    public void RemoveEnemyFromList(Enemy enemy)
    {
        enemyList.Remove(enemy);
    }
}
