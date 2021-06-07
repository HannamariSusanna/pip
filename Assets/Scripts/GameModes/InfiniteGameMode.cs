public class InfiniteGameMode : GameMode {

    private Player player;
    public void SetPlayer(Player player) {
        this.player = player;
    }
    public int CarrotBonus() {
        return player.GetCarrotsPicked() * 10; 
    }

    public int CollisionPenalty() {
        return 0;
    }
}