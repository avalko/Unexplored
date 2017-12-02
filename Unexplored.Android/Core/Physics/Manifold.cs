using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core.Physics
{
    public class Manifold
    {
        public Rigidbody A, B;

        public bool IsPenetrationX;
        public bool IsPenetrationY;
        public Vector2 Normal;
        public float Penetration;

        public Manifold(Rigidbody a, Rigidbody b)
        {
            A = a;
            B = b;
            Normal = Vector2.Zero;
            Penetration = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Solve()
        {
            Vector2 centerA = A.Box.Position + A.Box.Size * 0.5f; 
            Vector2 centerB = B.Box.Position + B.Box.Size * 0.5f;

            float nx = centerB.X - centerA.X;
            float ny = centerB.Y - centerA.Y;

            float aex = (A.Box.Max.X - A.Box.Min.X) * 0.5f;
            float bex = (B.Box.Max.X - B.Box.Min.X) * 0.5f;

            float xOverlap = aex + bex - Math.Abs(nx);

            if (xOverlap > 0)
            {
                float aey = (A.Box.Max.Y - A.Box.Min.Y) * 0.5f;
                float bey = (B.Box.Max.Y - B.Box.Min.Y) * 0.5f;

                float yOverlap = aey + bey - Math.Abs(ny);
                if (yOverlap > 0)
                {
                    if (xOverlap < yOverlap)
                    {
                        Normal.X = nx < 0 ? -1 : 1;
                        Normal.Y = 0;
                        Penetration = xOverlap;
                        IsPenetrationX = true;
                    }
                    else
                    {
                        Normal.X = 0;
                        Normal.Y = ny < 0 ? -1 : 1;
                        Penetration = yOverlap;
                        IsPenetrationY = true;
                    }

                    return true;
                }
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Resolve(float epsilon)
        {
            var a = A;
            var b = B;

            float rx = b.Velocity.X - a.Velocity.X;
            float ry = b.Velocity.Y - a.Velocity.Y;

            float velAlongNormal = rx * Normal.X + ry * Normal.Y;

            if (velAlongNormal > 0)
                return;

            float j = -velAlongNormal;
            j /= (a.InverseMass + b.InverseMass);

            a.ApplyImpulse(-j * Normal);
            b.ApplyImpulse(j * Normal);

            return;
            var tx = rx - (Normal.X * velAlongNormal);
            var ty = ry - (Normal.Y * velAlongNormal);
            var tl = (float)Math.Sqrt(tx * tx + ty * ty);

            if (tl > epsilon)
            {
                tx /= tl;
                ty /= tl;
            }

            var jt = -(rx * tx + ry * ty);
            jt /= (a.InverseMass + b.InverseMass);

            // Don't apply tiny friction impulses
            if (Math.Abs(jt) < epsilon)
            {
                return;
            }

            if (Math.Abs(jt) < j)
            {
                tx = tx * jt;
                ty = ty * jt;

            }
            else
            {
                tx = tx * -j * 0.03f * 0.03f;
                ty = ty * -j * 0.03f * 0.03f;
            }

            a.ApplyImpulse(-tx, -ty);
            b.ApplyImpulse(tx, ty);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PositionalCorrection()
        {
            var a = A;
            var b = B;

            const float percent = 0.7f;
            const float slop = 0.05f;

            float m = Math.Max(Penetration - slop, 0) / (a.InverseMass + b.InverseMass);

            float cx = m * Normal.X * percent;
            float cy = m * Normal.Y * percent;

            a.Box.Position.X -= cx * a.InverseMass;
            a.Box.Position.Y -= cy * a.InverseMass;

            b.Box.Position.X += cx * b.InverseMass;
            b.Box.Position.Y += cy * b.InverseMass;
        }
    }
}
