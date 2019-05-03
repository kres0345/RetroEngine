using System;

namespace RetroEngine
{
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

}
