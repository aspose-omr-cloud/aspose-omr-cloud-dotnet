// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="HttpStatusCode.cs">
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
  using Newtonsoft.Json;
  using Newtonsoft.Json.Converters;
  
  /// <summary>
  /// 
  /// </summary>  
  [JsonConverter(typeof(StringEnumConverter))]
  public  enum HttpStatusCode 
  {  	
		/// <summary>
        /// Enum value Continue
        /// </summary>            
        Continue,
		
		/// <summary>
        /// Enum value SwitchingProtocols
        /// </summary>            
        SwitchingProtocols,
		
		/// <summary>
        /// Enum value OK
        /// </summary>            
        OK,
		
		/// <summary>
        /// Enum value Created
        /// </summary>            
        Created,
		
		/// <summary>
        /// Enum value Accepted
        /// </summary>            
        Accepted,
		
		/// <summary>
        /// Enum value NonAuthoritativeInformation
        /// </summary>            
        NonAuthoritativeInformation,
		
		/// <summary>
        /// Enum value NoContent
        /// </summary>            
        NoContent,
		
		/// <summary>
        /// Enum value ResetContent
        /// </summary>            
        ResetContent,
		
		/// <summary>
        /// Enum value PartialContent
        /// </summary>            
        PartialContent,
		
		/// <summary>
        /// Enum value MultipleChoices
        /// </summary>            
        MultipleChoices,
		
		/// <summary>
        /// Enum value Ambiguous
        /// </summary>            
        Ambiguous,
		
		/// <summary>
        /// Enum value MovedPermanently
        /// </summary>            
        MovedPermanently,
		
		/// <summary>
        /// Enum value Moved
        /// </summary>            
        Moved,
		
		/// <summary>
        /// Enum value Found
        /// </summary>            
        Found,
		
		/// <summary>
        /// Enum value Redirect
        /// </summary>            
        Redirect,
		
		/// <summary>
        /// Enum value SeeOther
        /// </summary>            
        SeeOther,
		
		/// <summary>
        /// Enum value RedirectMethod
        /// </summary>            
        RedirectMethod,
		
		/// <summary>
        /// Enum value NotModified
        /// </summary>            
        NotModified,
		
		/// <summary>
        /// Enum value UseProxy
        /// </summary>            
        UseProxy,
		
		/// <summary>
        /// Enum value Unused
        /// </summary>            
        Unused,
		
		/// <summary>
        /// Enum value TemporaryRedirect
        /// </summary>            
        TemporaryRedirect,
		
		/// <summary>
        /// Enum value RedirectKeepVerb
        /// </summary>            
        RedirectKeepVerb,
		
		/// <summary>
        /// Enum value BadRequest
        /// </summary>            
        BadRequest,
		
		/// <summary>
        /// Enum value Unauthorized
        /// </summary>            
        Unauthorized,
		
		/// <summary>
        /// Enum value PaymentRequired
        /// </summary>            
        PaymentRequired,
		
		/// <summary>
        /// Enum value Forbidden
        /// </summary>            
        Forbidden,
		
		/// <summary>
        /// Enum value NotFound
        /// </summary>            
        NotFound,
		
		/// <summary>
        /// Enum value MethodNotAllowed
        /// </summary>            
        MethodNotAllowed,
		
		/// <summary>
        /// Enum value NotAcceptable
        /// </summary>            
        NotAcceptable,
		
		/// <summary>
        /// Enum value ProxyAuthenticationRequired
        /// </summary>            
        ProxyAuthenticationRequired,
		
		/// <summary>
        /// Enum value RequestTimeout
        /// </summary>            
        RequestTimeout,
		
		/// <summary>
        /// Enum value Conflict
        /// </summary>            
        Conflict,
		
		/// <summary>
        /// Enum value Gone
        /// </summary>            
        Gone,
		
		/// <summary>
        /// Enum value LengthRequired
        /// </summary>            
        LengthRequired,
		
		/// <summary>
        /// Enum value PreconditionFailed
        /// </summary>            
        PreconditionFailed,
		
		/// <summary>
        /// Enum value RequestEntityTooLarge
        /// </summary>            
        RequestEntityTooLarge,
		
		/// <summary>
        /// Enum value RequestUriTooLong
        /// </summary>            
        RequestUriTooLong,
		
		/// <summary>
        /// Enum value UnsupportedMediaType
        /// </summary>            
        UnsupportedMediaType,
		
		/// <summary>
        /// Enum value RequestedRangeNotSatisfiable
        /// </summary>            
        RequestedRangeNotSatisfiable,
		
		/// <summary>
        /// Enum value ExpectationFailed
        /// </summary>            
        ExpectationFailed,
		
		/// <summary>
        /// Enum value UpgradeRequired
        /// </summary>            
        UpgradeRequired,
		
		/// <summary>
        /// Enum value InternalServerError
        /// </summary>            
        InternalServerError,
		
		/// <summary>
        /// Enum value NotImplemented
        /// </summary>            
        NotImplemented,
		
		/// <summary>
        /// Enum value BadGateway
        /// </summary>            
        BadGateway,
		
		/// <summary>
        /// Enum value ServiceUnavailable
        /// </summary>            
        ServiceUnavailable,
		
		/// <summary>
        /// Enum value GatewayTimeout
        /// </summary>            
        GatewayTimeout,
		
		/// <summary>
        /// Enum value HttpVersionNotSupported
        /// </summary>            
        HttpVersionNotSupported
		
  }
}
