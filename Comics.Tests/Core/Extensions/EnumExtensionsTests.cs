using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using Comics.Core.Extensions;

using NFluent;

using Xunit;

namespace Comics.Tests.Core.Extensions
{
    public class EnumExtensionsTests
    {
        public enum SampleEnum
        {
            [Display(Name = "The First")]
            First,
            Second
        }

        [Fact]
        public void ToDisplayName_GetsAttributeValueIfProvided()
        {
            var result = SampleEnum.First.ToDisplayName();

            Check.That(result).IsEqualTo("The First");
        }

        [Fact]
        public void ToDisplayName_GetsEnumDefaultIfNotProvided()
        {
            var result = SampleEnum.Second.ToDisplayName();

            Check.That(result).IsEqualTo("Second");
        }
    }
}
