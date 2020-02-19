// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="ApiVersion.cs">
//   Copyright (c) 2019 Aspose.Omr for Cloud
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

namespace Aspose.Omr.Cloud.Sdk
{
    /// <summary>
    /// The availiable api versions.
    /// </summary>
    public class ApiVersion
    {
        public string Version { get; set; }
        private ApiVersion(string version)
        {
            Version = version;
        }
        /// <summary>
        /// Current API version
        /// </summary>
        public static ApiVersion V1 = new ApiVersion("1.0");

        /// <summary>
        /// Stable version
        /// </summary>
        public static ApiVersion V2 = new ApiVersion("2.0");

        /// <summary>
        /// Frozen version
        /// </summary>
        public static ApiVersion V3 = new ApiVersion("3.0");

        public override string ToString()
        {
            return Version;
        }
    }

}
