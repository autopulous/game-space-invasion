using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Invaders
{
    class Countdown : SimpleSprite
    {
        public Countdown(SpriteType Mode, Form Form, Bitmap Animation, Point LeftTop, PointF Velocity, SoundEffect SoundEffect) : base(Mode, Form, Animation, LeftTop, Velocity, SoundEffect)
        {
        }
    }
}