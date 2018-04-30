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
namespace Aspose.OMR.Client.Utility
{
    using System.Security.Cryptography;

    /// <summary>
    /// Provides utility methods to encrypt and decrypt data
    /// </summary>
    public static class SecurityUtility
    {
        /// <summary>
        /// Encrpyts provided data
        /// </summary>
        /// <param name="secret">Byte array data</param>
        /// <returns>Encrypted data</returns>
        public static byte[] Encrypt(byte[] secret)
        {
            byte[] ciphertext = ProtectedData.Protect(secret, null, DataProtectionScope.CurrentUser);
            return ciphertext;
        }

        /// <summary>
        /// Decrypts encrypted byte data
        /// </summary>
        /// <param name="ciphertext">Encrypted byte array data</param>
        /// <returns>Decrypted data</returns>
        public static byte[] Decrpyt(byte[] ciphertext)
        {
            byte[] plainText = ProtectedData.Unprotect(ciphertext, null, DataProtectionScope.CurrentUser);
            return plainText;
        }
    }
}
