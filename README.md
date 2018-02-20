<img src="TestFiles/Demonstration/OmrBanner.png">



# Aspose.OMR for Cloud

[Aspose.OMR for Cloud](https://products.aspose.cloud/omr/cloud) is a REST API that helps you to perform optical mark recognition in the cloud. You can get [binaries](https://github.com/asposecloud/Aspose.OMR-Cloud/releases) to **start** working **immediately** and recognize various OMR forms. 
Developers can embed optical recognition in any type of application to extract data from images of tests, exams, questionnaires, surveys, etc. In the repository you can find examples on how to start using Aspose.OMR API in your project. 



## Contents

The repository contains sample applications that demonstrate how to perform common OMR with Aspose.OMR API.
<p align="center">
  <a href="https://github.com/asposecloud/Aspose.OMR-Cloud/archive/master.zip">
    <img src="http://i.imgur.com/hwNhrGZ.png" />
  </a>
</p>


Directory | Description
--------- | -----------
[ConsoleClient](ConsoleClient)  | A .NET 2.0 console application that demonstrates main Aspose.OMR for Cloud functionality.
[Aspose.OMR.Client](OMRclient)  | An open source .NET application with GUI that helps you work with OMR templates and perform OMR operations.
[TestFiles](TestFiles) | Sample images and templates that demonstrate how OMR works on real data.



## Quickstart

You can perform tasks out of the box without writing a single line of code. That requires two simple steps:


### 1. Receive Cloud Keys
Aspose.Cloud credentials are required to use Aspose.OMR for Cloud API. You can acquire App SID and App Key by registrating at [Aspose Cloud Dashboard](https://dashboard.aspose.cloud/). It will take only a couple of minutes.

### 2. Get OMR Client
Check [releases](https://github.com/asposecloud/Aspose.OMR-Cloud/releases) section and download MSI package that installs OMR.Client. Using this GUI application you can create OMR templates, correct or update existing OMR templates and perform optical mark recognition using Aspose.OMR.Cloud engine. 

More info and documentation can be found at: [Client documentation](https://docs.aspose.cloud/display/omrcloud/Aspose.OMR.Client+Application)


## Using OMR Cloud API in your projects
It is quite easy to use OMR in your projects. All you need to do is:

1. Get [Aspose Cloud](https://dashboard.aspose.cloud/) credentials - App Key and App Sid.
2. Install Aspose.OMR Cloud SDK via [nuget](https://www.nuget.org/packages/Aspose.OMR-Cloud/).
3. Use OMR.Client to create templates or use one from [our examples](https://github.com/asposecloud/Aspose.OMR-Cloud/tree/master/TestFiles).
3. Start using [OMR functions](https://docs.aspose.cloud/display/omrcloud/API+Specifications) and get recognition results.


Check [Console Client](https://github.com/asposecloud/Aspose.OMR-Cloud/tree/master/ConsoleClient) solution for a simple demonstration of how OMR functions can be called from your code.

## How does it work?

Simply prepare your questions with our simple markup language. Below you can see survey example.

<sub>


```
?image=LogoImage1.jpg
	align=left


?text=Name__________________________________              Date____________

#What is Aspose.OMR main function?
	() OCR () Capture human-marked data
	() There is no main function () Enhance images
#Can Aspose.OMR process not only scans, but also photos?
	() Yes, indeed! () No
#Aspose.OMR is available on any platform, because it is:
	() Cross-platform code () Cloud service
#Aspose.OMR works with any kind of OMR forms: tests, exams, questionnaires, surveys, etc.
	() Yes, indeed! () No
...

?text=						Answer sheet section

?answer_sheet=MainQuestions
	elements_count=50
	columns_count=5

?text=Sign________________________________

?image=LogoImage2.png
	align=right
```
</sub>

## What do you get?

You’ll get a nice and sharp survey ready to print!

<img src="TestFiles/Demonstration/GeneratedImage.png" width=400>

ETA: December 2017

## What is next?

Simply make mobile snapshots or scan filled forms, upload them into OMR.Client or call API and you have the results!

<img src="TestFiles/Demonstration/AsposePhoto.jpg" height=300> <img src="TestFiles/Demonstration/AsposeRecognition.jpg" height=300> <img src="TestFiles/Demonstration/Answers.png" height=300>


## Roadmap
In the upcoming releases, we are set to implement a number of new features:

 - [ ] support diverse interviewee’s marks (ticks, crosses, corrections, etc) on various forms with the use of neural networks;
 - [ ] simplify OMR form preparation by introducing an easy markup language. For example, to get the particular survey form ready you only need to provide questions and answers;
 - [ ] reduce your expenses by preprocessing and compressing your images;
 - [ ] enhance the client with plugins to provide additional functionality;
 - [ ] present standalone script for batch processing in Python.




## Resources

+ **Website:** [www.aspose.com](https://www.aspose.com/)
+ **Product Home:** [Aspose.OMR for Cloud](https://products.aspose.cloud/omr/cloud)
+ **Documentation:** [Aspose.OMR for Cloud Documentation](https://docs.aspose.cloud/display/omrcloud/Home)
+ **Cloud Dasboard:** [Aspose Cloud](https://dashboard.aspose.cloud/)
+ **Forum:** [Aspose.OMR for Cloud Forum](https://forum.aspose.cloud/c/omr)
+ **Blog:** [Aspose.OMR for Cloud Blog](https://blog.aspose.cloud/category/aspose-products/aspose.omr-product-family/)
+ **Nuget:** [Aspose.OMR-Cloud](https://www.nuget.org/packages/Aspose.OMR-Cloud/)
+ **OMR Client Releases:** [Github Releases](https://github.com/asposecloud/Aspose.OMR-Cloud/releases)
