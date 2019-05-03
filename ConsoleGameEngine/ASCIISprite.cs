using System;
using System.Linq;

namespace RetroEngine
{
    /// <summary>
    /// Holds ascii drawing that represents a game object.
    /// </summary>
    public class ASCIISprite
    {
        public char[,] ascii { get; set; }
        public bool[,] collision { get; set; }
        public bool solid { get; set; }

        public ASCIISprite()
        {
            this.solid = true;
        }
        public ASCIISprite(char[,] draw)
        {
            this.solid = true;
            this.ascii = draw;
        }
        public ASCIISprite(char[,] draw, bool[,] collision)
        {
            this.ascii = draw;
            this.collision = collision;
        }

        /// <summary>
        /// Returns width of GameObject.
        /// </summary>
        public int width() => ascii.GetLength(1);
        /// <summary>
        /// Returns height of GameObject.
        /// </summary>
        public int height() => ascii.GetLength(0);

        /// <summary>
        /// Generates collision based on char array.
        /// </summary>
        /// <param name="charArray">Char array of GameObject</param>
        /// <param name="excluded">Character to exclude from collision generation, defaults to {float space}(' ')</param>
        /// <returns>Generated collision</returns>
        public static bool[,] GenerateCollision(char[,] charArray, char excluded = ' ')
        {
            bool[,] Collision = new bool[charArray.GetLength(0), charArray.GetLength(1)];

            for (int y = 0; y < charArray.GetLength(0); y++)
            {
                for (int x = 0; x < charArray.GetLength(1); x++)
                {
                    if (excluded == charArray[y, x])
                    {
                        continue;
                    }

                    Collision[y, x] = true;
                }
            }
            return Collision;
        }
        /// <summary>
        /// Generates collision based on char array with array of characters to exclude from generation.
        /// </summary>
        /// <param name="charArray">Char array of GameObject</param>
        /// <param name="excluded">Multiple characters to exclude from collision generation</param>
        /// <returns>Generated collision</returns>
        public static bool[,] GenerateCollision(char[,] charArray, char[] excluded)
        {
            bool[,] Collision = new bool[charArray.GetLength(0), charArray.GetLength(1)];

            for (int y = 0; y < charArray.GetLength(0); y++)
            {
                for (int x = 0; x < charArray.GetLength(1); x++)
                {
                    if (excluded.Contains(charArray[y, x]))
                    {
                        continue;
                    }

                    Collision[y, x] = true;
                }
            }
            return Collision;
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