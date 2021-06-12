using UnityEngine;

public abstract class GameMode {
  protected Player player;
  protected FinishDialog finishDialog;
  protected PointsCalculator pointsCalculator;
  protected int carrotsPicked = 0;
  protected int collisions = 0;

  protected int carrotBonus;
  protected int collisionPenalty;
  protected bool collisionsAllowedAfterHealthLoss;

  public void SetFinishDialog(FinishDialog finishDialog) {
    finishDialog.SetGameMode(this);
    this.finishDialog = finishDialog;
  }
  public void SetPlayer(Player player) {
    this.player = player;
  }

  public int GetGameModeBonus() {
    return pointsCalculator.GetGameModePoints();
  }
  public void SetPointsCalculator(GameObject pointsCalculator) {
    this.pointsCalculator = pointsCalculator.GetComponent<PointsCalculator>();
  }

  public void SetCarrotBonus(int bonus) {
    this.carrotBonus = bonus;
  }
  public int GetCarrotBonus() {
    return carrotsPicked * carrotBonus;
  }

  public void SetCollisionPenalty(int penalty) {
    this.collisionPenalty = penalty;
  }
  public int GetCollisionPenalty() {
    return collisions * collisionPenalty;
  }

  public void SetCollisionsAllowedAfterHealthLoss(bool isAllowed) {
    this.collisionsAllowedAfterHealthLoss = isAllowed;
  }

  public void IncreaseCarrotsPicked(int amount) {
    this.carrotsPicked += amount;
  }

  public void IncreaseCollisions(int amount) {
    if (!collisionsAllowedAfterHealthLoss && player.GetCurrentHealth() <= 0) {
      finishDialog.Show();
    } else {
      this.collisions += amount;
    }
  }
}