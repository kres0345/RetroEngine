using System;
using RetroEngine;

namespace SecondGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.SizeHeight = 30;
            Settings.SizeWidth = 100;

            Game.StartMethod = Start;
            Game.UpdateMethod = Update;
            Debug.DrawGameBorder = true;

            Game.Play();
        }

        readonly static char[,] GroundSprite = new char[,]
        {
            { '#' }
        };

        readonly static char[,] PlayerSprite = new char[,]
        {
            { '8', ')', ' ' },
            { '[', ']', ' ' },
            { '[', ']', '=' },
            { '[', ']', ' ' }
        };

        static void Start()
        {
            GameObject floor = new GameObject();
            floor.transform.localScale = new Vector2(20, 1);
            floor.transform.position = new Vector2(2, 25);

            floor.sprite.ascii = GroundSprite;
            floor.sprite.collision = ASCIISprite.GenerateCollision(GroundSprite);

            GameObject.Instantiate(floor);

            GameObject player = new GameObject();
            player.transform.position = new Vector2(3, 3);

            player.sprite.ascii = PlayerSprite;
            player.sprite.collision = ASCIISprite.GenerateCollision(PlayerSprite);

            player.ActivateRigidbody();
            player.rigidbody.mass = Rigidbody.CalculateMass(player.sprite.collision);
            player.rigidbody.UpdateCenterOfMass(player.sprite);

            GameObject.Instantiate(player);
        }

        static void Update()
        {

        }
    }
}
