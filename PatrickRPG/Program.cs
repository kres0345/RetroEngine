using System;
using System.Security.Cryptography;
using RetroEngine;

namespace PatrickRPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.SizeHeight = 70;
            Settings.SizeWidth = 300;

            Game.StartMethod = Start;
            Game.UpdateMethod = Update;

            Debug.DrawGameBorder = true;
            //Debug.Status.HierarchyArea = Debug.Status.RelativePosition.By;
            //Debug.Status.LoggingArea = Debug.Status.RelativePosition.Below;
            //Debug.Status.LogMaxLength = 8;

            //Debug.Status.HierarchyFrameUpdate = true;

            Game.Play();
        }

        readonly static char[,] PersonSprite = new char[,]
        {
            { '\u2588' } //\u2588
        };

        static float playerSpeed = 100;
        static GameObject enemy, player;

        static void Start()
        {
            enemy = new GameObject();
            enemy.name = "Enemy";
            enemy.transform.position = new Vector2(8, 5);
            enemy.sprite.ascii = PersonSprite;
            
            bool[,] personCollision = ASCIISprite.GenerateCollision(PersonSprite);
            enemy.sprite.collision = personCollision;


            player = new GameObject();
            player.name = "Player";
            player.transform.position = new Vector2(15, 5);
            player.sprite.ascii = PersonSprite;
            player.sprite.collision = personCollision;

            player = GameObject.Instantiate(player);
            enemy = GameObject.Instantiate(enemy);
            player.Update();
            enemy.Update();
        }

        static void Update()
        {
            bool changed = false;

            if (Input.GetKey(ConsoleKey.D))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x + (1 * PlayerSpeed), player1GameObject.transform.position.y);
                player.transform.Translate(new Vector2(playerSpeed * 2, 0) * Time.deltaTime);

                changed = true;
            }
            if (Input.GetKey(ConsoleKey.W))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y - (1 * PlayerSpeed));
                player.transform.Translate(new Vector2(0, -playerSpeed) * Time.deltaTime);

                changed = true;
            }
            if (Input.GetKey(ConsoleKey.S))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x, player1GameObject.transform.position.y + (1 * PlayerSpeed));
                player.transform.Translate(new Vector2(0, playerSpeed) * Time.deltaTime);

                changed = true;
            }
            if (Input.GetKey(ConsoleKey.A))
            {
                //player1GameObject.transform.position = new Vector2(player1GameObject.transform.position.x - (1 * PlayerSpeed), player1GameObject.transform.position.y);
                player.transform.Translate(new Vector2(-playerSpeed * 2, 0) * Time.deltaTime);

                changed = true;
            }

            if (changed)
            {
                player.Update();
            }
        }
    }
}
