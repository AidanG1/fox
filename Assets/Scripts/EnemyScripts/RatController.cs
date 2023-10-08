using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatController : EnemyController
{
    public void Start()
    {
        getBounds(true);
    }

    // Update is called once per frame
    void Update()
    {
        movementLeftRight();
    }

}
