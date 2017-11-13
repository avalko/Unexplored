using Microsoft.Xna.Framework;
using Unexplored.Core;
using Unexplored.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Controllers
{
    class CameraController : BasicController<HeroObject>
    {
        private const float MaxDistance = 10;
        private const float MaxSquaredDistance = MaxDistance * MaxDistance;

        private HeroObject hero;
        private Camera2DComponent camera;

        Vector2 distanationOffset;
        Vector2 cameraMaxPosition;
        Vector2 lastMove;
        bool moveTo;
        Vector2 moveDistanation;

        public CameraController(HeroObject hero, Vector2 mapSize)
            : base(hero)
        {
            camera = ComponentManager.Instance.Get<Camera2DComponent>();
            this.hero = hero;

            distanationOffset = (-Constants.SceneSize + Constants.ScaledTileSize) / 2;
            cameraMaxPosition = -mapSize * Constants.ScaledTileSize + Constants.ScaledTileSize;
        }

        public void UpdateHero(HeroObject hero)
        {
            this.hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 distanation = -(hero.Transform.Position * Constants.ScaleFactor + distanationOffset);

            if ((camera.Location - distanation).LengthSquared() > 2 * 2)
            {
                camera.Location = Vector2.Clamp(Vector2.Lerp(camera.Location, distanation, 12.0f * (float)gameTime.ElapsedGameTime.TotalSeconds), cameraMaxPosition, Vector2.Zero);
            }

            /*if ((distanation - lastMove).LengthSquared() > 100 * 100)
            {
                var move = Vector2.Clamp(Vector2.Lerp(camera.Location, distanation, 0.1f), cameraMaxPosition, Vector2.Zero);
                if ((move - distanation).LengthSquared() < 10)
                {
                    move = distanation;
                }
                camera.Location = move;
                moveTo = true;
                lastMove = distanation;
            }
            */
            /*if (moveTo)
            {
                camera.Location = Vector2.Clamp(Vector2.Lerp(camera.Location, distanation, 0.1f), cameraMaxPosition, Vector2.Zero);
                if ((camera.Location - distanation).LengthSquared() < 1)
                {
                    moveTo = false;
                }
            }*/
        }
    }
}
