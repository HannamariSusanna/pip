using UnityEngine;

public class BoostAbility : Ability {

    public float boostSpeed = 3f;

    new void Update() {
        base.Update();
        if (isActive) {
            player.body.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
            isActive = false;
        }
        if (player.body.velocity.sqrMagnitude <= player.GetSqrMaxVelocity() && player.animator.GetBool("BoostActive")) {
            player.animator.SetBool("BoostActive", false);
        }
    }

    public override void Use() {
        if (currentEnergy >= energyCost) {
            player.animator.SetBool("BoostActive", true);
            isActive = true;
            currentEnergy -= energyCost;
            energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
            if (currentEnergy < energyCost) {
                energyBar.Disabled();
            }
        }
    }
}