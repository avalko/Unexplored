using System;
using System.Collections.Generic;
using Unexplored.Game.Attributes;
using Unexplored.Game.Objects;

namespace Unexplored.Game
{
    public class LevelObjects
    {
        [GameObjects("hero")]
        public HeroObject[] HeroObjects;

        [GameObjects("enemy")]
        public EnemyObject[] EnemyObjects;

        [GameObjects("pressure_plate")]
        public PressurePlateObject[] PressurePlateObjects;

        [GameObjects("platform")]
        public PlatformObject[] PlatformObjects;

        [GameObjects("trapdoor")]
        public TrapdoorObject[] TrapdoorObjects;

        [GameObjects("lever")]
        public LeverObject[] LeverObjects;


        public GameObject[] RigidbodiesObjects;
        public int RigidbodiesObjectsCount;

        public void MapAllObjects(Action<GameObject> callback)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> rigidbodiesObjects = new List<GameObject>();

            gameObjects.AddRange(HeroObjects);
            gameObjects.AddRange(EnemyObjects);
            gameObjects.AddRange(PressurePlateObjects);
            gameObjects.AddRange(PlatformObjects);
            gameObjects.AddRange(TrapdoorObjects);
            gameObjects.AddRange(LeverObjects);

            for (int index = 0; index < gameObjects.Count; ++index)
            {
                var gameObject = gameObjects[index];
                callback(gameObject);

                if (gameObject.IsRigidbody)
                    rigidbodiesObjects.Add(gameObject);
            }

            RigidbodiesObjects = rigidbodiesObjects.ToArray();
            RigidbodiesObjectsCount = RigidbodiesObjects.Length;
            rigidbodiesObjects.Clear();
        }
    }
}
