using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BlizzardApi.Util
{
    public static class EnumExtension
    {
        public static string ToDescriptionString(this Enum This)
        {
            Type type = This.GetType();

            string name = Enum.GetName(type, This);

            MemberInfo member = type.GetMembers()
                .Where(w => w.Name == name)
                .FirstOrDefault();

            DescriptionAttribute attribute = member != null
                ? member.GetCustomAttributes(true)
                    .Where(w => w.GetType() == typeof(DescriptionAttribute))
                    .FirstOrDefault() as DescriptionAttribute
                : null;

            return attribute != null ? attribute.Description : name;
        }
    }
}
