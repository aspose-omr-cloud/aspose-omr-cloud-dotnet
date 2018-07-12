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
    using System.Windows;
    using TemplateModel;

    /// <summary>
    /// View model class for Barcode element
    /// </summary>
    public sealed class BarcodeViewModel : BaseQuestionViewModel
    {
        /// <summary>
        /// Indicates that element is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// 
        /// </summary>
        private BarcodeTypes selectedBarcodeType;

        /// <summary>
        /// 
        /// </summary>
        private int? qrVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarcodeViewModel"/> class
        /// </summary>
        /// <param name="name">Name of the grid question</param>
        /// <param name="area">Area of the question</param>
        /// <param name="templateViewModel">The view model of parent template</param>
        public BarcodeViewModel(string name, Rect area, TemplateViewModel templateViewModel)
        {
            this.InitializeValues(name, area.Top, area.Left, area.Width, area.Height);
            this.ParentTemplate = templateViewModel;
            this.SelectedBarcodeType = BarcodeTypes.qr;
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
            this.Name = name;
            this.Top = top;
            this.Left = left;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Gets or sets a value indicating whether item is selected
        /// </summary>
        public override bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets value indicating whether question is valid (unused)
        /// </summary>
        public override bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets element orientation (unused in barcode)
        /// </summary>
        public override Orientations Orientation { get; set; }

        /// <summary>
        /// Gets or sets the type of barcode
        /// </summary>
        public BarcodeTypes SelectedBarcodeType
        {
            get { return this.selectedBarcodeType; }
            set
            {
                this.selectedBarcodeType = value;
                if (value == BarcodeTypes.qr)
                {
                    // set default qr version
                    this.QrVersion = 1;
                }
                else
                {
                    this.QrVersion = null;
                }

                this.OnPropertyChanged();
                this.OnPropertyChanged("QrVersionVisible");
            }
        }

        /// <summary>
        /// Gets or sets the qr version (only for qr codes)
        /// </summary>
        public int? QrVersion
        {
            get { return this.qrVersion; }
            set
            {
                this.qrVersion = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets value indicating whether qr version property should be visible
        /// </summary>
        public bool QrVersionVisible
        {
            get { return this.SelectedBarcodeType == BarcodeTypes.qr; }
        }

        /// <summary>
        /// Creates copy of current question
        /// </summary>
        /// <returns>Freshly created copy</returns>
        public override BaseQuestionViewModel CreateCopy()
        {
            BarcodeViewModel copy = new BarcodeViewModel(this.Name, new Rect(this.Left, this.Top, this.Width, this.Height), this.ParentTemplate);
            copy.SelectedBarcodeType = this.SelectedBarcodeType;
            if (this.SelectedBarcodeType == BarcodeTypes.qr)
            {
                copy.QrVersion = this.QrVersion;
            }

            return copy;
        }

        /// <summary>
        /// Applyes to current element style and properties of provided element
        /// </summary>
        /// <param name="ethalon">Ethalon element</param>
        public void ApplyStyle(BarcodeViewModel ethalon)
        {
            // update size
            this.Width = ethalon.Width;
            this.Height = ethalon.Height;

            this.SelectedBarcodeType = ethalon.SelectedBarcodeType;
            if (ethalon.SelectedBarcodeType == BarcodeTypes.qr)
            {
                this.QrVersion = ethalon.QrVersion;
            }
        }

        /// <summary>
        /// Shrinks element (unused in barcode)
        /// </summary>
        public override void Shrink()
        {
            return;
        }
    }
}
