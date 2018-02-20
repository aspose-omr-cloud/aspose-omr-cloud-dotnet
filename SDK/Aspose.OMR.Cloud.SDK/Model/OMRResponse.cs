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
using System.Text;

namespace Aspose.OMR.Cloud.SDK.Model {
  public class OMRResponse {
        public string Code { get; set; }

        public string Status { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorText { get; set; }


        public Payload Payload { get; set; }

        public ServerStat ServerStat { get; set; }

        public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OMRResponse {\n");
      sb.Append("  Code: ").Append(this.Code).Append("\n");
      sb.Append("  Status: ").Append(this.Status).Append("\n");
      sb.Append("  ErrorCode: ").Append(this.ErrorCode).Append("\n");
      sb.Append("  ErrorText: ").Append(this.ErrorText).Append("\n");
      sb.Append("  Payload: ").Append(this.Payload).Append("\n");
      sb.Append("  ServerStat: ").Append(this.ServerStat).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
  }
  }
