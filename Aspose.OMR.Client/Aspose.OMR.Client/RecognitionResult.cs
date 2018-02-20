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
namespace Aspose.OMR.Client
{
    /// <summary>
    /// Represent recognition result info for single question
    /// </summary>
    public class RecognitionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecognitionResult"/> class.
        /// </summary>
        /// <param name="questionName">The question name</param>
        /// <param name="answerKey">The answer key</param>
        public RecognitionResult(string questionName, string answerKey)
        {
            this.QuestionName = questionName;
            this.AnswerKey = answerKey;
        }

        /// <summary>
        /// Gets or sets the question name
        /// </summary>
        public string QuestionName { get; set; }

        /// <summary>
        /// Gets or sets the answer key
        /// </summary>
        public string AnswerKey { get; set; }
    }
}
