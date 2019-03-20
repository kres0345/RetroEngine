using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RetroEngine;

namespace ConsoleGameEngineGame
{
    class MyGame
    {

        static void Main(string[] args)
        {
            // Assigns start method
            Game.StartMethod = Start;
            Game.UpdateMethod = Update;

            //Debug.InitiateLog();

            // Starts game
            Game.Play();
        }

        public static GameObject player1obj;
        public static GameObject bulletobj;

        static float PlayerSpeed = 1;
        static float BulletSpeed = 0.5f;
        /* www.oocities.org/spunk1111/men.htm#bald
         *    __
            .'  `'.
           /  _    |
           #_/.\==/.\
          (, \_/ \\_/
           |    -' |
           \   '=  /
           /`-.__.'
        .-'`-.___|__
       /    \       `.
        */
        /*
        static char[][] Player1 = new char[10][] {
            new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', '_', '_'},
            new char[] { ' ', ' ', ' ', ' ', ' ', '.','\'', ' ', ' ', '`','\'', '.' },
            new char[] { ' ', ' ', ' ', ' ', '/', ' ', ' ', '_', ' ', ' ', ' ', ' ', '|' },
            new char[] { ' ', ' ', ' ', ' ', '#', '_', '/', '.','\\', '=', '=', '/', '.', '\\'},
            new char[] { ' ', ' ', ' ', '(', ',', ' ','\\', '_', '/', ' ','\\','\\', '_', '/' },
            new char[] { ' ', ' ', ' ', ' ', '|', ' ', ' ', ' ', ' ', '-','\'', ' ', '|', },
            new char[] { ' ', ' ', ' ', ' ','\\', ' ', ' ', ' ','\'', '=', ' ', ' ', '/', },
            new char[] { ' ', ' ', ' ', ' ', '/', '`', '-', '.', '_', '_', '.','\'', },
            new char[] { ' ', '.', '-','\'', '`', '-', '.', '_', '_', '_', '|', '_', '_' },
            new char[] { '/', ' ', ' ', ' ', ' ','\\', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '`', '.' }
        };*/
        /*
        static char[][] Player1 = new char[3][]
        {
            new char[] { '{', '0', '}' },
            new char[] { '{', '+', '}' },
            new char[] { '{', '0', '}' }
        };
        static char[][] Bullet = new char[1][]
        {
            new char[] { '=', '=', '>' }
        };*/
        static char[,] Player1 = new char[3,3]
        {
            { '{', '0', '}' },
            { '{', '+', '}' },
            { '{', '0', '}' }
        };
        static char[,] Bullet = new char[1, 3]
        {
            { '=', '=', '>' }
        };

        static void Start()
        {
            //GameObject obj = new GameObject();
            player1obj = new GameObject();
            player1obj.transform.position = new Vector2(3, 3);
            player1obj.sprite.draw = Player1;
            
            //Game.Objects.Add("Player1", player1obj);
            bulletobj = new GameObject();
            bulletobj.sprite.draw = Bullet;
            bulletobj.SetActive(false);

            player1obj = GameObject.Instantiate(player1obj);
            bulletobj = GameObject.Instantiate(bulletobj);
            //Game.Objects.Add("Bullet", bulletobj);
        }

        // Called every frame update
        static void Update()
        {
            //GameObject player1 = Game.Objects["Player1"];
            //GameObject bullet = Game.Objects["Bullet"];

            GameObject player1 = player1obj.Get();
            GameObject bullet = bulletobj.Get();

            //GameObject player1 = Game.Objects[(int)player1obj.identifier];
            //GameObject bullet = Game.Objects[(int)bulletobj.identifier];

            if (bullet.activeSelf)
            {
                bullet.transform.position = new Vector2(bullet.transform.position.x + BulletSpeed, bullet.transform.position.y); 
                //* Time.deltaTime
            }

            if (Input.GetKey(ConsoleKey.D))
            {
                player1.transform.position = new Vector2(player1.transform.position.x + (1 * PlayerSpeed), player1.transform.position.y);
            }
            else if (Input.GetKey(ConsoleKey.W))
            {
                player1.transform.position = new Vector2(player1.transform.position.x, player1.transform.position.y - (1 * PlayerSpeed));
            }
            else if (Input.GetKey(ConsoleKey.S))
            {
                player1.transform.position = new Vector2(player1.transform.position.x, player1.transform.position.y + (1 * PlayerSpeed));
            }
            else if (Input.GetKey(ConsoleKey.A))
            {
                player1.transform.position = new Vector2(player1.transform.position.x - (1 * PlayerSpeed), player1.transform.position.y);
            }
            else if (Input.GetKey(ConsoleKey.Spacebar))
            {
                bullet.transform.position = new Vector2(player1.transform.position.x + 1, player1.transform.position.y + 1);
                //bullet.transform.position.Add(player1.sprite.width, 0);
                bullet.SetActive(true);
            }
            else if (Input.GetKey(ConsoleKey.DownArrow))
            {
                if (bullet.activeSelf)
                {
                    bullet.transform.position = new Vector2(bullet.transform.position.x, bullet.transform.position.y + 1);
                }
            }
            else if (Input.GetKey(ConsoleKey.UpArrow))
            {
                if (bullet.activeSelf)
                {
                    bullet.transform.position = new Vector2(bullet.transform.position.x, bullet.transform.position.y - 1);
                }
            }
            else if (Input.GetKey(ConsoleKey.Backspace))
            {
                bullet.SetActive(false);
            }
            else if(Input.GetKey(ConsoleKey.Escape))
            {
                Game.Exit();
            }
        }
    }
}

namespace RetroEngine
{

    class Input
    {
        /// <summary>
        /// Checks for key presses during each frame.
        /// </summary>
        public static bool ListenForKeys { get; set; } = true;

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
    }

    class Debug
    {
        /// <summary>
        /// Currently affects performance massively.  For drawing coordinate system only once refer to DrawCoordinateSystem method.
        /// </summary>
        public static bool DrawCoordinateSystemEveryFrame { get; set; } = false;

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

        private static StreamWriter fs = null;

        /// <summary>
        /// Draws coordinate system
        /// </summary>
        public static void DrawCoordinateSystem()
        {
            int Width = CoordinateWidth == null ? Game.GameSizeWidth : (int)CoordinateWidth;
            int Height = CoordinateHeight == null ? Game.GameSizeHeight : (int)CoordinateHeight;
            

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
        /// Logs a message to 'latest_log.txt'.
        /// </summary>
        public static void Log(object message) => write(message, LogType.INFO);
        public static void LogError(object message) => write(message, LogType.ERROR);
        public static void LogWarning(object message) => write(message, LogType.WARNING);

        private enum LogType { INFO, WARNING, ERROR, }

        private static void write(object message, LogType logType)
        {
            if (fs == null)
            {
                fs = initLog();
            }

            fs.WriteLineAsync($"[{DateTime.Now.ToShortTimeString()}][{logType.ToString()}] " + message);
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
    }

    class Game
    {
        /// <summary>
        /// List of all GameObjects.
        /// </summary>
        /// <remarks>Objects must NOT be removed from this list, they should be nullified.</remarks>
        public static List<GameObject> Objects = new List<GameObject>();
        public static Action UpdateMethod { get; set; }
        public static Action StartMethod { get; set; }
        public static int TotalFrames { get; private set; } = 0;
        public static int GameSizeWidth { get; set; } = 200;
        public static int GameSizeHeight { get; set; } = 100;
        public static long GameStartedTimestamp { get; private set; }

        //private static Timer updateTimer;
        private static bool gamePlaying = false;
        private static long previousFrameTimestamp = 0;
        private static char[,] Gamefield { get; set; } = new char[GameSizeHeight, GameSizeWidth];
        private static char[,] GamefieldRendered { get; set; } = new char[GameSizeHeight, GameSizeWidth];
        private static int?[,] CollisionMap { get; set; } = new int?[GameSizeHeight, GameSizeWidth];
        private static int?[,] CollisionMapRendered { get; set; } = new int?[GameSizeHeight, GameSizeWidth];
        public static Tuple<bool, List<int>>[,] CollidedMap { get; set; } = new Tuple<bool, List<int>>[GameSizeHeight, GameSizeWidth];
        private static List<GameObject> previousFrameObjects = new List<GameObject>();

        public static void Exit() => gamePlaying = false;

        public static void Play()
        {
            // Internal start
            Console.CursorVisible = false;
            Input.ListenKeys();

            // External start method
            if (StartMethod != null)
            {
                StartMethod.Invoke();
            }

            foreach (GameObject obj in Utility.SortGameObjects(Objects))
            {
                DrawCharArray(obj.sprite.draw, obj.transform.position);
                HandleCollisions(obj.sprite.collision, obj.transform.position, (int)obj.identifier);
            }

            previousFrameObjects = Objects;
            GameStartedTimestamp = Utility.TimeStamp();
            previousFrameTimestamp = Utility.TimeStamp();

            gamePlaying = true;

            // Prevents window from closing
            while (gamePlaying) {

                // Internal Update loop
                long currentTimestamp = Utility.TimeStamp();
                float delta = currentTimestamp - previousFrameTimestamp;

                try { Time.deltaTime = delta / (float)1000; }
                catch (DivideByZeroException) { Time.deltaTime = 0; }

                //previousFrameTimestamp = TimeStamp();

                if (Settings.FPSCounter)
                {
                    float FPS;
                    float timepassed;
                    try
                    {
                        timepassed = (float)(Utility.TimeStamp() - GameStartedTimestamp) / (float)1000;
                        FPS = TotalFrames / timepassed;
                    }
                    catch (DivideByZeroException) { FPS = 0; timepassed = 0; }

                    Console.Title = $"FPS: {FPS}, deltaTime: {Time.deltaTime}, delta: {delta}";
                }

                previousFrameTimestamp = Utility.TimeStamp();

                //Console.Clear();

                if (Debug.DrawCoordinateSystemEveryFrame)
                {
                    Debug.DrawCoordinateSystem();
                }

                // Draw gameobjects
                foreach (GameObject obj in Utility.SortGameObjects(Objects))
                {
                    DrawCharArray(obj.sprite.draw, obj.transform.position);
                    HandleCollisions(obj.sprite.collision, obj.transform.position, (int)obj.identifier);
                }

                CleanBuffered();
                previousFrameObjects = Objects;
                UpdateBuffer();
                Gamefield = GamefieldRendered;

                // External update loop
                if (UpdateMethod != null)
                {
                    UpdateMethod.Invoke();
                }

                TotalFrames++;
            }
        }

        private static void DrawCharArray(char[,] sprite, Vector2 position)
        {
            if (sprite == null)
            {
                return;
            }
            //Vector2 position = obj.transform.position;
            //char[][] sprite = obj.sprite.draw;

            for (int y = 0; y < sprite.GetLength(0); y++)
            {
                for (int x = 0; x < sprite.GetLength(1); x++)
                {
                    SetCell(sprite[y, x], (int)position.x + x, (int)position.y + y);
                    /*
                    if (sprite[y][x] != ' ')
                    {
                        //Utility.SetPixel(sprite[y][x], (int)pos.x + x, (int)pos.y + y);
                        SetCell(sprite[y][x], (int)pos.x + x, (int)pos.y + y);
                    }*/
                }
            }
        }

        private static void HandleCollisions(bool[][] collision, Vector2 position, int identifier)
        {
            if (collision == null)
            {
                return;
            }

            for (int y = 0; y < collision.Length; y++)
            {
                for (int x = 0; x < collision[0].Length; x++)
                {
                    //TODO: Implement collision system
                    if (CollisionMap[y, x] == null)
                    {
                        CollisionMap[y, x] = identifier;
                    }
                    else
                    {
                        //Objects[identifier].events.OnCollisionEnter
                    }
                }
            }
        }

        private static void UpdateBuffer()
        {
            for (int y = 0; y < GameSizeHeight; y++)
            {
                for (int x = 0; x < GameSizeWidth; x++)
                {
                    char value = Gamefield[y, x];
                    if (value != '\0')
                    {
                        Utility.SetPixel(value, x, y);
                    }
                }
            }
        }

        private static void CleanBuffered()
        {
            List<GameObject> objectsList = Objects;
            for (int i = 0; i < Math.Min(previousFrameObjects.Count, objectsList.Count); i++)
            {
                if (!previousFrameObjects[i].transform.position.EqualsInt(objectsList[i].transform.position))
                {
                    System.Diagnostics.Debug.Print("Object is not equal to new transform");
                    Vector2 position = objectsList[i].transform.position;
                    for (int y = 0; y < objectsList[i].sprite.draw.GetLength(0); y++)
                    {
                        for (int x = 0; x < objectsList[i].sprite.draw.GetLength(1); x++)
                        {
                            Gamefield[(int)position.y + y, (int)position.x + x] = ' ';
                            //SetCell(' ', (int)position.x + x, (int)position.y + y);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Set a specific cell's value.
        /// </summary>
        public static void SetCell(char value, int x, int y)
        {
            if (0 < x && x < GameSizeWidth &&
                0 < y && y < GameSizeHeight)
            {
                Gamefield[y, x] = GamefieldRendered[y, x] == value ? '\0' : value;
            }
            /*
            if (GamefieldRendered[y, x] == value)
            {
                Gamefield[y, x] = '\0';
                return;
            }

            Gamefield[y, x] = value;*/
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
                    /*
                    if (GamefieldRendered[y, x + i] == values[i])
                    {
                        Gamefield[y, x + i] = values[i];
                        continue;
                    }

                    Gamefield[y, x + i] = values[i];*/
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

    class GameObject
    {
        public ASCIISprite sprite { get; set; }
        public Transform transform { get; set; }
        public Events events { get; }
        public bool[][] collision { get; set; }
        public string name { get; set; }
        public int? identifier { get; private set; }
        public bool activeSelf { get; private set; }

        public GameObject()
        {
            sprite = new ASCIISprite();
            transform = new Transform();
            events = new Events();
            name = "gameobject";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(ASCIISprite sprite)
        {
            this.sprite = sprite;
            transform = new Transform();
            events = new Events();
            name = "gameobject";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(Transform transform)
        {
            this.transform = transform;
            sprite = new ASCIISprite();
            events = new Events();
            name = "gameobject";
            identifier = null;
            activeSelf = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObject"></param>
        public GameObject(GameObject gameObject)
        {
            this.sprite = gameObject.sprite;
            this.transform = gameObject.transform;
            this.events = gameObject.events;
            this.collision = gameObject.collision;
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

            //System.Diagnostics.Debug.WriteLine("ID: " + identifier);
            //System.Diagnostics.Debug.WriteLine("Objects: " + Game.Objects);

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
                return;

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
        /// <param name="obj">The GameObject to instantiate</param>
        public static GameObject Instantiate(GameObject obj)
        {
            GameObject t = new GameObject
            {
                identifier = Game.Objects.Count
            };
            Game.Objects.Add(t);
            return t;
        }
        public static GameObject Instantiate(GameObject obj, string name)
        {
            GameObject t = new GameObject
            {
                identifier = Game.Objects.Count,
                name = name
            };
            Game.Objects.Add(t);
            return t;
        }

        /// <summary>
        /// Find GameObject by name.
        /// </summary>
        /// <param name="name"></param>
        public static GameObject Find(string name)
        {
            return Game.Objects.Where(i => i.name == name).FirstOrDefault();
        }
        
        public class Events
        {
            public Func<int> OnCollisionEnter = null;
            public Func<int> OnCollisionStay = null;
            public Func<int> OnCollisionExit = null;
        }
    }

    class Transform
    {
        public Vector2 position { get; set; }
        public int z_index { get; set; }

        public Transform()
        {
            position = new Vector2();
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

    class ASCIISprite
    {
        public char[,] draw { get; set; }
            /*
            get { return draw; }
            set
            {
                UpdateDimensions();
                draw = value;
            }*/
        public bool[][] collision { get; set; }
        /*
        public int width
        {
            get { return width; }
            private set {
                if (value > 0)
                    width = value;
                else
                    throw new Exceptions.HeightOrWidthLessThanOrEqualToZeroException();
            }
        }
        public int height
        {
            get { return height; }
            private set {
                if (value > 0)
                    height = value;
                else
                    throw new Exceptions.HeightOrWidthLessThanOrEqualToZeroException();
            }
        }*/

        public ASCIISprite()
        {

        }/*
        public ASCIISprite(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.draw = new char[width, height];
        }*/
        public ASCIISprite(char[,] draw)
        {
            this.draw = draw;
            //this.width = draw.Length;
            //this.height = draw[0].Length;
        }
        public ASCIISprite(char[,] draw, bool[][] collision)
        {
            this.draw = draw;
            //this.width = draw.Length;
            //this.height = draw[0].Length;
            this.collision = collision;
        }

        public int width() => draw.GetLength(0);
        public int height() => draw.GetLength(1);

        /*
        private void UpdateDimensions()
        {
            if (this.draw == null || this.draw.GetLength(0) == 0 || draw.GetLength(1) == 0)
            {
                return;
            }

            this.width = this.draw.Length;
            this.height = this.draw[0].Length;
        }*/

        public static bool[][] GenerateCollision(char[][] draw)
        {
            bool[][] Collision = new bool[draw.Length][];
            for (int y = 0; y < draw.Length; y++)
            {
                Collision[y] = new bool[draw[y].Length];
                for (int x = 0; x < draw[y].Length; x++)
                {
                    if (draw[y][x] != ' ')
                    {
                        Collision[y][x] = true;
                    }
                }
            }
            return Collision;
        }
        public static bool[][] GenerateCollision(ASCIISprite sprite)
        {
            bool[][] Collision = new bool[sprite.draw.Length][];
            for (int y = 0; y < sprite.draw.GetLength(0); y++)
            {
                for (int x = 0; x < sprite.draw.GetLength(1); x++)
                {
                    Collision[y][x] = sprite.draw[y, x] != ' ';
                }
            }
            return Collision;
        }
    }

    /// <summary>
    /// Vector2 holds 2-dimensionel coordinate set(x and y).
    /// </summary>
    struct Vector2
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
        public static Vector2 zero
        {
            get
            {
                return new Vector2(0, 0);
            }
        }

        public static Vector2 right
        {
            get
            {
                return new Vector2(1, 0);
            }
        }

        public static Vector2 left
        {
            get
            {
                return new Vector2(-1, 0);
            }
        }

        public static Vector2 up
        {
            get
            {
                return new Vector2(0, -1);
            }
        }

        public static Vector2 down
        {
            get
            {
                return new Vector2(0, 1);
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

        /// <summary>
        /// Compares Vector2, same as using the '==' operator.
        /// </summary>
        public bool Equals(Vector2 vector2)
        {
            return x == vector2.x && y == vector2.y;
        }

        /// <summary>
        /// Compares Vector2, rounded to nearest whole integer.
        /// </summary>
        public bool EqualsInt(Vector2 vector2)
        {
            return (int)x == (int)vector2.x && (int)y == (int)vector2.y;
        }

        public Vector2 Integer()
        {
            return new Vector2((int)x, (int)y);
        }

        /// <summary>
        /// Sets the x and y value with one call.
        /// </summary>
        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Add(float x, float y)
        {
            this.x += x;
            this.y += y;
        }


        public override string ToString()
        {
            return $"({x.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {y.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        public static Vector2 operator *(float d, Vector2 a)
        {
            return new Vector2(a.x * d, a.y * d);
        }
        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.x * d, a.y * d);
        }
        public static Vector2 operator *(decimal d, Vector2 a)
        {
            return new Vector2(a.x * (float)d, a.y * (float)d);
        }
        public static Vector2 operator *(Vector2 a, decimal d)
        {
            return new Vector2(a.x * (float)d, a.y * (float)d);
        }
    }

    class Time
    {
        /// <summary>
        /// Still experimenting with this...
        /// </summary>
        public static float deltaTime
        {
            get; set;
        }
        
    }

    class Settings
    {
        /// <summary>
        /// Defines the coordinate (0, 0) point.
        /// </summary>
        public static CoordinateSystemType CoordinateSystemCenter = CoordinateSystemType.TopLeft;
        public static bool FPSCounter = true;

        /// <summary>
        /// Makes the grid in the console equal size.
        /// The width - height ratio changes from 1:1 to 2:1. 
        /// (A char placed in one cell is placed in 2 cells.)
        /// </summary>
        public static bool SquareMode { get; set; } = false;
    }

    class Utility
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
        /// <summary>
        /// Gets all gameobjects that are static
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> GetGameObjects()
        {
            List<GameObject> list = new List<GameObject>();
            list.AddRange(Game.Objects);
            return list;
        }*/

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

    public enum CoordinateSystemType
    {
        TopLeft, BottomLeft, Middle, TopRight, BottomRight,
    }

    public class Exceptions
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
}


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