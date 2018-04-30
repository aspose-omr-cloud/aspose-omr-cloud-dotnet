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
    using System;
    using System.Collections.Generic;
    using ViewModels;

    /// <summary>
    /// Represents actions that changes question position
    /// </summary>
    public class ChangeQuestionPositionAction : IUndoRedoAction
    {
        private List<BaseQuestionViewModel> selectedItems;
        private double topChange;
        private double leftChange;

        private double widthKoef;
        private double heightKoef;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeQuestionPositionAction"/> class
        /// </summary>
        /// <param name="selectedItems">Changed questions</param>
        /// <param name="topChange">Top position change</param>
        /// <param name="leftChange">Left position change</param>
        /// <param name="widthKoef">Koefficient of width change</param>
        /// <param name="heightKoef">Koefficient of height change</param>
        public ChangeQuestionPositionAction(List<BaseQuestionViewModel> selectedItems, double topChange, double leftChange, double widthKoef, double heightKoef)
        {
            this.selectedItems = selectedItems;
            this.topChange = topChange;
            this.leftChange = leftChange;
            this.widthKoef = widthKoef;
            this.heightKoef = heightKoef;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < this.selectedItems.Count; i++)
            {
                this.selectedItems[i].Top -= this.topChange;
                this.selectedItems[i].Left -= this.leftChange;

                if (Math.Abs(this.widthKoef) > 0.01 || Math.Abs(this.heightKoef) > 0.01)
                {
                    this.ProcessQuestion(i, this.Divide);
                }
            }
        }

        /// <summary>
        /// Unexecute action, restore original state
        /// </summary>
        public void UnExecute()
        {
            for (int i = 0; i < this.selectedItems.Count; i++)
            {
                this.selectedItems[i].Top += this.topChange;
                this.selectedItems[i].Left += this.leftChange;

                if (Math.Abs(this.widthKoef) > 0.01 || Math.Abs(this.heightKoef) > 0.01)
                {
                    this.ProcessQuestion(i, this.Multiply);
                }
            }
        }

        /// <summary>
        /// Apply changes to question
        /// </summary>
        /// <param name="i">Index of the question to process</param>
        /// <param name="op">Operation to apply</param>
        private void ProcessQuestion(int i, Func<double, double, double> op)
        {
            this.selectedItems[i].Width = op(this.selectedItems[i].Width, this.widthKoef);
            this.selectedItems[i].Height = op(this.selectedItems[i].Height, this.heightKoef);

            if (this.selectedItems[i] is ChoiceBoxViewModel)
            {
                this.ResizeBubbles((ChoiceBoxViewModel)this.selectedItems[i], op);
            }
            else if (this.selectedItems[i] is GridViewModel)
            {
                GridViewModel grid = (GridViewModel)this.selectedItems[i];

                for (int j = 0; j < grid.ChoiceBoxes.Count; j++)
                {
                    ChoiceBoxViewModel childChoiceBox = grid.ChoiceBoxes[j];

                    childChoiceBox.Width = op(childChoiceBox.Width, this.widthKoef);
                    childChoiceBox.Height = op(childChoiceBox.Height, this.heightKoef);
                    childChoiceBox.Top = op(childChoiceBox.Top, this.heightKoef);
                    childChoiceBox.Left = op(childChoiceBox.Left, this.widthKoef);

                    this.ResizeBubbles(childChoiceBox, op);
                }
            }
        }

        /// <summary>
        /// Resize child bubbles
        /// </summary>
        /// <param name="choiceBox">Parent choice box</param>
        /// <param name="op">Operaion that needs to be applied</param>
        private void ResizeBubbles(ChoiceBoxViewModel choiceBox, Func<double, double, double> op)
        {
            for (int j = 0; j < choiceBox.BubblesCount; j++)
            {
                choiceBox.Bubbles[j].Width = op(choiceBox.Bubbles[j].Width, this.widthKoef);
                choiceBox.Bubbles[j].Height = op(choiceBox.Bubbles[j].Height, this.heightKoef);
                choiceBox.Bubbles[j].Top = op(choiceBox.Bubbles[j].Top, this.heightKoef);
                choiceBox.Bubbles[j].Left = op(choiceBox.Bubbles[j].Left, this.widthKoef);
            }
        }

        /// <summary>
        /// Multiply method
        /// </summary>
        private double Multiply(double x, double y)
        {
            return x * y;
        }

        /// <summary>
        /// Divide method
        /// </summary>
        private double Divide(double x, double y)
        {
            return x / y;
        }
    }
}
