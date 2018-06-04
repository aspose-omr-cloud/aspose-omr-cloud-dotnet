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
  /// Represents information about file.
  /// </summary>
  public class OMRFunctionParam {
    /// <summary>
    /// FunctionParam  depends on operation
    /// </summary>
    /// <value>FunctionParam  depends on operation</value>
    public string FunctionParam { get; set; }

    /// <summary>
    /// AdditionalParam depends on operation 
    /// </summary>
    /// <value>AdditionalParam depends on operation </value>
    public string AdditionalParam { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OMRFunctionParam {\n");
      sb.Append("  FunctionParam: ").Append(FunctionParam).Append("\n");
      sb.Append("  AdditionalParam: ").Append(AdditionalParam).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }


}
}
