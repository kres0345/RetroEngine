using System;
using RetroEngine;

namespace aTestGame
{
    //Note: class- and filename is unimportant
    class GameScript
    {
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

            Debug.Status.hierarchyFrameUpdate = true;


            for (int y = 0; y < Game.Background.GetLength(0); y++)
            {
                for (int x = 0; x < Game.Background.GetLength(1); x++)
                {
                    Game.Background[y, x] = '*';
                }
            }

            // Starts game
            Game.Play();
        }

        public static GameObject player1GameObject;
        public static GameObject bulletPrefab;

        static float PlayerSpeed = 1;
        static float BulletSpeed = 0.2f;

        readonly static char[,] PlayerSprite = new char[,]
        {
            { '{', '0', '}' },
            { '{', '+', '}' },
            { '{', '0', '}' }
        };
        readonly static char[,] BulletSprite = new char[,]
        {
            { '=', '=', '>' }
        };
        readonly static char[,] SquareSprite = new char[,]
        {
            { '#', '#' },
            { '#', '#' }
        };

        static void Start()
        {
            //GameObject obj = new GameObject();
            player1GameObject = new GameObject();
            player1GameObject.transform.position = new Vector2(3, 3);
            player1GameObject.sprite.ascii = PlayerSprite;
            player1GameObject.sprite.collision = ASCIISprite.GenerateCollision(player1GameObject.sprite.ascii);

            //Game.Objects.Add("Player1", player1obj);
            bulletPrefab = new GameObject();
            bulletPrefab.sprite.ascii = BulletSprite;
            //bulletPrefab.sprite.collision = ASCIISprite.GenerateCollision(bulletPrefab.sprite.draw);
            bulletPrefab.name = "Bullet";

            player1GameObject = GameObject.Instantiate(player1GameObject);
            //bulletobj = GameObject.Instantiate(bulletobj);

        }

        // Called every frame update
        static void Update()
        {
            /*
            if (bulletobj.activeSelf)
            {
                bulletobj.transform.position = new Vector2(bulletobj.transform.position.x + BulletSpeed, bulletobj.transform.position.y);
            }*/

            bool changed = false;

            if (Input.GetKey(ConsoleKey.D))
            {
                player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x + (1 * PlayerSpeed), player1GameObject.transform.position.y);

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.W))
            {
                player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y - (1 * PlayerSpeed));

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.S))
            {
                player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y + (1 * PlayerSpeed));

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.A))
            {
                player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x - (1 * PlayerSpeed), player1GameObject.transform.position.y);

                changed = true;
            }
            else if (Input.GetKey(ConsoleKey.Spacebar))
            {

                //GameObject newBullet = bulletobj.Clone();
                GameObject newBullet = GameObject.Instantiate(bulletPrefab.Clone());

                newBullet.transform.position = player1GameObject.transform.position + new Vector2(0, 1);
                newBullet.rigidbody.velocity = new Vector2(BulletSpeed, -0.1f);
                newBullet.events.OnCollisionStay = new Action<int>(BulletOnCollisionStay);
                newBullet.Update();

                GameObject.Destroy(newBullet, 2);

                /*
                bulletobj.transform.position = new Vector2(player1obj.transform.position.x + 1, player1obj.transform.position.y + 1);
                //bullet.transform.position.Add(player1.sprite.width, 0);
                bulletobj.SetActive(true);
                bulletobj.transform.rigidbody.velocity = new Vector2(BulletSpeed, 0);
                */
                Debug.Log("Fired weapon..");
            }
            else if (Input.GetKey(ConsoleKey.Escape))
            {
                Game.Exit();
            }
            else if (Input.GetKey(ConsoleKey.D1))
            {
                bulletPrefab.sprite.ascii = BulletSprite;
                Debug.LogWarning("Selected [Bullet]");
            }
            else if (Input.GetKey(ConsoleKey.D2))
            {
                bulletPrefab.sprite.ascii = SquareSprite;
                Debug.LogWarning("Selected [Square]");
            }

            //bulletobj.Update();
            if (changed)
            {
                player1GameObject.Update();
            }
        }

		public static void BulletOnCollisionStay(int collider_identifier)
        {

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
