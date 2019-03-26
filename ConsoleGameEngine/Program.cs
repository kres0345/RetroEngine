using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RetroEngine
{
    public enum CoordinateSystemType { TopLeft, BottomLeft, Middle, TopRight, BottomRight, }

    public class Rigidbody
    {
        public Vector2 velocity { get; set; }

        public Rigidbody()
        {
            velocity = new Vector2(0, 0);
        }
        public Rigidbody(Vector2 velocity)
        {
            this.velocity = velocity;
        }
    }

    public class GameObject
    {
        public ASCIISprite sprite { get; set; }
        public Transform transform { get; set; }
        public Rigidbody rigidbody { get; set; }
        public Events events { get; }
        public string name { get; set; }
        public int? identifier { get; private set; }
        public bool activeSelf { get; private set; }

        public GameObject()
        {
            sprite = new ASCIISprite();
            transform = new Transform();
            rigidbody = new Rigidbody();
            events = new Events();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(ASCIISprite sprite)
        {
            this.sprite = sprite;
            transform = new Transform();
            rigidbody = new Rigidbody();
            events = new Events();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(Transform transform)
        {
            this.transform = transform;
            rigidbody = new Rigidbody();
            sprite = new ASCIISprite();
            events = new Events();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(GameObject gameObject)
        {
            this.sprite = gameObject.sprite;
            this.rigidbody = gameObject.rigidbody;
            this.transform = gameObject.transform;
            this.events = gameObject.events;
            this.name = gameObject.name;
            this.activeSelf = gameObject.activeSelf;
        }

        /// <summary>
        /// Gets the instantiated GameObject.
        /// </summary>
        /// <returns>Already instantiated GameObject</returns>
        public GameObject Get()
        {
            if (identifier == null || (int)identifier <= -1)
            {
                return null;
            }

            if (identifier > Game.Objects.Count - 1)
            {
                throw new Exceptions.GameObjectNotInstantiatedException();
            }

            //Debug.Log("ID: " + identifier);
            //Debug.Log("Objects: " + Game.Objects);

            return Game.Objects[(int)identifier];
        }
        
        /// <summary>
        /// Returns clone of current object.
        /// </summary>
        /// <returns>Cloned GameObject</returns>
        public GameObject Clone() => new GameObject(this);

        /// <summary>
        /// Replaces the instantiated GameObject with this.
        /// </summary>
        public void Update()
        {
            if (identifier == null)
            {
                Debug.Log("Id is null of " + name);
                return;
            }

            Game.Objects[(int)identifier] = this;
        }

        /// <summary>
        /// Destroys GameObject, optionally after delay.
        /// </summary>
        /// <param name="delay">Destroys GameObject after delay</param>
        public void Destroy(float delay = 0f)
        {
            if (delay <= 0)
            {
                Game.Objects[(int)this.identifier] = null;
            }
            else
            {
                Task.Run(async () =>
                {
                    await Task.Delay((int)(delay * 1000));
                    Game.Objects[(int)this.identifier] = null;
                });
            }
        }

        public void SetActive(bool value)
        {
            activeSelf = value;
        }

        /// <summary>
        /// Destroys GameObject specified.
        /// </summary>
        /// <param name="obj">The GameObject to destroy</param>
        /// <param name="delay">Time before GameObject is destroyed.</param>
        public static void Destroy(GameObject obj, float delay = 0f)
        {
            if (delay <= 0)
            {
                Game.Objects[(int)obj.identifier] = null;
            }
            else
            {
                Task.Run(async () =>
                {
                    await Task.Delay((int)(delay * 1000));
                    Game.Objects[(int)obj.identifier] = null;
                });
            }
        }

        /// <summary>
        /// Instantiates GameObject.
        /// </summary>
        /// <param name="original">The GameObject to instantiate</param>
        public static GameObject Instantiate(GameObject original)
        {
            GameObject t = new GameObject(original);
            t.identifier = Game.Objects.Count;

            Game.Objects.Add(t);
            return t;
        }
        public static GameObject Instantiate(GameObject original, Vector2 position)
        {
            GameObject t = new GameObject(original);
            t.identifier = Game.Objects.Count;
            t.transform.position = position;

            Game.Objects.Add(t);
            return t;
        }

        /// <summary>
        /// Find GameObject by name.
        /// </summary>
        /// <param name="name"></param>
        public static GameObject Find(string name) => Game.Objects.Where(i => i.name == name).FirstOrDefault();

        public override string ToString() => this.name;

        public class Events
        {
#pragma warning disable 0649
            public Func<int> OnCollisionEnter;
            public Func<int> OnCollisionStay;
            public Func<int> OnCollisionExit;
#pragma warning restore 0649

            public bool TryGetOnCollisionEnter(out Func<int> OnCollisionEnter)
            {
                OnCollisionEnter = this.OnCollisionEnter;
                return this.OnCollisionEnter != null;
            }

            public bool TryGetOnCollisionStay(out Func<int> OnCollisionStay)
            {
                OnCollisionStay = this.OnCollisionStay;
                return this.OnCollisionStay != null;
            }

            public bool TryGetOnCollisionExit(out Func<int> OnCollisionExit)
            {
                OnCollisionExit = this.OnCollisionExit;
                return this.OnCollisionExit != null;
            }
        }
    }

    public class Transform
    {
        public Vector2 position { get; set; }
        /// <summary>
        /// The z_index represents the order in which the gameobject is drawn.
        /// And if you remove the '_', it sounds like a toothpaste brand.
        /// </summary>
        public int z_index { get; set; }

        public Transform()
        {
            position = new Vector2(0, 0);
            z_index = 10;
        }
        public Transform(Vector2 position)
        {
            this.position = position;
            z_index = 10;
        }

        /// <summary>
        /// 'Adds' vector2 to transform position.
        /// </summary>
        public void Translate(Vector2 translation)
        {
            position += translation;
        }
    }

    /// <summary>
    /// Holds ascii drawing that represents a game object.
    /// </summary>
    public class ASCIISprite
    {
        public char[,] draw { get; set; }
        public bool[,] collision { get; set; }
        public bool solid { get; set; }

        public ASCIISprite()
        {
            this.solid = true;
        }
        public ASCIISprite(char[,] draw)
        {
            this.solid = true;
            this.draw = draw;
        }
        public ASCIISprite(char[,] draw, bool[,] collision)
        {
            this.draw = draw;
            this.collision = collision;
        }

        /// <summary>
        /// Returns width of GameObject.
        /// </summary>
        public int width() => draw.GetLength(1);
        /// <summary>
        /// Returns height of GameObject.
        /// </summary>
        public int height() => draw.GetLength(0);

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

    /// <summary>
    /// Vector2 holds 2-dimensionel coordinate set(x and y).
    /// </summary>
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        
        /// <summary>
        /// Shorthand for writing Vector2(0, 0).
        /// </summary>
        public static Vector2 zero { get; } = new Vector2(0, 0);
        /// <summary>
        /// Shorthand for writing Vector(1, 0).
        /// </summary>
        public static Vector2 right { get; } = new Vector2(1, 0);
        /// <summary>
        /// Shorthand for writing Vector(-1, 0).
        /// </summary>
        public static Vector2 left { get; } = new Vector2(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector(0, -1).
        /// </summary>
        public static Vector2 up { get; } = new Vector2(0, -1);
        /// <summary>
        /// Shorthand for writing Vector(0, 1).
        /// </summary>
        public static Vector2 down { get; } = new Vector2(0, 1);

        /// <summary>
        /// Compares this with another Vector2.
        /// </summary>
        public bool Equals(Vector2 vector2)
        {
            return GetHashCode() == vector2.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Compare this with another Vector2, with rounded numbers.
        /// </summary>
        public bool EqualsRound(Vector2 vector2)
        {
            return (int)x == (int)vector2.x && (int)y == (int)vector2.y;
        }

        /// <summary>
        /// Returns Vector2 with rounded numbers.
        /// </summary>
        /// <returns></returns>
        public Vector2 Rounded()
        {
            return new Vector2((int)x, (int)y);
        }

        /// <summary>
        /// Set the x and y value.
        /// </summary>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Add to the x and y value.
        /// </summary>
        public void Add(float x, float y)
        {
            this.x += x;
            this.y += y;
        }


        public override string ToString()
        {
            return $"({x.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {y.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return (int)a.x == (int)b.x && (int)a.y == (int)b.y;
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            a.x += b.x;
            a.y += b.y;
            return a;
            //return new Vector2(a.x + b.x, a.y + b.y);
        }
        public static Vector2 operator +(Vector2 a, float b)
        {
            a.x += b;
            a.y += b;
            return a;
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            return a;
            //return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(float d, Vector2 a)
        {
            a.x *= d;
            a.y *= d;
            return a;
            //return new Vector2(a.x * d, a.y * d);
        }
        public static Vector2 operator *(Vector2 a, float d)
        {
            a.x *= d;
            a.y *= d;
            return a;
            //return new Vector2(a.x * d, a.y * d);
        }
        public static Vector2 operator *(decimal d, Vector2 a)
        {
            a.x *= (float)d;
            a.y *= (float)d;
            return a;
            //return new Vector2(a.x * (float)d, a.y * (float)d);
        }
        public static Vector2 operator *(Vector2 a, decimal d)
        {
            a.x *= (float)d;
            a.y *= (float)d;
            return a;
            //return new Vector2(a.x * (float)d, a.y * (float)d);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            return a;
        }
    }

    public static class UI
    {

        public class Image
        {

        }
    }

    public static class Settings
    {
        /// <summary>
        /// WARNING: Isn't implemented yet, or will never be, up for debate.
        /// Defines the coordinate (0, 0) point.
        /// </summary>
        public static CoordinateSystemType CoordinateSystemCenter = CoordinateSystemType.TopLeft;

        /// <summary>
        /// Defines game boundaries and render size.
        /// </summary>
        public static int GameSizeWidth { get; set; } = 25;
        /// <summary>
        /// Defines game boundaries and render size.
        /// </summary>
        public static int GameSizeHeight { get; set; } = 10;

        /// <summary>
        /// Makes the grid in the console equal size.
        /// The width - height ratio changes from 1:1 to 2:1. 
        /// (A char placed in one cell is placed in 2 cells.)
        /// </summary>
        public static bool SquareMode { get; set; } = false;

        public static int TargetFramesPerSecond { get; set; } = 60;
    }

    public static class Input
    {
        /// <summary>
        /// Checks for key presses during each frame.
        /// </summary>
        public static bool ListenForKeys { get; set; } = true;

        public static float HorizontalAxis { get; private set; }
        public static float VerticalAxis { get; private set; }

        private static Dictionary<int, ConsoleKey> frameKeys = new Dictionary<int, ConsoleKey>();
        private static Task keyListener = null;
        //private static int lastFrame = 0;

        public static bool GetKey(ConsoleKey key)
        {
            if (frameKeys.TryGetValue(Game.TotalFrames + 1, out ConsoleKey pressedKey))
            {
                return pressedKey == key;
            }
            else { return false; }
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
                    frameKeys[Game.TotalFrames + 1] = key;
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
    }

    public static class Game
    {
        /// <summary>
        /// List of all GameObjects.
        /// </summary>
        /// <remarks>Objects must NOT be removed from this list, they should be nullified.</remarks>
        public static List<GameObject> Objects { get; } = new List<GameObject>();
        public static char[,] Background { get; set; } = new char[Settings.GameSizeHeight, Settings.GameSizeWidth];
        public static Action UpdateMethod { get; set; }
        public static Action FixedUpdateMethod { get; set; }
        public static Action StartMethod { get; set; }
        public static int TotalFrames { get; private set; }
        public static long GameStartedTimestamp { get; private set; }

        //private static Timer updateTimer;
        private static bool gamePlaying;
        //private static long previousFrameTimestamp = 0;
        public static char[,] gamefield { get; set; } = new char[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static char[,] renderedGamefield { get; set; } = new char[Settings.GameSizeHeight, Settings.GameSizeWidth];
        //private static char[,] differentGamefield { get; set; } = new char[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static int?[,] collisionMap { get; set; } = new int?[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static int?[,] collisionMapRendered { get; set; } = new int?[Settings.GameSizeHeight, Settings.GameSizeWidth];
        //public static Tuple<bool, List<int>>[,] collidedMap { get; set; } = new Tuple<bool, List<int>>[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static List<GameObject> renderedObjects = new List<GameObject>();
        private static long lastFixedFrame;

        public static void Exit() => gamePlaying = false;

        public static void Play()
        {
            // Internal start
            long currentTimestamp = Utility.TimeStamp();
            Console.CursorVisible = false;
            Input.ListenKeys();

            if (Debug.DrawGameBorder)
            {
                for (int x = 0; x < Settings.GameSizeWidth; x++)
                {
                    Utility.SetPixel('-', x, Settings.GameSizeHeight);
                }

                for (int y = 0; y < Settings.GameSizeHeight; y++)
                {
                    Utility.SetPixel('|', Settings.GameSizeWidth, y);
                }
            }

            // External start method
            if (StartMethod != null)
            {
                StartMethod.Invoke();
            }

            // Draw gameobjects
            foreach (GameObject obj in Utility.SortGameObjects(Objects))
            {
                HandleGameObject(obj);
            }

            //previousFrameObjects = Objects;
            GameStartedTimestamp = Utility.TimeStamp();
            lastFixedFrame = Utility.TimeStamp();

            long previousTimestamp = currentTimestamp;
            currentTimestamp = Utility.TimeStamp();

            gamePlaying = true;

            // Prevents window from closing
            while (gamePlaying)
            {
                // Internal Update loop
                renderedObjects = Objects;
                renderedGamefield = gamefield;
                previousTimestamp = currentTimestamp;
                currentTimestamp = Utility.TimeStamp();
                gamefield = new char[Settings.GameSizeHeight, Settings.GameSizeWidth];
                

                float delta = currentTimestamp - previousTimestamp;
                Time.deltaTime = delta != 0 ? delta / (float)1000 : 0;

                if (currentTimestamp - lastFixedFrame >= (float)1000 / Settings.TargetFramesPerSecond)
                {
                    // Internal fixed update
                    lastFixedFrame = currentTimestamp;

                    foreach (GameObject item in Objects)
                    {
                        if (item == null || !item.activeSelf || item.identifier == null)
                        {
                            continue;
                        }
                        item.transform.position += item.rigidbody.velocity;
                    }

                    // External fixed update
                    if (FixedUpdateMethod != null)
                    {
                        FixedUpdateMethod.Invoke();
                    }
                }

                if (Debug.FPSCounter)
                {
                    float FPS;
                    float timepassed;
                    try
                    {
                        timepassed = (float)(Utility.TimeStamp() - GameStartedTimestamp) / (float)1000;
                        FPS = TotalFrames / timepassed;
                    }
                    catch (DivideByZeroException) { FPS = 0; timepassed = 0; }

                    Console.Title = $"FPS: {FPS}, deltaTime: {Time.deltaTime}";
                }

                if (Debug.DrawCoordinateSystemEveryFrame)
                    Debug.DrawCoordinateSystem();

                // Draw gameobjects
                //collisionMap = new int?[GameSizeHeight, GameSizeWidth];

                //gamefield = new char[GameSizeHeight, GameSizeWidth];
                //gamefieldRendered = new char[GameSizeHeight, GameSizeWidth];
                foreach (GameObject obj in Utility.SortGameObjects(Objects))
                {
                    HandleGameObject(obj);
                }

                CleanBuffered();
                UpdateBuffer();

                // External update loop
                if (UpdateMethod != null)
                {
                    UpdateMethod.Invoke();
                }

                TotalFrames++;
            }
        }

        private static void HandleGameObject(GameObject obj)
        {
            if (obj == null || !obj.activeSelf || obj.sprite.draw == null)
            {
                return;
            }

            //HandleCollisions(obj.sprite.collision, obj.transform.position, (int)obj.identifier);
            PlaceCharArray(obj.sprite.draw, (int)obj.transform.position.x, (int)obj.transform.position.y);
        }

        private static void PlaceCharArray(char[,] sprite, int x, int y)
        {
            if (sprite == null)
            {
                return;
            }

            int spriteHeight = sprite.GetLength(0);
            int spriteWidth = sprite.GetLength(1);
            for (int loop_y = 0; loop_y < spriteHeight; loop_y++)
            {
                for (int loop_x = 0; loop_x < spriteHeight; loop_x++)
                {
                    SetCell(sprite[loop_y, loop_x], x + loop_x, y + loop_y);
                }
            }
        }

        private static void HandleCollisions(bool[,] collision, Vector2 position, int identifier)
        {
            if (collision == null)
            {
                return;
            }

            bool collided = false;

            for (int y = 0; y < collision.GetLength(0); y++)
            {
                for (int x = 0; x < collision.GetLength(1); x++)
                {
                    //TODO: Implement collision system
                    if (collisionMap[(int)position.y + y, (int)position.x + x] == null)
                    {
                        collisionMap[(int)position.y + y, (int)position.x + x] = identifier;
                    }
                    else
                    {
                        collided = true;
                    }
                }
            }

            if (collided)
            {
                Debug.Log("Collided");
                //Objects[identifier].events.OnCollisionEnter
            }
        }

        private static void CleanBuffered()
        {
            /*
            for (int i = 0; i < Math.Min(renderedObjects.Count, Objects.Count); i++)
            {
                if (renderedObjects[i].transform.position.Integer() != Objects[i].transform.position.Integer()) //TODO: Fix this
                {
                    Vector2 position = renderedObjects[i].transform.position;

                    int spriteHeight = renderedObjects[i].sprite.draw.GetLength(0);
                    int spriteWidth = renderedObjects[i].sprite.draw.GetLength(1);

                    for (int y = 0; y < spriteHeight; y++)
                    {
                        for (int x = 0; x < spriteWidth; x++)
                        {
                            gamefield[(int)position.y + y, (int)position.x + x] = ' ';
                            //SetCell(' ', (int)position.x + x, (int)position.y + y);
                        }
                    }
                }
            }*/
        }

        private static void UpdateBuffer()
        {
            for (int y = 0; y < Settings.GameSizeHeight; y++)
            {
                for (int x = 0; x < Settings.GameSizeWidth; x++)
                {
                    char value = gamefield[y, x];
                    if (value != '\0')
                    {
                        Utility.SetPixel(value, x, y);
                    }
                }
            }
        }

        /// <summary>
        /// Set a specific cell's value.
        /// </summary>
        public static void SetCell(char value, int x, int y)
        {
            if (0 < x && x < Settings.GameSizeWidth &&
                0 < y && y < Settings.GameSizeHeight)
            {
                //Debug.Log(gamefield[y, x] + " : " + gamefieldRendered[y, x]);
                gamefield[y, x] = value;
                //gamefield[y, x] = gamefieldRendered[y, x] == value ? '\0' : value;
            }
        }
        /// <summary>
        /// Sets cell values by an character array, at a location.
        /// </summary>
        /// <param name="HorizontalText">Char array put horizontally or vertically.</param>
        public static void SetCell(char[] values, int x, int y, bool HorizontalText = true)
        {
            if (HorizontalText)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    SetCell(values[i], x + i, y);
                }
            }
            else
            {
                for (int i = 0; i < values.Length; i++)
                {
                    SetCell(values[i], x, y + i);
                }
            }
        }
    }

    public static class Debug
    {
        /// <summary>
        /// Currently affects performance massively.  For drawing coordinate system only once refer to DrawCoordinateSystem method.
        /// </summary>
        public static bool DrawCoordinateSystemEveryFrame { get; set; } = false;
        /// <summary>
        /// Window title is updated to current FPS.
        /// </summary>
        public static bool FPSCounter = true;
        /// <summary>
        /// The maximum width of coordinate system, setting the value to null results in max width being equal to Game.GameSizeWidth.
        /// </summary>
        public static int? CoordinateWidth { get; set; } = null; //100
        /// <summary>
        /// The maximum height of coordinate system, setting the value to null results in max height being equal to Game.GameSizeHeight.
        /// </summary>
        public static int? CoordinateHeight { get; set; } = null; //30
        /// <summary>
        /// The interval of numbers displayed on each axis.
        /// </summary>
        public static int CoordinateInterval { get; set; } = 5;
        /// <summary>
        /// Draws border around the game area.
        /// </summary>
        public static bool DrawGameBorder { get; set; } = false;
        /// <summary>
        /// Log mode, <code>Spool</code> to log the output to a file, <code>DebugIDE</code> to write to special IDE debugging channel.
        /// </summary>
        public static logMode LogMode { get; set; } = logMode.StatusLog;
        private static StreamWriter spoolWriter = null;

        private enum logType { INFO, WARNING, ERROR, }
        public enum logMode { Spool, DebugIDE, StatusLog }

        /// <summary>
        /// Draws coordinate system
        /// </summary>
        public static void DrawCoordinateSystem()
        {
            int Width = CoordinateWidth == null ? Settings.GameSizeWidth : (int)CoordinateWidth;
            int Height = CoordinateHeight == null ? Settings.GameSizeHeight : (int)CoordinateHeight;


            Game.SetCell('x', 0, 0);
            for (int x = CoordinateInterval; x + CoordinateInterval < Math.Min(Width, Console.BufferWidth); x += CoordinateInterval)
            {
                //Utility.SetPixel('|', x, 0);
                Game.SetCell('|', x, 1);
                //Utility.SetPixel(x.ToString(), x, 2);
                Game.SetCell(x.ToString().ToCharArray(), x, 3);
            }

            for (int y = CoordinateInterval; y + CoordinateInterval < Math.Min(Height, Console.BufferHeight); y += CoordinateInterval)
            {
                //Utility.SetPixel("--", 0, y);
                Game.SetCell("--".ToCharArray(), 1, y);

                //Utility.SetPixel(y.ToString(), 4, y);
                Game.SetCell(y.ToString().ToCharArray(), 4, y);
            }
        }

        /// <summary>
        /// Logs a message to the current logging target defined by <code>LogMode</code>.
        /// </summary>
        public static void Log(object message)
        {
            switch (LogMode)
            {
                case logMode.Spool:
                    write(message, logType.INFO);
                    return;
                case logMode.DebugIDE:
                    System.Diagnostics.Debug.WriteLine(message);
                    return;
                case logMode.StatusLog:
                    Status.Log(message);
                    return;
            }
        }
        public static void LogError(object message)
        {
            switch (LogMode)
            {
                case logMode.Spool:
                    write(message, logType.ERROR);
                    return;
                case logMode.DebugIDE:
                    System.Diagnostics.Debug.WriteLine(message);
                    return;
                case logMode.StatusLog:
                    Status.LogError(message);
                    return;
            }
        }
        public static void LogWarning(object message)
        {
            switch (LogMode)
            {
                case logMode.Spool:
                    write(message, logType.WARNING);
                    return;
                case logMode.DebugIDE:
                    System.Diagnostics.Debug.WriteLine(message);
                    return;
                case logMode.StatusLog:
                    Status.LogWarning(message);
                    return;
            }
        }

        private static void write(object message, logType logType)
        {
            if (spoolWriter == null)
            {
                spoolWriter = initLog();
            }

            spoolWriter.WriteLineAsync($"[{DateTime.Now.ToShortTimeString()}][{logType.ToString()}] " + message);
        }

        private static StreamWriter initLog()
        {
            if (File.Exists("latest_log.txt"))
            {
                File.Move("latest_log.txt", $"log_{File.ReadLines("latest_log.txt").First()}.txt");
            }

            StreamWriter sw = new StreamWriter("latest_log.txt");
            sw.WriteLine(Utility.TimeStamp());
            sw.WriteLine("\nLog initialized\n");
            return sw;
        }

        /// <summary>
        /// Class for controlling the "Status" pane.
        /// </summary>
        public static class Status
        {
            /// <summary>
            /// The offset of the status area on the X axis.
            /// </summary>
            public static int OffsetX = 1;
            /// <summary>
            /// The offset of the status area on the Y axis.
            /// </summary>
            public static int OffsetY = 1;
            /// <summary>
            /// Defines the max length of logs, 0 is no limit.
            /// </summary>
            public static int LogMaxLength { get; set; } = 9;
            /// <summary>
            /// The line indicator of the status area.
            /// </summary>
            public static char LogIndicator { get; set; } = '>';
            public static char HierarchyIndicator { get; set; } = '§';
            public static RelativePosition LoggingArea { get; set; } = RelativePosition.None;
            public static RelativePosition HierarchyArea { get; set; } = RelativePosition.None;

            private static int hierarchyAreaLongestLine = 0;
            private static int loggingAreaLongest = 0;
            private static int logLineCount = 0;
            //private static List<string> logText { get; } = new List<string>();

            public enum RelativePosition { Below, By, None }
            

            public static void UpdateHierarchy()
            {
                if (HierarchyArea == RelativePosition.None)
                    return;
                
                for (int i = 0; i < Game.Objects.Count; i++)
                {
                    GameObject item = Game.Objects[i];
                    string gameObjectString;
                    if (item == null)
                    {
                        gameObjectString = "(destroyed)";
                    }
                    else
                    {
                        gameObjectString = $"({item.activeSelf}) {item.identifier}:{item.name} - {item.transform.position} - {item.rigidbody.velocity}";
                    }

                    hierarchyAreaLongestLine = Math.Max(hierarchyAreaLongestLine, gameObjectString.Length);
                    
                    WriteLine(gameObjectString + new string(' ', hierarchyAreaLongestLine - gameObjectString.Length), i, HierarchyArea, HierarchyIndicator);

                    //WriteLine($"({item.activeSelf}) {item.identifier}:{item.name} - {item.transform.position} - {item.rigidbody.velocity}", i, HierarchyArea, HierarchyIndicator);
                }
            }
            /*
            public static void UpdateLog()
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                logLineCount = 0;
                for (int i = 0; i < logText.Count; i++)
                {
                    WriteLine(logText[i]);
                }
            }*/

            private static void MoveLogs()
            {
                int sourceHeight = LogMaxLength == 0 ? logLineCount : Math.Min(LogMaxLength - 1, logLineCount);
                
                if (LoggingArea == RelativePosition.Below)
                {
                    Console.MoveBufferArea(OffsetX, Settings.GameSizeHeight + OffsetY, loggingAreaLongest + 1, sourceHeight, OffsetX, Settings.GameSizeHeight + OffsetY + 1);
                }
                else if (LoggingArea == RelativePosition.By)
                {
                    Console.MoveBufferArea(Settings.GameSizeWidth + OffsetX, OffsetY, loggingAreaLongest + 1, sourceHeight, Settings.GameSizeWidth + OffsetX, OffsetY + 1);
                }
            }

            public static void Log(object message)
            {
                WriteLine(message);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }
            public static void LogWarning(object message)
            {
                WriteLine(message.ToString(), ConsoleColor.Yellow);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }
            public static void LogError(object message)
            {
                WriteLine(message.ToString(), ConsoleColor.Red);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }

            private static void SetCurrentLine(int lineIndex) => logLineCount = lineIndex;

            private static void WriteLine(object value)
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                MoveLogs();

                if (LoggingArea == RelativePosition.By)
                {
                    Console.SetCursorPosition(Settings.GameSizeWidth + OffsetX, OffsetY);// + currentLogLine);
                }
                else if (LoggingArea == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.GameSizeHeight + OffsetY);// + currentLogLine);
                }

                Console.Write(LogIndicator);
                Console.Write(value);
                logLineCount++;
            }

            private static void WriteLine(object value, int line, RelativePosition area, char lineIndicator)
            {
                if (area == RelativePosition.None)
                    return;

                if (area == RelativePosition.By)
                {
                    Console.SetCursorPosition(Settings.GameSizeWidth + OffsetX, line + OffsetY);
                }
                else if (area == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.GameSizeHeight + OffsetY + line);
                }
                Console.Write(lineIndicator);
                Console.Write(value);
            }

            private static void WriteLine(object value, ConsoleColor textColor)
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                MoveLogs();

                if (LoggingArea == RelativePosition.By)
                {
                    Console.SetCursorPosition(Settings.GameSizeWidth + OffsetX, OffsetY);
                }
                else if (LoggingArea == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.GameSizeHeight + OffsetY);
                }

                Console.Write(LogIndicator);

                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = textColor;
                Console.Write(value);
                Console.ForegroundColor = defaultColor;
                logLineCount++;
            }
        }
    }

    public static class Time
    {
        /// <summary>
        /// Still experimenting with this...
        /// </summary>
        public static float deltaTime
        {
            get; set;
        }

    }

    public static class Exceptions
    {
        public class GameObjectNotInstantiatedException : Exception
        {
            public GameObjectNotInstantiatedException()
            {
            }

            public GameObjectNotInstantiatedException(string message) : base(message)
            {
            }
        }

        public class HeightOrWidthLessThanOrEqualToZeroException : Exception
        {
            public HeightOrWidthLessThanOrEqualToZeroException()
            {
            }

            public HeightOrWidthLessThanOrEqualToZeroException(string message) : base(message)
            {
            }
        }
    }


    internal static class Utility
    {
        public static void SetPixel(string value, int x, int y)
        {
            if (0 < x && x < Console.BufferWidth &&
                0 < y && y < Console.BufferHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(value);
            }
        }
        public static void SetPixel(string value, Vector2 coordinates)
        {
            if (0 < coordinates.x && coordinates.x < Console.BufferWidth &&
                0 < coordinates.y && coordinates.y < Console.BufferHeight)
            {
                Console.SetCursorPosition((int)coordinates.x, (int)coordinates.y);
                Console.Write(value);
            }
        }
        public static void SetPixel(char value, int x, int y)
        {
            if (0 < x && x < Console.BufferWidth &&
                0 < y && y < Console.BufferHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(value);
            }
        }
        public static void SetPixel(char value, Vector2 coordinates)
        {
            if (0 < coordinates.x && coordinates.x < Console.BufferWidth &&
                0 < coordinates.y && coordinates.y < Console.BufferHeight)
            {
                Console.SetCursorPosition((int)coordinates.x, (int)coordinates.y);
                Console.Write(value);
            }
        }

        /// <summary>
        /// Returns unix timestamp.
        /// </summary>
        public static long TimeStamp() => DateTimeOffset.Now.ToUnixTimeMilliseconds();

        /// <summary>
        /// Sorts and removes disabled objects.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<GameObject> SortGameObjects(List<GameObject> list)
        {
            return list;
            //list.RemoveAll(a => !a.activeSelf);
            //list.RemoveAll(a => a == null);
            //return list.OrderBy(o => o.transform.z_index).ToList();
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