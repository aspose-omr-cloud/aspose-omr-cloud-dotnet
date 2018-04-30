/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.Controls
{
    using System;
    using System.Windows.Markup;

    /// <summary>
    /// Extensions used to bind enum values as items source directly in XAML
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        /// <summary>
        /// Type of the enum that is used as binding source
        /// </summary>
        private Type enumType;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumBindingSourceExtension"/> class
        /// </summary>
        /// <param name="enumType">The enum type used as binding source</param>
        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        /// <summary>
        /// Gets or sets the used enum type
        /// </summary>
        public Type EnumType
        {
            get
            {
                return this.enumType;
            }
            set
            {
                if (this.enumType != value && value != null)
                {
                    Type actualEnumType = Nullable.GetUnderlyingType(value) ?? value;

                    if (!actualEnumType.IsEnum)
                    {
                        throw new ArgumentException("Type must be for an Enum.");
                    }

                    this.enumType = value;
                }
            }
        }

        /// <summary>
        /// Provides enum values for binding
        /// </summary>
        /// <param name="serviceProvider">A service provider helper</param>
        /// <returns>Binding values</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (this.EnumType == null)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(this.EnumType) ?? this.EnumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == this.EnumType)
            {
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
