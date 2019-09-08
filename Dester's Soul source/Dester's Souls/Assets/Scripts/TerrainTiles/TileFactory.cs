using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFactory : MonoBehaviour
{
    public GameObject[] palette;
    public GameObject Get(int id, int posX, int posY, Map mp)
    {
        GameObject gObj = Instantiate(palette[id], new Vector3(posX, posY,0), Quaternion.identity);
        IPositionInitializer ipi = gObj.GetComponent(typeof(IPositionInitializer)) as IPositionInitializer;
        ipi.InitializePosition(posX, posY, mp);
        return gObj;
    }
}
