using UnityEngine;

public class ForceFieldAbility : Ability {

    public int activationEnergyRequirement = 100;
    public float consumeRate = 50f;
    public ParticleSystem ps;
    public Collider2D shieldCollider;
    public Rigidbody2D shieldBody;

    void Start() {
        DisableForceField();
        var main = ps.main;
        main.startLifetime = maxEnergy / consumeRate;
    }

    new void Update() {
        base.Update();
        if (isActive) {
            ConsumeEnergy(consumeRate * Time.deltaTime);
        }
    }

    public override void Use() {
        if (!isActive && currentEnergy >= activationEnergyRequirement) {
            shieldCollider.enabled = true;
            isActive = true;
            shieldBody.mass = 1000f;
            energyBar.Disabled();
            ps.Play();
        }
    }

    private void ConsumeEnergy(float amount) {
        currentEnergy -= amount;
        energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
        if (currentEnergy < amount) {
            DisableForceField();
        }
    }

    private void DisableForceField() {
        isActive = false;
        ps.Stop();
        shieldCollider.enabled = false;
        shieldBody.mass = 0.0001f;
    }
}