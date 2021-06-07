using UnityEngine;

public class BoostAbility : Ability {

    public float boostSpeed = 3f;

    new void Update() {
        base.Update();
        if (isActive) {
            playerBody.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
            isActive = false;
        }
    }

    public override void Use() {
        if (currentEnergy >= energyCost) {
            isActive = true;
            currentEnergy -= energyCost;
            energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
            if (currentEnergy < energyCost) {
                energyBar.Disabled();
            }
        }
    }
}