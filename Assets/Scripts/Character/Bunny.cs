using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunny : Player
{
  new void Start() {
    base.Start();
    ability.SetMaxEnergy(maxEnergy);
    ability.SetRechargeRate(energyRechargeRate);
  }
}
