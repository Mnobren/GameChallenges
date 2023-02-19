using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : EnemyBehaviour
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        moveSpeed = 3f;
        turnSpeed = 1f;
        award = 10;

        base.Awake();
    }
}
