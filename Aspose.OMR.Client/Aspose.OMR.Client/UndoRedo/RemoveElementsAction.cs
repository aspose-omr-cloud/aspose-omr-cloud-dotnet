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
namespace Aspose.OMR.Client.UndoRedo
{
    using System.Collections.ObjectModel;
    using ViewModels;

    /// <summary>
    /// Represents action that removes questions from page
    /// </summary>
    public class RemoveElementsAction : IUndoRedoAction
    {
        /// <summary>
        /// Processed questions
        /// </summary>
        private readonly BaseQuestionViewModel[] elements;

        /// <summary>
        /// Collection of page questions
        /// </summary>
        private readonly ObservableCollection<BaseQuestionViewModel> pageQuestions;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveElementsAction"/> class
        /// </summary>
        /// <param name="addedElements">Questions to remove/restore</param>
        /// <param name="pageElements">Page questions collection</param>
        public RemoveElementsAction(BaseQuestionViewModel[] addedElements, ObservableCollection<BaseQuestionViewModel> pageElements)
        {
            this.elements = addedElements;
            this.pageQuestions = pageElements;
        }

        /// <summary>
        /// Execute action, remove questions
        /// </summary>
        public void Execute()
        {
            foreach (var elementViewModel in this.elements)
            {
                this.pageQuestions.Remove(elementViewModel);
            }
        }

        /// <summary>
        /// Unexecute action, restore questions
        /// </summary>
        public void UnExecute()
        {
            foreach (var elementViewModel in this.elements)
            {
                this.pageQuestions.Add(elementViewModel);
            }
        }
    }
}
