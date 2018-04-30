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
    /// Represents action that changes bubbles count in choice box
    /// </summary>
    public class ChangeBubblesCountAction : IUndoRedoAction
    {
        private ChoiceBoxViewModel choiceBox;
        private List<BubbleViewModel> bubblesBefore;
        private List<BubbleViewModel> bubblesAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceBoxViewModel"/> class
        /// </summary>
        /// <param name="choiceBox">Changed choice box question</param>
        /// <param name="bubblesBefore">Child bubbles before the change</param>
        /// <param name="bubblesAfter">Child bubbles after the change</param>
        public ChangeBubblesCountAction(ChoiceBoxViewModel choiceBox, List<BubbleViewModel> bubblesBefore, List<BubbleViewModel> bubblesAfter)
        {
            this.choiceBox = choiceBox;
            this.bubblesBefore = bubblesBefore;
            this.bubblesAfter = bubblesAfter;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            this.choiceBox.Bubbles.Clear();
            this.choiceBox.AddBubbles(this.bubblesAfter);
        }

        /// <summary>
        /// Unexecute action, restore original bubbles
        /// </summary>
        public void UnExecute()
        {
            this.choiceBox.Bubbles.Clear();
            this.choiceBox.AddBubbles(this.bubblesBefore);
        }
    }
}
