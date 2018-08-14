# [How can I create an activity for data conversion in Design Automation API?](https://stackoverflow.com/questions/51496188/how-can-i-create-an-activity-for-data-conversion-in-design-automation-api)

## PostWorkItem
```json
{
  "ActivityId": "Translate-STEP2DWG",
  "Arguments": {
    "InputArguments": [
      {
        "Resource": "https://s3.amazonaws.com/AutoCAD-Core-Engine-Services/TestDwg/3DStep.stp",
        "Name": "HostDwg"
      }
    ],
    "OutputArguments": [
      {
        "Name": "Result",
        "HttpVerb": "POST"
      }
    ]
  }
}
```
## CreateActivity
```json
{
      "Id": "Translate-STEP2DWG",
      "AppPackages": [],
      "HostApplication": "AcTranslators.exe",
      "RequiredEngineVersion": "22.0",
      "Parameters": {
        "InputParameters": [
          {
            "Name": "HostDwg",
            "LocalFileName": "source.stp"
          }
        ],
        "OutputParameters": [
          {
            "Name": "Result",
            "LocalFileName": "result.dwg"
          }
        ]
      },
      "Instruction": {
        "CommandLineParameters": "-i source.stp -o result.dwg",
        "Script": ""
      },
      "AllowedChildProcesses": [
      ],
      "IsPublic": true,
      "Version": 1,
      "Description": ""
    }

```
