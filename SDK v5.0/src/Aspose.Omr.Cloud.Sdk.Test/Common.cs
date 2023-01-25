/*
 * Copyright (C) 2023 Aspose Pty Ltd. All Rights Reserved.
 *
 * Licensed under the MIT License (hereinafter the "License");
 * you may not use this file except in accordance with the License.
 * You can obtain a copy of the License at
 *
 *      https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.IO;
using Newtonsoft.Json.Linq;

class Common
{
    static readonly string demoDataSubmoduleName = "aspose-omr-cloud-demo-data";
    static readonly string configFileName = "test_config.json";
    static readonly string basePath = "";
    static readonly string DataFolderName = "Data";
    static readonly string ResultFolderName = "Temp";
    static JObject Config = null;

    static Common()
    {
        var current = Directory.GetParent(NUnit.Framework.TestContext.CurrentContext.TestDirectory);
        string configFileRelativePath = Path.Combine(Common.demoDataSubmoduleName, configFileName);
        while (current != null && !File.Exists(Path.Combine(current.FullName, configFileRelativePath)))
        {
            current = current.Parent;
        }
        basePath = Path.Combine(current.FullName, demoDataSubmoduleName);
    }

    public static string GetDataFolderDir()
    {
        return Path.Combine(basePath, DataFolderName);
    }

    public static string GetResultFolderDir()
    {
        return Path.Combine(basePath, ResultFolderName);
    }

    public static string GetURL()
    {
        string configFilePath = Path.Combine(basePath, configFileName);
        Common.Config = JObject.Parse(File.ReadAllText(configFilePath));
        return Config["base_path"].ToString();
    }
}
