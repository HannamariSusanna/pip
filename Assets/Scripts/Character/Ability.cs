using UnityEngine;
public abstract class Ability: MonoBehaviour {
    public EnergyBar energyBar;
    public float energyCost;

    protected Player player;
    protected float currentEnergy;
    protected bool isActive = false;

    public abstract void Use();

    protected void Start() {
        currentEnergy = player.maxEnergy;
    }

    protected void Update() {
        RechargeEnergy();
    }

    public void SetPlayer(Player player) {
        this.player = player;
    }

    public void RechargeEnergy() {
        if (!isActive && currentEnergy < player.maxEnergy) {
            currentEnergy += player.energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Min(player.maxEnergy, currentEnergy);
            energyBar.SetEnergy(Mathf.RoundToInt(currentEnergy));
            if (currentEnergy >= energyCost) {
                energyBar.Enabled();
            }
        }
    }
}