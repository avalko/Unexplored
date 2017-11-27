using System;
using System.Collections.Generic;
using Unexplored.Core.Base;
using Unexplored.Game.Attributes;
using Unexplored.Game.GameObjects;
using Unexplored.Game.Structures;

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

        [GameObjects(null, "text")]
        public TextObject[] TextObjects;

        [GameObjects(null, "spike")]
        public SpikeObject[] SpikeObjects;

        [GameObjects("trigger")]
        public TriggerObject[] TriggerObjects;

        [GameObjects("warp")]
        public WarpObject[] WarpObjects;

        public int LightingsCount;
        public Light[] Lightings;
        public GameObject[] Colliders;
        public GameObject[] AllObjects;

        public void MapAllObjects(Action<GameObject> callback, Action<GameObject> customCallback)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            List<GameObject> rigidbodiesObjects = new List<GameObject>();

            gameObjects.AddRange(EnemyObjects);
            gameObjects.AddRange(WarpObjects);
            gameObjects.AddRange(SpikeObjects);
            gameObjects.AddRange(LeverObjects);
            gameObjects.AddRange(TrapdoorObjects);
            gameObjects.AddRange(PressurePlateObjects);
            gameObjects.AddRange(HeroObjects);
            gameObjects.AddRange(PlatformObjects);
            // for Tiled position fixed (Y -= 16px)
            gameObjects.ForEach(customCallback);

            gameObjects.InsertRange(0, TextObjects);
            gameObjects.AddRange(TriggerObjects);
            gameObjects.AddRange(Colliders);
            gameObjects.Reverse(); // IMPORTANT!!!! (Colliders - first)

            AllObjects = gameObjects.ToArray();
            LightingsCount = Lightings.Length;

            gameObjects.ForEach(callback);
        }

        internal void Dispose()
        {
            HeroObjects = null;
            EnemyObjects = null;
            PressurePlateObjects = null;
            PlatformObjects = null;
            TrapdoorObjects = null;
            LeverObjects = null;
            Colliders = null;
            Lightings = null;
            SpikeObjects = null;
            TextObjects = null;
            AllObjects = null;
            GC.Collect();
        }
    }
}
