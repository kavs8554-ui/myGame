using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using myGame.Model.enemies;

namespace myGame.View.enemies
{
    public class EnemyView
    {
        public void Draw(SpriteBatch spriteBatch, EnemyModel enemy, Texture2D normalTexture, Texture2D tricksterTexture)
        {
            if (!enemy.IsAlive) return;

            Texture2D texture = (enemy is TricksterEnemyModel) ? tricksterTexture : normalTexture;
            if (texture == null) return;

            float scale = (enemy.Radius * 2f) / texture.Width;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            spriteBatch.Draw(
                texture,
                enemy.Position,
                null,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}