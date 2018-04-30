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
namespace Aspose.OMR.Client.UndoRedo
{
    using System.Collections.Generic;
    using ViewModels;

    /// <summary>
    /// Represents action that aligns questions
    /// </summary>
    public class TrackAlignAction : IUndoRedoAction
    {
        private List<BaseQuestionViewModel> trackedElements;
        private List<double> leftChanges;
        private List<double> topChanges;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackAlignAction"/> class
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="leftChanges">List of changes to left value of each changed question</param>
        /// <param name="topChanges">List of changes to top value of each changed question</param>
        public TrackAlignAction(List<BaseQuestionViewModel> trackedElements, List<double> leftChanges, List<double> topChanges)
        {
            this.trackedElements = trackedElements;
            this.leftChanges = leftChanges;
            this.topChanges = topChanges;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < this.trackedElements.Count; i++)
            {
                if (this.leftChanges != null)
                {
                    this.trackedElements[i].Left -= this.leftChanges[i];
                }
                if (this.topChanges != null)
                {
                    this.trackedElements[i].Top -= this.topChanges[i];
                }
            }
        }

        /// <summary>
        /// Unexecute action, restore original state
        /// </summary>
        public void UnExecute()
        {
            for (int i = 0; i < this.trackedElements.Count; i++)
            {
                if (this.leftChanges != null)
                {
                    this.trackedElements[i].Left += this.leftChanges[i];
                }
                if (this.topChanges != null)
                {
                    this.trackedElements[i].Top += this.topChanges[i];
                }
            }
        }
    }
}
