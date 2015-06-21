using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Oogstplanner.Common
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();
            var memberInfo = type.GetMember(element.ToString());

            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes != null && attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return element.ToString();
        }

        public static IEnumerable<string> GetDescriptions(this Enum element)
        {
            foreach (Enum value in Enum.GetValues(element.GetType()))
            {
                if (element.HasFlag(value))
                {
                    yield return value.GetDescription();
                }
            }
        }
    }
}
