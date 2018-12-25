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
namespace Aspose.OMR.Client.Controls
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Views;

    /// <summary>
    /// Contains helper methods for controls and visual tree navigation
    /// </summary>
    public static class ControlHelper
    {
        /// <summary>
        /// Find parent CustomCanvas in visual tree
        /// </summary>
        /// <param name="initial">Initial control</param>
        /// <returns>Custom canvas</returns>
        public static CustomCanvas FindParentCanvas(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                if (current is Visual)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    current = LogicalTreeHelper.GetParent(current);
                }

                result = current;
                if (result is CustomCanvas)
                {
                    break;
                }
            }

            return (CustomCanvas)result;
        }

        /// <summary>
        /// Find parent choice box control
        /// </summary>
        /// <param name="initial">Initial control</param>
        /// <returns>Parent choice box</returns>
        public static OmrChoiceBox FindParentChoiceBox(DependencyObject initial)
        {
            DependencyObject current = initial;
            DependencyObject result = initial;

            while (current != null)
            {
                if (current is Visual)
                {
                    current = VisualTreeHelper.GetParent(current);
                }
                else
                {
                    current = LogicalTreeHelper.GetParent(current);
                }

                result = current;
                if (result is OmrChoiceBox)
                {
                    break;
                }
            }

            return (OmrChoiceBox)result;
        }

        /// <summary>
        /// Find child of specified type in visual tree
        /// </summary>
        /// <typeparam name="T">Dependency object</typeparam>
        /// <param name="parent">Starting point</param>
        /// <param name="childName">Name of child control if any</param>
        /// <returns></returns>
        public static T FindChild<T>(this DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// Find canvas childs as content presenters list
        /// </summary>
        /// <param name="canvas">Searched canvas</param>
        /// <returns>List of content presenters</returns>
        public static List<ContentPresenter> GetChildPresenters(CustomCanvas canvas)
        {
            List<ContentPresenter> presenters = new List<ContentPresenter>();
            int childsCount = VisualTreeHelper.GetChildrenCount(canvas);

            for (int i = 0; i < childsCount; i++)
            {
                presenters.Add(VisualTreeHelper.GetChild(canvas, i) as ContentPresenter);
            }

            return presenters;
        }

        /// <summary>
        /// Finds all selected base omr elements inside canvas
        /// </summary>
        /// <param name="canvas">Canvas containing searched items</param>
        /// <returns>List of content presenters with selected items</returns>
        public static List<ContentPresenter> GetSelectedChildPresenters(CustomCanvas canvas)
        {
            List<ContentPresenter> presenters = new List<ContentPresenter>();
            int childsCount = VisualTreeHelper.GetChildrenCount(canvas);

            for (int i = 0; i < childsCount; i++)
            {
                ContentPresenter childPresenter = (ContentPresenter) VisualTreeHelper.GetChild(canvas, i);
                BaseOmrElement childOmrItem = VisualTreeHelper.GetChild(childPresenter, 0) as BaseOmrElement;

                if (childOmrItem != null && childOmrItem.IsSelected)
                {
                    presenters.Add(childPresenter);
                }
            }

            return presenters;
        }

        /// <summary>
        /// Finds all unselected child presenters on parent canvas
        /// </summary>
        /// <param name="canvas">Canvas containing searched items</param>
        /// <returns>List of content presenters with unselected items</returns>
        public static List<ContentPresenter> GetUnselectedChildPresenters(CustomCanvas canvas)
        {
            List<ContentPresenter> presenters = new List<ContentPresenter>();
            int childsCount = VisualTreeHelper.GetChildrenCount(canvas);

            for (int i = 0; i < childsCount; i++)
            {
                ContentPresenter childPresenter = (ContentPresenter)VisualTreeHelper.GetChild(canvas, i);
                BaseOmrElement childOmrItem = VisualTreeHelper.GetChild(childPresenter, 0) as BaseOmrElement;

                if (childOmrItem != null && !childOmrItem.IsSelected)
                {
                    presenters.Add(childPresenter);
                }
            }

            return presenters;
        }

        /// <summary>
        /// Hide all views excluding main window to enable busy indication
        /// </summary>
        public static void HideAllChildViews()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is MainWindow)
                {
                    continue;
                }

                win.Hide();
            }
        }

        /// <summary>
        /// Restore all hidden views
        /// </summary>
        public static void RestoreHidderChildViews()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win is MainWindow)
                {
                    continue;
                }

                if (win.Visibility == Visibility.Hidden)
                {
                    win.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
