using System;
using System.Collections.Generic;
using Unexplored.Core.Base;
using Unexplored.Game.Attributes;
using Unexplored.Game.GameObjects;

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

        public GameObject[] Colliders;
        public GameObject[] AllObjects;

        public void MapAllObjects(Action<GameObject> callback, Action<GameObject> noneColliderCallback)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> rigidbodiesObjects = new List<GameObject>();

            gameObjects.AddRange(HeroObjects);
            gameObjects.AddRange(EnemyObjects);
            gameObjects.AddRange(PressurePlateObjects);
            gameObjects.AddRange(PlatformObjects);
            gameObjects.AddRange(TrapdoorObjects);
            gameObjects.AddRange(LeverObjects);

            gameObjects.ForEach(noneColliderCallback);
            gameObjects.AddRange(Colliders);
            gameObjects.ForEach(callback);
            //gameObjects.Reverse();

            AllObjects = gameObjects.ToArray();
        }
    }
}
