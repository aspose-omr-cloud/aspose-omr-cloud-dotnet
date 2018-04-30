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
    /// Represents action that shrinks questions
    /// </summary>
    public class TrackShrinkAction :IUndoRedoAction
    {
        private List<BaseQuestionViewModel> copiesAfter;
        private List<BaseQuestionViewModel> questions;
        private List<BaseQuestionViewModel> copiesBefore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackShrinkAction"/> class
        /// </summary>
        /// <param name="questions">Changed questions</param>
        /// <param name="copiesBefore">Copies of the questions before the change</param>
        /// <param name="copiesAfter">Copies of the questions after the change</param>
        public TrackShrinkAction(List<BaseQuestionViewModel> questions, List<BaseQuestionViewModel> copiesBefore, List<BaseQuestionViewModel> copiesAfter)
        {
            this.questions = questions;
            this.copiesBefore = copiesBefore;
            this.copiesAfter = copiesAfter;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            this.ChangeOriginalQuestions(this.copiesAfter);
        }

        /// <summary>
        /// Unexecute action, restore original state
        /// </summary>
        public void UnExecute()
        {
            this.ChangeOriginalQuestions(this.copiesBefore);
        }

        /// <summary>
        /// Apply changes to questions
        /// </summary>
        /// <param name="models">Model to use to restore questions states</param>
        private void ChangeOriginalQuestions(List<BaseQuestionViewModel> models)
        {
            for (int i = 0; i < this.questions.Count; i++)
            {
                if (this.questions[i] is ChoiceBoxViewModel)
                {
                    ChoiceBoxViewModel choiceBox = (ChoiceBoxViewModel)this.questions[i];
                    ChoiceBoxViewModel target = (ChoiceBoxViewModel)models[i];
                    this.ApplyChanges(choiceBox, target);
                }
                else if (this.questions[i] is GridViewModel)
                {
                    GridViewModel grid = (GridViewModel)this.questions[i];
                    GridViewModel target = (GridViewModel)models[i];
                    this.ApplyChanges(grid, target);
                }
            }
        }

        /// <summary>
        /// Apply changes for specific choice box 
        /// </summary>
        /// <param name="source">Item to change</param>
        /// <param name="target">Target item</param>
        private void ApplyChanges(ChoiceBoxViewModel source, ChoiceBoxViewModel target)
        {
            for (var i = 0; i < source.Bubbles.Count; i++)
            {
                source.Bubbles[i].Top = target.Bubbles[i].Top;
                source.Bubbles[i].Left = target.Bubbles[i].Left;
            }

            source.Top = target.Top;
            source.Left = target.Left;
            source.Width = target.Width;
            source.Height = target.Height;
        }

        /// <summary>
        /// Apply changes for specific grid
        /// </summary>
        /// <param name="source">Item to change</param>
        /// <param name="target">Target item</param>
        private void ApplyChanges(GridViewModel source, GridViewModel target)
        {
            for (var i = 0; i < source.ChoiceBoxes.Count; i++)
            {
                this.ApplyChanges(source.ChoiceBoxes[i], target.ChoiceBoxes[i]);
            }

            source.Top = target.Top;
            source.Left = target.Left;
            source.Width = target.Width;
            source.Height = target.Height;
        }
    }
}
