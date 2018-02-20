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
using System;
using System.Text;

namespace Aspose.OMR.Cloud.SDK.Model
{
    public class ResponseMessage
    {
        public ResponseMessage() { }
        public ResponseMessage(byte[] ResponseStream, int Code, String Status )
        {
            this.Code = Code;
            this.Status = Status;
            this.ResponseStream = ResponseStream;
        }
        public int Code { get; set; }
        public string Status { get; set; }

        public string Message { get; set; }
        public byte[] ResponseStream { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ResponseMessage {\n");
            sb.Append("  Code: ").Append(this.Code).Append("\n");
            sb.Append("  Message: ").Append(this.Message).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}


