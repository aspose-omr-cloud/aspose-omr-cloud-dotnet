# Com.Aspose.Omr..OmrApi

All URIs are relative to *https://api.aspose.cloud/v1.1*

Method | HTTP request | Description
------------- | ------------- | -------------
[**PostRunOmrTask**](OmrApi.md#postrunomrtask) | **POST** /omr/{name}/runOmrTask | Run specific OMR task


<a name="postrunomrtask"></a>
# **PostRunOmrTask**
> OMRResponse PostRunOmrTask (string name, string actionName, OMRFunctionParam param, string storage, string folder)

Run specific OMR task

### Example
```csharp
using System;
using System.Diagnostics;
using Com.Aspose.Omr.Api;
using Com.Aspose.Omr.Client;
using Com.Aspose.Omr.Model;

namespace Example
{
    public class PostRunOmrTaskExample
    {
        public void main()
        {
            
            // You can acquire App SID and App Key by registrating at Aspose Cloud Dashboard https://dashboard.aspose.cloud
            string APP_KEY = "xxxxx";
            string APP_SID = "xxxxx"

            var apiInstance = new OmrApi(APP_KEY, APP_SID, "https://api.aspose.cloud/v1.1");
            var name = name_example;  // string | Name of the file to recognize.
            var actionName = actionName_example;  // string | Action name ['CorrectTemplate', 'FinalizeTemplate', 'RecognizeImage']
            var param = new OMRFunctionParam(); // OMRFunctionParam | Function params, specific for each actionName (optional) 
            var storage = storage_example;  // string | Image's storage. (optional) 
            var folder = folder_example;  // string | Image's folder. (optional) 

            try
            {
                // Run specific OMR task
                OMRResponse result = apiInstance.PostRunOmrTask(name, actionName, param, storage, folder);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling OmrApi.PostRunOmrTask: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **name** | **string**| Name of the file to recognize. | 
 **actionName** | **string**| Action name [&#39;CorrectTemplate&#39;, &#39;FinalizeTemplate&#39;, &#39;RecognizeImage&#39;] | 
 **param** | [**OMRFunctionParam**](OMRFunctionParam.md)| Function params, specific for each actionName | [optional] 
 **storage** | **string**| Image&#39;s storage. | [optional] 
 **folder** | **string**| Image&#39;s folder. | [optional] 

### Return type

[**OMRResponse**](OMRResponse.md)

### Authorization

Library uses OAUTH2 authorization internally

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

