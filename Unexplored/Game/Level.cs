using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core;
using Unexplored.Core.Components.Logic;
using Unexplored.Core.Types;
using Unexplored.Game.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Structures;
using Unexplored.Game.Attributes;

namespace Unexplored.Game
{
    public class Level
    {
        const int TilesPerChunk = 32;
        const double Timeout = 100;

        private Map map;
        private double timeout;
        private Chunks chunks;
        private SpriteBatch spriteBatch;

        public LevelObjects Objects;
        public Collider[] Colliders;

        public int CurrentMapWidth => map.Width;
        public int CurrentMapHeight => map.Height;
        public Vector2 CurrentMapSize => new Vector2(CurrentMapWidth, CurrentMapHeight);

        public Level(Map map)
        {
            this.map = map;
            int maxTilesCountInChunk = TilesPerChunk * TilesPerChunk * 3;
            chunks = new Chunks(map.Width, map.Height, TilesPerChunk, maxTilesCountInChunk);
            
            GetTilesFromIntArray(map.LevelItems);
            GetTilesFromIntArray(map.LevelBasic);
            GetTilesFromIntArray(map.LevelGhostly);

            GetAllColliders();

            Objects = new LevelObjects();
            AssignObjects(Objects);
        }

        public void SetSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTilesFrustum(Vector2 startView, Vector2 endView)
        {
            chunks.Update(startView, endView);
        }

        public void Update(GameTime gameTime)
        {
            if (timeout < Timeout)
            {
                timeout += gameTime.ElapsedGameTime.TotalMilliseconds;
                return;
            }

            for (int i = 0; i < Objects.HeroObjects.Length; i++)
            {
                var hero = Objects.HeroObjects[i];
                hero.Update(gameTime);
                for (int colliderindex = 0; colliderindex < Colliders.Length; colliderindex++)
                {
                    hero.CheckCollision(Colliders[colliderindex].Box);
                }
                hero.UpdateEnd(gameTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Draw()
        {
            chunks.DrawVisibleTiles(spriteBatch);
        }

        private void AssignObjects<T>(T distanation)
        {
            var distanationType = typeof(T);
            var field = distanationType.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance);

            if (map.LevelObjects != null)
            {
                // Перебираем все поля сцены.
                Reflection.ForEachFieldsWithAttribute<GameObjectsAttribute>(field, (sceneField, gameObjectAttribute) =>
                {
                    if (!sceneField.FieldType.IsArray)
                        return;

                    string typeName = sceneField.FieldType.Name.Replace("[]", "");
                    var type = typeof(Level).Assembly.GetTypes().First(oneType => oneType.Name == typeName);

                    List<GameObject> goList = new List<GameObject>();

                    // Пребираем все объекты на уровне.
                    foreach (var mapObject in map.LevelObjects)
                    {
                        string gameObjectName = mapObject.Name.Split('@')[0];
                        // Проверяем есть ли в сцене объект который нужно в неё передать.
                        if (gameObjectAttribute.Name == gameObjectName)
                        {
                            // Создаем новый объект
                            var gameObjectInstance = Activator.CreateInstance(type);
                            // Если это GameObject (HeroObject, ...)
                            if (gameObjectInstance is GameObject gameObject)
                            {
                                // Задаем ему начальные параметры.
                                gameObject.Transform.Size = mapObject.Position.Size;
                                gameObject.Transform.Position = mapObject.Position.Location;
                                gameObject.Type = mapObject.Type;

                                // Проверям наличие кастомных аттрибутов.
                                if (mapObject.Properties != null
                                    && mapObject.Properties.Count > 0)
                                {
                                    var gameObjectFields = gameObjectInstance.GetType().GetFields(System.Reflection.BindingFlags.Public
                                        | System.Reflection.BindingFlags.Instance);

                                    Reflection.MapDictionaryToFieldsWithAttribute<CustomPropertyAttribute>(gameObjectInstance,
                                        gameObjectFields, mapObject.Properties);
                                }

                                // Добавляем объект в список.
                                goList.Add((GameObject)gameObjectInstance);
                            }
                        }
                    }

                    var array = (GameObject[])Array.CreateInstance(type, goList.Count);
                    goList.CopyTo(array);
                    goList.Clear();

                    // Присваиваем полю сцены массив
                    sceneField.SetValue(distanation, array);
                });
            }
        }

        private void GetAllColliders()
        {
            List<Collider> colliders = new List<Collider>();

            if (map.LevelColliders != null)
            {
                var colliderFields = typeof(Collider).GetFields(System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance);
                foreach (var mapObject in map.LevelColliders)
                {
                    var collider = new Collider();
                    collider.Name = mapObject.Name;
                    collider.Type = mapObject.Type;

                    var position = mapObject.Position.Location;
                    var size = mapObject.Position.Size;
                    collider.Box = new Box(position, size);

                    if (mapObject.Properties != null
                                    && mapObject.Properties.Count > 0)
                    {
                        collider = (Collider)Reflection.MapDictionaryToFieldsWithAttribute<CustomPropertyAttribute>(collider,
                            colliderFields, mapObject.Properties);
                    }

                    colliders.Add(collider);
                }
            }

            Colliders = colliders.ToArray();
            colliders.Clear();
        }

        private void GetTilesFromIntArray(int[,] array)
        {
            if (array != null)
                for (int i = 0; i < array.Length; ++i)
                {
                    int x = i % map.Width;
                    int y = i / map.Width;
                    int tileIndex = array[y, x] - 1;
                    if (tileIndex > 0)
                    {
                        var tile = Tile.Create(x, y, tileIndex, Map.TileSetWidth);
                        int chunkX = x / TilesPerChunk;
                        int chunkY = y / TilesPerChunk;

                        chunks.SetTile(chunkX, chunkY, tile);
                    }
                }
        }
    }
}
