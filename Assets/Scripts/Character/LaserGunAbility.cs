using UnityEngine;

public class LaserGunAbility : Ability {

    public int activationEnergyRequirement = 100;
    public float consumeRate = 50f;
    public LineRenderer lr;

    new void Start() {
        base.Start();
    }

    new void Update() {
        base.Update();
    }

    public override void Use() {
        if (!isActive && currentEnergy >= activationEnergyRequirement) {
            isActive = true;
        }
    }

    public override string GetName()
    {
        return "Laser Gun";
    }
}