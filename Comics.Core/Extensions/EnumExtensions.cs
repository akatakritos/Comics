using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Comics.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDisplayName(this Enum value)
        {
            var attr = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DisplayAttribute), inherit: false)
                .Cast<DisplayAttribute>()
                .FirstOrDefault();

            return attr?.Name ?? value.ToString();
        }
    }
}
