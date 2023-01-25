# Aspose.OMR Cloud SDK for .NET

**Aspose.OMR Cloud** is an easy-to-use and versatile online service for designing, rendering and recognizing hand-filled answer sheets, surveys, applications, and similar forms. With it, you can quickly develop on-premise and web-based .NET software for optical mark recognition (OMR) that requires minimal resources on the client side - all the work is performed by remote services.

This software development kit (SDK) simplifies the interaction with Aspose.OMR Cloud services, allowing you to focus on business logic rather than the technical details. It handles all the routine operations such as establishing connections, sending API requests, and parsing responses, wrapping all these tasks into a few simple methods that can be used in any .NET application.

## Contents of this package

The repository contains:

- Aspose.OMR Cloud SDK for .NET (_aspose-omr-cloud-dotnet_)
- Demo project (_aspose-omr-cloud-dotnet/OMRDemo_) - a simple C# console application that demonstrates how to use Aspose.OMR Cloud SDK for .NET for generating and recognizing OMR forms.
- Demo data (_aspose-omr-cloud-demo-data_):
    - Configuration file (_aspose-omr-cloud-demo-data/config.json_);
    - Source code of the questionnaire, associated images and a scanned image of the filled questionnaire for recognition test (_aspose-omr-cloud-demo-data/Data_);
    - The folder for storing a generated printable form and recognition results (_aspose-omr-cloud-demo-data/Temp_).

## Licensing

Aspose.OMR Cloud SDK for .NET, demo applications, documentation, and form templates are distributed under [MIT License](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/blob/master/LICENSE).

## Prerequisites

The system on which you run the package must meet the following requirements:

### Aspose.OMR Cloud SDK for .NET

- .NET Standard 2.0

### Demo project

- .NET Core 3.1
- Microsoft Visual Studio 2012 or later

## Third party dependencies

The following third party libraries are used by the solution:

### Aspose.OMR Cloud SDK for .NET

- [JsonSubTypes 1.2.0](https://www.nuget.org/packages/JsonSubTypes/1.2.0)
- [RestSharp 105.1.0](https://www.nuget.org/packages/RestSharp/105.1.0)

### Demo project

- [Newtonsoft.Json 13.0.1](https://www.nuget.org/packages/Newtonsoft.Json/13.0.1)

## Installation and usage

1. Clone or download the repository.
2. Open _aspose-omr-cloud-dotnet\\aspose.Omr.Cloud.Sdk.sln_ with Microsoft Visual Studio.
3. Run **OMRDemo** project (press **F5** key).

The demo application generates a printable OMR-ready form (_aspose-omr-cloud-demo-data\\Temp\\Aspose_test.omr_) along with the recognition pattern file (_aspose-omr-cloud-demo-data/Temp/Aspose_test.omr_). The latter is used by Aspose.OMR recognition engine to match filled bubbles with template fields.

Then it recognizes the scanned image of the filled questionnaire and saves results in _aspose-omr-cloud-demo-data\\Temp\\Aspose_test.csv_ file.

### Configuring

The configuration file (_aspose-omr-cloud-demo-data/config.json_) contains the basic parameters for working with Aspose.OMR Cloud.

- **base_path** - root URL of Aspose.OMR Cloud service.
- **data_folder** - path to the folder with the source code of the questionnaire, associated images and scanned images of the filled questionnaires. Specify the path relative to the configuration file.
- **result_folder** - path to the folder for storing a generated printable form, recognition pattern and recognition results. Specify the path relative to the configuration file.

## How it works?

Aspose.OMR Cloud supports end-to-end OMR process - from designing a form to recognizing its filled hardcopies. The workflow includes the following stages:

![Stages of the OMR process](omr-cloud-stages.png)

All resource-consuming tasks (generation and recognition of OMR forms) are done in the cloud, freeing up resources on the end user's device. All cloud tasks are queued, which prevents multiple simultaneous requests from interfering with each other and consuming too much processor time.

Aspose.OMR Cloud SDK for .NET implements wrapper classes that allow you to interact with the Aspose.OMR Cloud REST API without writing low-level HTTP requests and parsing responses. The demo project source code (_aspose-omr-cloud-dotnet\\OMRDemo\\Program.cs_) contains extensive comments on all major parts of the code, so you can easily understand the basics without having to consult the SDK reference documentation.

### Generating a printable form

To generate a printable form and a recognition pattern file, send the form sources and page configuration to the Aspose.OMR Cloud queue and get the results a few seconds later.

The processing time can vary from a few milliseconds to a couple of seconds, depending on the current load of the Aspose.OMR Cloud service.

#### GenerateTemplateApi

This class contains wrapper methods for generating the printable form from the source code and working with the form generation queue. [More](docs/GenerateTemplateApi.md).

#### OmrGenerateTask

This class allows you to prepare a request for sending the form source code, associated images, and page layout to the generation queue. [More](docs/OmrGenerateTask.md).

##### Page layout

The paper size, orientation, font, and other layout settings are configured through [PageSettings](docs/PageSettings.md) object which is passed to the `OmrGenerateTask` constructor.

##### Working with images

Aspose.OMR Cloud allows you to customize forms by adding images (such as your company logo) to them. In addition to describing the image element in the form's source code, each image file must be directly submitted to the generation queue.

Images are provided to the `OmrGenerateTask` constructor as a [Dictionary<string, byte[]>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2) object, where the key contains the image file name, and the value contains the contents of the image file as an array of bytes.

#### OMRGenerationResponse

This class contains the current state of the queued form generation request, along with the printable OMR form and recognition pattern file, if the form has been generated. [More](docs/OMRGenerationResponse.md).

### Recognizing a filled form

To recognize the filled form, send its scanned image or photo along with the recognition pattern file to the Aspose.OMR Cloud queue and get the results a few seconds later.

The processing time can vary from a few milliseconds to a couple of seconds, depending on the current load of the Aspose.OMR Cloud service.

#### RecognizeTemplateApi

This class contains wrapper methods for recognizing the scanned or photographed form and working with the form recognition queue. [More](docs/RecognizeTemplateApi.md).

#### OmrRecognizeTask

This class allows you to prepare a request for sending the form image to the recognition queue. [More](docs/OmrGenerateTask.md).

The form must be accompanied by the recognition pattern (.OMR) file, which tells Aspose.OMR recognition engine how to match filled bubbles with template fields. Please note that the recognition template file must be taken from the same generation response as the printable form, otherwise the recognition results are not guaranteed to be correct.

##### Recognition accuracy threshold

A respondent can fill out the form with a pen, pencil or marker, and use various marks inside the bubbles - from a solid fill to small crosses or checks.

Recognition accuracy threshold (`recognitionThreshold`) parameter determines how marks are processed during recognition. You can provide a value from 0 to 100. Lower values allow even the lightest marks to be recognized, but may cause dirt or paper defects to be treated as marks. Higher values require a more solid fill and may cause pencil marks or small checkmarks to be ignored.

![Recognition accuracy threshold](recognition-threshold.png)

**Important:** Instruct respondents to use the same type of marks for all bubbles. Otherwise, recognition results may be inaccurate. If you plan to use your smartphone's camera instead of a scanner, we recommend a solid fill with a pen or marker.

#### OMRRecognitionResponse

This class contains the current state of the queued form recognition request, along with recognition results, if the form has been recognized. [More](docs/OMRRecognitionResponse.md).

## Resources

- [Aspose.OMR Cloud product family](https://products.aspose.cloud/omr/family/)
- [Technical documentation](https://docs.aspose.cloud/omr/)
- [Aspose.OMR Cloud API Reference](https://apireference.aspose.cloud/omr/)
- [Free Support Forum](https://forum.aspose.cloud/c/omr/8)
- [GitHub repositories](https://github.com/aspose-omr-cloud)
