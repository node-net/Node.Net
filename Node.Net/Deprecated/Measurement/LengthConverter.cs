namespace Node.Net.Deprecated.Measurement
{
    public class LengthConverter : System.ComponentModel.ExpandableObjectConverter
    {
        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(Length)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            var length = value as Length;
            if (!ReferenceEquals(null, length))
            {
                return length.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Length.Parse(value.ToString());
            }
            throw new System.NotSupportedException("Cannot convert to Length from type " + value.GetType().FullName);
        }
    }
}
