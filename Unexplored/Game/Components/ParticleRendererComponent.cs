using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Game.Attributes;
using Unexplored.Game.Structures;
using Microsoft.Xna.Framework.Graphics;

namespace Unexplored.Game.Components
{
    struct ParticleOffset
    {
        public int TileX;
        public int TileY;
        public Vector2 Offset;

        public ParticleOffset(int tile, Vector2 offset)
        {
            var point = Tile.GetTileOffset(tile);
            TileX = point.X;
            TileY = point.Y;
            Offset = offset;
        }
    }

    public struct ParticleItem
    {
        ParticleOffset[] Items;

        public ParticleItem CreateFromHorizontalTiles(params int[] tiles)
        {
            ParticleItem particle = new ParticleItem();
            particle.Items = new ParticleOffset[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                particle.Items[i] = new ParticleOffset(tiles[i], new Vector2(Tile.Size * i, 0));
            }
            return particle;
        }
    }

    public abstract class Particle
    {
        public Vector2 Position;
        public bool Die;
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }

    public class ParticleRendererComponent : BehaviorComponent
    {
        private static List<Particle> particles;

        static ParticleRendererComponent()
        {
            particles = new List<Particle>(100);
        }

        public static void AddParticle(Vector2 position, Particle particle)
        {
            particle.Position = position;
            particles.Add(particle);
        }

        public override void Update(GameTime gameTime)
        {
            int particlesCount = particles.Count;
            int i = 0;
            while (i < particlesCount)
            {
                particles[i].Update(gameTime);
                if (particles[i].Die)
                {
                    particles.Remove(particles[i]);
                    --particlesCount;
                }
                else
                    ++i;
            }
        }

        public override void Draw()
        {
            int particlesCount = particles.Count;
            int i = 0;
            while (i < particlesCount)
            {
                particles[i++].Draw(spriteBatch);
            }
        }
    }
}
