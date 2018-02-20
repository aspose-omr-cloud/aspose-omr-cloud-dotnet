/*
 * Copyright (c) 2017 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *       https://github.com/asposecloud/Aspose.OMR-Cloud/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace Aspose.OMR.Client.Utility
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Helper handling bubbles answers mapping 
    /// </summary>
    public static class AnswersMappingHelper
    {
        /// <summary>
        /// English upperacse letters
        /// </summary>
        private static readonly string[] EnglishLettersUppercase =
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "G", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z"
        };

        /// <summary>
        /// English lowercase letters
        /// </summary>
        private static readonly string[] EnglishLettersLowercase =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"
        };

        /// <summary>
        /// Digits from 1 to 9
        /// </summary>
        private static readonly string[] DigitsFromOne = {"1", "2", "3", "4", "5", "6", "7", "8", "9"};

        /// <summary>
        /// Digits from 0 to 9
        /// </summary>
        private static readonly string[] DigitsFromZero = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};

        /// <summary>
        /// Dictionary for correspondence between display shortcut string and actual mapping values
        /// </summary>
        private static readonly Dictionary<string, string[]> AnswersMappingDictionary;

        /// <summary>
        /// Dictionary with user defined mapping
        /// </summary>
        private static readonly Dictionary<string, string[]> CustomMappingDictionary;

        /// <summary>
        /// Display shortcut for english uppercase letters
        /// </summary>
        private static string englishLettersKeyUpper = "A..Z";

        /// <summary>
        /// Display shortcut for english lowercase letters
        /// </summary>
        private static string englishLettersKeyLower = "a..z";

        /// <summary>
        /// Display shortcut for digits from 1 to 9
        /// </summary>
        private static string digitsFromOneKey = "1..9";

        /// <summary>
        /// Display shortcut for digits from 0 to 9
        /// </summary>
        private static string digitsFromZeroKey = "0..9";

        /// <summary>
        /// Initializes static members of the <see cref="AnswersMappingHelper"/> class.
        /// </summary>
        static AnswersMappingHelper()
        {
            AnswersMappingDictionary = new Dictionary<string, string[]>();
            CustomMappingDictionary = new Dictionary<string, string[]>();

            // add default values
            AnswersMappingDictionary.Add(englishLettersKeyUpper, EnglishLettersUppercase);
            AnswersMappingDictionary.Add(englishLettersKeyLower, EnglishLettersLowercase);
            AnswersMappingDictionary.Add(digitsFromOneKey, DigitsFromOne);
            AnswersMappingDictionary.Add(digitsFromZeroKey, DigitsFromZero);

            // load from settings and add user entries in format : "name" (or key) - "values"
            CustomMappingDictionary = UserSettingsUtility.LoadCustomMappings();
            foreach (var entry in CustomMappingDictionary)
            {
                AnswersMappingDictionary.Add(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Get all mappings shortcut strings
        /// </summary>
        /// <returns>Answers mapping list</returns>
        public static List<string> GetAnswersMappings()
        {
            return new List<string>(AnswersMappingDictionary.Keys);
        }

        /// <summary>
        /// Get full mapping values by provided shortcut
        /// </summary>
        /// <param name="key">Shortcut string</param>
        /// <returns>Mapping values</returns>
        public static string[] GetMappingByKey(string key)
        {
            return AnswersMappingDictionary.First(x => x.Key == key).Value;
        }

        /// <summary>
        /// Checks if mapping with specified key already exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if mapping exists, false otherwise</returns>
        public static bool CheckMappingExists(string key)
        {
            return AnswersMappingDictionary.ContainsKey(key);
        }

        /// <summary>
        /// Add custom mapping and save settings
        /// </summary>
        /// <param name="key">Shortcut string</param>
        /// <param name="mappingValues">Mapping values</param>
        public static void AddCustomMapping(string key, string[] mappingValues)
        {
            AnswersMappingDictionary.Add(key, mappingValues);
            CustomMappingDictionary.Add(key, mappingValues);
            UserSettingsUtility.SaveCustomMappings(CustomMappingDictionary);
        }
    }
}
