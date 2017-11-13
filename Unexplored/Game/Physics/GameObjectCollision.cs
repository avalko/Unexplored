using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Game.Physics
{
    public struct GameObjectCollision
    {
        public GameObject CheckingObject;
        public GameObject VerifiableObject;

        public Collision Collision;

        public GameObjectCollision(GameObject checkingObject, GameObject verifiableObject)
        {
            CheckingObject = checkingObject;
            VerifiableObject = verifiableObject;
            Collision = new Collision();
        }

        public bool Check()
        {
            var positionA = CheckingObject.Transform.Position + CheckingObject.Rigidbody.OffsetMin;
            var positionB = VerifiableObject.Transform.Position + VerifiableObject.Rigidbody.OffsetMin;
            var sizeA = CheckingObject.Transform.Size - CheckingObject.Rigidbody.OffsetMin - CheckingObject.Rigidbody.OffsetMax;
            var sizeB = VerifiableObject.Transform.Size - VerifiableObject.Rigidbody.OffsetMin - CheckingObject.Rigidbody.OffsetMax;

            Vector2 direction = positionA - positionB;
            Vector2 halfSizeA = sizeA / 2;
            Vector2 halfSizeB = sizeB / 2;
            Vector2 overlap = halfSizeA + halfSizeB - new Vector2(Math.Abs(direction.X), Math.Abs(direction.Y));

            if (overlap.X > 0 && overlap.Y > 0)
            {
                if (overlap.X > overlap.Y)
                {
                    if (direction.X < 0)
                        Collision.Normal = new Vector2(-1, 0);
                    else
                        Collision.Normal = new Vector2(1, 0);
                    Collision.Penetration = overlap.X;
                    return true;
                }
                else
                {
                    if (direction.Y < 0)
                        Collision.Normal = new Vector2(0, -1);
                    else
                        Collision.Normal = new Vector2(0, 1);
                    Collision.Penetration = overlap.Y;
                    return true;
                }
            }

            return false;
        }

        public bool OptimizedAABBvsAABB(Rigidbody a, Rigidbody b, out Collision colission)
        {
            colission = new Collision();
            Vector2 direction = a.Transform.Position - b.Transform.Position;

            float aExtent = a.Transform.Size.X / 2;
            float bExtent = b.Transform.Size.X / 2;

            // Вычисление наложения по оси x
            float xOverlap = aExtent + bExtent - Math.Abs(direction.X);

            // Проверка SAT по оси x
            if (xOverlap > 0)
            {
                // Вычисление половины ширины вдоль оси y для каждого объекта
                aExtent = a.Transform.Size.Y / 2;
                bExtent = b.Transform.Size.Y / 2;

                // Вычисление наложения по оси y
                float yOverlap = aExtent + bExtent - Math.Abs(direction.Y);

                // Проверка SAT по оси y
                if (yOverlap > 0)
                {
                    // Определяем, по какой из осей проникновение наименьшее
                    if (xOverlap > yOverlap)
                    {
                        // Указываем в направлении B, зная, что n указывает в направлении от A к B
                        if (direction.X < 0)
                            colission.Normal = new Vector2(-1, 0);
                        else
                            colission.Normal = new Vector2(0, 0);
                        colission.Penetration = xOverlap;
                        return true;
                    }
                    else
                    {
                        // Указываем в направлении B, зная, что n указывает в направлении от A к B
                        if (direction.Y < 0)
                            colission.Normal = new Vector2(0, -1);
                        else
                            colission.Normal = new Vector2(0, 1);
                        colission.Penetration = yOverlap;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
