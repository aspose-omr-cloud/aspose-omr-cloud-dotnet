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
namespace Aspose.OMR.Client
{
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using TemplateModel;
    using ViewModels;

    /// <summary>
    /// Converts view model template to data model and back
    /// </summary>
    public static class TemplateConverter
    {
        /// <summary>
        /// Converts template view model to template model
        /// </summary>
        /// <param name="templateViewModel">TemplateViewModel to convert</param>
        /// <returns>Resulting template</returns>
        public static OmrTemplate ConvertViewModelToModel(TemplateViewModel templateViewModel)
        {
            OmrTemplate template = new OmrTemplate();
            template.TemplateId = templateViewModel.TemplateId;
            template.FinalizationComplete = templateViewModel.FinalizationComplete;
            template.Name = templateViewModel.TemplateName;
            template.IsGenerated = templateViewModel.IsGeneratedTemplate;

            OmrPage page = template.AddPage();
            page.Height = templateViewModel.PageHeight;
            page.Width = templateViewModel.PageWidth;
            page.ImageName = templateViewModel.TemplateImageName;
            page.ImageFormat = templateViewModel.ImageFileFormat;

            foreach (BaseQuestionViewModel element in templateViewModel.PageQuestions)
            {
                if (element is ChoiceBoxViewModel)
                {
                    AddChoiceBoxElement(page, (ChoiceBoxViewModel) element);
                }
                else if (element is GridViewModel)
                {
                    AddGridElement(page, (GridViewModel) element);
                }
            }

            return template;
        }

        /// <summary>
        /// Converts template data model to template view model
        /// </summary>
        /// <param name="template">OmrTemplate to convert</param>
        /// <returns>Resulting TemplateViewModel</returns>
        public static TemplateViewModel ConvertModelToViewModel(OmrTemplate template)
        {
            TemplateViewModel templateViewModel = new TemplateViewModel(template.FinalizationComplete, template.TemplateId);
            templateViewModel.TemplateName = template.Name;
            templateViewModel.IsGeneratedTemplate = template.IsGenerated;

            OmrPage page = template.Pages[0];
            templateViewModel.TemplateImageName = page.ImageName;
            templateViewModel.ImageFileFormat = page.ImageFormat;

            templateViewModel.PageWidth = page.Width;
            templateViewModel.PageHeight = page.Height;

            List<BaseQuestionViewModel> elements = new List<BaseQuestionViewModel>();

            foreach (OmrElement modelElement in page.Elements)
            {
                if (modelElement is ChoiceBoxElement)
                {
                    ChoiceBoxViewModel choiceBoxViewModel = CreateChoiceBoxViewModel((ChoiceBoxElement)modelElement, templateViewModel);
                    elements.Add(choiceBoxViewModel);
                }
                else if (modelElement is GridElement)
                {
                    GridViewModel gridViewModel = CreateGridViewModel((GridElement)modelElement, templateViewModel);
                    elements.Add(gridViewModel);
                }
            }

            templateViewModel.AddQuestions(elements);

            templateViewModel.IsDirty = false;
            return templateViewModel;
        }

        /// <summary>
        /// Check provided image size and format and compress image
        /// </summary>
        /// <param name="templateImage">Template image</param>
        /// <param name="imageFileFormat">Provided image format</param>
        /// <param name="imageSizeInBytes">Image size in bytes</param>
        /// <returns>Image as base64 string</returns>
        public static string CheckAndCompressImage(BitmapImage templateImage, string imageFileFormat, long imageSizeInBytes)
        {
            string imageData;
            long imageSize = imageSizeInBytes;

            if (imageSize > 1024 * 500)
            {
                imageData = TemplateSerializer.CompressImageBase64(templateImage, 90);
            }
            else
            {
                if (string.IsNullOrEmpty(imageFileFormat))
                {
                    imageData = TemplateSerializer.CompressImageBase64(templateImage, 100);
                }
                else if (imageFileFormat.ToLowerInvariant().Equals(".png"))
                {
                    imageData = TemplateSerializer.PngToBase64(templateImage);
                }
                else if (imageFileFormat.ToLowerInvariant().Equals(".tiff") || imageFileFormat.ToLowerInvariant().Equals(".tif"))
                {
                    imageData = TemplateSerializer.TiffToBase64(templateImage);
                }
                else if (imageFileFormat.ToLowerInvariant().Equals(".gif"))
                {
                    imageData = TemplateSerializer.GifToBase64(templateImage);
                }
                else if (imageFileFormat.ToLowerInvariant().Equals(".jpg") || imageFileFormat.ToLowerInvariant().Equals(".jpeg"))
                {
                    imageData = TemplateSerializer.CompressImageBase64(templateImage, 100);
                }
                else
                {
                    imageData = TemplateSerializer.CompressImageBase64(templateImage, 100);
                }
            }

            return imageData;
        }

        /// <summary>
        /// Creates Grid element from GridViewModel and adds to the OmrPage
        /// </summary>
        /// <param name="page">Page to add element to</param>
        /// <param name="gridViewModel">ViewModel to take data from</param>
        private static void AddGridElement(OmrPage page, GridViewModel gridViewModel)
        {
            GridElement grid = page.AddGridElement(gridViewModel.Name, (int)gridViewModel.Width,
                (int)gridViewModel.Height,
                (int)gridViewModel.Top,
                (int)gridViewModel.Left);

            foreach (ChoiceBoxViewModel choiceBoxViewModel in gridViewModel.ChoiceBoxes)
            {
                ChoiceBoxElement choiceBoxElement = grid.AddChoiceBox(
                    choiceBoxViewModel.Name,
                    (int)choiceBoxViewModel.Width,
                    (int)choiceBoxViewModel.Height,
                    (int)(gridViewModel.Top + choiceBoxViewModel.Top),
                    (int)(gridViewModel.Left + choiceBoxViewModel.Left));

                foreach (var bubble in choiceBoxViewModel.Bubbles)
                {
                    choiceBoxElement.AddBubble(
                        bubble.Name,
                        (int)bubble.Width,
                        (int)bubble.Height,
                        (int)(gridViewModel.Top + choiceBoxViewModel.Top + bubble.Top),
                        (int)(gridViewModel.Left + choiceBoxViewModel.Left + bubble.Left),
                        bubble.IsValid);
                }
            }
        }

        /// <summary>
        /// Creates ChoiceBox element from ChoiceBoxViewModel and adds to the OmrPage
        /// </summary>
        /// <param name="page">Page to add element to</param>
        /// <param name="choiceBoxViewModel">ViewModel to take data from</param>
        private static void AddChoiceBoxElement(OmrPage page, ChoiceBoxViewModel choiceBoxViewModel)
        {
            ChoiceBoxElement choiceBox = page.AddChoiceBoxElement(
                choiceBoxViewModel.Name,
                (int)choiceBoxViewModel.Width,
                (int)choiceBoxViewModel.Height,
                (int)choiceBoxViewModel.Top,
                (int)choiceBoxViewModel.Left);

            foreach (var bubble in choiceBoxViewModel.Bubbles)
            {
                choiceBox.AddBubble(
                    bubble.Name,
                    (int)bubble.Width,
                    (int)bubble.Height,
                    (int)(choiceBoxViewModel.Top + bubble.Top),
                    (int)(choiceBoxViewModel.Left + bubble.Left),
                    bubble.IsValid);
            }
        }

        /// <summary>
        /// Creates choice box view model from choice box model
        /// </summary>
        /// <param name="choiceBox">Choice box model data</param>
        /// <param name="templateViewModel">Parent template</param>
        /// <returns>Created choice box view model</returns>
        private static ChoiceBoxViewModel CreateChoiceBoxViewModel(ChoiceBoxElement choiceBox, TemplateViewModel templateViewModel)
        {
            ChoiceBoxViewModel choiceBoxViewModel = new ChoiceBoxViewModel(
                choiceBox.Name,
                choiceBox.Top,
                choiceBox.Left,
                choiceBox.Width,
                choiceBox.Height,
                templateViewModel,
                null);

            foreach (OmrBubble modelBubble in choiceBox.Bubbles)
            {
                BubbleViewModel bubbleViewModel = new BubbleViewModel(
                    choiceBox.BubbleWidth,
                    choiceBox.BubbleHeight,
                    modelBubble.Top - choiceBox.Top,
                    modelBubble.Left - choiceBox.Left,
                    choiceBoxViewModel);

                bubbleViewModel.Name = modelBubble.Value;
                bubbleViewModel.IsValid = modelBubble.IsValid;

                choiceBoxViewModel.Bubbles.Add(bubbleViewModel);
            }

            return choiceBoxViewModel;
        }

        /// <summary>
        /// Creates grid view model from grid model
        /// </summary>
        /// <param name="gridElement">Grid model data</param>
        /// <param name="templateViewModel">Parent template</param>
        /// <returns>Created grid view model</returns>
        private static GridViewModel CreateGridViewModel(GridElement gridElement, TemplateViewModel templateViewModel)
        {
            GridViewModel gridViewModel = new GridViewModel(
                gridElement.Name,
                gridElement.Top,
                gridElement.Left,
                gridElement.Width,
                gridElement.Height,
                templateViewModel,
                gridElement.Orientation);

            gridViewModel.ChoiceBoxes.Clear();

            foreach (var omrElement in gridElement.ChoiceBoxes)
            {
                var choiceBox = (ChoiceBoxElement)omrElement;

                ChoiceBoxViewModel choiceBoxViewModel = new ChoiceBoxViewModel(
                    choiceBox.Name,
                    choiceBox.Top - gridElement.Top,
                    choiceBox.Left - gridElement.Left,
                    choiceBox.Width,
                    choiceBox.Height,
                    null,
                    gridViewModel);

                foreach (OmrBubble modelBubble in choiceBox.Bubbles)
                {
                    BubbleViewModel bubbleViewModel = new BubbleViewModel(
                        choiceBox.BubbleWidth,
                        choiceBox.BubbleHeight,
                        modelBubble.Top - choiceBox.Top,
                        modelBubble.Left - choiceBox.Left,
                        choiceBoxViewModel);

                    bubbleViewModel.Name = modelBubble.Value;
                    bubbleViewModel.IsValid = modelBubble.IsValid;

                    choiceBoxViewModel.Bubbles.Add(bubbleViewModel);
                }

                gridViewModel.ChoiceBoxes.Add(choiceBoxViewModel);
            }

            return gridViewModel;
        }
    }
}
