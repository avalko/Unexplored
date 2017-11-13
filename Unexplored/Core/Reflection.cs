using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unexplored.Core
{

    public static class Reflection
    {
        public static void ForEachFieldsWithAttribute<T>(System.Reflection.FieldInfo[] fields, Action<System.Reflection.FieldInfo, T> callback)
        {
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(false);
                foreach (var attr in attributes)
                {
                    if (attr is T attribute)
                    {
                        callback(field, attribute);
                    }
                }
            }
        }

        public static object MapDictionaryToFieldsWithAttribute<T>(object instance, System.Reflection.FieldInfo[] fileds, Dictionary<string, object> values)
            where T : Attribute
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
            if (fileds == null)
                throw new ArgumentNullException(nameof(fileds));

            // Перебираем все поля instance.
            ForEachFieldsWithAttribute<T>(fileds,
                (field, attribute) =>
                {
                    // Перебираем все параметры.
                    foreach (var valuePair in values)
                    {
                        // Если имя параметра соответсвует имени переменной.
                        if (field.Name == valuePair.Key)
                        {
                            // Если тип параметра соответсвует типу переменной.
                            if (valuePair.Value.GetType() == field.FieldType)
                            {
                                // Присваиваем полю значение параметра.
                                field.SetValue(instance, valuePair.Value);
                            }
                            else
                            {
                                string errorMessage =
                                    $"Error: parameter type {valuePair.Key} does not match the type in the object {instance.GetType()}!";
                                throw new Exception(errorMessage);
                            }
                        }
                    }
                });
            return instance;
        }
    }
}
