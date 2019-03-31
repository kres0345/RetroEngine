using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RetroEngine
{
    public enum CoordinateSystemType { TopLeft, BottomLeft, Middle, TopRight, BottomRight, }
    public enum Direction { Right, Left, Up, Down, }

    public class Rigidbody
    {
        /// <summary>
        /// The velocity of the parent GameObject.
        /// Added every
        /// </summary>
        public Vector2 velocity { get; set; }
        public float mass { get; set; } = 1;
        public bool useGravity { get; set; } = true;

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
            return new Vector2(0, mass * Settings.GravityMultiplier);
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
            this.rigidbody.Reset();
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
                Debug.LogError("Id is null of " + name);
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
            public Action<int> OnCollisionEnter;
            public Action<int> OnCollisionStay;
            public Action<int> OnCollisionExit;
            internal List<int> LastFrameCollisions = new List<int>();

            public bool TryGetOnCollisionEnter(out Action<int> OnCollisionEnter)
            {
                OnCollisionEnter = this.OnCollisionEnter;
                return this.OnCollisionEnter != null;
            }

            public bool TryGetOnCollisionStay(out Action<int> OnCollisionStay)
            {
                OnCollisionStay = this.OnCollisionStay;
                return this.OnCollisionStay != null;
            }

            public bool TryGetOnCollisionExit(out Action<int> OnCollisionExit)
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
            position = position + translation;
        }
    }

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


    public struct Mathf
    {

        /// <summary>
        /// Interpolate liniarily between a and b by t.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t)
        {

            return a * (1 - t) + b * t;
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
        /// Calculates the position that is a percentage(<code>t</code>) of the travel between position a and b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return new Vector2(Mathf.Lerp(a.x, b.x, t), Mathf.Lerp(a.y, b.y, t));
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow((b.x - a.x), 2) + Math.Pow((b.y - a.y), 2));
        }

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
        public static Vector2 operator /(Vector2 a, float b)
        {
            a.x /= b;
            a.y /= b;
            return a;
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            return a;
        }
    }

    public static class UI
    {

    }

    public static class Settings
    {
        /// <summary>
        /// WARNING: Isn't implemented yet, or will never be, up for debate.
        /// Defines the coordinate (0, 0) point.
        /// </summary>
        //TODO: Make a decision.
        //public static CoordinateSystemType CoordinateSystemCenter = CoordinateSystemType.TopLeft;

        /// <summary>
        /// Defines game boundaries and render size.
        /// </summary>
        public static int SizeWidth { get; set; } = 25;
        /// <summary>
        /// Defines game boundaries and render size.
        /// </summary>
        public static int SizeHeight { get; set; } = 10;

        /// <summary>
        /// Makes the console grids regular quadrilateral.
        /// The width - height ratio changes from 1:1 to 2:1. 
        /// (A char placed in one cell is placed in 2 cells.)
        /// </summary>
        //TODO: Implement Square Mode
        public static bool SquareMode { get; set; } = false;

        /// <summary>
        /// Makes the borders a collider, preventing GameObjects from exiting the scene.
        /// </summary>
        public static bool BorderCollider { get; set; } = true;

        /// <summary>
        /// The multiplier of gravity:mass force calculated.
        /// </summary>
        public static float GravityMultiplier = 0.2f;
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
        //private static List<ConsoleKey> frameKeys = new List<ConsoleKey>();
        private static Task keyListener = null;
        //private static int lastFrame = 0;
        //private static int lastFrameKeyPressed = 0;

        public static bool GetKeyDown(ConsoleKey key)
        {

            return false;
        }

        public static bool GetKey(ConsoleKey key)
        {
            //return frameKeys.Contains(key);

            
            if (frameKeys.TryGetValue(Time.frameCount + 1, out ConsoleKey pressedKey))
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
                    /*
                    frameKeys.Insert(0, key);
                    if (frameKeys.Count > 10)
                    {
                        frameKeys.RemoveRange(10, frameKeys.Count);
                    }*/
                    frameKeys[Time.frameCount + 1] = key;
                    
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
        public static char[,] Background { get; set; }// = new char[Settings.SizeHeight, Settings.SizeWidth];
        public static Action UpdateMethod { get; set; }
        public static Action FixedUpdateMethod { get; set; }
        public static Action StartMethod { get; set; }
        public static long GameStartedTimestamp { get; private set; }

        private const int BorderIdentifier = -1;
        
        private static bool gamePlaying;
        private static char[,] renderedGamefield { get; set; }// = new char[Settings.SizeHeight, Settings.SizeWidth];
        private static char[,] gamefield { get; set; }// = new char[Settings.SizeHeight, Settings.SizeWidth];
        private static int?[,] collisionMap { get; set; }// = new int?[Settings.SizeHeight, Settings.SizeWidth];
        //private static int?[,] collisionMapRendered { get; set; } = new int?[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static List<GameObject> renderedObjects = new List<GameObject>();
        private static long lastFixedFrame;
        private static List<int>[,] collisions { get; set; }// = new List<int>[Settings.SizeHeight, Settings.SizeWidth];
        private static List<int>[,] handledCollisions;
        private static char[,] gamefieldEmpty;

        private static void InitializeFields()
        {
            Background = new char[Settings.SizeHeight, Settings.SizeWidth];
            gamefield = new char[Settings.SizeHeight, Settings.SizeWidth];
            collisionMap = new int?[Settings.SizeHeight, Settings.SizeWidth];
            collisions = new List<int>[Settings.SizeHeight, Settings.SizeWidth];
            gamefieldEmpty = new char[Settings.SizeHeight, Settings.SizeWidth];

            for (int y = 0; y < gamefieldEmpty.GetLength(0); y++)
            {
                for (int x = 0; x < gamefieldEmpty.GetLength(1); x++)
                {
                    gamefieldEmpty[y, x] = ' ';
                }
            }
        }

        /// <summary>
        /// Ends main game loop and finishes Game.Play() call.
        /// </summary>
        public static void Exit() => gamePlaying = false;

        /// <summary>
        /// Starts main game loop(non-async). 
        /// </summary>
        public static void Play()
        {
            // Internal start
            InitializeFields();
            long currentTimestamp = Utility.TimeStamp();
            Console.CursorVisible = false;
            Input.ListenKeys();

            int WidthOffset = 5;
            int HeightOffset = 5;
            if (Debug.Status.HierarchyArea == Debug.Status.RelativePosition.By || Debug.Status.LoggingArea == Debug.Status.RelativePosition.By)
            {
                WidthOffset += 40;
            }
            if (Debug.Status.HierarchyArea == Debug.Status.RelativePosition.Below || Debug.Status.LoggingArea == Debug.Status.RelativePosition.Below)
            {
                HeightOffset += 5;
            }

            Console.SetWindowSize(Settings.SizeWidth + WidthOffset, Settings.SizeHeight + HeightOffset);

            if (Debug.DrawGameBorder)
            {
                for (int x = 0; x < Settings.SizeWidth; x++)
                {
                    Utility.SetPixel('-', x, Settings.SizeHeight);
                }

                for (int y = 0; y < Settings.SizeHeight; y++)
                {
                    Utility.SetPixel('|', Settings.SizeWidth, y);
                }
            }

            // TODO: Fix the wallpaper feature
            // Known bugs: Interferes with Game.Objects thereby preventing GameObjects from being instantiated.
            //UpdateWallpaper();

            // External start method
            if (StartMethod != null)
            {
                StartMethod.Invoke();
            }

            // Draw gameobjects
            for (int i = 0; i < Objects.Count; i++)
            {
                HandleGameObject(Objects[i]);
            }

            //previousFrameObjects = Objects;
            GameStartedTimestamp = Utility.TimeStamp();
            lastFixedFrame = Utility.TimeStamp();

            long previousTimestamp = currentTimestamp;
            currentTimestamp = Utility.TimeStamp();

            gamePlaying = true;



            // Main game loop.
            while (gamePlaying)
            {
                // Internal Update loop
                renderedObjects = Objects;
                renderedGamefield = gamefield;
                previousTimestamp = currentTimestamp;
                currentTimestamp = Utility.TimeStamp();
                gamefield = new char[Settings.SizeHeight, Settings.SizeWidth];
                

                float delta = currentTimestamp - previousTimestamp;
                Time.deltaTime = delta != 0 ? delta / (float)1000 : 0;

                if (Debug.FPSCounter)
                {
                    float FPS;
                    float timepassed = (float)(Utility.TimeStamp() - GameStartedTimestamp);
                    if (timepassed != 0)
                    {
                        timepassed = timepassed / (float)1000;
                        FPS = Time.frameCount / timepassed;
                    }
                    else
                    {
                        FPS = 0;
                    }
                    Console.Title = $"FPS: {FPS}";
                }

                // Updates current timestamp
                //currentTimestamp = Utility.TimeStamp();

                // Fixed framerate update
                if ((Utility.TimeStamp() - lastFixedFrame) * Time.timeScale >= (float)1000 / Time.fixedDeltaTime)
                {
                    // Internal fixed update
                    lastFixedFrame = Utility.TimeStamp();

                    for (int i = 0; i < Objects.Count; i++)
                    {
                        if (Objects[i] == null || !Objects[i].activeSelf || Objects[i].identifier == null)
                        {
                            continue;
                        }
                        
                        if (Objects[i].rigidbody.velocity == Vector2.zero)
                        {
                            Objects[i].rigidbody.moving = false;
                            continue;
                        }

                        Objects[i].rigidbody.startPosition = Objects[i].transform.position;
                        Objects[i].rigidbody.moving = true;

                        //Todo: Fix this physics thing.
                        #region
                        //Objects[i].transform.position += Objects[i].rigidbody.velocity;

                        //Vector2 estimatedPosition = Objects[i].transform.position + Objects[i].rigidbody.velocity;
                        /*
                        if (estimatedPosition.x < 0)
                        {
                            //TODO: Edit, just discovered this doesnt take collision into consideration.
                            if (Objects[i].events.LastFrameCollisions.Contains(BorderIdentifier))
                            {
                                if (Objects[i].events.TryGetOnCollisionStay(out Action<int> OnCollisionStay))
                                {
                                    OnCollisionStay(BorderIdentifier);
                                }
                            }
                            else
                            {
                                if (Objects[i].events.TryGetOnCollisionEnter(out Action<int> OnCollisionEnter))
                                {
                                    OnCollisionEnter(BorderIdentifier);
                                    if (Objects[i].sprite.solid)
                                    {

                                    }
                                    Objects[i].events.LastFrameCollisions.Add(BorderIdentifier);
                                }
                            }
                        }*/
                        #endregion
                    }

                    // External fixed update
                    if (FixedUpdateMethod != null)
                    {
                        FixedUpdateMethod.Invoke();
                    }

                    if (Debug.Status.HierarchyFrameUpdate)
                    {
                        Debug.Status.UpdateHierarchy();
                    }

                    if (Debug.DrawCoordinateSystemEveryFrame)
                    {
                        Debug.DrawCoordinateSystem();
                    }
                }
                
                handledCollisions = collisions;


                for (int i = 0; i < Objects.Count; i++)
                {
                    HandleGameObject(Objects[i]);
                }

                HandleCollisions();


                UpdateBuffer();

                // External update loop
                if (UpdateMethod != null)
                {
                    UpdateMethod.Invoke();
                }

                Time.frameCount++;
            }
        }

        private static void HandleGameObject(GameObject obj)
        {
            if (obj == null || !obj.activeSelf)
            {
                return;
            }

            //Move rigidbody by velocity.
            if (obj.rigidbody.velocity != Vector2.zero && obj.rigidbody.moving)
            {
                float timeLeft = (Utility.TimeStamp() - lastFixedFrame) / ((float)1000 / Time.fixedDeltaTime);

                Vector2 finalPosition;
                if (obj.rigidbody.useGravity)
                {
                    finalPosition = obj.rigidbody.startPosition + obj.rigidbody.velocity + obj.rigidbody.GravityForce();
                }
                else
                {
                    finalPosition = obj.rigidbody.startPosition + obj.rigidbody.velocity;
                }

                obj.transform.position = Vector2.Lerp(obj.rigidbody.startPosition, finalPosition, timeLeft);
            }

            if (obj.sprite.ascii != null)
            {
                PlaceCharArray(obj.sprite.ascii, (int)obj.transform.position.x, (int)obj.transform.position.y);
            }

            if (obj.sprite.collision != null)
            {
                UpdateCollisionSingle(obj.sprite.collision, obj.transform.position, (int)obj.identifier);
            }
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
                for (int loop_x = 0; loop_x < spriteWidth; loop_x++)
                {
                    SetCell(sprite[loop_y, loop_x], x + loop_x, y + loop_y);
                }
            }
        }

        private static void UpdateCollisionSingle(bool[,] col, Vector2 pos, int identifier)
        {
            if (col == null)
            {
                return;
            }
            

            for (int y = 0; y < col.GetLength(0); y++)
            {
                for (int x = 0; x < col.GetLength(1); x++)
                {
                    if ((int)pos.y + y >= collisions.GetLength(0) || (int)pos.x + x >= collisions.GetLength(1) || (int)pos.y + y < 0 || (int)pos.x + x < 0)
                    {
                        continue;
                    }

                    if (collisions[(int)pos.y + y, (int)pos.x + x] == null)
                    {
                        collisions[(int)pos.y + y, (int)pos.x + x] = new List<int> { identifier };
                        continue;
                    }

                    if (collisions[(int)pos.y + y, (int)pos.x + x].Contains(identifier))
                    {
                        continue;
                    }

                    collisions[(int)pos.y + y, (int)pos.x + x].Add(identifier);
                }
            }
            #region
            /*
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
            }*/

            /*
            if (collided)
            {
                Debug.Log("Collided");
                int collidedIdentifier = 0;
                if (Objects[identifier].events.LastFrameCollisions.Contains(collidedIdentifier)) //Already collided
                {
                    if (Objects[identifier].events.TryGetOnCollisionStay(out Action<int> OnCollisionStay))
                    {
                        Debug.Log("Collision Stay");
                        OnCollisionStay(collidedIdentifier);
                    }
                }
                else //Havent collided yet.
                {
                    if (Objects[identifier].events.TryGetOnCollisionEnter(out Action<int> OnCollisionEnter))
                    {
                        Debug.Log("Collision Enter");
                        OnCollisionEnter(collidedIdentifier);
                    }
                }
            }*/
            #endregion
        }

        private static void HandleCollisions()
        {
            //List<int> alreadyCalled = new List<int>();
            Dictionary<int, List<int>> collisionHistory = new Dictionary<int, List<int>>();

            for (int y = 0; y < collisions.GetLength(0); y++)
            {
                for (int x = 0; x < collisions.GetLength(1); x++)
                {
                    if (collisions[y, x] == null || collisions[y, x].Count <= 1)
                    {
                        continue;
                    }

                    List<int> collidingObjects = collisions[y, x];
                    //Debug.Log(collidingID[0]);

                    for (int i = 0; i < collidingObjects.Count; i++)
                    {
                        int identifier = collidingObjects[i];

                        //foreach (int item in collidingObjects.Where((v, o) => o != i).ToList())
                        for (int index = 0; index < collidingObjects.Count; index++)
                        {
                            if (collidingObjects[index] == identifier)
                            {
                                continue;
                            }

                            if (Objects[identifier] == null)
                            {
                                continue;
                            }

                            if (collisionHistory.TryGetValue(identifier, out List<int> value))
                            {
                                if (value.Contains(collidingObjects[index]))
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                collisionHistory[identifier] = new List<int>();
                            }

                            if (Objects[identifier].events.LastFrameCollisions.Contains(collidingObjects[index]))
                            {
                                if (Objects[identifier].events.TryGetOnCollisionStay(out Action<int> OnCollisionStay))
                                {
                                    OnCollisionStay(collidingObjects[index]);
                                }
                            }
                            else if (Objects[identifier].events.TryGetOnCollisionEnter(out Action<int> OnCollisionEnter))
                            {
                                OnCollisionEnter(collidingObjects[index]);
                            }

                            
                            
                            collisionHistory[identifier].Add(collidingObjects[index]);
                            Objects[identifier].events.LastFrameCollisions.Add(collidingObjects[index]);
                        }
                    }
                }
            }
        }
        
        /*
        private static void CleanBuffered()
        {
            
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
            }
        }*/

        private static void UpdateWallpaper()
        {
            //TODO: This currently breaks the game.
            //Not quite sure what gives.
            //Skipping for now...
            bool[,] updateMap = new bool[Settings.SizeHeight, Settings.SizeWidth];

            List<GameObject> sortedGameobjects = Objects;
            sortedGameobjects.RemoveAll(obj => obj.activeSelf);

            for (int i = 0; i < sortedGameobjects.Count; i++)
            {
                char[,] draw = sortedGameobjects[i].sprite.ascii;
                Vector2 position = sortedGameobjects[i].transform.position;

                for (int y = 0; y < draw.GetLength(0); y++)
                {
                    for (int x = 0; x < draw.GetLength(1); x++)
                    {
                        updateMap[(int)position.y + y, (int)position.x + x] = true;
                    }
                }
            }

            for (int y = 0; y < updateMap.GetLength(0); y++)
            {
                for (int x = 0; x < updateMap.GetLength(1); x++)
                {
                    if (updateMap[y, x])
                        continue;

                    if (gamefield[y, x] != Background[y, x] && Background[y, x] != '\0')
                    {
                        gamefield[y, x] = Background[y, x];
                    }
                }
            }
        }

        private static void UpdateBuffer()
        {
            for (int y = 0; y < gamefield.GetLength(0); y++)
            {
                for (int x = 0; x < gamefield.GetLength(1); x++)
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
            if (0 < x && x < Settings.SizeWidth &&
                0 < y && y < Settings.SizeHeight)
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
            int Width = CoordinateWidth == null ? Settings.SizeWidth : (int)CoordinateWidth;
            int Height = CoordinateHeight == null ? Settings.SizeHeight : (int)CoordinateHeight;


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

        /// <summary>
        /// Refreshes game area.
        /// </summary>
        public static void RefreshScreen()
        {
            for (int y = 0; y < Settings.SizeHeight; y++)
                for (int x = 0; x < Settings.SizeWidth; x++)
                    Utility.SetPixel(' ', x, y);
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
            public static int OffsetY = 2;
            /// <summary>
            /// Defines the max length of logs, 0 is no limit.
            /// </summary>
            public static int LogMaxLength { get; set; } = 9;
            /// <summary>
            /// The line indicator of the log area.
            /// </summary>
            public static char LogIndicator { get; set; } = '>';
            /// <summary>
            /// The lien indicator of the hierarchy area.
            /// </summary>
            public static char HierarchyIndicator { get; set; } = '§';
            /// <summary>
            /// The area to log to.
            /// </summary>
            public static RelativePosition LoggingArea { get; set; } = RelativePosition.None;
            public static RelativePosition HierarchyArea { get; set; } = RelativePosition.None;
            public static bool HierarchyFrameUpdate { get; set; } = false;
            /// <summary>
            /// Truncates logs to fit in window, prevents sudden line breaks on long log messages.
            /// </summary>
            public static bool TruncateLogs { get; set; } = true;

            private static int hierarchyAreaLongestLine = 0;
            private static int loggingAreaLongest = 0;
            private static int logLineCount = 0;
            private static string lastLog;
            private static int equalLogsStreak = 0;
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
                        gameObjectString = i + "(destroyed)";
                        //continue;
                    }
                    else
                    {
                        gameObjectString = $"{i}({item.activeSelf}):{item.name} - {item.transform.position} - {item.rigidbody.velocity}";
                    }

                    hierarchyAreaLongestLine = Math.Max(hierarchyAreaLongestLine, gameObjectString.Length);
                    
                    WriteLine(gameObjectString + new string(' ', hierarchyAreaLongestLine - gameObjectString.Length), i, HierarchyArea, HierarchyIndicator);

                    //Console.WindowTop = 0;
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
                    Console.MoveBufferArea(OffsetX, Settings.SizeHeight + OffsetY, loggingAreaLongest + 1, sourceHeight, OffsetX, Settings.SizeHeight + OffsetY + 1);
                }
                else if (LoggingArea == RelativePosition.By)
                {
                    Console.MoveBufferArea(Settings.SizeWidth + OffsetX, OffsetY, loggingAreaLongest + 1, sourceHeight, Settings.SizeWidth + OffsetX, OffsetY + 1);
                }
            }

            public static void Log(object message)
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                if ((string)message == lastLog)
                {
                    equalLogsStreak++;
                    message = $"({equalLogsStreak}){message}";
                }
                else
                {
                    equalLogsStreak = 0;
                    MoveLogs();
                    lastLog = (string)message;
                }

                WriteLine(message);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }
            public static void LogWarning(object message)
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                if ((string)message == lastLog)
                {
                    equalLogsStreak++;
                    message = $"({equalLogsStreak}){message}";
                }
                else
                {
                    equalLogsStreak = 0;
                    MoveLogs();
                    lastLog = (string)message;
                }

                WriteLine(message, ConsoleColor.Yellow);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }
            public static void LogError(object message)
            {
                if (LoggingArea == RelativePosition.None)
                    return;

                if ((string)message == lastLog)
                {
                    equalLogsStreak++;
                    message = $"({equalLogsStreak}){message}";
                }
                else
                {
                    equalLogsStreak = 0;
                    MoveLogs();
                    lastLog = (string)message;
                }

                WriteLine(message, ConsoleColor.Red);
                loggingAreaLongest = Math.Max(loggingAreaLongest, message.ToString().Length);
            }

            private static void SetCurrentLine(int lineIndex) => logLineCount = lineIndex;

            private static void WriteLine(object value)
            {
                /*
                if (LoggingArea == RelativePosition.None)
                    return;

                MoveLogs();
                */
                if (LoggingArea == RelativePosition.By)
                {
                    Console.SetCursorPosition(Settings.SizeWidth + OffsetX, OffsetY);// + currentLogLine);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(Console.WindowWidth - Settings.SizeWidth + OffsetX - 4);
                    }
                }
                else if (LoggingArea == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.SizeHeight + OffsetY);// + currentLogLine);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(OffsetX + Console.WindowWidth - 1);
                    }
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
                    Console.SetCursorPosition(Settings.SizeWidth + OffsetX, line + OffsetY);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(Console.WindowWidth - Settings.SizeWidth + OffsetX - 4);
                    }
                }
                else if (area == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.SizeHeight + OffsetY + line);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(OffsetX + Console.WindowWidth - 1);
                    }
                }
                Console.Write(lineIndicator);
                Console.Write(value);
            }

            private static void WriteLine(object value, ConsoleColor textColor)
            {
                if (LoggingArea == RelativePosition.By)
                {
                    Console.SetCursorPosition(Settings.SizeWidth + OffsetX, OffsetY);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(Console.WindowWidth - Settings.SizeWidth + OffsetX - 4);
                    }
                }
                else if (LoggingArea == RelativePosition.Below)
                {
                    Console.SetCursorPosition(OffsetX, Settings.SizeHeight + OffsetY);

                    if (TruncateLogs)
                    {
                        value = value.ToString().Truncate(OffsetX + Console.WindowWidth - 1);
                    }
                }

                //Console.Write(LogIndicator);

                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = textColor;
                Console.Write(LogIndicator);
                Console.Write(value);
                Console.ForegroundColor = defaultColor;
                logLineCount++;
            }
        }
    }

    public static class Time
    {

        public static float timeScale { get; set; } = 1.0f;
        public static float fixedDeltaTime { get; set; } = 50;
        public static int frameCount { get; internal set; }
        /// <summary>
        /// Still experimenting with this...
        /// </summary>
        public static float deltaTime { get; internal set; }

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

    public static class StringExt
    {
        /// <summary>
        /// Truncates string to maxLength. Converts negative maxLength to positive.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;

            return value.Substring(0, Math.Min(value.Length, Math.Abs(maxLength)));
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

        /*
        public static void FillPopulatedCells(char[,] matrix, char[,] replaceMatrix, char fill = ' ')
        {
            
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    if (matrix[y, x] != replaceMatrix[y, x] && matrix[y, x] != '\0')
                    {
                        matrix[y, x] = replaceMatrix[y, x];
                    }
                }
            }
        }*/
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