using System;
using RetroEngine;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            // Defines start and update method.
            Game.StartMethod = Start;
            Game.UpdateMethod = Update;

            // Set game height and width.
            Settings.GameSizeHeight = 50;
            Settings.GameSizeWidth = 100;

            // Updates window title to current fps every frame.
            Debug.FPSCounter = true;

            // Begins game and drawing loop.
            Game.Play();
        }

        // Called with Game.Play(). Used for game character initialization initialization.
        static void Start()
        {

        }

        // Called every frame.
        static void Update()
        {

        }
    }
}
