using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Invaders
{
	class Turret : ModalSprite
	{
        private static float HorizontalStep = 6f;
        private static float VerticalStep = 6f;
        private static float DiagonalStep = (float) Math.Sqrt((double)(((HorizontalStep + VerticalStep) / 2) * ((HorizontalStep + VerticalStep) / 2)) / 2);

        private static PointF StopVector = new PointF(0f, 0f);

        public static Turret FactoryMethod(Form Form, Point Location)
        {
            List<SimpleSprite> Animations = new List<SimpleSprite>()
            { 
                new SimpleSprite(SpriteType.Waiting, Form, Properties.Resources.test_pattern, Location, StopVector, null),
                new SimpleSprite(SpriteType.Rolling, Form, Properties.Resources.turret, Location, StopVector, new SoundEffect(new Stream[] { Properties.Resources.Rolling }, 100, false)),
                new SimpleSprite(SpriteType.Skidding, Form, Properties.Resources.turret, Location, StopVector, new SoundEffect(new Stream[] { Properties.Resources.Short_Skid }, 101, false)),
                new SimpleSprite(SpriteType.Shooting, Form, Properties.Resources.turret, Location, StopVector, new SoundEffect(new Stream[] { Properties.Resources.Explosion }, 100000, false)),
                new SimpleSprite(SpriteType.Exploding, Form, Properties.Resources.turret, Location, StopVector, new SoundEffect(new Stream[] { Properties.Resources.Explosion }, 1000, false)),
                new SimpleSprite(SpriteType.Dead, Form, Properties.Resources.turret, Location, StopVector, null)
            };

            return new Turret(Animations, Location);
        }

        private Turret(List<SimpleSprite> Animations, Point Location) : base(Animations, SpriteType.Rolling, Location, StopVector)
        {
            // Establish the turret operating boundries

            this.Sinistral = new Zone(new Rectangle(0, 0, 0, this.Height), ZoneType.Wrapping);
            this.Dextral = new Zone(new Rectangle(this.Width, 0, 0, this.Height), ZoneType.Wrapping);
            this.Cieling = new Zone(new Rectangle(0, 0, this.Width, 0), ZoneType.Blocking);
            this.Floor = new Zone(new Rectangle(0, this.Height, this.Width, 0), ZoneType.Blocking);

            this.Visible = true;
        }

        public void LeftUpThrust()
        {
            Velocity = new PointF(-DiagonalStep, -DiagonalStep);
        }

        public void LeftThrust()
        {
            Velocity = new PointF(-HorizontalStep, 0f);
        }

        public void LeftDownThrust()
        {
            Velocity = new PointF(-DiagonalStep, +DiagonalStep);
        }

        public void UpThrust()
        {
            Velocity = new PointF(0f, -VerticalStep);
        }

        public void Stop()
        {
            Velocity = StopVector;
        }

        public void DownThrust()
        {
            Velocity = new PointF(0f, +VerticalStep);
        }

        public void RightUpThrust()
        {
            Velocity = new PointF(+DiagonalStep, -DiagonalStep);
        }

        public void RightThrust()
        {
            Velocity = new PointF(+HorizontalStep, 0f);
        }

        public void RightDownThrust()
        {
            Velocity = new PointF(+DiagonalStep, +DiagonalStep);
        }

        public Point Gunpoint
		{
            get
            {
                return new Point((int) (Left + Width / 2f), (int) Top - 10);
            }
		}
	}
}
