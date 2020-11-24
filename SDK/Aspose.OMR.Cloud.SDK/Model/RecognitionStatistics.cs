// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="RecognitionStatistics.cs">
//   Copyright (c) 2020 Aspose.Omr for Cloud
// </copyright>
// <summary>
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aspose.Omr.Cloud.Sdk.Model 
{
  using System.Collections.Generic;
  using System.Text;

  /// <summary>
  /// OMRResponseDetails
  /// </summary>  
  public class RecognitionStatistics 
  {                       
        /// <summary>
        /// Gets or sets Name
        /// </summary>  
        public string Name { get; set; }
		
        /// <summary>
        /// Warnings and other messages regarding task, etc.
        /// </summary>  
        public List<string> TaskMessages { get; set; }
		
        /// <summary>
        /// Indicates if each particular task passed or failed,
        /// </summary>  
        public string TaskResult { get; set; }
		
        /// <summary>
        /// Gets or sets RunSeconds
        /// </summary>  
        public double? RunSeconds { get; set; }
		
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()  
        {
          var sb = new StringBuilder();
          sb.Append("class RecognitionStatistics {\n");
          sb.Append("  Name: ").Append(this.Name).Append("\n");
          sb.Append("  TaskMessages: ").Append(this.TaskMessages).Append("\n");
          sb.Append("  TaskResult: ").Append(this.TaskResult).Append("\n");
          sb.Append("  RunSeconds: ").Append(this.RunSeconds).Append("\n");
          sb.Append("}\n");
          return sb.ToString();
        }
    }
}
