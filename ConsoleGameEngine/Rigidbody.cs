namespace RetroEngine
{
    public class Rigidbody : Object
    {
        /// <summary>
        /// The velocity of the parent GameObject.
        /// Added every
        /// </summary>
        public Vector2 velocity { get; set; }
        public float mass { get; set; }
        public float density { get; set; } = 1;
        public Vector2 centerOfMass { get; set; }
        public float gravityScale { get; set; } = 1;

        internal Vector2 internalVelocity { get; set; }
        internal Vector2 startPosition;
        internal bool moving = false;

        public Rigidbody()
        {
            velocity = new Vector2(0, 0);
        }
        public Rigidbody(Vector2 velocity)
        {
            this.velocity = velocity;
        }

        internal void Reset()
        {
            startPosition = new Vector2();
            moving = false;
        }
        
        internal Vector2 GravityForce()
        {
            return new Vector2(0, mass * density * gravityScale * Settings.GravityMultiplier);
        }

        internal Vector2 CalculateCenterOfMass(ASCIISprite sprite)
        {
            int totalX = 0, totalY = 0;
            int points = 0;

            for (int y = 0; y < sprite.collision.GetLength(0); y++)
            {
                for (int x = 0; x < sprite.collision.GetLength(1); x++)
                {
                    if (sprite.collision[y, x])
                    {
                        totalX += x;
                        totalY += y;
                        points++;
                    }
                }
            }

            return new Vector2(points != 0 ? totalX / points : 0, points != 0 ? totalY / points : 0);
        }

        public void UpdateCenterOfMass(ASCIISprite sprite)
        {
            centerOfMass = CalculateCenterOfMass(sprite);
        }

        public static float CalculateMass(bool[,] collision)
        {
            return CalculateMass(collision, new Vector2(1, 1));
        }
        public static float CalculateMass(bool[,] collision, Vector2 scale)
        {
            float mass = 0;
            for (int y = 0; y < collision.GetLength(0); y++)
            {
                for (int x = 0; x < collision.GetLength(1); x++)
                {
                    if (collision[y, x])
                    {
                        mass++;
                    }
                }
            }
            return mass * scale.x * scale.y;
        }
    }

}


/*
/// <summary>
/// Returns downwards direction with <c>Settings.CoordinateSystemCenter</c> in mind.
/// </summary>
public static Vector2 down()
{
    switch (Settings.CoordinateSystemCenter)
    {
        case CoordinateSystemType.BottomRight:
        case CoordinateSystemType.TopRight:
        case CoordinateSystemType.TopLeft:
            return new Vector2(0, 1);

        case CoordinateSystemType.BottomLeft:
        case CoordinateSystemType.Middle:
            return new Vector2(0, -1);

        default: //This won't happen
            return new Vector2();
    }
}

/// <summary>
/// Returns upwards direction with <c>Settings.CoordinateSystemCenter</c> in mind.
/// </summary>
public static Vector2 up()
{
    switch (Settings.CoordinateSystemCenter)
    {
        case CoordinateSystemType.BottomRight:
        case CoordinateSystemType.TopRight:
        case CoordinateSystemType.TopLeft:
            return new Vector2(0, -1);

        case CoordinateSystemType.BottomLeft:
        case CoordinateSystemType.Middle:
            return new Vector2(0, 1);

        default: //This won't happen
            return new Vector2();
    }
}


/// <summary>
/// Returns upwards direction with <c>Settings.CoordinateSystemCenter</c> in mind.
/// </summary>
public static Vector2 right()
{
    switch (Settings.CoordinateSystemCenter)
    {
        case CoordinateSystemType.BottomRight:
        case CoordinateSystemType.TopRight:
            return new Vector2(-1, 0);

        case CoordinateSystemType.BottomLeft:
        case CoordinateSystemType.TopLeft:
            return new Vector2(1, 0);

        case CoordinateSystemType.Middle:
            return new Vector2(1, 0);

        default: //This won't happen
            return new Vector2();
    }
}


/// <summary>
/// Returns upwards direction with <c>Settings.CoordinateSystemCenter</c> in mind.
/// </summary>
public static Vector2 left()
{
    switch (Settings.CoordinateSystemCenter)
    {
        case CoordinateSystemType.BottomRight:
        case CoordinateSystemType.TopRight:
            return new Vector2(1, 0);

        case CoordinateSystemType.BottomLeft:
        case CoordinateSystemType.TopLeft:
            return new Vector2(-1, 0);

        case CoordinateSystemType.Middle:
            return new Vector2(-1, 0);

        default: //This won't happen
            return new Vector2();
    }
}*/

#region Timer
/*
updateTimer = new Timer((e) =>
{
    // Internal Update loop
    long currentTimestamp = TimeStamp();
    float delta = (float)currentTimestamp - (float)previousFrameTimestamp;

    try { Time.deltaTime = delta / 1000; }
    catch (DivideByZeroException) { Time.deltaTime = 0; }

    previousFrameTimestamp = TimeStamp();


    if (Settings.FPSCounter)
    {
        float FPS;
        float timepassed;
        try
        {
            timepassed = (float)(TimeStamp() - gameStartedTimestamp) / (float)1000;
            FPS = TotalFrames / timepassed;
        }
        catch (DivideByZeroException)
        {
            FPS = 0;
            timepassed = 0;
        }

        Console.Title = $"FPS: {FPS}, time: {timepassed}";
    }


    Console.Clear();


    if (Debug.ShowCoordinateSystem)
    {
        Debug.DrawCoordinateSystem();
    }

    // Draw gameobjects
    foreach (GameObject obj in Utility.SortGameObjects(Utility.GetGameObjects()))
    {

        DrawGameObject(obj);
    }

    // External update loop
    if (UpdateMethod != null)
    {
        UpdateMethod.Invoke();
    }

    TotalFrames++;
}, null, 0, (int)(1000 / TargetFramesPerSecond));
*/
#endregion