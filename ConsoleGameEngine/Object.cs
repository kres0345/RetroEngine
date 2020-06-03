using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine
{
    public class Object
    {
        public GameObject gameObject;
        public Transform transform;
        public Rigidbody rigidbody;
        /// <summary>
        /// When disabled, no calculations including properties of the rigidbody will be done.
        /// </summary>
        public bool rigidbodyEnabled { get; set; } = false;
        public string name;
        public string tag;
        internal int? identifier;

        public override string ToString() => name;
        public int? GetInstanceID() => identifier;

        public bool CompareTag(string tag)
        {
            return this.tag == tag;
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
        /// <param name="original">The GameObject to instantiate</param>
        public static GameObject Instantiate(GameObject original)
        {
            GameObject t = new GameObject(original);
            t.identifier = Game.Objects.Count;

            Game.Objects.Add(t);
            return t;
        }
        public static GameObject Instantiate(GameObject original, Vector2 position)
        {
            GameObject t = new GameObject(original);
            t.identifier = Game.Objects.Count;
            t.transform.position = position;

            Game.Objects.Add(t);
            return t;
        }
    }
}
