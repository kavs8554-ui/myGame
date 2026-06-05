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
            TricksterEnemyModel trickster = null;

            foreach (var enemy in model.CurrentLevel.Enemies)
            {
                if (enemy.IsAlive)
                    aliveEnemies++;
                if (enemy is TricksterEnemyModel t)
                    trickster = t;
            }

            if (trickster != null && aliveEnemies == 1)
            {
                trickster.IsVulnerable = true;
                if (model.Player.ControlsSwapped)
                {
                    model.Player.ControlsSwapped = false;
                    model.Player.SwapTimer = 0;
                }
            }

            if (trickster != null && !trickster.IsAlive && model.Player.ControlsSwapped)
            {
                model.Player.ControlsSwapped = false;
                model.Player.SwapTimer = 0;
            }
        }

        public void OnTricksterHit(TricksterEnemyModel trickster, GameModel model)
        {
            if (trickster == null || trickster.HasTriggeredSwap) return;
            model.Player.ControlsSwapped = true;
            model.Player.SwapTimer = _swapDuration;
            trickster.HasTriggeredSwap = true;
        }
    }
}
