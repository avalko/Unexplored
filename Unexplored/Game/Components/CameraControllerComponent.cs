using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Components;
using Unexplored.Core;
using Unexplored.Game.Structures;

namespace Unexplored.Game.Components
{
    public class CameraControllerComponent : BehaviorComponent
    {
        private GameObject otherObject;
        private IGameCamera camera;

        Vector2 distanationOffset;

        public CameraControllerComponent()
        {
            Drawable = false;
            distanationOffset = (-Constants.SceneSize + Constants.ScaledTileSize) / 2;
        }

        public void SetOtherObject(GameObject otherObject)
        {
            this.otherObject = otherObject;
        }

        public override void Initialize()
        {
            camera = SceneManager.Camera;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 distanation = -(otherObject.Transform.Position * Constants.ScaleFactor + distanationOffset);

            Vector2 direction = (camera.Location - distanation);
            if (direction.LengthSquared() > 5 * 5)
            {
                camera.Location = Vector2.LerpPrecise(camera.Location, distanation, 8.0f * (float)(gameTime.ElapsedGameTime.TotalSeconds));
                //camera.Location = distanation;
            }
        }
    }
}
