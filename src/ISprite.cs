using System.Drawing;

namespace Invaders
{
    public enum SpriteType { Abandoned, Absorbing, Afterburner, Attacking, Beaming, Blasting, Boosting, Bouncing, Burning, Captured, Capturing, Catching, Climbing, Crashing, Creeping, Crippled, Critical, Damaged, Darting, Dead, Decayed, Decaying, Digging, Diving, Drifting, Driling, Dry, Dying, Embarassed, Empty, Energized, Enraged, Evacuated, Exploded, Exploding, Firing, Floating, Flooded, Flooding, Flying, Generating, Generated, Gliding, Healthy, Hopping, Hovering, Injured, Intense, Invincible, Jumping, Landing, Launching, Lost, Melted, Melting, Naked, None, Overloaded, Overloading, Paving, Poisoned, Pouring, Racing, Regenerated, Restored, Revived, Rolling, Rubble, Scratched, Shattered, Shocked, Shocking, Shooting, Sick, Sitting, Skidding, Sleeping, Smoldering, Soaring, Stopping, Surprised, Swimming, Throwing, Triumphant, Twitching, Undead, Vandalized, Venting, Waiting, Walking, Wet };

    interface ISprite
    {
        // The mode indicates the animation to display at any point in the sprite lifecycle

        SpriteType Mode { get; set; }

        // The boundries for this sprite
        // The boundry mode dictates what happens when the sprite touches the wall
        // Boundries may be gapped at the corners do with these feature as you will

        Zone Sinistral { get; set; }        // Left wall
        Zone Dextral { get; set; }          // Right wall
        Zone Cieling { get; set; }          // Top wall
        Zone Floor { get; set; }            // Bottom wall

        // The size and mass of the sprite for collision detection and inertial effects

        Rectangle ScreenCoverage { get; }           // Sprite boundaries

        Size Size { get; }                     // Sprite width and height taken from the bitmap

        int Width { get; }                     // Sprite width is determined according to the bitmap size
        int Height { get; }                    // Sprite height is determined according to the bitmap size

        int Area { get; }                      // Sprite area in pixels (width * height)

        float Mass { get; set; }               // Sprite mass in pixels - default: (width * height)
        PointF MassCenter { get; set; }        // Sprite rotational center - default: (width / 2), (height / 2)

        // Control when the sprite is being displayed

        bool Hidden { get; set; }              // Convenience proxy property
        bool Visible { get; set; }             // Convenience proxy property

        // The screen facing controls the direction of thrust

        float Orientation { get; set; }        // Angle in degrees from starting orientation

        // The screen location is stored as a real to enable smooth low speed motion

        RectangleF Location { get; }           // Sprite screen position

        PointF LeftTop { get; set; }           // Sprite screen position of the upper port corner
        PointF RightTop { get; set; }          // Sprite screen position of the upper starboard corner
        PointF RightBottom { get; set; }       // Sprite screen position of the sprite starboard corner
        PointF LeftBottom { get; set; }        // Sprite screen position of the sprite port corner

        float Left { get; set; }               // Sprite screen position of the port edge
        float Right { get; set; }              // Sprite screen position of the starboard edge
        float Top { get; set; }                // Sprite screen position of the upper edge
        float Bottom { get; set; }             // Sprite screen position of the upper edge

        PointF Center { get; set; }            // Sprite screen position at the center

        // Sprite rotational forces are applied and measured at the top-center point
        // Sprite rotations occur around the center of mass
        // Sprite orientation is the difference between the starting position and the present position
        // Sprite inertia is measured pixels/second on the screen X axis an screen Y axis
        // External forces are measured in pixels/second/second on the screen X axis and screen Y axis
        // Sprite forces are measures in pixels/second/second on the sprite X axis and sprite Y axis

        float Spin { get; set; }               // Pixels per millisecond perpendicular vector at top point of the sprite center line around the geometric center
        float Damper { get; set; }             // Pixels per millisecond reduction in the spin rate
        float Torque { get; set; }             // Pixels per millisecond applied perpendicullarly to the top point of the sprite center line

        // The wobble, vibration, jumpiness of the sprite for a bumpy road, turbulance, or loosing control effect

        PointF Jitter { get; set; }            // The oscillation rate at the center of mass (pixels per millisecond vector)
        float Decay { get; set; }              // The rate of oscillation decay (pixels per millisecond vector)
        PointF Shake { get; set; }             // The oscillation impulse at the center of mass (pixels per millisecond vector)

        // The intrinsic movement, direction, and acceleration of the sprite

        PointF Velocity { get; set; }          // Pixels per millisecond vector of travel at the center of mass
        float Friction { get; set; }           // Pixels per millisecond vector (rolling resistance, drag)
        PointF Thrust { get; set; }            // Pixels per millisecond vector

        // The medium through which the sprite is moving

        Point WindSource { get; set; }         // Screen coordinates
        float Wind { get; set; }               // Pixels per millisecond vector

        // The deflective force of a massive extern body

        Point GravitySource { get; set; }      // Screen coordinates
        float Gravity { get; set; }            // Pixels per millisecond vector
                
        // Methods to assign composite property values via individual paramters

        void SetLeftTop(int X, int Y);
        void SetRightTop(int X, int Y);
        void SetRightBottom(int X, int Y);
        void SetLeftBottom(int X, int Y);

        void SetCenter(int X, int Y);

        void SetMassCenter(float X, float Y);

        void SetShake(float X, float Y);

        void SetJitter(float X, float Y);

        void SetVelocity(float X, float Y);

        void SetThrust(float X, float Y);

        void SetWindSource(int X, int Y);

        void SetGravitySource(int X, int Y);

        // Methods to perform actions

        // physics methods

        void ApplyPhysics();                                    // Apply a time unit of force and then update the sprite

        // force (accleration) methods

        void ApplyForces();                                      // Update the inertial state of the sprite

        void ApplyTorque();                                     // Add an impulse of the current torque to the rotational inertia amount
        void ApplyTorque(float Torque);                         // Add an impulse of torque to the rotational inertia amount

        void ApplyShake();                                      // Add an impulse of the current shake and decay to the vibrational inertia amount
        void ApplyShake(PointF Shake);                          // Add an impulse of shake to the vibrational inertia amount

        void ApplyThrust();                                     // Add an impulse of the current thrust, gravity, wind, and friction to the translational inertia amount
        void ApplyThrust(PointF Thrust);                        // Add an impulse of thrust to the translational inertia amount

        void ApplyWind();                                       // Blow the sprite by the current wind amount
        void ApplyWind(PointF WindSource, float Wind);          // Blow the sprite by the current wind amount
        void ApplyWind(float X, float Y, float Wind);           // Blow the sprite by the current wind amount

        void ApplyGravity();                                    // Attract the sprite by the current gravitaional amount
        void ApplyGravity(PointF GravitySource, float Gravity); // Attract the sprite by the current gravitaional amount
        void ApplyGravity(float X, float Y, float Gravity);     // Attract the sprite by the current gravitaional amount

        // momentum (inertia) methods

        void ApplyMomentum();                                   // Update the orientation, jitter, and location of the sprite

        void ApplySpin();                                       // Rotate the sprite by the current rotational inertia amount
        void ApplySpin(float Spin);                             // Rotate the sprite by an arbitrary rotational inertia amount

        void ApplyJitter();                                     // Vibrate the sprite by the current vibrational inertia amount
        void ApplyJitter(PointF Jitter);                        // Vibrate the sprite by an arbitrary vibrational inertia amount
        void ApplyJitter(float X, float Y);                     // Vibrate the sprite by an arbitrary vibrational inertia amount

        void ApplyVelocity();                                   // Translate the sprite by the current translational inertia amount
        void ApplyVelocity(PointF Velocity);                    // Translate the sprite by an arbitrary translational inertia amount
        void ApplyVelocity(float X, float Y);                   // Translate the sprite by an arbitrary translational inertia amount

        void Draw(Graphics g);
    }
}
