using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetroEngine
{
    public static class Input
    {
        /// <summary>
        /// Checks for key presses during each frame.
        /// </summary>
        public static bool ListenForKeys { get; set; } = true;

        public static float HorizontalAxis { get; private set; }
        public static float VerticalAxis { get; private set; }

        //private static Dictionary<int, ConsoleKey> frameKeys = new Dictionary<int, ConsoleKey>();
        //private static List<ConsoleKey> frameKeys = new List<ConsoleKey>();
        private static Task keyListener = null;
        //private static int lastFrame = 0;
        //private static int lastFrameKeyPressed = 0;
        
        /// <summary>
        /// List that contains keys that were pressed during _this_ frame. Reset every frame.
        /// </summary>
        private static List<ConsoleKey> currentFrameKeys = new List<ConsoleKey>();
        private static List<ConsoleKey> lastFrameKeys = new List<ConsoleKey>();

        public static bool GetKey(ConsoleKey key)
        {
            //return frameKeys.Contains(key);
            return currentFrameKeys.Contains(key);
            /*
            if (frameKeys.TryGetValue(Time.frameCount + 1, out ConsoleKey pressedKey))
            {
                return pressedKey == key;
            }
            else { return false; }*/
        }

        /// <summary>
        /// Returns true if last frame did receive key, but current frame did not.
        /// </summary>
        /// <returns></returns>
        public static bool GetKeyUp(ConsoleKey key){
            return lastFrameKeys.Contains(key) && !currentFrameKeys.Contains(key);
        }

        /// <summary>
        /// Returns true if last frame did not receive key, but current frame did.
        /// </summary>
        /// <returns></returns>
        public static bool GetKeyDown(ConsoleKey key)
        {
            return !lastFrameKeys.Contains(key) && currentFrameKeys.Contains(key);
        }

        /// <summary>
        /// Initiates key listening thread.
        /// </summary>
        public static void ListenKeys()
        {
            if (keyListener != null)
            {
                return;
            }

            keyListener = Task.Run(() =>
            {
                ListenForKeys = true;
                while (ListenForKeys)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    Debug.Status.Log("New key");
                    currentFrameKeys.Add(key);
                    /*
                    frameKeys.Insert(0, key);
                    if (frameKeys.Count > 10)
                    {
                        frameKeys.RemoveRange(10, frameKeys.Count);
                    }*/
                    //frameKeys[Time.frameCount + 1] = key;
                    
                //frameKeys.Add(Game.TotalFrames + 1, Console.ReadKey(true).Key);
                    //lastFrame = Game.TotalFrames + 1;
                }
            });
        }

        public enum Axis { Horizontal, Vertical, }

        /// <summary>
        /// Try using <code>HorizontalAxis</code>/<code>VerticalAxis</code> instead.
        /// </summary>
        public static float GetAxis(Axis axis)
        {
            if (axis.Equals(Axis.Horizontal))
            {
                return HorizontalAxis;
            }
            else
            {
                return VerticalAxis;
            }
        }

        /// <summary>
        /// Called every frame to re-instantiate currentFrameKeys and update lastFrameKeys.
        /// </summary>
        internal static void UpdateKeyFrame(){
            lastFrameKeys = currentFrameKeys;
            currentFrameKeys = new List<ConsoleKey>();
            Debug.Status.Log("New frame");
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