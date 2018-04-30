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
    /// Represents action that changes grid options count
    /// </summary>
    public class ChangeGridOptionsCountAction : IUndoRedoAction
    {
        private GridViewModel gridViewModel;
        private List<List<BubbleViewModel>> bubblesBefore;
        private List<List<BubbleViewModel>> bubblesAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeGridOptionsCountAction"/> class
        /// </summary>
        /// <param name="gridViewModel">Changed grid view model</param>
        /// <param name="bubblesBefore">Bubbles in child choice boxes before the change</param>
        /// <param name="bubblesAfter">Bubbles in child choice boxes after the change</param>
        public ChangeGridOptionsCountAction(GridViewModel gridViewModel, List<List<BubbleViewModel>> bubblesBefore, List<List<BubbleViewModel>> bubblesAfter)
        {
            this.gridViewModel = gridViewModel;
            this.bubblesBefore = bubblesBefore;
            this.bubblesAfter = bubblesAfter;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            this.gridViewModel.UpdateChoiceBoxesBubbles(this.bubblesAfter);
        }

        /// <summary>
        /// Unexecute action, restore original choices count
        /// </summary>
        public void UnExecute()
        {
            this.gridViewModel.UpdateChoiceBoxesBubbles(this.bubblesBefore);
        }
    }
}
