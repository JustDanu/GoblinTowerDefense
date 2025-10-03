using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSummonBehaviour : Tower
{
    protected override void Start()
    {
        base.Start();
        Destroy(gameObject, 10f);
    }

    public override void sell()
    {
        //Nuhuh
    }
}
