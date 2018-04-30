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
namespace Aspose.OMR.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using TemplateModel;
    using Utility;

    /// <summary>
    /// Base view model for all types of questions
    /// </summary>
    public abstract class BaseQuestionViewModel : ViewModelBase
    {
        /// <summary>
        /// Border thickness of visual borders around questions and bubbles
        /// </summary>
        protected const double BorderThickness = 1.5;

        /// <summary>
        /// Pixels between question border and question's content
        /// </summary>
        protected const int BorderOffset = 3;

        /// <summary>
        /// Pixels between question's content
        /// </summary>
        protected const int ItemsOffset = 3;

        /// <summary>
        /// Element name
        /// </summary>
        private string name;

        /// <summary>
        /// Element width
        /// </summary>
        private double width;

        /// <summary>
        /// Element height
        /// </summary>
        private double height;

        /// <summary>
        /// Element top position on parent canvas
        /// </summary>
        private double top;

        /// <summary>
        /// Element left position on parent canvas
        /// </summary>
        private double left;

        /// <summary>
        /// Gets or sets the view model of the template containing this question
        /// </summary>
        public TemplateViewModel ParentTemplate { get; set; }

        /// <summary>
        /// Gets the collection that stores all answer mappings 'keys'
        /// This is items source in mappings drop-down menu
        /// </summary>
        public ObservableCollection<string> AnswersMapping
        {
            get { return new ObservableCollection<string>(AnswersMappingHelper.GetAnswersMappings()); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether question is selected
        /// </summary>
        public abstract bool IsSelected { get; set; }

        /// <summary>
        /// Gets a value indicating whether question is valid (i.e. contains valid child elements)
        /// </summary>
        public abstract bool IsValid { get; }

        /// <summary>
        /// Gets or sets the question orientation
        /// </summary>
        public abstract Orientations Orientation { get; set; }

        /// <summary>
        /// Gets or sets custom mapping command
        /// </summary>
        public RelayCommand AddCustomMappingCommand { get; set; }

        /// <summary>
        /// Gets or sets element name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element top position in canvas
        /// </summary>
        public double Top
        {
            get { return this.top; }
            set
            {
                this.top = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element left position in canvas
        /// </summary>
        public double Left
        {
            get { return this.left; }
            set
            {
                this.left = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element width
        /// </summary>
        public double Width
        {
            get { return this.width; }
            set
            {
                this.width = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets element Height
        /// </summary>
        public double Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Abstract method to create copy of the question
        /// </summary>
        /// <returns>Created copy</returns>
        public abstract BaseQuestionViewModel CreateCopy();

        /// <summary>
        /// Abstract method to shrink element's area to it's child content
        /// </summary>
        public abstract void Shrink();
    }
}
