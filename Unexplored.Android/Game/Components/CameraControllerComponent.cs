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
        public GameObject CurrentObject => otherObject;

        private GameObject otherObject, oldObject;
        private IGameCamera camera;

        Vector2 distanationOffset;
        bool isGoTo;
        double goToTimeout, currentTimeout;
        float goToSpeed;

        public CameraControllerComponent()
        {
            Drawable = false;
            distanationOffset = (-Constants.SceneSize + Constants.ScaledTileSize) / 2;
        }

        public void GoTo(GameObject otherObject, float speed, double timeout)
        {
            if (isGoTo)
                return;

            oldObject = this.otherObject;
            this.otherObject = otherObject;
            isGoTo = true;
            goToSpeed = speed;
            goToTimeout = timeout;
        }

        public void SetOtherObject(GameObject otherObject)
        {
            this.otherObject = otherObject;
        }

        public override void Initialize()
        {
            camera = SceneManager.CurrentCamera;
        }

        public override void Update(GameTime gameTime)
        {
            if (isGoTo)
            {
                currentTimeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (currentTimeout >= goToTimeout)
                {
                    isGoTo = false;
                    currentTimeout = 0;
                    goToTimeout = 0;
                    otherObject = oldObject;
                }
            }

            Vector2 distanation = -(otherObject.Transform.Position * Constants.ScaleFactor + distanationOffset);
            Vector2 direction = (camera.Location - distanation);
            if (direction.LengthSquared() > 5 * 5)
            {
                camera.Location = Vector2.LerpPrecise(camera.Location, distanation, (isGoTo ? goToSpeed : 8.0f) * (float)(gameTime.ElapsedGameTime.TotalSeconds));
                //camera.Location = distanation;
            }
        }
    }
}
