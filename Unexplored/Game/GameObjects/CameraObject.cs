using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Core;
using Unexplored.Game.Components;

namespace Unexplored.Game.GameObjects
{
    class CameraObject : Core.Base.GameObject
    {
        public CameraObject()
        {
            SetComponents(
                new Core.Components.Camera2DComponent(),
                new CameraControllerComponent()
                );
        }
    }
}
