namespace BToolbox.Helpers
{
    public class EnumToStringConverter<TEnum> : EnumConverter<TEnum, string>
    {
        public EnumToStringConverter(string defaultValue = null, Dictionary<TEnum, string> convertedValues = null)
            : base(defaultValue, convertedValues) { }
    }
}
