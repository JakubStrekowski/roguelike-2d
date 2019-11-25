using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RatMoveState
{
    public abstract void RatMove(Rat context);
    public abstract void RatInit(Rat context);
}
