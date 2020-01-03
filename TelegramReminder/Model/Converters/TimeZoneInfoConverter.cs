using System;
using System.ComponentModel;

namespace TelegramReminder.Model.Converters
{
    public abstract class ConverterProvider
    {
        public abstract void AddConverter();
    }

    public class TimeZoneInfoConverterProvider : ConverterProvider
    {
        public override void AddConverter()
        {
            TypeDescriptor.AddAttributes(typeof(TimeZoneInfo), new[] { new TypeConverterAttribute(typeof(TimeZoneInfoConverter)) });
        }
    }

    //[TypeConverter(typeof(TimeZoneInfoConverter))]
    public class TimeZoneInfoConverter : TypeConverter
    {
        public TimeZoneInfoConverter()
        { }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return base.CanConvertFrom(context, sourceType);
        }
    }
}
