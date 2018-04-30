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
    using System.Collections.ObjectModel;
    using ViewModels;

    /// <summary>
    /// Represents action that adds questions to the page
    /// </summary>
    public class AddElementsAction : IUndoRedoAction
    {
        /// <summary>
        /// Processed questions
        /// </summary>
        private readonly BaseQuestionViewModel[] elements;

        /// <summary>
        /// Collection of page questions
        /// </summary>
        private readonly ObservableCollection<BaseQuestionViewModel> pageElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddElementsAction"/> class
        /// </summary>
        /// <param name="addedElements">Questions to add/remove</param>
        /// <param name="pageElements">Page questions collection</param>
        public AddElementsAction(BaseQuestionViewModel[] addedElements, ObservableCollection<BaseQuestionViewModel> pageElements)
        {
            this.elements = addedElements;
            this.pageElements = pageElements;
        }

        /// <summary>
        /// Execute action, add questions to the page
        /// </summary>
        public void Execute()
        {
            foreach (var elementViewModel in this.elements)
            {
                this.pageElements.Add(elementViewModel);
            }
        }

        /// <summary>
        /// Unexecute action, remove questions from page
        /// </summary>
        public void UnExecute()
        {
            foreach (var elementViewModel in this.elements)
            {
                elementViewModel.IsSelected = false;
                this.pageElements.Remove(elementViewModel);
            }
        }
    }
}
