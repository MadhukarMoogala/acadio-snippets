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

Using >NET Design Automation Client 

