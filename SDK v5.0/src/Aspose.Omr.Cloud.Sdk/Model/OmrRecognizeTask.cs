/*
 * Copyright (C) 2023 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */


/* 
 * Aspose OMR Cloud V5 API
 *
 * Aspose OMR Cloud V5 API
 *
 * OpenAPI spec version: v5
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Aspose.Omr.Cloud.Sdk.Model
{
    /// <summary>
    /// OmrRecognizeTask
    /// </summary>
    [DataContract]
        public partial class OmrRecognizeTask :  IEquatable<OmrRecognizeTask>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OmrRecognizeTask" /> class.
        /// </summary>
        /// <param name="images">images (required).</param>
        /// <param name="omrFile">omrFile (required).</param>
        /// <param name="recognitionThreshold">recognitionThreshold.</param>
        /// /// <param name="outputFormat">Output Format.</param>
        public OmrRecognizeTask(List<byte[]> images = default(List<byte[]>), byte[] omrFile = default(byte[]), 
            int? recognitionThreshold = default(int?), S3DataType outputFormat = default(S3DataType))
        {
            // to ensure "images" is required (not null)
            if (images == null)
            {
                throw new InvalidDataException("images is a required property for OmrRecognizeTask and cannot be null");
            }
            else
            {
                this.Images = images;
            }
            // to ensure "omrFile" is required (not null)
            if (omrFile == null)
            {
                throw new InvalidDataException("omrFile is a required property for OmrRecognizeTask and cannot be null");
            }
            else
            {
                this.OmrFile = omrFile;
            }
            this.RecognitionThreshold = recognitionThreshold;
            this.OutputFormat = outputFormat;
        }
        
        /// <summary>
        /// Gets or Sets Images
        /// </summary>
        [DataMember(Name="images", EmitDefaultValue=false)]
        public List<byte[]> Images { get; set; }

        /// <summary>
        /// Gets or Sets OmrFile
        /// </summary>
        [DataMember(Name="omrFile", EmitDefaultValue=false)]
        public byte[] OmrFile { get; set; }

        /// <summary>
        /// Gets or Sets RecognitionThreshold
        /// </summary>
        [DataMember(Name="recognitionThreshold", EmitDefaultValue=false)]
        public int? RecognitionThreshold { get; set; }

        /// <summary>
        /// Gets or Sets output format
        /// </summary>
        [DataMember(Name = "outputFormat", EmitDefaultValue = false)]
        public S3DataType OutputFormat { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OmrRecognizeTask {\n");
            sb.Append("  Images: ").Append(Images).Append("\n");
            sb.Append("  OmrFile: ").Append(OmrFile).Append("\n");
            sb.Append("  RecognitionThreshold: ").Append(RecognitionThreshold).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as OmrRecognizeTask);
        }

        /// <summary>
        /// Returns true if OmrRecognizeTask instances are equal
        /// </summary>
        /// <param name="input">Instance of OmrRecognizeTask to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(OmrRecognizeTask input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Images == input.Images ||
                    this.Images != null &&
                    input.Images != null &&
                    this.Images.SequenceEqual(input.Images)
                ) && 
                (
                    this.OmrFile == input.OmrFile ||
                    this.OmrFile != null &&
                    input.OmrFile != null &&
                    this.OmrFile.SequenceEqual(input.OmrFile)
                ) && 
                (
                    this.RecognitionThreshold == input.RecognitionThreshold ||
                    (this.RecognitionThreshold != null &&
                    this.RecognitionThreshold.Equals(input.RecognitionThreshold))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Images != null)
                    hashCode = hashCode * 59 + this.Images.GetHashCode();
                if (this.OmrFile != null)
                    hashCode = hashCode * 59 + this.OmrFile.GetHashCode();
                if (this.RecognitionThreshold != null)
                    hashCode = hashCode * 59 + this.RecognitionThreshold.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}