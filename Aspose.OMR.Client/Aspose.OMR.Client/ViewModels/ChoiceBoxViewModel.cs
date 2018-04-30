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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using TemplateModel;
    using UndoRedo;
    using Utility;

    /// <summary>
    /// View model class for choice box question
    /// </summary>
    public sealed class ChoiceBoxViewModel : BaseQuestionViewModel
    {
        /// <summary>
        /// Minimum bubble size in pixels
        /// </summary>
        private const int MinBubbleSize = 10;

        /// <summary>
        /// Amount of bubbles used when creating new questions
        /// Updated when user manually changes bubbles count inside question
        /// </summary>
        private static int latestBubblesCount = 4;

        /// <summary>
        /// Mapping value used when creating new questions
        /// Updated when user manually changed bubbles answers mapping
        /// </summary>
        private static int latestSelectedMapping;

        /// <summary>
        /// Maximum number of allowed bubbles inside question
        /// </summary>
        private readonly int maxAllowedBubblesCount = 50;

        /// <summary>
        /// Collection of bubbles inside element
        /// </summary>
        private ObservableCollection<BubbleViewModel> bubbles;

        /// <summary>
        /// Selected mapping key in mapping selection drop-down menu
        /// </summary>
        private string selectedAnswersMapping;

        /// <summary>
        /// Indicates that question is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Choice box orientation
        /// </summary>
        private Orientations orientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceBoxViewModel"/> class.
        /// This constructor is used in copying and model convertions
        /// </summary>
        /// <param name="name">Question name</param>
        /// <param name="top">Top position on parent canvas</param>
        /// <param name="left">Left position on parent canvas</param>
        /// <param name="width">Question's width</param>
        /// <param name="height">Question's height</param>
        /// <param name="parentTemplate">The view model of parent template</param>
        /// <param name="parentGrid">The parent grid view model</param>
        public ChoiceBoxViewModel(string name, double top, double left, double width, double height, TemplateViewModel parentTemplate, GridViewModel parentGrid)
        {
            this.InitializeValues(name, top, left, width, height);
            this.SelectedMapping = this.AnswersMapping[latestSelectedMapping];
            this.ParentTemplate = parentTemplate;
            this.ParentGrid = parentGrid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceBoxViewModel"/> class.
        /// This constructor is called when user creates new questions
        /// </summary>
        /// <param name="name">Question name</param>
        /// <param name="area">Question area</param>
        /// <param name="parentTemplate">The view model of parent template</param>
        /// <param name="parentGrid">The parent grid view model</param>
        public ChoiceBoxViewModel(string name, Rect area, TemplateViewModel parentTemplate, GridViewModel parentGrid)
        {
            this.InitializeValues(name, area.Top, area.Left, area.Width, area.Height);
            this.ParentTemplate = parentTemplate;
            this.ParentGrid = parentGrid;

            this.Orientation = this.Width >= this.Height ? Orientations.Horizontal : Orientations.Vertical;

            if (this.Orientation == Orientations.Horizontal)
            {
                this.FitBubblesHorizontal(latestBubblesCount);
            }
            else
            {
                this.FitBubblesVertical(latestBubblesCount);
            }

            this.SelectedMapping = this.AnswersMapping[latestSelectedMapping];
        }

        /// <summary>
        /// Gets or sets the grid view model containing this question
        /// </summary>
        public GridViewModel ParentGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether item is selected
        /// </summary>
        public override bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;

                // when question is unselected make sure every child bubble is unselected
                if (!value)
                {
                    this.UnselectBubbles();
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets value indicating whether question is valid (i.e. contains valid bubbles)
        /// </summary>
        public override bool IsValid
        {
            get { return this.Bubbles.All(x => x.IsValid); }
        }

        /// <summary>
        /// Gets or sets the collection of bubbles
        /// </summary>
        public ObservableCollection<BubbleViewModel> Bubbles
        {
            get { return this.bubbles; }
            set
            {
                this.bubbles = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets choice box orientation
        /// </summary>
        public override Orientations Orientation
        {
            get { return this.orientation; }
            set
            {
                this.orientation = value;
                this.OnPropertyChanged();
            }
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

                this.UpdateBubblesNames();
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the number of bubbles inside question
        /// </summary>
        public int BubblesCount
        {
            get { return this.Bubbles.Count; }
            set
            {
                if (value <= 0)
                {
                    return;
                }

                this.UpdateBubbleCount(value, true);
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Creates copy of current question
        /// </summary>
        /// <returns>Question copy</returns>
        public override BaseQuestionViewModel CreateCopy()
        {
            ChoiceBoxViewModel elementCopy = new ChoiceBoxViewModel(this.Name, this.Top, this.Left, this.Width,
                this.Height, this.ParentTemplate, this.ParentGrid);
            elementCopy.SelectedMapping = this.SelectedMapping;

            foreach (BubbleViewModel bubble in this.Bubbles)
            {
                var bubbleCopy = new BubbleViewModel(bubble.Width, bubble.Height, bubble.Top, bubble.Left, elementCopy);
                bubbleCopy.Name = bubble.Name;
                elementCopy.bubbles.Add(bubbleCopy);
            }

            return elementCopy;
        }

        /// <summary>
        /// Applyes to current element style and properties of provided element
        /// </summary>
        /// <param name="ethalon">Ethalon element</param>
        public void ApplyStyle(ChoiceBoxViewModel ethalon)
        {
            // update size
            this.Width = ethalon.Width;
            this.Height = ethalon.Height;

            this.SelectedMapping = ethalon.SelectedMapping;

            // update bubbles collection
            this.Bubbles.Clear();
            foreach (BubbleViewModel bubble in ethalon.Bubbles)
            {
                var newBubble = new BubbleViewModel(bubble.Width, bubble.Height, bubble.Top, bubble.Left, this);
                newBubble.Name = bubble.Name;
                this.Bubbles.Add(newBubble);
            }
        }

        /// <summary>
        /// Change element size and position to become min bounding box of bubbles
        /// </summary>
        public override void Shrink()
        {
            double minTop = this.Bubbles.Min(x => x.Top);
            double minLeft = this.Bubbles.Min(x => x.Left);

            // first shrink top left positions
            this.Top += minTop;
            this.Left += minLeft;

            // update bubbles position to compensate parent question top left change
            foreach (BubbleViewModel bubbleViewModel in this.Bubbles)
            {
                bubbleViewModel.Top -= minTop;
                bubbleViewModel.Left -= minLeft;
            }

            // then calculate new width and height
            double maxBottom = this.Bubbles.Max(x => x.Top + x.Height);
            double maxRight = this.Bubbles.Max(x => x.Left + x.Width);

            this.Width = maxRight;
            this.Height = maxBottom;
        }

        /// <summary>
        /// Adds bubbles to current question
        /// </summary>
        /// <param name="bubblesToAdd">Items to add</param>
        public void AddBubbles(List<BubbleViewModel> bubblesToAdd)
        {
            foreach (BubbleViewModel bubble in bubblesToAdd)
            {
                this.Bubbles.Add(bubble);
            }

            this.OnPropertyChanged("BubblesCount");
        }

        /// <summary>
        /// Update bubbles count within question
        /// </summary>
        /// <param name="newCount">New amount of bubbles</param>
        /// <param name="toTrack">Flag indicating whether action should be tracked for Undo/Redo support</param>
        public void UpdateBubbleCount(int newCount, bool toTrack)
        {
            if (newCount > this.maxAllowedBubblesCount)
            {
                return;
            }

            // update static count field so that freshly added questions has latest amount of bubbles
            latestBubblesCount = newCount;

            var bubblesBefore = new List<BubbleViewModel>();
            bubblesBefore.AddRange(this.Bubbles);

            this.Bubbles.Clear();
            if (this.Orientation == Orientations.Horizontal)
            {
                this.FitBubblesHorizontal(newCount);
            }
            else
            {
                this.FitBubblesVertical(newCount);
            }

            this.UpdateBubblesNames();

            var bubblesAfter = new List<BubbleViewModel>();
            bubblesAfter.AddRange(this.Bubbles);

            if (toTrack)
            {
                ActionTracker.TrackChangeBubblesCount(this, bubblesBefore, bubblesAfter);
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
            this.Bubbles = new ObservableCollection<BubbleViewModel>();

            this.Name = name;
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;

            this.AddCustomMappingCommand = new RelayCommand(x => this.OnAddCustomMapping());
        }

        /// <summary>
        /// Position bubbles inside horizontally oriented question
        /// </summary>
        /// <param name="count">Amount of bubbles</param>
        private void FitBubblesHorizontal(int count)
        {
            double questionWidth = this.Width - 2 * BorderOffset - 2 * BorderThickness;
            double questionHeight = this.Height - 2 * BorderOffset - 2 * BorderThickness;

            double bubbleWidth = (questionWidth - (count - 1) * ItemsOffset) / count;
            double bubbleHeight = questionHeight;

            // if can't fit bubbles by height, do not add any bubbles
            if (bubbleHeight < MinBubbleSize)
            {
                return;
            }

            // if can't fit bubbles by width, reduce bubbles count and recalculate
            while (bubbleWidth < MinBubbleSize)
            {
                count--;
                bubbleWidth = (questionWidth - (count - 1) * ItemsOffset) / count;
            }

            for (int i = 0; i < count; i++)
            {
                double leftPosition = BorderOffset + i * ItemsOffset + i * bubbleWidth;

                var bubble = new BubbleViewModel(bubbleWidth, bubbleHeight, BorderOffset, leftPosition, this);
                this.Bubbles.Add(bubble);
            }
        }

        /// <summary>
        /// Position bubbles inside vertically oriented question
        /// </summary>
        /// <param name="count">Amount of bubbles</param>
        private void FitBubblesVertical(int count)
        {
            double questionWidth = this.Width - 2 * BorderOffset - 2 * BorderThickness;
            double questionHeight = this.Height - 2 * BorderOffset - 2 * BorderThickness;

            double bubbleHeight = (questionHeight - (count - 1) * ItemsOffset) / count;
            double bubbleWidth = questionWidth;

            // if can't fit bubbles by width, do not add any bubbles
            if (bubbleWidth < MinBubbleSize)
            {
                return;
            }

            // if can't fit bubbles by height, reduce bubbles count and recalculate
            while (bubbleHeight < MinBubbleSize)
            {
                count--;
                bubbleHeight = (questionHeight - (count - 1) * ItemsOffset) / count;
            }

            for (int i = 0; i < count; i++)
            {
                double topPosition = BorderOffset + i * ItemsOffset + i * bubbleHeight;
                var bubble = new BubbleViewModel(bubbleWidth, bubbleHeight, topPosition, BorderOffset, this);
                this.Bubbles.Add(bubble);
            }
        }

        /// <summary>
        /// Updates bubble names according to selected mapping
        /// </summary>
        private void UpdateBubblesNames()
        {
            string[] answerValues = AnswersMappingHelper.GetMappingByKey(this.SelectedMapping);

            for (int i = 0, j = 0; i < this.Bubbles.Count; i++, j++)
            {
                if (answerValues.Length <= j)
                {
                    j -= answerValues.Length;
                }

                this.Bubbles[i].Name = answerValues[j];
            }
        }

        /// <summary>
        /// Unselects all child bubbles
        /// </summary>
        private void UnselectBubbles()
        {
            foreach (BubbleViewModel bubble in this.Bubbles)
            {
                bubble.IsSelected = false;
            }
        }
    }
}