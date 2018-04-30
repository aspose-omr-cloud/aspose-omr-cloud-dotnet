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
    using TemplateModel;

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
        /// Adds action to Undo stack
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

        /// <summary>
        /// Tracks changes to questions positions
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="topChange">Top position change</param>
        /// <param name="leftChange">Left position change</param>
        /// <param name="widthKoef">Koefficient of width change</param>
        /// <param name="heightKoef">Koefficient of height change</param>
        public static void TrackChangeQuestionsPosition(List<BaseQuestionViewModel> trackedElements, double topChange, double leftChange, double widthKoef, double heightKoef)
        {
            var action = new ChangeQuestionPositionAction(trackedElements, topChange, leftChange, widthKoef, heightKoef);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks changes to bubbles properties
        /// </summary>
        /// <param name="trackedElements">Changed bubbles</param>
        /// <param name="topChange">Top position change</param>
        /// <param name="leftChange">Left position change</param>
        /// <param name="widthKoef">Koefficient of width change</param>
        /// <param name="heightKoef">Koefficient of height change</param>
        public static void TrackChangeBubble(List<BubbleViewModel> trackedElements, double topChange, double leftChange, double widthKoef, double heightKoef)
        {
            ChangeBubblePositionAction action = new ChangeBubblePositionAction(trackedElements, topChange, leftChange, widthKoef, heightKoef);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks questions shrink action
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="copiesBefore">Copies of the questions before the change</param>
        /// <param name="copiesAfter">Copies of the questions after the change</param>
        public static void TrackShrink(List<BaseQuestionViewModel> trackedElements, List<BaseQuestionViewModel> copiesBefore, List<BaseQuestionViewModel> copiesAfter)
        {
            TrackShrinkAction action = new TrackShrinkAction(trackedElements, copiesBefore, copiesAfter);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks questions alignment
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="leftChanges">List of changes to left value of each changed question</param>
        /// <param name="topChanges">List of changes to top value of each changed question</param>
        public static void TrackAlign(List<BaseQuestionViewModel> trackedElements, List<double> leftChanges, List<double> topChanges)
        {
            TrackAlignAction action = new TrackAlignAction(trackedElements, leftChanges, topChanges);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks apply formatting action
        /// </summary>
        /// <param name="trackedElements">Changed questions</param>
        /// <param name="copiesBefore">Copies of questions before the change</param>
        /// <param name="ethalon">Target ethalon question</param>
        public static void TrackApplyFormatting(List<BaseQuestionViewModel> trackedElements, List<BaseQuestionViewModel> copiesBefore, BaseQuestionViewModel ethalon)
        {
            ApplyFormattingAction action = new ApplyFormattingAction(trackedElements, copiesBefore, ethalon);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks changes to bubbles count in choice box question
        /// </summary>
        /// <param name="choiceBox">Changed choice box question</param>
        /// <param name="bubblesBefore">Child bubbles before the change</param>
        /// <param name="bubblesAfter">Child bubbles after the change</param>
        public static void TrackChangeBubblesCount(ChoiceBoxViewModel choiceBox, List<BubbleViewModel> bubblesBefore, List<BubbleViewModel> bubblesAfter)
        {
            ChangeBubblesCountAction action = new ChangeBubblesCountAction(choiceBox, bubblesBefore, bubblesAfter);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks changes to options count in grid question
        /// </summary>
        /// <param name="gridViewModel">Changed grid view model</param>
        /// <param name="bubblesBefore">Bubbles in child choice boxes before the change</param>
        /// <param name="bubblesAfter">Bubbles in child choice boxes after the change</param>
        public static void TrackChangeOptionsCount(GridViewModel gridViewModel, List<List<BubbleViewModel>> bubblesBefore, List<List<BubbleViewModel>> bubblesAfter)
        {
            ChangeGridOptionsCountAction action = new ChangeGridOptionsCountAction(gridViewModel, bubblesBefore, bubblesAfter);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks changes to sections count in grid question
        /// </summary>
        /// <param name="gridViewModel">Changed grid view model</param>
        /// <param name="choiceBoxesBefore">Child choice boxes before the change</param>
        /// <param name="choiceBoxesAfter">Child choice boxes after the change</param>
        public static void TrackChangeSectionsCount(GridViewModel gridViewModel, List<ChoiceBoxViewModel> choiceBoxesBefore, List<ChoiceBoxViewModel> choiceBoxesAfter)
        {
            ChangeGridSectionsCountAction action = new ChangeGridSectionsCountAction(gridViewModel, choiceBoxesBefore, choiceBoxesAfter);
            TrackAction(action);
        }

        /// <summary>
        /// Tracks orientation change in grid question
        /// </summary>
        /// <param name="gridViewModel">Changed grid view model</param>
        /// <param name="orientationBefore">Orientation value before the change</param>
        /// <param name="orientationAfter">Orientation value after the change</param>
        /// <param name="choiceBoxesBefore">Child choice boxes before the change</param>
        /// <param name="choiceBoxesAfter">Child choice boxes after the change</param>
        public static void TrackGridOrientationChange(GridViewModel gridViewModel, Orientations orientationBefore,
            Orientations orientationAfter, List<ChoiceBoxViewModel> choiceBoxesBefore,
            List<ChoiceBoxViewModel> choiceBoxesAfter)
        {
            ChangeGridOrientationAction action = new ChangeGridOrientationAction(gridViewModel, orientationBefore, orientationAfter, choiceBoxesBefore, choiceBoxesAfter);
            TrackAction(action);
        }
    }
}
