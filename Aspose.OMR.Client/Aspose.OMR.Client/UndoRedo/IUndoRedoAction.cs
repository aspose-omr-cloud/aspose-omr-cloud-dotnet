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
    /// <summary>
    /// Represents undoable action 
    /// </summary>
    public interface IUndoRedoAction
    {
        /// <summary>
        /// Performs action that is specified for the command (i.e. "direct" action)
        /// </summary>
        void Execute();

        /// <summary>
        /// Performs undo action that is specified for the command (i.e. "reversed" action)
        /// </summary>
        void UnExecute();
    }
}
