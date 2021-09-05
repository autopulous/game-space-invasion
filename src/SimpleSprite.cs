using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Invaders
{
    /// <summary>
    /// Animation and sound for a sprite (single animation frame set)
    /// </summary>

    class SimpleSprite : ISprite
    {
        private SpriteType _Mode = SpriteType.None;

        private Form Form; // Set in the constructor

        private Zone _Sinistral;
        private Zone _Dextral;
        private Zone _Cieling;
        private Zone _Floor;

        private Bitmap[] _Frame; // Set in the constructor
        private uint _Frames; // Set in the constructor
        private uint _Index = 0; // First (or only) frame of an animated sprite

        private int _Iterator = 1; // increment

        private float _Orientation = 0f; // upright orientation +360/-360 degrees

        private RectangleF _PreviousScreenCoverage;
        private RectangleF _ScreenCoverage; // set in the constructor

        private float _Mass; // set in the constructor
        private PointF _MassCenter; // set in the constructor

        private bool _Visible = false;

        private float _Spin = 0f; // no spin
        private float _Damper = 0f; // no damper
        private float _Torque = 0F; // no torque

        private PointF _Jitter = new PointF(0f, 0f); // no jitter
        private float _Decay = 0f; // no decay
        private PointF _Shake = new PointF(0f, 0f); // no shake

        private PointF _Velocity; // set in the constructor
        private float _Friction = 0f; // no friction
        private PointF _Thrust = new PointF(0f, 0f); // no thrust

        private Point _WindSource = new Point(0, 0); // no wind
        private float _Wind = 0f; // no wind

        private Point _GravitySource = new Point(0, 0); // no gravity
        private float _Gravity = 0f; // no gravity

        private SoundEffect SoundEffect; // set in the constructor

        private Random Aleatory; // set in the constructor

        public SimpleSprite(SpriteType Mode, Form Form, Bitmap Animation, Point LeftTop, PointF Velocity, SoundEffect SoundEffect)
        {
            this.Mode = Mode;

            this.Form = Form;

            this._Frame = ParseFrames(Animation);

            this._Frames = (uint)this._Frame.Length;

            this._Index = 0;

            this.Size = Animation.Size; // set ScreenCoverage size

            this.Mass = this.Area; // default
            this.MassCenter = new PointF((float)this.Width / 2f, (float)this.Height / 2f); // default

            this.Velocity = Velocity;

            this.LeftTop = LeftTop; // set the screen location

            this.SoundEffect = SoundEffect;

            this.Aleatory = new Random((int)DateTime.Now.Ticks);
        }

        // http://stackoverflow.com/a/26178389/639691

        // Parses individual Bitmap frames from a multi-frame Bitmap into an array of Bitmaps

        private Bitmap[] ParseFrames(Bitmap Animation)
        {
            // Get the number of animation frames to copy into a Bitmap array

            int Length = Animation.GetFrameCount(FrameDimension.Time);

            // Allocate a Bitmap array to hold individual frames from the animation

            Bitmap[] Frames = new Bitmap[Length];

            // Copy the animation Bitmap frames into the Bitmap array

            for (int Index = 0; Index < Length; Index++)
            {
                // Set the current frame within the animation to be copied into the Bitmap array element

                Animation.SelectActiveFrame(FrameDimension.Time, Index);

                // Create a new Bitmap element within the Bitmap array in which to copy the next frame

                Frames[Index] = new Bitmap(Animation.Size.Width, Animation.Size.Height);

                // Copy the current animation frame into the new Bitmap array element

                Graphics.FromImage(Frames[Index]).DrawImage(Animation, new Point(0, 0));
            }

            // Return the array of Bitmap frames

            return Frames;
        }

        public SpriteType Mode
        {
            get
            {
                return _Mode;
            }
            set
            {
                _Mode = value;
            }
        }

        public Zone Sinistral
        {
            get
            {
                return _Sinistral;
            }
            set
            {
                _Sinistral = value;
            }
        }

        public Zone Dextral
        {
            get
            {
                return _Dextral;
            }
            set
            {
                _Dextral = value;
            }
        }

        public Zone Cieling
        {
            get
            {
                return _Cieling;
            }
            set
            {
                _Cieling = value;
            }
        }

        public Zone Floor
        {
            get
            {
                return _Floor;
            }
            set
            {
                _Floor = value;
            }
        }

        public uint Frames
        {
            get
            {
                return _Frames;
            }
        }

        public uint Index
        {
            get
            {
                return _Index;
            }
            set
            {
                _Index = value >= _Frames ? _Frames - 1 : value;
            }
        }

        public int Iterator
        {
            get
            {
                return _Iterator;
            }
            set
            {
                _Iterator = 0 == value ? 0 : value / Math.Abs(value); // zero, plus one and minus one are the only valid incrementors
            }
        }

        public Rectangle ScreenCoverage
        {
            get
            {
                return Rectangle.Round(_ScreenCoverage);
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return System.Drawing.Size.Round(Size);
            }
            private set
            {
                _ScreenCoverage.Size = value;
            }
        }

        public int Width
        {
            get
            {
                return (int)_ScreenCoverage.Width;
            }
        }

        public int Height
        {
            get
            {
                return (int)_ScreenCoverage.Height;
            }
        }

        public int Area
        {
            get
            {
                return (int)_ScreenCoverage.Height * (int)_ScreenCoverage.Width;
            }
        }

        public float Mass
        {
            get
            {
                return _Mass;
            }
            set
            {
                _Mass = value;
            }
        }

        public PointF MassCenter
        {
            get
            {
                return _MassCenter;
            }
            set
            {
                _MassCenter = value;
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
                return _Visible;
            }
            set
            {
                _Visible = value;

                if (_Visible)
                {
                    Form.Invalidate(new Rectangle((int)_ScreenCoverage.X, (int)_ScreenCoverage.Y, (int)_ScreenCoverage.Width, (int)_ScreenCoverage.Height));
                }
            }
        }

        public float Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value % 360;
            }
        }

        public RectangleF Location
        {
            get
            {
                return _ScreenCoverage;
            }
            set
            {
                _ScreenCoverage = value;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public PointF LeftTop
        {
            get
            {
                return _ScreenCoverage.Location;
            }
            set
            {
                _ScreenCoverage.Location = value;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public PointF RightTop
        {
            get
            {
                return new PointF(_ScreenCoverage.Right, _ScreenCoverage.Top);
            }
            set
            {
                _ScreenCoverage.X = value.X - ScreenCoverage.Width;
                _ScreenCoverage.Y = value.Y;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public PointF RightBottom
        {
            get
            {
                return new PointF(_ScreenCoverage.Right, _ScreenCoverage.Bottom);
            }
            set
            {
                _ScreenCoverage.X = value.X - _ScreenCoverage.Width;
                _ScreenCoverage.Y = value.Y - _ScreenCoverage.Height;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public PointF LeftBottom
        {
            get
            {
                return new PointF(_ScreenCoverage.Left, _ScreenCoverage.Bottom);
            }
            set
            {
                _ScreenCoverage.X = value.X;
                _ScreenCoverage.Y = value.Y - _ScreenCoverage.Height;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public float Left
        {
            get
            {
                return _ScreenCoverage.Left;
            }
            set
            {
                _ScreenCoverage.X = value;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public float Right
        {
            get
            {
                return _ScreenCoverage.Right;
            }
            set
            {
                _ScreenCoverage.X = value - _ScreenCoverage.Width;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public float Top
        {
            get
            {
                return _ScreenCoverage.Top;
            }
            set
            {
                _ScreenCoverage.Y = value;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public float Bottom
        {
            get
            {
                return _ScreenCoverage.Bottom;
            }
            set
            {
                _ScreenCoverage.Y = value - _ScreenCoverage.Height;
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public PointF Center
        {
            get
            {
                return new PointF((float)_ScreenCoverage.X + ((float)_ScreenCoverage.Width / 2f), (float)_ScreenCoverage.Y + ((float)_ScreenCoverage.Height / 2f));
            }
            set
            {
                _ScreenCoverage.X = (int)(value.X - ((float)_ScreenCoverage.Width / 2f));
                _ScreenCoverage.Y = (int)(value.Y - ((float)_ScreenCoverage.Height / 2f));
                _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
            }
        }

        public float Spin
        {
            get
            {
                return _Spin;
            }
            set
            {
                _Spin = value;
            }
        }

        public float Damper
        {
            get
            {
                return _Damper;
            }
            set
            {
                _Damper = value;
            }
        }

        public float Torque
        {
            get
            {
                return _Torque;
            }
            set
            {
                _Torque = value;
            }
        }

        public PointF Jitter
        {
            get
            {
                return _Jitter;
            }
            set
            {
                _Jitter = value;
            }
        }

        public float Decay
        {
            get
            {
                return _Decay;
            }
            set
            {
                _Decay = value;
            }
        }

        public PointF Shake
        {
            get
            {
                return _Shake;
            }
            set
            {
                _Shake = value;
            }
        }

        public PointF Velocity
        {
            get
            {
                return _Velocity;
            }
            set
            {
                _Velocity = value;
            }
        }

        public float Friction
        {
            get
            {
                return _Friction;
            }
            set
            {
                _Friction = value;
            }
        }

        public PointF Thrust
        {
            get
            {
                return _Thrust;
            }
            set
            {
                _Thrust = value;
            }
        }

        public Point WindSource
        {
            get
            {
                return _WindSource;
            }
            set
            {
                _WindSource = value;
            }
        }

        public float Wind
        {
            get
            {
                return _Wind;
            }
            set
            {
                _Wind = value;
            }
        }

        public Point GravitySource
        {
            get
            {
                return _GravitySource;
            }
            set
            {
                _GravitySource = value;
            }
        }

        public float Gravity
        {
            get
            {
                return _Gravity;
            }
            set
            {
                _Gravity = value;
            }
        }

        public void SetLeftTop(int X, int Y)
        {
            _ScreenCoverage.X = X;
            _ScreenCoverage.Y = Y;
            _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
        }

        public void SetRightTop(int X, int Y)
        {
            _ScreenCoverage.X = X - _ScreenCoverage.Width;
            _ScreenCoverage.Y = Y;
            _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
        }

        public void SetRightBottom(int X, int Y)
        {
            _ScreenCoverage.X = X - _ScreenCoverage.Width;
            _ScreenCoverage.Y = Y - _ScreenCoverage.Height;
            _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
        }

        public void SetCenter(int X, int Y)
        {
            _ScreenCoverage.X = (int)(X - ((float)_ScreenCoverage.Width / 2f));
            _ScreenCoverage.Y = (int)(Y - ((float)_ScreenCoverage.Height / 2f));
            _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
        }

        public void SetLeftBottom(int X, int Y)
        {
            _ScreenCoverage.X = X;
            _ScreenCoverage.Y = Y - _ScreenCoverage.Height;
            _PreviousScreenCoverage = _ScreenCoverage; // set for boundry interaction testing
        }

        public void SetMassCenter(float X, float Y)
        {
            _MassCenter.X = X;
            _MassCenter.Y = Y;
        }

        public void SetShake(float X, float Y)
        {
            _Shake.X = X;
            _Shake.Y = Y;
        }

        public void SetJitter(float X, float Y)
        {
            _Jitter.X = X;
            _Jitter.Y = Y;
        }

        public void SetVelocity(float X, float Y)
        {
            _Velocity.X = X;
            _Velocity.Y = Y;
        }

        public void SetThrust(float X, float Y)
        {
            _Thrust.X = X;
            _Thrust.Y = Y;
        }

        public void SetWindSource(int X, int Y)
        {
            _WindSource.X = X;
            _WindSource.Y = Y;
        }

        public void SetGravitySource(int X, int Y)
        {
            _GravitySource.X = X;
            _GravitySource.Y = Y;
        }

        public void ApplyPhysics()
        {
            ApplyForces();
            ApplyMomentum();
            EncounterBoundries();
        }

        public void ApplyForces()
        {
            // Intrinsic forces

            ApplyTorque();
            ApplyShake();
            ApplyThrust();

            // Extrinsic forces

            ApplyWind();
            ApplyGravity();
        }

        public void ApplyTorque()
        {
            ApplyTorque(_Torque);
        }

        public void ApplyTorque(float Torque)
        {
            _Spin = Retard(_Spin + _Torque, _Damper);
        }

        public void ApplyShake()
        {
            ApplyShake(_Shake);
        }

        public void ApplyShake(PointF Shake)
        {
            _Jitter.X = Retard(_Jitter.X + _Shake.X, _Decay);
            _Jitter.Y = Retard(_Jitter.Y + _Shake.Y, _Decay);
        }

        public void ApplyThrust()
        {
            ApplyThrust(_Thrust);
        }

        public void ApplyThrust(PointF Thrust)
        {
            _Velocity.X = Retard(_Velocity.X + _Thrust.X, _Friction);
            _Velocity.Y = Retard(_Velocity.Y + _Thrust.Y, _Friction);
        }

        public void ApplyWind() // Needs to be limited to no faster than the speed of the wind
        {
            ApplyExternalForce(_WindSource.X, _WindSource.Y, _Wind);
        }

        public void ApplyWind(PointF WindSource, float Wind)  // Needs to be scaled for area and limited to no faster than the speed of the wind
        {
            ApplyExternalForce(WindSource.X, WindSource.Y, Wind);
        }

        public void ApplyWind(float X, float Y, float Wind) // Needs to be scaled for area and limited to no faster than the speed of the wind
        {
            ApplyExternalForce(X, Y, Wind);
        }

        public void ApplyGravity()
        {
            ApplyExternalForce(_GravitySource.X, _GravitySource.Y, _Gravity); // Needs to be scaled for mass and distance
        }

        public void ApplyGravity(PointF GravitySource, float Gravity)
        {
            ApplyExternalForce(GravitySource.X, GravitySource.Y, Gravity); // Needs to be scaled for mass and distance
        }

        public void ApplyGravity(float X, float Y, float Gravity)
        {
            ApplyExternalForce(X, Y, Gravity); // Needs to be scaled for mass and distance
        }

        public void ApplyMomentum()
        {
            ApplySpin();
            ApplyJitter();
            ApplyVelocity();
        }

        public void ApplySpin()
        {
            ApplySpin(Spin);
        }

        public void ApplySpin(float Spin)
        {
            _Spin += _Torque;
        }

        public void ApplyJitter()
        {
            ApplyJitter(Jitter.X, Jitter.Y);
        }

        public void ApplyJitter(PointF Jitter)
        {
            ApplyJitter(Jitter.X, Jitter.Y);
        }

        public void ApplyJitter(float X, float Y)
        {
            if (0f == X && 0f == Y) return;

            // Convert jitter limits into a random (plus or minus) sprite position offset

            X = ((float)Aleatory.NextDouble() * X * 2) - X;
            Y = ((float)Aleatory.NextDouble() * Y * 2) - Y;

            _ScreenCoverage.X += X;
            _ScreenCoverage.Y += Y;

            // Invalidate a rectangle that includes the new and old sprite locations to be updated on the next form paint event

            Form.Invalidate(new Rectangle((int)(X >= 0f ? _ScreenCoverage.X - (X) : _ScreenCoverage.X), (int)(Y >= 0f ? _ScreenCoverage.Y - (Y) : _ScreenCoverage.Y), (int)(_ScreenCoverage.Width + Math.Ceiling(Math.Abs(X))), (int)(_ScreenCoverage.Height + Math.Ceiling(Math.Abs(Y)))));
        }

        public void ApplyVelocity()
        {
            ApplyVelocity(Velocity.X, Velocity.Y);
        }

        public void ApplyVelocity(PointF Velocity)
        {
            ApplyVelocity(Velocity.X, Velocity.Y);
        }

        public void ApplyVelocity(float X, float Y)
        {
            if (0f == X && 0f == Y) return;

            _ScreenCoverage.X += (int)X;
            _ScreenCoverage.Y += (int)Y;

            // Invalidate a rectangle that includes the new and old sprite locations to be updated on the next form paint event

            Form.Invalidate(new Rectangle((int)(X >= 0f ? _ScreenCoverage.X - (X) : _ScreenCoverage.X), (int)(Y >= 0f ? _ScreenCoverage.Y - (Y) : _ScreenCoverage.Y), (int)(_ScreenCoverage.Width + Math.Ceiling(Math.Abs(X))), (int)(_ScreenCoverage.Height + Math.Ceiling(Math.Abs(Y)))));
        }

        public void FirstFrame()
        {
            _Index = 0;

            Visible = true;
        }

        public void NextFrame()
        {
            Visible = true;
        }

        public void LastFrame()
        {
            _Index = _Frames - 1;

            Visible = true;
        }

        public void Draw(Graphics g)
        {
            if (Hidden) return;

            g.DrawImage(_Frame[_Index], Rectangle.Round(_ScreenCoverage), 0f, 0f, _ScreenCoverage.Width, _ScreenCoverage.Height, GraphicsUnit.Pixel);

            // Cycle to the next animation frame (handles positive and negative index wrap around)

            _Index = ((uint)((int)_Index + _Iterator + _Frames) % _Frames);
        }

        #region Internal Methods

        private float Retard(float Momentum, float Entropy)
        {
            return Math.Abs(Momentum) > Entropy ? Momentum - (Math.Sign(Momentum) * Entropy) : 0;
        }

        private void ApplyExternalForce(float X, float Y, float Magnitude)
        {
            if (0 == Magnitude) return;

            X -= _ScreenCoverage.X;
            Y -= _ScreenCoverage.Y;

            float Distance = (float)Math.Sqrt((X * X) + (Y * Y));

            if (0 == Distance) return; // No force direction because we're on the source

            _Velocity.X += Magnitude * (X / Distance);
            _Velocity.Y += Magnitude * (Y / Distance);
        }

        private void EncounterBoundries()
        {
            // Calculate the X and Y offsets to build the proper collision coverage parallelagram

            float X = _ScreenCoverage.X - _PreviousScreenCoverage.X;
            float Y = _ScreenCoverage.Y - _PreviousScreenCoverage.Y;

            // Build the collision coverage parallelagram

            PointF E, F, G, H;

            if (Math.Sign(X) == Math.Sign(Y))
            {
                // upper right and lower left

                E = new PointF(_ScreenCoverage.X + _ScreenCoverage.Width, _ScreenCoverage.Y);
                F = new PointF(_ScreenCoverage.X, _ScreenCoverage.Y + _ScreenCoverage.Height);
                G = new PointF(_PreviousScreenCoverage.X + _PreviousScreenCoverage.Width, _PreviousScreenCoverage.Y);
                H = new PointF(_PreviousScreenCoverage.X, _PreviousScreenCoverage.Y + _PreviousScreenCoverage.Height);
            }
            else
            {
                // upper left and lower right

                E = new PointF(_ScreenCoverage.X, _ScreenCoverage.Y);
                F = new PointF(_ScreenCoverage.X + _ScreenCoverage.Width, _ScreenCoverage.Y + _ScreenCoverage.Height);
                G = new PointF(_PreviousScreenCoverage.X, _PreviousScreenCoverage.Y);
                H = new PointF(_PreviousScreenCoverage.X + _PreviousScreenCoverage.Width, _PreviousScreenCoverage.Y + _PreviousScreenCoverage.Height);
            }

            // Detect entry into a boundry coverage

            // Polygon XXX;
            // Polygon YYY;

            // Polygon.Intersects(XXX, YYY);

            // XXX.Intersects(YYY);

            // PolygonIntersection(_Sinistral.ScreenCoverage, E, F, G, H);
            // PolygonIntersection(_Dextral.ScreenCoverage, E, F, G, H);
            // PolygonIntersection(_Cieling.ScreenCoverage, E, F, G, H);
            // PolygonIntersection(_Floor.ScreenCoverage, E, F, G, H);

            // Resolve collision effects

            // Update the collision reference position

            _PreviousScreenCoverage = _ScreenCoverage;
        }
    }
        #endregion
}