using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RetroEngine
{
    public enum CoordinateSystemType { TopLeft, BottomLeft, Middle, TopRight, BottomRight, }
    public enum Direction { Right, Left, Up, Down, }


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

        /*
        /// <summary>
        /// Makes the console grids regular quadrilateral.
        /// The width - height ratio changes from 1:1 to 2:1. 
        /// (A char placed in one cell is placed in 2 cells.)
        /// </summary>
        public static bool SquareMode { get; set; } = false;
        */
        public static bool SquareMode {get;set;} = false;

        /// <summary>
        /// Makes the borders a collider, preventing GameObjects from exiting the scene.
        /// </summary>
        public static bool BorderCollider { get; set; } = true;

        /// <summary>
        /// The multiplier of gravity:mass force calculated.
        /// </summary>
        public static float GravityMultiplier = 0.2f;

        /// <summary>
        /// Not implemented.
        /// </summary>
        public static Vector2 gravity = new Vector2(0, 1);
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
        
        private static bool gameRunning;
        private static char[,] renderedGamefield { get; set; }// = new char[Settings.SizeHeight, Settings.SizeWidth];
        private static char[,] gamefield { get; set; }// = new char[Settings.SizeHeight, Settings.SizeWidth];
        private static int?[,] collisionMap { get; set; }// = new int?[Settings.SizeHeight, Settings.SizeWidth];
        //private static int?[,] collisionMapRendered { get; set; } = new int?[Settings.GameSizeHeight, Settings.GameSizeWidth];
        private static List<GameObject> renderedObjects = new List<GameObject>();
        private static long lastFixedFrame;
        private static List<int>[,] collisions { get; set; }// = new List<int>[Settings.SizeHeight, Settings.SizeWidth];
        private static List<int>[,] handledCollisions;
        private static char[,] gamefieldEmpty;

        private static bool[,] emptySpots;


        private static void InitializeFields()
        {
            Background = new char[Settings.SizeHeight, Settings.SizeWidth];
            gamefield = new char[Settings.SizeHeight, Settings.SizeWidth];
            collisionMap = new int?[Settings.SizeHeight, Settings.SizeWidth];
            collisions = new List<int>[Settings.SizeHeight, Settings.SizeWidth];
            gamefieldEmpty = new char[Settings.SizeHeight, Settings.SizeWidth];
            emptySpots = new bool[Settings.SizeHeight, Settings.SizeWidth];

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
        public static void Exit() => gameRunning = false;

        /// <summary>
        /// Starts main game loop(non-async). 
        /// </summary>
        public static void Play()
        {
            // Internal start
            InitializeFields();
            long time2 = Utility.TimeStamp();
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

            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
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

            // TODO: Fix the wallpaper feature. Or ultimately replace it. Rather the latter
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

            long time1 = time2;
            time2 = Utility.TimeStamp();

            gameRunning = true;

            // Main game loop.
            while (gameRunning)
            {
                // Clock updates
                time2 = Utility.TimeStamp();
                Time.deltaTime = time2 - time1;
                time1 = time2;
                //float delta = currentTimestamp - previousTimestamp;
                //Time.deltaTime = delta != 0 ? delta / (float)1000 : 0;
                

                /*
                previousTimestamp = currentTimestamp;
                currentTimestamp = Utility.TimeStamp();
                */

                // Internal update loop
                renderedObjects = Objects;
                renderedGamefield = gamefield;
                gamefield = new char[Settings.SizeHeight, Settings.SizeWidth];

                if (Debug.FPSCounter)
                {

                    //float FPS;
                    float timepassed = (float)(Utility.TimeStamp() - GameStartedTimestamp);
                    /*if (timepassed != 0)
                    {
                        timepassed = timepassed / (float)1000;
                        FPS = Time.frameCount / timepassed;
                    }
                    else
                    {
                        FPS = 0;
                    }*/
                    Console.Title = $"FPS: {1 / Time.deltaTime}";
                }

                // Updates current timestamp
                //currentTimestamp = Utility.TimeStamp();

                // Fixed framerate update
                #region Fixed frame update function
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

                        if (Objects[i].rigidbodyEnabled)
                        {
                            if (Objects[i].rigidbody.velocity == Vector2.zero)
                            {
                                Objects[i].rigidbody.moving = false;
                                continue;
                            }

                            Objects[i].rigidbody.startPosition = Objects[i].transform.position;
                            Objects[i].rigidbody.moving = true;
                        }

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

                    // Updates keys, every fixed frame
                    Input.UpdateKeyFrame();
                }
                #endregion

                handledCollisions = collisions;


                for (int i = 0; i < Objects.Count; i++)
                {
                    HandleGameObject(Objects[i]);
                }

                HandleCollisions();

                RefreshBuffer();
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
            if (obj.rigidbodyEnabled && obj.rigidbody.velocity != Vector2.zero && obj.rigidbody.moving)
            {
                float timeLeft = (Utility.TimeStamp() - lastFixedFrame) / ((float)1000 / Time.fixedDeltaTime);

                Vector2 finalPosition;
                if (obj.rigidbody.gravityScale != 0)
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
                // TODO: Don't.. and I mean DONT. do dis every frame. Maybe do some pre-calculated arrays instead, and then update those as the scale changes.
                PlaceCharArray(Utility.Multiply2DArray(obj.sprite.ascii, (int)obj.transform.localScale.x, (int)obj.transform.localScale.y), (int)obj.transform.position.x, (int)obj.transform.position.y);
                //PlaceCharArray(obj.sprite.ascii, (int)obj.transform.position.x, (int)obj.transform.position.y);
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
        
        /// <summary>
        /// Removes left over pixels
        /// </summary>
        private static void RefreshBuffer()
        {
            emptySpots = new bool[renderedGamefield.GetLength(0), renderedGamefield.GetLength(1)];

            for (int y = 0; y < renderedGamefield.GetLength(0); y++)
            {
                for (int x = 0; x < renderedGamefield.GetLength(1); x++)
                {
                    if (gamefield[y, x] == '\0' && renderedGamefield[y, x] != '\0')
                    {
                        emptySpots[y, x] = true;
                    }
                }
            }
        }

        private static void UpdateBuffer()
        {
            for (int y = 0; y < emptySpots.GetLength(0); y++)
            {
                for (int x = 0; x < emptySpots.GetLength(1); x++)
                {
                    if (emptySpots[y, x])
                    {
                        Utility.SetPixel('\0', x, y);
                    }
                }
            }

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
    public static class ArrayExtensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this Array target)
        {
            foreach (var item in target)
                yield return (T)item;
        }
    }

    internal static class Utility
    {
        // A pixel is considered a single cell in the command prompt.
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

        public static char[,] Multiply2DArray(char[,] array, int scaleX, int scaleY)
        {
            char[,] newArray = new char[array.GetLength(0) * scaleY, array.GetLength(1) * scaleX];

            for (int y = 0; y < array.GetLength(0); y++)
            {
                for (int x = 0; x < array.GetLength(1); x++)
                {
                    char value = array[y, x];
                    for (int y_1 = 0; y_1 < scaleY; y_1++)
                    {
                        for (int x_1 = 0; x_1 < scaleX; x_1++)
                        {
                            newArray[y + y_1, x + x_1] = value;
                        }
                    }
                }
            }

            return newArray;
        }

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