using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkeletonMoveState
{
    public abstract void SkeletonMove(Skeleton context);
    public abstract void SkeletonInit(Skeleton context);
}
