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
namespace Aspose.OMR.Client.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using TemplateModel;
    using Utility;

    /// <summary>
    /// View model class for grid question
    /// </summary>
    public sealed class GridViewModel : BaseQuestionViewModel
    {
        /// <summary>
        /// Minimum bubble size in pixels
        /// </summary>
        private const int MinChoiceBoxSize = 20;

        /// <summary>
        /// Amount of bubbles used when creating new questions
        /// Updated when user manually changes bubbles count inside question
        /// </summary>
        private static int latestQuestionsCount = 4;

        /// <summary>
        /// Mapping value used when creating new questions
        /// Updated when user manually changed bubbles answers mapping
        /// </summary>
        private static int latestSelectedMapping;

        /// <summary>
        /// Maximum number of allowed bubbles inside question
        /// </summary>
        private readonly int maxAllowedSectionsCount = 50;

        /// <summary>
        /// Amount of options for each child choice box 
        /// </summary>
        private int optionsCount = 4;

        /// <summary>
        /// Indicates whether question is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Grid orientation
        /// </summary>
        private Orientations orientation;

        /// <summary>
        /// Collection of choiceboxes inside grid
        /// </summary>
        private ObservableCollection<ChoiceBoxViewModel> choiceBoxes;

        /// <summary>
        /// Selected mapping key in mapping selection drop-down menu
        /// </summary>
        private string selectedAnswersMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewModel"/> class
        /// </summary>
        /// <param name="name">Name of the grid question</param>
        /// <param name="area">Area of the question</param>
        public GridViewModel(string name, Rect area)
        {
            this.InitializeValues(name, area.Top, area.Left, area.Width, area.Height);

            this.Orientation = this.Width >= this.Height ? Orientations.Horizontal : Orientations.Vertical;
            this.SelectedMapping = this.AnswersMapping[latestSelectedMapping];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridViewModel"/> class.
        /// This constructor is used in copying and model convertions
        /// </summary>
        /// <param name="name">Question name</param>
        /// <param name="top">Top position on parent canvas</param>
        /// <param name="left">Left position on parent canvas</param>
        /// <param name="width">Question's width</param>
        /// <param name="height">Question's height</param>
        public GridViewModel(string name, double top, double left, double width, double height)
        {
            this.InitializeValues(name, top, left, width, height);
            this.SelectedMapping = this.AnswersMapping[latestSelectedMapping];
        }

        /// <summary>
        /// Gets or sets currently selected answer mapping
        /// This is selected value in mapping drop-down menu
        /// </summary>
        public string SelectedMapping
        {
            get { return this.selectedAnswersMapping; }
            set
            {
                this.selectedAnswersMapping = value;

                // remember selected value so that freshly created questions has latest mapping
                latestSelectedMapping = this.AnswersMapping.IndexOf(value);

                foreach (var choiceBox in this.ChoiceBoxes)
                {
                    choiceBox.SelectedMapping = value;
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the collection of choiceboxes inside grid
        /// </summary>
        public ObservableCollection<ChoiceBoxViewModel> ChoiceBoxes
        {
            get { return this.choiceBoxes; }
            set
            {
                this.choiceBoxes = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets question orientation
        /// </summary>
        public override Orientations Orientation
        {
            get { return this.orientation; }
            set
            {
                this.orientation = value;
                this.ChoiceBoxes.Clear();
                if (this.Orientation == Orientations.Horizontal)
                {
                    this.FitChoiceBoxesHorizontal(latestQuestionsCount);
                }
                else
                {
                    this.FitChoiceBoxesVertical(latestQuestionsCount);
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the amount of sections (i.e. choiceboxes) inside grid
        /// </summary>
        public int SectionsCount
        {
            get { return this.ChoiceBoxes.Count; }
            set
            {
                this.UpdateSectionsCount(value);
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the options count
        /// </summary>
        public int OptionsCount
        {
            get { return this.optionsCount; }
            set
            {
                this.optionsCount = value;

                // update child choice boxes
                for (int i = 0; i < this.ChoiceBoxes.Count; i++)
                {
                    this.ChoiceBoxes[i].BubblesCount = this.OptionsCount;
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether question is selected
        /// </summary>
        public override bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;

                // when question is unselected make sure every child element is unselected
                if (!value)
                {
                    foreach (var choiceBox in this.ChoiceBoxes)
                    {
                        choiceBox.IsSelected = false;
                    }
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets value indicating whether question is valid (i.e. contains valid choiceboxes)
        /// </summary>
        public override bool IsValid
        {
            get { return this.ChoiceBoxes.All(x => x.IsValid); }
        }

        /// <summary>
        /// Creates copy of current question
        /// </summary>
        /// <returns>Question copy</returns>
        public override BaseQuestionViewModel CreateCopy()
        {
            GridViewModel elementCopy = new GridViewModel(this.Name, this.Top, this.Left, this.Width, this.Height);

            elementCopy.SelectedMapping = this.SelectedMapping;

            foreach (var choiceBox in this.ChoiceBoxes)
            {
                var choiceBoxCopy = choiceBox.CreateCopy(); 
                elementCopy.ChoiceBoxes.Add((ChoiceBoxViewModel) choiceBoxCopy);
            }

            return elementCopy;
        }

        /// <summary>
        /// Change element size and position to become min bounding box of bubbles
        /// </summary>
        public override void Shrink()
        {
            // call shirnk for child elements
            foreach (var choiceBox in this.ChoiceBoxes)
            {
                choiceBox.Shrink();
            }

            double minTop = this.ChoiceBoxes.Min(x => x.Top);
            double minLeft = this.ChoiceBoxes.Min(x => x.Left);

            // first shrink top left positions
            this.Top += minTop;
            this.Left += minLeft;

            // update bubbles position
            foreach (var bubbleViewModel in this.ChoiceBoxes)
            {
                bubbleViewModel.Top -= minTop;
                bubbleViewModel.Left -= minLeft;
            }

            // then calculate new width and height
            double maxBottom = this.ChoiceBoxes.Max(x => x.Top + x.Height);
            double maxRight = this.ChoiceBoxes.Max(x => x.Left + x.Width);
            this.Width = maxRight;
            this.Height = maxBottom;
        }

        /// <summary>
        /// Applyes to current element style and properties of provided element
        /// </summary>
        /// <param name="ethalon">Ethalon element</param>
        public void ApplyStyle(GridViewModel ethalon)
        {
            // update size
            this.Width = ethalon.Width;
            this.Height = ethalon.Height;

            this.SelectedMapping = ethalon.SelectedMapping;

            // update child collection
            this.ChoiceBoxes.Clear();
            foreach (var item in ethalon.ChoiceBoxes)
            {
                var copy = item.CreateCopy();
                this.ChoiceBoxes.Add((ChoiceBoxViewModel) copy);
            }
        }

        /// <summary>
        /// Position choice boxes inside horizontally oriented grid
        /// </summary>
        /// <param name="count">amount of child choice boxes</param>
        private void FitChoiceBoxesHorizontal(int count)
        {
            double questionWidth = this.Width - 2 * BorderOffset - 2 * BorderThickness;
            double questionHeight = this.Height - 2 * BorderOffset - 2 * BorderThickness;

            double choiceBoxWidth = (questionWidth - (count - 1) * ItemsOffset) / count;
            double choiceBoxHeight = questionHeight;

            // if can't fit bubbles by height, do not add any bubbles
            if (choiceBoxHeight < MinChoiceBoxSize)
            {
                return;
            }

            // if can't fit bubbles by width, reduce bubbles count and recalculate
            while (choiceBoxWidth < MinChoiceBoxSize)
            {
                count--;
                choiceBoxWidth = (questionWidth - (count - 1) * ItemsOffset) / count;
            }

            for (int i = 0; i < count; i++)
            {
                double leftPosition = BorderOffset + i * choiceBoxWidth + i * ItemsOffset;
                Rect choiceBoxArea = new Rect(leftPosition, BorderOffset, choiceBoxWidth, choiceBoxHeight);
                var choiceBox = new ChoiceBoxViewModel(string.Empty, choiceBoxArea);

                this.ChoiceBoxes.Add(choiceBox);
            }
        }

        /// <summary>
        /// Position choice boxes inside vertically oriented grid
        /// </summary>
        /// <param name="count">amount of child choice boxes</param>
        private void FitChoiceBoxesVertical(int count)
        {
            double questionWidth = this.Width - 2 * BorderOffset - 2 * BorderThickness;
            double questionHeight = this.Height - 2 * BorderOffset - 2 * BorderThickness;

            double choiceBoxHeight = (questionHeight - (count - 1) * ItemsOffset) / count;
            double choiceBoxWidth = questionWidth;

            // if can't fit bubbles by height, do not add any bubbles
            if (choiceBoxWidth < MinChoiceBoxSize)
            {
                return;
            }

            // if can't fit bubbles by width, reduce bubbles count and recalculate
            while (choiceBoxHeight < MinChoiceBoxSize)
            {
                count--;
                choiceBoxHeight = (questionHeight - (count - 1) * ItemsOffset) / count;
            }

            for (int i = 0; i < count; i++)
            {
                double topPosition = BorderOffset + i * ItemsOffset + i * choiceBoxHeight;
                Rect choiceBoxArea = new Rect(BorderOffset, topPosition, choiceBoxWidth, choiceBoxHeight);

                var choiceBox = new ChoiceBoxViewModel(string.Empty, choiceBoxArea);
                this.ChoiceBoxes.Add(choiceBox);
            }
        }

        /// <summary>
        /// Initializes question values
        /// </summary>
        /// <param name="name">Question name</param>
        /// <param name="top">Top position on parent canvas</param>
        /// <param name="left">Left position on parent canvas</param>
        /// <param name="width">Question's width</param>
        /// <param name="height">Question's height</param>
        private void InitializeValues(string name, double top, double left, double width, double height)
        {
            this.ChoiceBoxes = new ObservableCollection<ChoiceBoxViewModel>();

            this.Name = name;
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;

            this.AddCustomMappingCommand = new RelayCommand(x => this.OnAddCustomMapping());
        }

        /// <summary>
        /// Update amount of sections inside grid
        /// </summary>
        /// <param name="newCount">New amount of sections</param>
        private void UpdateSectionsCount(int newCount)
        {
            if (newCount > this.maxAllowedSectionsCount)
            {
                return;
            }

            // update static count field so that freshly added questions has latest amount of bubbles
            latestQuestionsCount = newCount;

            this.ChoiceBoxes.Clear();
            if (this.Orientation == Orientations.Horizontal)
            {
                this.FitChoiceBoxesHorizontal(newCount);
            }
            else
            {
                this.FitChoiceBoxesVertical(newCount);
            }
        }

        /// <summary>
        /// Open window to add custom mapping
        /// </summary>
        private void OnAddCustomMapping()
        {
            CustomMappingViewModel vm = new CustomMappingViewModel();

            if (!vm.CustomMappingAdded)
            {
                return;
            }

            this.OnPropertyChanged(nameof(this.AnswersMapping));

            string newItem = AnswersMappingHelper.GetAnswersMappings()
                .FirstOrDefault(x => x.Equals(vm.CustomMappingName));

            if (newItem != null)
            {
                this.SelectedMapping = newItem;
            }
        }
    }
}
