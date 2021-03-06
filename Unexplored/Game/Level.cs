﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Unexplored.Core;
using Unexplored.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unexplored.Game.Structures;
using Unexplored.Game.Attributes;
using Unexplored.Core.Base;
using Unexplored.Core.Physics;
using Unexplored.Core.Components;
using Unexplored.Core.Attributes;

namespace Unexplored.Game
{
    public class Level : IDisposable
    {
        const int TilesPerChunk = 32;

        private Map map;
        private Chunks chunks;
        private SpriteBatch spriteBatch;

        public LevelObjects Objects;

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


            Objects = new LevelObjects();
            AssignObjects(Objects);
            GetAllColliders();
            AssignLightings();
        }

        public void Dispose()
        {
            Objects.Dispose();
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
                // Перебираем все поля.
                Reflection.ForEachFieldsWithAttribute<GameObjectsAttribute>(field, (objectField, gameObjectAttribute) =>
                {
                    if (!objectField.FieldType.IsArray)
                        return;

                    string typeName = objectField.FieldType.Name.Replace("[]", "");
                    var type = typeof(Level).Assembly.GetTypes().First(oneType => oneType.Name == typeName);

                    List<GameObject> goList = new List<GameObject>();

                    // Пребираем все объекты на уровне.
                    foreach (var mapObject in map.LevelObjects)
                    {
                        string gameObjectName = mapObject.Name.Split('@')[0];
                        // Проверяем есть ли в сцене объект который нужно в неё передать.
                        if (gameObjectAttribute.Type == mapObject.Type || gameObjectAttribute.Name == gameObjectName)
                        {
                            object gameObjectInstance;
                            // Создаем новый объект
                            if (mapObject.Argument == null)
                                gameObjectInstance = Activator.CreateInstance(type);
                            else
                                gameObjectInstance = Activator.CreateInstance(type, new[] { mapObject.Argument });

                            // Если это GameObject (HeroObject, ...)
                            if (gameObjectInstance is GameObject gameObject)
                            {
                                // Задаем ему начальные параметры.
                                gameObject.Transform.Size = mapObject.Position.Size;
                                gameObject.Transform.Position = mapObject.Position.Location;
                                gameObject.Tag = mapObject.Type;
                                gameObject.Id = mapObject.Id;

                                // Проверям наличие кастомных аттрибутов.
                                if (mapObject.Properties != null
                                    && mapObject.Properties.Count > 0 && gameObject.Components?.Length > 0)
                                {
                                    foreach (var component in gameObject.Components)
                                    {
                                        var componentFields = component.GetType()
                                            .GetFields(System.Reflection.BindingFlags.Public |
                                                        System.Reflection.BindingFlags.NonPublic |
                                                        System.Reflection.BindingFlags.Instance);

                                        Reflection.MapDictionaryToFieldsWithAttribute<CustomPropertyAttribute>(component,
                                            componentFields, mapObject.Properties);
                                    }
                                }

                                gameObject.PreventInitialization();

                                // Добавляем объект в список.
                                goList.Add((GameObject)gameObjectInstance);
                            }
                        }
                    }

                    var array = (GameObject[])Array.CreateInstance(type, goList.Count);
                    goList.CopyTo(array);
                    goList.Clear();

                    // Присваиваем полю объекта массив
                    objectField.SetValue(distanation, array);
                });

                
            }
        }

        private void AssignLightings()
        {
            if (map.LevelLightings != null)
            {
                Objects.Lightings = new Light[map.LevelLightings.Length];
                int index = 0;

                var lightFields = typeof(Light).GetFields(System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance);

                foreach (var mapObject in map.LevelLightings)
                {
                    var light = new Light();

                    var position = mapObject.Position.Location;
                    var size = mapObject.Position.Size;
                    var centerPosition = position + size / 2;
                    light.Position = centerPosition;
                    light.Opacity = 1.0f;

                    if (mapObject.Properties != null
                                    && mapObject.Properties.Count > 0)
                    {
                        light = (Light)Reflection.MapDictionaryToFieldsWithAttribute<CustomPropertyAttribute>(light,
                            lightFields, mapObject.Properties);
                    }

                    Objects.Lightings[index] = light;

                    ++index;
                }
            }
        }

        private void GetAllColliders()
        {
            if (map.LevelColliders != null)
            {
                Objects.Colliders = new GameObject[map.LevelColliders.Length];
                int index = 0;

                var colliderFields = typeof(MapCollider).GetFields(System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.Instance);
                foreach (var mapObject in map.LevelColliders)
                {
                    var collider = new MapCollider();
                    collider.Name = mapObject.Name;
                    collider.Type = mapObject.Type;

                    var position = mapObject.Position.Location;
                    var size = mapObject.Position.Size;
                    //collider.Offset = new ColliderOffset(position, size, Vector2.Zero, Vector2.Zero);

                    if (mapObject.Properties != null
                                    && mapObject.Properties.Count > 0)
                    {
                        collider = (MapCollider)Reflection.MapDictionaryToFieldsWithAttribute<CustomPropertyAttribute>(collider,
                            colliderFields, mapObject.Properties);
                    }

                    Objects.Colliders[index] = GameObject
                        .Create("collider", false, new Transform(mapObject.Position.Location,
                                                mapObject.Position.Size), new ColliderComponent(true, collider));
                    Objects.Colliders[index].PreventInitialization();

                    ++index;
                }
            }
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
                        var tile = Tile.Create(x, y, tileIndex);
                        int chunkX = x / TilesPerChunk;
                        int chunkY = y / TilesPerChunk;

                        chunks.SetTile(chunkX, chunkY, tile);
                    }
                }
        }
    }
}
