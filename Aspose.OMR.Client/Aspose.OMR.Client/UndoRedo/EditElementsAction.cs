/*
 * Copyright (c) 2017 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/asposecloud/Aspose.OMR-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.UndoRedo
{
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Currently not used
    /// </summary>
    public class EditElementsAction : IUndoRedoAction
    {
        public EditElementsAction(List<object> element, List<string> propertyName, List<object> value)
        {
            this.Elements = element;
            this.Properties = new List<PropertyInfo>();
            foreach (string name in propertyName)
            {
                this.Properties.Add(element.GetType().GetTypeInfo().GetDeclaredProperty(name));
            }

            this.Values = value;
        }

        public EditElementsAction(object element, string propertyName, object oldValue, object value)
        {
            this.Elements = new List<object>() { element };
            this.Values = new List<object>() { value };
            this.OldValues = new List<object>() { oldValue };

            this.Properties = new List<PropertyInfo>();
            this.Properties.Add(element.GetType().GetTypeInfo().GetDeclaredProperty(propertyName));
        }

        public List<object> Elements { get; set; }

        public List<PropertyInfo> Properties { get; set; }

        public List<object> Values { get; set; }

        public List<object> OldValues { get; set; }

        public void Execute()
        {
            foreach (object item in this.Elements)
            {
                for (var i = 0; i < this.Properties.Count; i++)
                {
                    PropertyInfo property = this.Properties[i];
                    property.SetValue(item, this.Values[i], null);
                }
            }
        }

        public void UnExecute()
        {
            foreach (object item in this.Elements)
            {
                for (var i = 0; i < this.Properties.Count; i++)
                {
                    PropertyInfo property = this.Properties[i];
                    property.SetValue(item, this.OldValues[i], null);
                }
            }
        }
    }
}
