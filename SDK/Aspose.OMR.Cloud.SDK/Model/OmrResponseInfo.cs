/*
 * Copyright (c) 2018 Aspose Pty Ltd. All Rights Reserved.
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
 * Aspose.OMR for Cloud API Reference
 * Aspose.OMR for Cloud helps performing optical mark recognition in the cloud
 *
 * OpenAPI spec version: 1.1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */


using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Com.Aspose.Omr.Model {

  /// <summary>
  /// Represents information about OMR result.
  /// </summary>
  public class OmrResponseInfo {
    /// <summary>
    /// String value representing version of the response.
    /// </summary>
    /// <value>String value representing version of the response.</value>
    public string ResponseVersion { get; set; }

    /// <summary>
    /// Total amount of processed tasks
    /// </summary>
    /// <value>Total amount of processed tasks</value>
    public int? ProcessedTasksCount { get; set; }

    /// <summary>
    /// Total amount of successful tasks, i.e. tasks that completed without errors
    /// </summary>
    /// <value>Total amount of successful tasks, i.e. tasks that completed without errors</value>
    public int? SuccessfulTasksCount { get; set; }

    /// <summary>
    /// Additional information regarding performed task.
    /// </summary>
    /// <value>Additional information regarding performed task.</value>
    public OMRResponseDetails Details { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OmrResponseInfo {\n");
      sb.Append("  ResponseVersion: ").Append(ResponseVersion).Append("\n");
      sb.Append("  ProcessedTasksCount: ").Append(ProcessedTasksCount).Append("\n");
      sb.Append("  SuccessfulTasksCount: ").Append(SuccessfulTasksCount).Append("\n");
      sb.Append("  Details: ").Append(Details).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }


}
}
