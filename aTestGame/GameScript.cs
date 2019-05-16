using System;
using RetroEngine;

namespace aTestGame
{
    //Note: class- and filename is unimportant
    class GameScript
    {
        readonly static char[,] LargeFaceSprite = new char[,]
        {
            { '\0', '\0', '\0', ' ', '\0', '\0', '\0', '_', '_', '\0', '\0', '\0', '\0', '\0', '\0' },
            { '\0', '\0', '\0', '\0', '\0', '.','\'', ' ', ' ', '`','\'', '.', '\0', '\0', '\0' },
            { '\0', '\0', '\0', '\0', '/', ' ', ' ', '_', ' ', ' ', ' ', ' ', '|', '\0', '\0' },
            { '\0', '\0', '\0', '\0', '#', '_', '/', '.','\\', '=', '=', '/', '.', '\\', '\0'},
            { '\0', '\0', '\0', '(', ',', ' ','\\', '_', '/', ' ','\\','\\', '_', '/', '\0' },
            { '\0', '\0', '\0', '\0', '|', ' ', ' ', ' ', ' ', '-','\'', ' ', '|', '\0', '\0' },
            { '\0', '\0', '\0', '\0','\\', ' ', ' ', ' ','\'', '=', ' ', ' ', '/', '\0', '\0'},
            { '\0', '\0', '\0', '\0', '/', '`', '-', '.', '_', '_', '.','\'', '\0', '\0', '\0' },
            { '\0', '\0', '\0','\'', '`', '-', '.', '_', '_', '_', '|', '_', '_', '\0', '\0' },
            { '/', ' ', ' ', ' ', ' ','\\', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '`', '.' }
        };

        readonly static char[,] LaserSprite = new char[,]
        {
            { '\0', '&', '&', '&' },
            { 'Y',  'Y', 'Y', 'Y' }
        };

        readonly static char[,] PlayerSprite = new char[,]
        {
            { '{', '0', '}' },
            { '{', '+', '}' },
            { '{', '0', '}' }
        };
        readonly static char[,] BulletSprite = new char[,]
        {
            { '=', '>' }
        };
        readonly static char[,] SquareSprite = new char[,]
        {
            { '#', '#' },
            { '#', '#' }
        };
        readonly static char[,] WallSprite = new char[,]
        {
            { '#' },
            { '#' },
            { '#' },
            { '#' }
        };

        static void Main(string[] args)
        {
            // Assigns start method
            Game.StartMethod = Start;
            Game.UpdateMethod = Update;

            Debug.DrawGameBorder = true;
            Debug.Status.HierarchyArea = Debug.Status.RelativePosition.By;
            Debug.Status.LoggingArea = Debug.Status.RelativePosition.Below;
            Debug.Status.LogMaxLength = 8;

            Settings.SizeHeight = 30;
            Settings.SizeWidth = 120;

            Debug.Status.HierarchyFrameUpdate = true;

            // Starts game
            Game.Play();
        }

        public static GameObject player1GameObject;
        public static GameObject bulletPrefab;
        public static GameObject wallGameObject;

        static float PlayerSpeed = 1;
        static float BulletSpeed = 0.9f;

        static float HorizontalSpeedMultiplier = 2;



        static void Start()
        {
            //GameObject obj = new GameObject();
            player1GameObject = new GameObject();
            player1GameObject.transform.position = new Vector2(3, 3);
            player1GameObject.sprite.ascii = LargeFaceSprite;
            player1GameObject.sprite.collision = ASCIISprite.GenerateCollision(player1GameObject.sprite.ascii);

            //Game.Objects.Add("Player1", player1obj);
            bulletPrefab = new GameObject();
            bulletPrefab.sprite.ascii = BulletSprite;
            //bulletPrefab.sprite.collision = ASCIISprite.GenerateCollision(bulletPrefab.sprite.draw);
            bulletPrefab.name = "Bullet";

            wallGameObject = new GameObject();
            wallGameObject.transform.position = new Vector2(30, 7);
            wallGameObject.sprite.ascii = WallSprite;
            wallGameObject.sprite.collision = ASCIISprite.GenerateCollision(WallSprite);
            wallGameObject.events.OnCollisionStay = WallCollider;

            player1GameObject = GameObject.Instantiate(player1GameObject);
            wallGameObject = GameObject.Instantiate(wallGameObject);
        }

        // Called every frame update
        static void Update()
        {
            bool changed = false;

            if (Input.GetKey(ConsoleKey.D))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x + (1 * PlayerSpeed), player1GameObject.transform.position.y);
                player1GameObject.transform.Translate(new Vector2(PlayerSpeed * HorizontalSpeedMultiplier, 0));

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.W))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y - (1 * PlayerSpeed));
                player1GameObject.transform.Translate(new Vector2(0, -PlayerSpeed));

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.S))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y + (1 * PlayerSpeed));
                player1GameObject.transform.Translate(new Vector2(0, PlayerSpeed));

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.A))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x - (1 * PlayerSpeed), player1GameObject.transform.position.y);
                player1GameObject.transform.Translate(new Vector2(-PlayerSpeed * HorizontalSpeedMultiplier, 0));

                changed = true;
            }
            else if (Input.GetKeyDown(ConsoleKey.Spacebar))
            {

                //GameObject newBullet = bulletobj.Clone();
                GameObject newBullet = GameObject.Instantiate(bulletPrefab.Clone());

                newBullet.transform.position = player1GameObject.transform.position + new Vector2(1, 1);
                newBullet.rigidbody.velocity = new Vector2(BulletSpeed, 0.0f);
                newBullet.events.OnCollisionStay = new Action<int>(BulletOnCollisionStay);
                newBullet.events.OnCollisionEnter = new Action<int>(BulletOnCollisionStay);
                newBullet.Update();

                GameObject.Destroy(newBullet, 2);

                Debug.Log("Fired weapon..");
            }
            else if (Input.GetKeyDown(ConsoleKey.D1))
            {
                bulletPrefab.sprite.ascii = BulletSprite;
                bulletPrefab.sprite.collision = ASCIISprite.GenerateCollision(BulletSprite);
                Debug.LogWarning("Selected [Bullet]");
            }
            else if (Input.GetKeyDown(ConsoleKey.D2))
            {
                bulletPrefab.sprite.ascii = SquareSprite;
                bulletPrefab.sprite.collision = ASCIISprite.GenerateCollision(SquareSprite);
                Debug.LogWarning("Selected [Square]");
            }
            else if (Input.GetKeyDown(ConsoleKey.OemPeriod))
            {
                Debug.RefreshScreen();
            }
            else if (Input.GetKeyDown(ConsoleKey.Escape))
            {
                Game.Exit();
            }
            

            if (changed)
            {
                player1GameObject.Update();
            }
        }

		public static void BulletOnCollisionStay(int colliderIdentifier)
        {
            Debug.LogWarning("Bullet: "+colliderIdentifier);
        }

        public static void WallCollider(int colliderIdentifier)
        {
            Debug.LogWarning("Wall: "+colliderIdentifier);
        }
    }
}

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
        * /
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
        };* /
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
        };* /
        readonly static char[,] Player1 = new char[,]
        {
            { '{', '0', '}' },
            { '{', '+', '}' },
            { '{', '0', '}' }
        };
        readonly static char[,] Bullet = new char[,]
        {
            { '=', '=', '>' }
        };

        static void Start()
        {
            //GameObject obj = new GameObject();
            player1obj = new GameObject();
            player1obj.transform.position = new Vector2(3, 3);
            player1obj.sprite.draw = Player1;
            player1obj.sprite.collision = ASCIISprite.GenerateCollision(player1obj.sprite.draw);

            //Game.Objects.Add("Player1", player1obj);
            bulletobj = new GameObject();
            bulletobj.sprite.draw = Bullet;
            bulletobj.sprite.collision = ASCIISprite.GenerateCollision(bulletobj.sprite.draw);
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

            //GameObject player1 = Game.Objects[(int)player1obj.identifier];
            //GameObject bullet = Game.Objects[(int)bulletobj.identifier];

            if (bulletobj.activeSelf)
            {
                bulletobj.transform.position = new Vector2(bulletobj.transform.position.x + BulletSpeed, bulletobj.transform.position.y);
                //* Time.deltaTime
            }

            if (Input.GetKey(ConsoleKey.D))
            {
                player1obj.transform.position = new Vector2(player1obj.transform.position.x + (1 * PlayerSpeed), player1obj.transform.position.y);
            }
            else if (Input.GetKey(ConsoleKey.W))
            {
                player1obj.transform.position = new Vector2(player1obj.transform.position.x, player1obj.transform.position.y - (1 * PlayerSpeed));
            }
            else if (Input.GetKey(ConsoleKey.S))
            {
                player1obj.transform.position = new Vector2(player1obj.transform.position.x, player1obj.transform.position.y + (1 * PlayerSpeed));
            }
            else if (Input.GetKey(ConsoleKey.A))
            {
                player1obj.transform.position = new Vector2(player1obj.transform.position.x - (1 * PlayerSpeed), player1obj.transform.position.y);
            }
            else if (Input.GetKey(ConsoleKey.Spacebar))
            {
                bulletobj.transform.position = new Vector2(player1obj.transform.position.x + 1, player1obj.transform.position.y + 1);
                //bullet.transform.position.Add(player1.sprite.width, 0);
                bulletobj.SetActive(true);
            }
            else if (Input.GetKey(ConsoleKey.Backspace))
            {
                bulletobj.SetActive(false);
            }
            else if (Input.GetKey(ConsoleKey.Escape))
            {
                Game.Exit();
            }

            bulletobj.Update();
            player1obj.Update();

        }
    }
}*/
