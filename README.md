
<img src="TestData/Demonstration/OmrBanner.png">



# Aspose.OMR for Cloud

[Aspose.OMR for Cloud](https://products.aspose.cloud/omr/cloud) is a REST API that helps you to perform optical mark recognition in the cloud. We provide a series of [SDKs](https://github.com/aspose-omr-cloud). Along with that, you can get [binaries](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/releases) to start working immediately and recognize various OMR forms.

Developers can embed [optical recognition](https://en.wikipedia.org/wiki/Optical_mark_recognition) in any type of application to extract data from images of tests, exams, questionnaires, surveys, etc. In the repository you can find examples on how to start using [Aspose.OMR API](https://docs.aspose.cloud/display/omrcloud/OMR+API+Specification) in your project.


## Contents

The repository contains sample applications that demonstrate how to perform common OMR with Aspose.OMR API.
<p align="center">
  <a href="https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/archive/master.zip">
    <img src="http://i.imgur.com/hwNhrGZ.png" />
  </a>
</p>


Directory | Description
--------- | -----------
[Aspose.OMR.Client](Aspose.OMR.Client)  | An open source .NET application with GUI that helps you work with OMR templates and perform OMR operations.
[Aspose.OMR-Cloud.SDK](SDK)  |Aspose.OMR Cloud SDK provides functionality for using Aspose.OMR for Cloud API to recognize optical marks from sheet images in the cloud. It also contains demonstration console project.
[TestData](TestData) | Sample images and templates that demonstrate how OMR works on real data.



## Quickstart

You can perform tasks out of the box without writing a single line of code. That requires two simple steps:


### 1. Receive Cloud Keys
Aspose.Cloud credentials are required to use Aspose.OMR for Cloud API. You can acquire App SID and App Key by registrating at [Aspose Cloud Dashboard](https://dashboard.aspose.cloud/). It will take only a couple of minutes.

### 2. Get OMR Client
Check [releases](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/releases) section and download MSI package that installs OMR.Client. Using this GUI application you can create OMR templates, correct or update existing OMR templates and perform optical mark recognition using Aspose.OMR.Cloud engine. 

More info and documentation can be found at: [Client documentation](https://docs.aspose.cloud/display/omrcloud/Aspose.OMR.Client+Application)


## Using OMR Cloud API in your projects
It is quite easy to use OMR in your projects. All you need to do is:

1. Get [Aspose Cloud](https://dashboard.aspose.cloud/) credentials - App Key and App Sid.
2. Install Aspose.OMR Cloud SDK via [nuget](https://www.nuget.org/packages/Aspose.OMR-Cloud/).
3. Use OMR.Client to create templates or use one from [our examples](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/tree/master/TestData).
4. Start using [OMR functions](https://docs.aspose.cloud/display/omrcloud/OMR+API+Specification) and get recognition results.


Check [Aspose.OMR.Demo](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/tree/master/SDK/Aspose.OMR.Demo) solution for a simple demonstration of how OMR functions can be called from your code.

## How does it work?

Simply prepare your questions with our simple markup language. Below you can see survey example.

<sub>


```
?image=LogoImage1.jpg
	align=left

?text=Name__________________________________              Date____________

?grid=ID
	sections_count=8

#What is Aspose.OMR main function?
	() OCR () Capture human-marked data
	() There is no main function () Enhance images
#Can Aspose.OMR process not only scans, but also photos?
	() Yes, indeed! () No
#Aspose.OMR is available on any platform, because it is:
	() Cross-platform code () Cloud service
#Aspose.OMR works with any kind of OMR forms: tests, exams, questionnaires, surveys, etc.
	() Yes, indeed! () No
#Excellent recognition results can be achieved only for filled bubbles at least for:
	() 40% () 60% () 75% () 98%
#Does Aspose.OMR support bubbles mapping to any key names?
	() No () Partially (only "A, B, C..." or "1, 2, 3...") () Yes, any key names
#Do you have to mark up every question on the page?
	(Yes) Yes, that will help a lot! (No) No
#I found aspose omr to be a useful tool. (9 - strongly agree, 1 - strongly disagree)
	(9) (8) (7) (6) (5) (4) (3) (2) (1)
?text=						Answer sheet section

?answer_sheet=MainQuestions
	elements_count=15
	columns_count=5

?text=Sign________________________________

?image=LogoImage2.png
	align=right
?barcode=Student_ID
	value=5901234123457
	barcode_type=ean13
	x=900
	y=3050
	height=400
?barcode=AsposeWebsite
	value=aspose.com
	barcode_type=qr
	qr_version=1
	x=2000
	y=120
	height=360
?barcode=ArucoTest
	value=53
	barcode_type=aruco
	x=140
	y=3200
	height=198
```
</sub>

## What do you get?

You’ll get a nice and sharp survey ready to print!

<img src="TestData/Demonstration/GeneratedImage.png" width=400>


## What is next?

Simply make mobile snapshots or scan filled forms, upload them into OMR.Client or call API and you have the results!

<img src="TestData/Demonstration/AsposePhoto.jpg" height=300> <img src="TestData/Demonstration/AsposeRecognition.jpg" height=300> <img src="TestData/Demonstration/Answers.png" height=300>


## Roadmap
In the upcoming releases, we are set to implement a number of new features:

 - [X] reduce your expenses by preprocessing and compressing your images
 - [X] simplify OMR form preparation by introducing an easy markup language. For example, to get the particular survey form ready you only need to provide questions and answers 
 - [X] support PDF
 - [X] support barcodes and QR codes generation and recognition
 - [X] Grade OMR results based on rules
 - [ ] support diverse interviewee’s marks (ticks, crosses, corrections, etc) on various forms with the use of neural networks
 - [ ] Support multipage templates
 - [ ] API for gathering statistics over processed forms
 - [ ] Handprinted text recognition
 





## Resources

+ **Website:** [www.aspose.com](https://www.aspose.com/)
+ **Product Home:** [Aspose.OMR for Cloud](https://products.aspose.cloud/omr/net)
+ **Documentation:** [Aspose.OMR for Cloud Documentation](https://docs.aspose.cloud/display/omrcloud/Home)
+ **Cloud Dashboard:** [Aspose Cloud](https://dashboard.aspose.cloud/)
+ **Forum:** [Aspose.OMR for Cloud Forum](https://forum.aspose.cloud/c/omr)
+ **Blog:** [Aspose.OMR for Cloud Blog](https://blog.aspose.cloud/category/aspose-products/aspose.omr-product-family/)
+ **Nuget:** [Aspose.OMR-Cloud](https://www.nuget.org/packages/Aspose.OMR-Cloud/)
+ **OMR Client Releases:** [Github Releases](https://github.com/aspose-omr-cloud/aspose-omr-cloud-dotnet/releases)
+ **OMR Client Documentation:** [Aspose.OMR.Client Application](https://docs.aspose.cloud/display/omrcloud/Aspose.OMR.Client+Application)
