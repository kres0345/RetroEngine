using System;
using RetroEngine;

namespace aTestGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Assigns start method
            Game.StartMethod = Start;
            Game.UpdateMethod = Update;

            // Starts game
            Game.Play();
        }

        public static GameObject player1obj;
        public static GameObject bulletobj;

        static float PlayerSpeed = 1;
        static float BulletSpeed = 0.5f;

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

            if (bulletobj.activeSelf)
            {
                bulletobj.transform.position = new Vector2(bulletobj.transform.position.x + BulletSpeed, bulletobj.transform.position.y);
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
