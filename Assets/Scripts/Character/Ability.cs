using UnityEngine;
public abstract class Ability: MonoBehaviour {
    public EnergyBar energyBar;
    public Rigidbody2D playerBody;
    public float energyCost;

    protected float maxEnergy;
    protected float currentEnergy;
    protected float rechargeRate;
    protected bool isActive = false;

    public abstract void Use();

    protected void Update() {
        RechargeEnergy();
    }

    public void RechargeEnergy() {
        if (!isActive && currentEnergy < maxEnergy) {
            currentEnergy += rechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy);
            energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
            if (currentEnergy >= energyCost) {
                energyBar.Enabled();
            }
        }
    }

    public void SetMaxEnergy(float maxEnergy) {
        this.maxEnergy = maxEnergy;
        this.currentEnergy = maxEnergy;
    }

    public void SetRechargeRate(float rechargeRate) {
        this.rechargeRate = rechargeRate;
    }
}