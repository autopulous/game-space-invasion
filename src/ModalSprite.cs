using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Invaders
{
    // Delegate types used to hook up event notifiers

    public delegate void DeathEventHandler(object sender, EventArgs e);

    /// <summary>
    /// Animation and sound for a sprite that changes its appearance (multiple of animation frame sets)
    /// 
    /// Effectively subclasses SimpleSprite by implementing the ISprite interface via pass through methods
    /// this implementation choice was made to enable both SimpleSprites and ModalSprites commonly
    /// </summary>

    class ModalSprite : ISprite
    {
        private SimpleSprite Sprite;

        private List<SimpleSprite> Sprites;

        private Random Aleatory;

        public ModalSprite(List<SimpleSprite> Sprites, SpriteType Mode, PointF LeftTop, PointF Velocity)
        {
            this.Sprites = Sprites;

            this.Sprite = Sprites.Find(sprite => sprite.Mode == Mode);

            if (null == this.Sprite) throw new ArgumentException("Mode: " + Mode.ToString() + " not defined within the sprite set.");

            this.Mode = Mode;

            this.LeftTop = LeftTop;
            this.Velocity = Velocity;

            this.Aleatory = new Random((int)DateTime.Now.Ticks);
        }

        public SpriteType Mode
        {
            get
            {
                return Sprite.Mode;
            }
            set
            {
                SimpleSprite Sprite = Sprites.Find(sprite => sprite.Mode == value);

                if (null == Sprite) throw new ArgumentException("Mode: " + value.ToString() + " not defined within the sprite set.");

                // copy state of the previous simple sprite to the next simple sprite

                Sprite.Sinistral = this.Sinistral;
                Sprite.Dextral = this.Dextral;
                Sprite.Cieling = this.Cieling;
                Sprite.Floor = this.Floor;

                Sprite.Mass = this.Mass;
                Sprite.MassCenter = this.MassCenter;

                Sprite.Visible = this.Sprite.Visible;

                Sprite.Orientation = this.Sprite.Orientation;
                Sprite.Center = this.Sprite.Center;

                Sprite.Spin = this.Sprite.Spin;
                Sprite.Damper = this.Sprite.Damper;
                Sprite.Torque = this.Sprite.Torque;

                Sprite.Jitter = this.Sprite.Jitter;
                Sprite.Decay = this.Sprite.Decay;
                Sprite.Shake = this.Sprite.Shake;

                Sprite.Velocity = this.Sprite.Velocity;
                Sprite.Friction = this.Sprite.Friction;
                Sprite.Thrust = this.Sprite.Thrust;

                Sprite.WindSource = this.Sprite.WindSource;
                Sprite.Wind = this.Sprite.Wind;

                Sprite.GravitySource = this.Sprite.GravitySource;
                Sprite.Gravity = this.Sprite.Gravity;

                Sprite.Location = this.Sprite.Location;

                // replace the previous simple sprite with the next simple sprite

                this.Sprite = Sprite;
            }
        }

        public Zone Sinistral
        {
            get
            {
                return Sprite.Sinistral;
            }
            set
            {
                Sprite.Sinistral = value;
            }
        }

        public Zone Dextral
        {
            get
            {
                return Sprite.Dextral;
            }
            set
            {
                Sprite.Dextral = value;
            }
        }

        public Zone Cieling
        {
            get
            {
                return Sprite.Cieling;
            }
            set
            {
                Sprite.Cieling = value;
            }
        }

        public Zone Floor
        {
            get
            {
                return Sprite.Floor;
            }
            set
            {
                Sprite.Floor = value;
            }
        }

        public Rectangle ScreenCoverage
        {
            get
            {
                return Rectangle.Round(Sprite.ScreenCoverage);
            }
        }

        public Size Size
        {
            get
            {
                return Sprite.Size;
            }
        }

        public int Width
        {
            get
            {
                return (int) Sprite.Width;
            }
        }

        public int Height
        {
            get
            {
                return (int) Sprite.Height;
            }
        }

        public int Area
        {
            get
            {
                return Sprite.Area;
            }
        }

        public float Mass
        {
            get
            {
                return Sprite.Mass;
            }
            set
            {
                Sprite.Mass = value;
            }
        }

        public PointF MassCenter
        {
            get
            {
                return Sprite.MassCenter;
            }
            set
            {
                Sprite.MassCenter = value;
            }
        }

        public bool Hidden
        {
            get
            {
                return !Visible;
            }
            set
            {
                Visible = !value;
            }
        }

        public bool Visible
        {
            get
            {
                return Sprite.Visible;
            }
            set
            {
                Sprite.Visible = value;
            }
        }

        public float Orientation
        {
            get
            {
                return Sprite.Orientation;
            }
            set
            {
                Sprite.Orientation = value;
            }
        }

        public RectangleF Location
        {
            get
            {
                return Sprite.Location;
            }
            set
            {
                Sprite.Location = value;
            }
        }

        public PointF LeftTop
        {
            get
            {
                return Sprite.LeftTop;
            }
            set
            {
                Sprite.LeftTop = value;
            }
        }

        public PointF RightTop
        {
            get
            {
                return Sprite.RightTop;
            }
            set
            {
                Sprite.RightTop = value;
            }
        }

        public PointF RightBottom
        {
            get
            {
                return Sprite.RightBottom;
            }
            set
            {
                Sprite.RightBottom = value;
            }
        }

        public PointF LeftBottom
        {
            get
            {
                return Sprite.RightBottom;
            }
            set
            {
                Sprite.RightBottom = value;
            }
        }

        public float Left
        {
            get
            {
                return Sprite.Left;
            }
            set
            {
                Sprite.Left = value;
            }
        }

        public float Right
        {
            get
            {
                return Sprite.Right;
            }
            set
            {
                Sprite.Right = value;
            }
        }

        public float Top
        {
            get
            {
                return Sprite.Top;
            }
            set
            {
                Sprite.Top = value;
            }
        }

        public float Bottom
        {
            get
            {
                return Sprite.Bottom;
            }
            set
            {
                Sprite.Bottom = value;
            }
        }

        public PointF Center
        {
            get
            {
                return Sprite.Center;
            }
            set
            {
                Sprite.Center = value;
            }
        }

        public float Spin
        {
            get
            {
                return Sprite.Spin;
            }
            set
            {
                Sprite.Spin = value;
            }
        }

        public float Damper
        {
            get
            {
                return Sprite.Damper;
            }
            set
            {
                Sprite.Damper = value;
            }
        }

        public float Torque
        {
            get
            {
                return Sprite.Torque;
            }
            set
            {
                Sprite.Torque = value;
            }
        }

        public PointF Jitter
        {
            get
            {
                return Sprite.Jitter;
            }
            set
            {
                Sprite.Jitter = value;
            }
        }

        public float Decay
        {
            get
            {
                return Sprite.Decay;
            }
            set
            {
                Sprite.Decay = value;
            }
        }

        public PointF Shake
        {
            get
            {
                return Sprite.Shake;
            }
            set
            {
                Sprite.Shake = value;
            }
        }

        public PointF Velocity
        {
            get
            {
                return Sprite.Velocity;
            }
            set
            {
                Sprite.Velocity = value;
            }
        }

        public float Friction
        {
            get
            {
                return Sprite.Friction;
            }
            set
            {
                Sprite.Friction = value;
            }
        }

        public PointF Thrust
        {
            get
            {
                return Sprite.Thrust;
            }
            set
            {
                Sprite.Thrust = value;
            }
        }

        public Point WindSource
        {
            get
            {
                return Sprite.WindSource;
            }
            set
            {
                Sprite.WindSource = value;
            }
        }

        public float Wind
        {
            get
            {
                return Sprite.Wind;
            }
            set
            {
                Sprite.Wind = value;
            }
        }

        public Point GravitySource
        {
            get
            {
                return Sprite.GravitySource;
            }
            set
            {
                Sprite.GravitySource = value;
            }
        }

        public float Gravity
        {
            get
            {
                return Sprite.Gravity;
            }
            set
            {
                Sprite.Gravity = value;
            }
        }

        public void SetLeftTop(int X, int Y)
        {
            Sprite.SetLeftTop(X, Y);
        }

        public void SetRightTop(int X, int Y)
        {
            Sprite.SetRightTop(X, Y);
        }

        public void SetRightBottom(int X, int Y)
        {
            Sprite.SetRightBottom(X, Y);
        }

        public void SetLeftBottom(int X, int Y)
        {
            Sprite.SetLeftBottom(X, Y);
        }

        public void SetCenter(int X, int Y)
        {
            Sprite.SetCenter(X, Y);
        }

        public void SetMassCenter(float X, float Y)
        {
            Sprite.SetMassCenter(X, Y);
        }

        public void SetShake(float X, float Y)
        {
            Sprite.SetShake(X, Y);
        }

        public void SetJitter(float X, float Y)
        {
            Sprite.SetJitter(X, Y);
        }

        public void SetVelocity(float X, float Y)
        {
            Sprite.SetVelocity(X, Y);
        }

        public void SetThrust(float X, float Y)
        {
            Sprite.SetThrust(X, Y);
        }

        public void SetWindSource(int X, int Y)
        {
            Sprite.SetWindSource(X, Y);
        }

        public void SetGravitySource(int X, int Y)
        {
            Sprite.SetGravitySource(X, Y);
        }

        public void ApplyPhysics()
        {
            Sprite.ApplyPhysics();
        }

        public void ApplyForces()
        {
            Sprite.ApplyForces();
        }

        public void ApplyTorque()
        {
            Sprite.ApplyTorque();
        }

        public void ApplyTorque(float Torque)
        {
            Sprite.ApplyTorque(Torque);
        }

        public void ApplyShake()
        {
            Sprite.ApplyShake();
        }

        public void ApplyShake(PointF Shake)
        {
            Sprite.ApplyShake(Shake);
        }

        public void ApplyThrust()
        {
            Sprite.ApplyThrust();
        }

        public void ApplyThrust(PointF Thrust)
        {
            Sprite.ApplyThrust(Thrust);
        }

        public void ApplyWind()
        {
            Sprite.ApplyWind();
        }

        public void ApplyWind(PointF WindSource, float Wind)
        {
            Sprite.ApplyWind(WindSource, Wind);
        }

        public void ApplyWind(float X, float Y, float Wind)
        {
            Sprite.ApplyWind(X, Y, Wind);
        }

        public void ApplyGravity()
        {
            Sprite.ApplyGravity();
        }

        public void ApplyGravity(PointF GravitySource, float Gravity)
        {
            Sprite.ApplyGravity(GravitySource, Gravity);
        }

        public void ApplyGravity(float X, float Y, float Gravity)
        {
            Sprite.ApplyGravity(X, Y, Gravity);
        }

        public void ApplyMomentum()
        {
            Sprite.ApplyMomentum();
        }

        public void ApplySpin()
        {
            Sprite.ApplySpin();
        }

        public void ApplySpin(float Spin)
        {
            Sprite.ApplySpin(Spin);
        }

        public void ApplyJitter()
        {
            Sprite.ApplyJitter();
        }

        public void ApplyJitter(PointF Jitter)
        {
            Sprite.ApplyJitter(Jitter);
        }

        public void ApplyJitter(float X, float Y)
        {
            Sprite.ApplyJitter(X, Y);
        }

        public void ApplyVelocity()
        {
            Sprite.ApplyVelocity();
        }

        public void ApplyVelocity(PointF Velocity)
        {
            Sprite.ApplyVelocity(Velocity);
        }

        public void ApplyVelocity(float X, float Y)
        {
            Sprite.ApplyVelocity(X, Y);
        }

        public virtual void Draw(Graphics g)
        {
            Sprite.Draw(g);
        }
    }
}