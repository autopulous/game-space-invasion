using System;
using System.Drawing;

namespace Invaders
{
    class Zone : IZone
    {
        public Rectangle ScreenCoverage { get; set; }
        public ZoneType Mode { get; set; }

        public Zone(Rectangle ScreenCoverage, ZoneType Mode)
        {
            this.ScreenCoverage = ScreenCoverage;
            this.Mode = Mode;
        }
    }
}
