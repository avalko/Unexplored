using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Unexplored.Core;
using Unexplored.Core.Types;
using System.Globalization;
using Unexplored.Game.Attributes;
using Unexplored.Game.Structures;

namespace Unexplored.Game
{
    static class MapReader
    {
        public static Map ParseMap(string json)
        {
            if (json == null)
                throw new ArgumentNullException(nameof(json));

            Map map = new Map();

            var mapFields = typeof(Map)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            var mapObjectFields = typeof(MapObject)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            dynamic rawMap = JObject.Parse(json);
            JArray layers = rawMap.layers;
            int layersCount = layers.Count;
            int layerWidth = rawMap.width;
            int layerHeight = rawMap.height;

            map.Width = layerWidth;
            map.Height = layerHeight;

            Dictionary<string, dynamic> layersList = new Dictionary<string, dynamic>();
            for (int layerIndex = 0; layerIndex < layersCount; ++layerIndex)
            {
                dynamic layer = layers[layerIndex];
                layersList.Add((string)layer.name, layer);
            }

            Reflection.ForEachFieldsWithAttribute<MapLayerAttribute>(mapFields,
                (mapField, mapAttribute) =>
                {
                    var layer = layersList[mapAttribute.Tag];

                    if (mapAttribute.LayerType == LayerType.Tiles)
                    {
                        int[] mapData = ((JArray)layer.data).ToObject<int[]>();
                        int[,] mapResult = new int[layerWidth, layerHeight];
                        for (int i = 0; i < mapData.Length; i++)
                        {
                            int x = i % layerHeight;
                            int y = i / layerHeight;
                            mapResult[y, x] = mapData[i];
                        }
                        mapField.SetValue(map, mapResult);
                    }
                    else if (mapAttribute.LayerType == LayerType.Objects)
                    {
                        JArray objectsArray = layer.objects;
                        int objectsCount = objectsArray.Count;

                        MapObject[] objects = new MapObject[objectsCount];

                        for (int objectIndex = 0; objectIndex < objectsCount; ++objectIndex)
                        {
                            dynamic obj = objectsArray[objectIndex];
                            var currentObject = new MapObject
                            {
                                Position = new FRect((float)obj.x, (float)obj.y, (float)obj.width, (float)obj.height),
                                Name = (string)obj.name,
                                Type = (string)obj.type
                            };

                            Dictionary<string, object> propertiesDict = new Dictionary<string, object>();

                            if (obj.properties is JObject properties)
                            {
                                var currentPropertyType = ((JObject)obj.propertytypes).First;
                                var currentProperty = properties.First;
                                while (currentProperty != null)
                                {
                                    var prop = (JProperty)currentProperty;
                                    var type = (JProperty)currentPropertyType;
                                    propertiesDict.Add(prop.Name,
                                        GetJsonObjectByType(((JValue)type.Value).Value.ToString(), ((JValue)prop.Value).Value));
                                    currentProperty = currentProperty.Next;
                                    currentPropertyType = currentPropertyType.Next;
                                }
                            }

                            currentObject.Properties = propertiesDict;
                            objects[objectIndex] = currentObject;
                        }

                        mapField.SetValue(map, objects);
                    }
                });
            
            return map;
        }

        private static object GetJsonObjectByType(string type, object value)
        {
            Color ToColor(string hex)
            {
                uint argb = uint.Parse(hex.Replace("#", ""), NumberStyles.HexNumber);
                return Color.FromNonPremultiplied((byte)((argb & -16777216) >> 0x18),
                                      (byte)((argb & 0xff0000) >> 0x10),
                                      (byte)((argb & 0xff00) >> 8),
                                      (byte)(argb & 0xff));
            }

            switch (type)
            {
                case "float": return Convert.ToSingle(value);
                case "string": return Convert.ToString(value);
                case "bool": return Convert.ToBoolean(value);
                case "int": return Convert.ToInt32(value);
                case "color":
                    string color = value.ToString();
                    if (color.Length == 9)
                        return ToColor(color);
                    break;
            }

            return value;
        }

    }
}
