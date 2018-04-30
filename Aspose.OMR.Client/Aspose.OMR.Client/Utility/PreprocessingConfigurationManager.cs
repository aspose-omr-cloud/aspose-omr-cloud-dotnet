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
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Manages preprocessing configurations for templates
    /// </summary>
    public static class PreprocessingConfigurationManager
    {
        /// <summary>
        /// Configurations dictionary with (templateId, serialized configuration) format
        /// </summary>
        private static readonly Dictionary<string, string> StoredConfigs;

        /// <summary>
        /// Initializes static members of the <see cref="PreprocessingConfigurationManager"/> class.
        /// </summary>
        static PreprocessingConfigurationManager()
        {
            StoredConfigs = new Dictionary<string, string>();

            var loadedConfigs = UserSettingsUtility.LoadPreprocessingConfigs();
            foreach (var entry in loadedConfigs)
            {
                StoredConfigs.Add(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Get config by provided template id
        /// </summary>
        /// <param name="templateId">Template id</param>
        /// <returns>Preprocessing config</returns>
        public static string GetConfigByKey(string templateId)
        {
            return StoredConfigs.First(x => x.Key == templateId).Value;
        }

        /// <summary>
        /// Checks if config with specified key already exists
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>True if config exists, false otherwise</returns>
        public static bool CheckConfigExists(string key)
        {
            return StoredConfigs.ContainsKey(key);
        }

        /// <summary>
        /// Add config and save settings
        /// </summary>
        /// <param name="templateId">Template id</param>
        /// <param name="config">Serialized config</param>
        public static void AddConfig(string templateId, string config)
        {
            if (CheckConfigExists(templateId))
            {
                StoredConfigs[templateId] = config;
            }
            else
            {
                StoredConfigs.Add(templateId, config);
            }

            UserSettingsUtility.SavePreprocessingConfigs(StoredConfigs);
        }
    }
}
