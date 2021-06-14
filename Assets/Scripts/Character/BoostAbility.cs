using UnityEngine;

public class BoostAbility : Ability {

    public float boostSpeed = 3f;
    private static float boostToNormalThreshold = 0.2f;

    new void Update() {
        base.Update();
        if (!isActive && player.body.velocity.magnitude - player.GetMaxVelocity() <= boostToNormalThreshold && player.animator.GetBool("BoostActive")) {
            player.animator.SetBool("BoostActive", false);
        }
    }

    void FixedUpdate() {
        if (isActive) {
            player.body.AddForce(transform.right * boostSpeed, ForceMode2D.Impulse);
            isActive = false;
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

    public override string GetName()
    {
        return "Boost";
    }
}