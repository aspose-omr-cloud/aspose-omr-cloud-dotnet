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
using System.Collections.Generic;
using Aspose.OMR.Client.ViewModels;
using Aspose.OMR.TemplateModel;

namespace Aspose.OMR.Client.UndoRedo
{
    /// <summary>
    /// Represents action that changes grid orientation
    /// </summary>
    public class ChangeGridOrientationAction : IUndoRedoAction
    {
        private GridViewModel gridViewModel;
        private Orientations orientationBefore;
        private Orientations orientationAfter;
        private List<ChoiceBoxViewModel> choiceBoxesBefore;
        private List<ChoiceBoxViewModel> choiceBoxesAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeGridOrientationAction"/> class
        /// </summary>
        /// <param name="gridViewModel">Changed grid view model</param>
        /// <param name="orientationBefore">Orientation value before the change</param>
        /// <param name="orientationAfter">Orientation value after the change</param>
        /// <param name="choiceBoxesBefore">Child choice boxes before the change</param>
        /// <param name="choiceBoxesAfter">Child choice boxes after the change</param>
        public ChangeGridOrientationAction(GridViewModel gridViewModel, Orientations orientationBefore, Orientations orientationAfter, List<ChoiceBoxViewModel> choiceBoxesBefore, List<ChoiceBoxViewModel> choiceBoxesAfter)
        {
            this.gridViewModel = gridViewModel;
            this.orientationBefore = orientationBefore;
            this.orientationAfter = orientationAfter;
            this.choiceBoxesBefore = choiceBoxesBefore;
            this.choiceBoxesAfter = choiceBoxesAfter;
        }

        /// <summary>
        /// Execute action, apply changes
        /// </summary>
        public void Execute()
        {
            this.gridViewModel.ChoiceBoxes.Clear();
            this.gridViewModel.AddChoiceBoxes(this.choiceBoxesAfter, this.orientationAfter);
        }

        /// <summary>
        /// Unexecute action, restore original orientation and choice boxes
        /// </summary>
        public void UnExecute()
        {
            this.gridViewModel.ChoiceBoxes.Clear();
            this.gridViewModel.AddChoiceBoxes(this.choiceBoxesBefore, this.orientationBefore);
        }
    }
}
