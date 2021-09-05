using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;

//using System.Diagnostics;

namespace Invaders
{ 
    public partial class Game : Form
    {
        private static Countdown Countdown;

        private static Turret Turret;

        private static Timer Timer;

        public Game()
        {
            InitializeComponent();

            //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            //Debug.AutoFlush = true;

            Turret = Turret.FactoryMethod(this, new Point(0, 0));

            Countdown = new Countdown(SpriteType.Waiting, this, Properties.Resources.test_pattern, new Point((this.Width - Properties.Resources.test_pattern.Width) / 2, (this.Height - Properties.Resources.test_pattern.Height) / 2), new PointF(0f, 0f), null);

            Timer = new Timer();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Timer.Enabled = false;

            this.Paint += new PaintEventHandler(this.Countdown_Paint);

            Timer.Tick += Countdown_Ticked;

            Timer.Interval = 250;

            Timer.Enabled = true;
        }

        void Countdown_Ticked(object sender, EventArgs e)
        {
            Timer.Enabled = false;

            if (Countdown.Hidden)
            {
                Countdown.Iterator = -1;

                Countdown.LastFrame();
            }
            else
            {
                Countdown.NextFrame();
            }

            this.Update();
            
            Timer.Enabled = true;
        }

        private void Countdown_Paint(object sender, PaintEventArgs e)
        {
            Countdown.Draw(e.Graphics);

            if (Countdown.Index + 1 == Countdown.Frames)
            {
                Timer.Tick -= Countdown_Ticked;
                this.Paint -= Countdown_Paint;

                this.Paint += new PaintEventHandler(this.Game_Paint);

                Timer.Tick += Game_Ticked;

                Timer.Interval = 50;

                Timer.Enabled = true;

                Turret.Visible = true;
            }
        }

        void Game_Ticked(object sender, EventArgs e)
        {
            KeystrokeHandler();
            KeyboardHandler();

            Turret.ApplyPhysics();

            this.Update();
        }

        private void Game_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Turret.Draw(g);
        }

        private Keys PressedKey = Keys.None;

        private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            PressedKey = e.KeyCode;
        }

        private void KeystrokeHandler()
        {
            switch (PressedKey)
            {
                case Keys.Escape:
                    Application.Exit();
                    break;

                case Keys.Space:
                    //Shell._Location = Turret.Gunpoint;
                    break;

                default:
                    break;
            }

            PressedKey = Keys.None;
        }

        private void KeyboardHandler()
        {
            int X = 0;
            int Y = 0;
            int Boost = 0;
            int Break = 0;

            // Requires
            // Assembly: PresentationCore
            // Assembly: WindowsBase

            if (0 < (Keyboard.GetKeyStates(Key.Left) & KeyStates.Down)) { X -= 1; }
            if (0 < (Keyboard.GetKeyStates(Key.Right) & KeyStates.Down)) { X += 1; }

            if (0 < (Keyboard.GetKeyStates(Key.Up) & KeyStates.Down)) { Y -= 1; }
            if (0 < (Keyboard.GetKeyStates(Key.Down) & KeyStates.Down)) { Y += 1; }

            if (0 < (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down)) { Boost += 1; }
            if (0 < (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down)) { Boost += 1; }

            if (0 < (Keyboard.GetKeyStates(Key.End) & KeyStates.Down)) { Break += 1; }

            if (0 > X)
            {
                if (0 > Y)
                {
                    Turret.LeftUpThrust();
                }
                else if (0 == Y)
                {
                    Turret.LeftThrust();
                }
                else // if (0 < Y)
                {
                    Turret.LeftDownThrust();
                }
            }
            else if (0 == X)
            {
                if (0 > Y)
                {
                    Turret.UpThrust();
                }
                else if (0 == Y)
                {
                    if (0 < Break) Turret.Stop();
                }
                else // if (0 < Y)
                {
                    Turret.DownThrust();
                }
            }
            else // if (0 < X)
            {
                if (0 > Y)
                {
                    Turret.RightUpThrust();
                }
                else if (0 == Y)
                {
                    Turret.RightThrust();
                }
                else // if (0 < Y)
                {
                    Turret.RightDownThrust();
                }
            }
        }
    }
}
