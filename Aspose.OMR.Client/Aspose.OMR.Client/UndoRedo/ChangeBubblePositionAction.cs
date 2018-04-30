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
    /// Represents action that changes bubbles position
    /// </summary>
    public class ChangeBubblePositionAction : IUndoRedoAction
    {
        private List<BubbleViewModel> bubbles;
        private double topChange;
        private double leftChange;
        private double widthKoef;
        private double heightKoef;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeBubblePositionAction"/> class
        /// </summary>
        /// <param name="trackedElements">Changed bubbles</param>
        /// <param name="topChange">Top position change</param>
        /// <param name="leftChange">Left position change</param>
        /// <param name="widthKoef">Koefficient of width change</param>
        /// <param name="heightKoef">Koefficient of height change</param>
        public ChangeBubblePositionAction(List<BubbleViewModel> trackedElements, double topChange, double leftChange, double widthKoef, double heightKoef)
        {
            this.bubbles = trackedElements;
            this.topChange = topChange;
            this.leftChange = leftChange;
            this.widthKoef = widthKoef;
            this.heightKoef = heightKoef;
        }

        /// <summary>
        /// Execute action, apply changes to bubbles
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < this.bubbles.Count; i++)
            {
                this.bubbles[i].Top -= this.topChange;
                this.bubbles[i].Left -= this.leftChange;
                this.bubbles[i].Width /= this.widthKoef;
                this.bubbles[i].Height /= this.heightKoef;
            }
        }

        /// <summary>
        /// Unexecute action, return bubbules to original state
        /// </summary>
        public void UnExecute()
        {
            for (int i = 0; i < this.bubbles.Count; i++)
            {
                this.bubbles[i].Top += this.topChange;
                this.bubbles[i].Left += this.leftChange;
                this.bubbles[i].Width *= this.widthKoef;
                this.bubbles[i].Height *= this.heightKoef;
            }
        }
    }
}
