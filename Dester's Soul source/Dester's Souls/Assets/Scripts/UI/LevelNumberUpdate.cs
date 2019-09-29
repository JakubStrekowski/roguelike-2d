using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNumberUpdate : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text="Level: "+GameManager._instance.CurrentLevel.ToString();
    }
}
