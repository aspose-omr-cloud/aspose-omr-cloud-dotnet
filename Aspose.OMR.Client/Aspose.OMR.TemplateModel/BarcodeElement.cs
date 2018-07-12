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
namespace Aspose.OMR.TemplateModel
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Name = "Barcode", Namespace = "")]
    public class BarcodeElement : OmrElement
    {
        public BarcodeTypes BarcodeType { get; set; }

        /// <summary>
        /// Gets string representation of BarcodeType property
        /// </summary>
        [DataMember(Name = "BarcodeType")]
        public string BarcodeTypeString
        {
            get { return this.BarcodeType.ToString(); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.BarcodeType = default(BarcodeTypes);
                }
                else
                {
                    this.BarcodeType = (BarcodeTypes)Enum.Parse(typeof(BarcodeTypes), value.ToLowerInvariant());
                }
            }
        }

        public int? QrVersion { get; set; }

        [DataMember(Name = "QrVersion", EmitDefaultValue = false)]
        private int? QrVersionConditional {
            get
            {
                return this.BarcodeType == BarcodeTypes.qr ? (int?) this.QrVersion : null;
            }
            set
            {
                if (value.HasValue)
                {
                    this.QrVersion = (int) value;
                }
            }
        }
    }
}
