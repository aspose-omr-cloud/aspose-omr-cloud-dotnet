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
    /// Represents apply formatting action
    /// </summary>
    public class ApplyFormattingAction : IUndoRedoAction
    {
        private List<BaseQuestionViewModel> questions;
        private List<BaseQuestionViewModel> copiesBefore;
        private BaseQuestionViewModel ethalon;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyFormattingAction"/> class
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="copiesBefore">Copies of questions before the change</param>
        /// <param name="ethalon">Target ethalon question</param>
        public ApplyFormattingAction(List<BaseQuestionViewModel> trackedElements, List<BaseQuestionViewModel> copiesBefore, BaseQuestionViewModel ethalon)
        {
            this.questions = trackedElements;
            this.copiesBefore = copiesBefore;
            this.ethalon = ethalon;
        }

        /// <summary>
        /// Execute question, change all question to look like target ethalon
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < this.questions.Count; i++)
            {
                this.ChangeOriginalQuestions(i, this.ethalon);
            }
        }

        /// <summary>
        /// Unexecute action, return all questions to their original look
        /// </summary>
        public void UnExecute()
        {
            for (int i = 0; i < this.questions.Count; i++)
            {
                this.ChangeOriginalQuestions(i, this.copiesBefore[i]);
            }
        }

        private void ChangeOriginalQuestions(int i, BaseQuestionViewModel ethalonQuestion)
        {
            if (this.questions[i] is ChoiceBoxViewModel)
            {
                ChoiceBoxViewModel choiceBox = (ChoiceBoxViewModel)this.questions[i];
                ChoiceBoxViewModel target = (ChoiceBoxViewModel)ethalonQuestion;
                this.ApplyChanges(choiceBox, target);
            }
            else if (this.questions[i] is GridViewModel)
            {
                GridViewModel grid = (GridViewModel)this.questions[i];
                GridViewModel target = (GridViewModel)ethalonQuestion;
                this.ApplyChanges(grid, target);
            }
        }


        //TODO
        private void ApplyChanges(ChoiceBoxViewModel source, ChoiceBoxViewModel target)
        {
            for (var i = 0; i < source.Bubbles.Count; i++)
            {
                source.Bubbles[i].Top = target.Bubbles[i].Top;
                source.Bubbles[i].Left = target.Bubbles[i].Left;
                source.Bubbles[i].Width = target.Bubbles[i].Width;
                source.Bubbles[i].Height = target.Bubbles[i].Height;
                source.Bubbles[i].Name = target.Bubbles[i].Name;
            }

            source.Top = target.Top;
            source.Left = target.Left;
            source.Width = target.Width;
            source.Height = target.Height;
            source.SelectedMapping = target.SelectedMapping;
        }

        private void ApplyChanges(GridViewModel source, GridViewModel target)
        {
            for (var i = 0; i < source.ChoiceBoxes.Count; i++)
            {
                this.ApplyChanges(source.ChoiceBoxes[i], target.ChoiceBoxes[i]);
            }

            source.Width = target.Width;
            source.Height = target.Height;
            source.SelectedMapping = target.SelectedMapping;
        }
    }
}
