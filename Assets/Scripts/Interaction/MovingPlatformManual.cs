using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformManual : MovingPlatform, IDamageable
{
    public void DealDamage(float damage) {
        Activate();
    }    
}
