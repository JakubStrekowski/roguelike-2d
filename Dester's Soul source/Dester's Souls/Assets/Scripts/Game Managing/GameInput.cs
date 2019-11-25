using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput:MonoBehaviour
{
    private bool moveFinished;
    private float moveTimeStamp;
    private float moveCooldown = 0.25f;

    public TurnManager turnManager;

    private void Awake()
    {
        moveFinished = true;
        moveTimeStamp = Time.time;
        turnManager = gameObject.GetComponent<TurnManager>();
    }

    private void Update()
    {
        if (moveTimeStamp < Time.time&&!moveFinished)
        {
            moveFinished = true;
        }
        if (moveFinished)
        {
            turnManager.PlayInMap();
        }
    }

    private void KeyUsed()
    {
        moveFinished = false;
        moveTimeStamp = Time.time + moveCooldown;
    }

    public string TakeInput()
    {

        if (Input.GetKey(KeyCode.UpArrow))
        {
            KeyUsed();
            return "ArrowUp";
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            KeyUsed();
            return "ArrowDown";
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            KeyUsed();
            return "ArrowRight";
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            KeyUsed();
            return "ArrowLeft";
        }
        if (Input.GetKey(KeyCode.Space))
        {
            KeyUsed();
            return "Space";
        }
        if (Input.GetKey(KeyCode.Alpha1)|| Input.GetKeyDown(KeyCode.Keypad1))
        {
            KeyUsed();
            return "1";
        }
        if (Input.GetKey(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            KeyUsed();
            return "2";
        }
        if (Input.GetKey(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            KeyUsed();
            return "3";
        }
        if (Input.GetKey(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            KeyUsed();
            return "4";
        }
        if (Input.GetKey(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            KeyUsed();
            return "5";
        }
        if (Input.GetKey(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            KeyUsed();
            return "6";
        }
        if (Input.GetKey(KeyCode.R))
        {
            KeyUsed();
            return "R";
        }
        return null;
    }

    
}
