using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TelegramReminder.Model.Converters;

namespace TelegramReminder.Model
{
    public class ArgumentAttribute : Attribute
    {
        public string Key { get; }
        public bool Required { get; }

        public ArgumentAttribute(string key, bool required)
        {
            Key = key;
            Required = required;
        }
    }

    public class Arguments
    {
        static Arguments()
        {
            var type = typeof(ConverterProvider);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach(var t in types)
            {
                var instance = Activator.CreateInstance(t);
                var method = t.GetMethod("AddConverter");

                method.Invoke(instance, new object[] { });
            }
        }

        public static T Using<T> (IEnumerable<KeyValuePair<string, string>> args) where T: class
        {
            var destinationType = typeof(T);
            var properties = destinationType.GetProperties();

            if (!properties.Any())
                return null;

            var foundAttributes = new List<(ArgumentAttribute, PropertyInfo)>();
            foreach(var p in properties)
            {
                var attributes = p.GetCustomAttributes().OfType<ArgumentAttribute>();

                if (attributes.Any() is false)
                    continue;

                foundAttributes.Add((attributes.Single(), p));            
            }
                    
            var instance = Activator.CreateInstance<T>();

            foreach(var attribute in foundAttributes)
            {
                var inputPair = args.FirstOrDefault(a => a.Key == attribute.Item1.Key);

                if (inputPair.Equals(default(KeyValuePair<string, string>)))
                {
                    if (attribute.Item1.Required)
                        throw new FormatException();

                    continue;
                }

                var targetProperty = instance.GetType().GetProperty(attribute.Item2.Name);

                if (targetProperty.CanWrite is false)
                    throw new ArgumentException();

                var propertyType = targetProperty.PropertyType;

                var converter = TypeDescriptor.GetConverter(propertyType);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    targetProperty.SetValue(instance, converter.ConvertFrom(inputPair.Value));
                    continue;
                }
                else
                    throw new ArgumentException();
            }

            return instance;
        }
    }
}
