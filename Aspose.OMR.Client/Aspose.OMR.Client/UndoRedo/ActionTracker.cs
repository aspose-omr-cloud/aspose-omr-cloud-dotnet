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
    using System.Collections.Generic;

    /// <summary>
    /// Stores undo and redo actions and handles action tracking
    /// </summary>
    public static class ActionTracker
    {
        /// <summary>
        /// Max amount of commands stored
        /// </summary>
        private static readonly int Capacity = 100;

        /// <summary>
        /// List of Undo commands
        /// Linked list is used instead of stack to be able to remove oldest action if we reached capacity max
        /// </summary>
        private static readonly LinkedList<IUndoRedoAction> UndoActions = new LinkedList<IUndoRedoAction>();

        /// <summary>
        /// List of Redo commands
        /// Linked list is used instead of stack to be able to remove oldest action if we reached capacity max
        /// </summary>
        private static readonly LinkedList<IUndoRedoAction> RedoActions = new LinkedList<IUndoRedoAction>();

        /// <summary>
        /// Shows if a Redo operation can be executed
        /// </summary>
        /// <returns>True if can Redo, false otherwise</returns>
        public static bool CanRedo()
        {
            return RedoActions.Count > 0;
        }

        /// <summary>
        /// Shows if a Undo operation can be executed
        /// </summary>
        /// <returns>True if can Undo, false otherwise</returns>
        public static bool CanUndo()
        {
            return UndoActions.Count > 0;
        }

        /// <summary>
        /// Redoes commands
        /// </summary>
        /// <param name="levels">number of commands to redo</param>
        public static void Redo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (RedoActions.Count != 0)
                {
                    IUndoRedoAction command = RedoActions.Last.Value;
                    RedoActions.RemoveLast();
                    command.Execute();

                    if (UndoActions.Count >= Capacity)
                    {
                        UndoActions.RemoveFirst();
                    }

                    UndoActions.AddLast(command);
                }
            }
        }

        /// <summary>
        /// Undoes commands
        /// </summary>
        /// <param name="levels">number of commands to undo</param>
        public static void Undo(int levels)
        {
            for (int i = 0; i < levels; i++)
            {
                if (UndoActions.Count != 0)
                {
                    IUndoRedoAction command = UndoActions.Last.Value;
                    UndoActions.RemoveLast();
                    command.UnExecute();

                    if (RedoActions.Count >= Capacity)
                    {
                        RedoActions.RemoveFirst();
                    }

                    RedoActions.AddLast(command);
                }
            }
        }

        /// <summary>
        /// Clears Undo and Redo commands lists
        /// </summary>
        public static void ClearCommands()
        {
            UndoActions.Clear();
            RedoActions.Clear();
        }

        /// <summary>
        /// Tracks action
        /// </summary>
        /// <param name="action">Action to track</param>
        public static void TrackAction(IUndoRedoAction action)
        {
            if (UndoActions.Count >= Capacity)
            {
                UndoActions.RemoveFirst();
            }

            UndoActions.AddLast(action);
            RedoActions.Clear();
        }
    }
}
