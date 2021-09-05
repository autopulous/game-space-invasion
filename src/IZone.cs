using System.Drawing;

namespace Invaders
{
    public enum ZoneType { Deadly, Blocking, Reflecting, Wrapping, Scrolling };

    interface IZone
    {
        Rectangle ScreenCoverage { get; set; }
        ZoneType Mode { get; set; }
    }
}
