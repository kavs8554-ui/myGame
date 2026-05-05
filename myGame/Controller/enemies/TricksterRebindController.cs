using Microsoft.Xna.Framework;
using myGame.Model;
using myGame.Model.enemies;

namespace myGame.Controller.enemies
{
    public class TricksterRebindController
    {
        private float _swapDuration = 5f; 

        public void Update(GameModel model)
        {
            if (model.CurrentLevel == null || model.Player == null) return;

            int aliveEnemies = 0;
            TricksterEnemyModel chaos = null;

            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (enemy.IsAlive)
                {
                    aliveEnemies++;
                    if (enemy is TricksterEnemyModel c)
                        chaos = c;
                }
            }

            if (chaos != null && aliveEnemies == 1)
            {
                chaos.IsVulnerable = true;

                if (model.Player.ControlsSwapped)
                {
                    model.Player.ControlsSwapped = false;
                    model.Player.SwapTimer = 0;
                }
            }
        }

        public void OnChaosHit(TricksterEnemyModel chaos, GameModel model)
        {
            if (chaos == null || chaos.HasTriggeredSwap) return;

            model.Player.ControlsSwapped = true;
            model.Player.SwapTimer = _swapDuration;
            chaos.HasTriggeredSwap = true;
        }
    }
}
