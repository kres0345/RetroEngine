using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RetroEngine
{
    public class GameObject : Object
    {
        public ASCIISprite sprite { get; set; }
        public Events events { get; }
        //public int? identifier { get; internal set; }
        public bool activeSelf { get; private set; }

        public GameObject()
        {
            sprite = new ASCIISprite();
            transform = new Transform();
            rigidbody = new Rigidbody();
            events = new Events();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(ASCIISprite sprite)
        {
            this.sprite = sprite;
            transform = new Transform();
            rigidbody = new Rigidbody();
            events = new Events();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(Transform transform)
        {
            this.transform = transform;
            sprite = new ASCIISprite();
            events = new Events();
            rigidbody = new Rigidbody();
            name = "default name";
            identifier = null;
            activeSelf = true;
        }
        public GameObject(GameObject go)
        {
            this.sprite = go.sprite;
            this.rigidbody = go.rigidbody;
            if (go.rigidbodyEnabled)
                this.rigidbody.Reset();
            this.transform = go.transform;
            this.events = go.events;
            this.name = go.name;
            this.activeSelf = go.activeSelf;
        }

        /// <summary>
        /// Should only be called if you need the rigidbody.
        /// </summary>
        public void ActivateRigidbody()
        {
            rigidbodyEnabled = true;
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

            //Debug.Log("ID: " + identifier);
            //Debug.Log("Objects: " + Game.Objects);

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
            {
                Debug.LogError("ID is null of " + name);
                return;
            }

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
                Game.Objects[(int)identifier] = null;
            }
            else
            {
                Task.Run(async () =>
                {
                    await Task.Delay((int)(delay * 1000));
                    Game.Objects[(int)identifier] = null;
                });
            }
        }

        public void SetActive(bool value)
        {
            activeSelf = value;
        }

        

        /// <summary>
        /// Find GameObject by name.
        /// </summary>
        /// <param name="name"></param>
        public static GameObject Find(string name) => Game.Objects.Where(i => i.name == name).FirstOrDefault();

        public class Events
        {
            public Action<int> OnCollisionEnter;
            public Action<int> OnCollisionStay;
            public Action<int> OnCollisionExit;
            internal List<int> LastFrameCollisions = new List<int>();

            public bool TryGetOnCollisionEnter(out Action<int> OnCollisionEnter)
            {
                OnCollisionEnter = this.OnCollisionEnter;
                return this.OnCollisionEnter != null;
            }

            public bool TryGetOnCollisionStay(out Action<int> OnCollisionStay)
            {
                OnCollisionStay = this.OnCollisionStay;
                return this.OnCollisionStay != null;
            }

            public bool TryGetOnCollisionExit(out Action<int> OnCollisionExit)
            {
                OnCollisionExit = this.OnCollisionExit;
                return this.OnCollisionExit != null;
            }
        }
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