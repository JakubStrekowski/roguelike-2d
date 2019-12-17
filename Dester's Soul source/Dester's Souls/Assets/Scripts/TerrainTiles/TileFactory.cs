using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject[] tilePalette;
    public GameObject[] characerPalette;
    public GameObject[] itemPalette;

    TurnManager turnManager;



    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            turnManager = GameObject.Find("DungeonManager").GetComponent<TurnManager>();
            Debug.Log(turnManager.ToString());
        }
    }

    /*Tile Factory tile ids:
     * 0-9 floor variants
     * 10-19 wall variants
     * 20-24 staris
     * 
     * */

    public GameObject GetTile(int id, int posX, int posY, Map mp)
    {
        GameObject gObj = Instantiate(tilePalette[id], new Vector3(posX, posY, 0), Quaternion.identity);
        IPositionInitializer ipi = gObj.GetComponent(typeof(IPositionInitializer)) as IPositionInitializer;
        ipi.InitializePosition(posX, posY, mp, turnManager);
        return gObj;
    }

    /*Tile Factory character ids:
     * 0 empty
     * 1 player
     * 2 skeleton
     * 3 zombie
     * 4 slime
     * 5 rat
     * 
     * */
    public GameObject GetCharacter(int id, int posX, int posY, Map mp)
    {
        GameObject gObj = Instantiate(characerPalette[id], new Vector3(posX, posY, 0), Quaternion.identity);
        IPositionInitializer ipi = gObj.GetComponent(typeof(IPositionInitializer)) as IPositionInitializer;
        ipi.InitializePosition(posX, posY, mp, turnManager);
        return gObj;
    }

    /*Tile Factory items ids:
     * 0 empty
     * 1 coin
     * 2 healthPotion
     * 
     * */
    public GameObject GetItem(int id, int posX, int posY, Map mp)
    {
        GameObject gObj = Instantiate(itemPalette[id], new Vector3(posX, posY, 0), Quaternion.identity);
        IPositionInitializer ipi = gObj.GetComponent(typeof(IPositionInitializer)) as IPositionInitializer;
        ipi.InitializePosition(posX, posY, mp,turnManager);
        return gObj;
    }


}
