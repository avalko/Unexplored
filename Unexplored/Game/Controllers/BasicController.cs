using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Controllers
{
    class BasicController<T> where T : GameObject
    {
        protected T gameObject;

        public BasicController(T gameObject)
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            this.gameObject = gameObject;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
