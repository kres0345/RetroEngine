namespace RetroEngine
{
    public static class Time
    {

        public static float timeScale { get; set; } = 1.0f;
        public static float fixedFramesPerSecond { get; set; } = 50;
        public static int frameCount { get; internal set; }
        /// <summary>
        /// Still experimenting with this...
        /// </summary>
        public static float deltaTime { get; internal set; }
        public static float fixedDeltaTime { get; internal set; }

    }

}