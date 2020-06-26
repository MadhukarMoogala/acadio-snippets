# Convert DWG to DXF

refer - [Convert DWG to DXF using API v3 and .Net](https://stackoverflow.com/questions/62523572/convert-dwg-to-dxf-using-api-v3-and-net)

Before you use this please complete hands on tutorial on 

https://forge.autodesk.com/en/docs/design-automation/v3/tutorials/autocad/about_this_tutorial/

This post only shows how to write activity spec and workItem spec, the rest of workflow is same for any Design Automation application as shown in this [tutorial](https://forge.autodesk.com/en/docs/design-automation/v3/tutorials/autocad/about_this_tutorial/)

### Activity Spec:

POST https://developer.api.autodesk.com/da/us-east/v3/activities

```json
{
	"id": "{{ activityId  }}",
	"commandLine": "$(engine.path)\\accoreconsole.exe /i $(args[inputFile].path) /s $(settings[script].path)",
	"parameters": {
		"inputFile": {
			"zip": false,
			"ondemand": false,
			"verb": "get",
			"description": "Host drawing",
			"localName": "$(inputFile)"
		},
		"outputFile": {
			"zip": false,
			"ondemand": false,
			"verb": "put",
			"description": "output file",
			"localName": "result.dxf",
			"required": "true"
		}
	},
	"engine": "Autodesk.AutoCAD+24",
	"settings": {
		"script": "DXFOUT\nresult.dxf\n"
	},
	"description": "AutoCAD DWG To DXF"
}
```

Using .NETCore Design Automation Client 

```cs
var activity = new Activity()
            {
               
                CommandLine = new List<string>()
                    {
                        $"$(engine.path)\\accoreconsole.exe /i $(args[inputFile].path) /s $(settings[script].path)"
                    },
                Engine = TargetEngine,
                Settings = new Dictionary<string, ISetting>()
                    {
                        { "script", new StringSetting() { Value = "DXFOUT\nresult.dxf\n\n" } }
                    },
                Parameters = new Dictionary<string, Parameter>()
                    {
                        { "inputFile", new Parameter() { Verb= Verb.Get, LocalName = "$(HostDwg)",  Required = true } },                       
                        { "outputFile", new Parameter() { Verb= Verb.Put,  LocalName = "result.dxf", Required= true} }
                    },
                Id = ActivityName
            };
            if(myApp != null)
            {
                activity.Appbundles = new List<string>()
                    {
                        myApp
                    };
            }
```

## WorkItem Spec

POST : https://developer.api.autodesk.com/da/us-east/v3/workitems

```bash
{
	"activityId": "{{ activity  }}",
	"arguments": {
		"inputFile": {
			"url": "http://download.autodesk.com/us/samplefiles/acad/blocks_and_tables_-_metric.dwg",
			"localName": "$(inputFile)",
			"verb": "get"
		},
		"outputFile": {
			"url": "https://developer.api.autodesk.com/oss/v2/signedresources/6088797f-72b7-48ad-8426-ec8cf720629e?region=US",
			"verb": "put",
			"localName": "result.dxf"
		},
		"onComplete": {
			"verb": "post",
			"url": "https://dxfout.requestcatcher.com/"
		}
	}
}
```

Using .NETCore Design Automation Client 

```csharp
var workItemStatus = await api.CreateWorkItemAsync(new Autodesk.Forge.DesignAutomation.Model.WorkItem()
            {
                ActivityId = myActivity,
                Arguments = new Dictionary<string, IArgument>() {
                              {
                               "inputFile",
                               new XrefTreeArgument() {
                                Url = DownloadUrl,
                                Verb = Verb.Get
                               }
                              }, {
                               "outputFile",
                               new XrefTreeArgument() {
                                Verb = Verb.Put, Url = UploadUrl
                               }
                              }
                             }
            });
```

