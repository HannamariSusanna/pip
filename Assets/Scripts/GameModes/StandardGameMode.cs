public class StandardGameMode : GameMode {
    private Player player;
    public void SetPlayer(Player player) {
        this.player = player;
    }

    public int CarrotBonus() {
        return player.GetCarrotsPicked() * 1000;
    }

    public int CollisionPenalty() {
        return -player.GetCollisionCount() * 100;
    }
}