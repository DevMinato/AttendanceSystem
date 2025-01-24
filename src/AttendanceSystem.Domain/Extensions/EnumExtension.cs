using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AttendanceSystem.Domain.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Gets the display name of an enum
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            if (enumValue == null)
            {
                return "";
            }
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length <= 0) return value.ToString();
            var outString = ((DisplayAttribute)attrs[0]).Name;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetName();
            }

            return outString;
        }

        /// <summary>
        /// Gets the short name of an enum
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>System.String.</returns>
        public static string DisplayShortName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            if (enumValue == null)
            {
                return "";
            }
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length <= 0) return value.ToString();
            var outString = ((DisplayAttribute)attrs[0]).ShortName;

            if (((DisplayAttribute)attrs[0]).ResourceType != null)
            {
                outString = ((DisplayAttribute)attrs[0]).GetShortName();
            }

            return outString;
        }

        public static bool TryParseFromDisplayName<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute && attribute.Name == description)
                {
                    return true;
                }
                else if (field.Name == description)
                {
                    return true;
                }
            }

            return false;
        }

        public static T GetValueFromDisplayName<T>(string description) where T : Enum
        {
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if (attribute.Name == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }


        public static string GetDescription(this Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default
                    : enumMember.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Name;
        }
    }
}
