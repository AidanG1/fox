using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : EnemyController
{
    public void Start()
    {
        GetBounds(true);
    }

    void Update()
    {
        MovementLeftRight();
    }
}
