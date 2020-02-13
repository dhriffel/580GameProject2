using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _580GameProject2
{
        public struct BoundingRectangle
        {
            public float X;
            public float Y;

            public float Width;
            public float Height;

            public BoundingRectangle(float X, float Y, float Width, float Height)
            {
                this.X = X;
                this.Y = Y;
                this.Width = Width;
                this.Height = Height;
            }
            public bool CollidesWith(BoundingRectangle other)
            {
                return (this.X > other.X + other.Width
                    || this.X + this.Width < other.X
                    || this.Y > other.Y + other.Height
                    || this.Y + this.Width < other.Y);
            }

            public static implicit operator Rectangle(BoundingRectangle br)
            {
                return new Rectangle((int)br.X, (int)br.Y, (int)br.Width, (int)br.Height);
            }

        }
    }
