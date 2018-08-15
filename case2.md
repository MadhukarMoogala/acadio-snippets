# [Dwg comparison through design automation](https://stackoverflow.com/questions/51615529/dwg-comparison-through-design-automation)

**Post Workitem**
```json
{
	"Arguments": {
		"InputArguments": [
			{
				"Resource": "https://madhukar-fda.s3.us-west-2.amazonaws.com/Kitchens1.dwg",
				"Name": "HostDwg"
			},
			{
				"Resource": "https://madhukar-fda.s3.us-west-2.amazonaws.com/Kitchens2.dwg",
				"Name": "ToCompareWith"
			}
		],
		"OutputArguments": [
			{
				"Name": "Result",
				"HttpVerb": "POST"
			}
		]
	},
	"ActivityId": "FPDCompare"
}
```
**Custom Activity**
```json
{
	"HostApplication": "",
	"RequiredEngineVersion": "23.0",
	"Parameters": {
		"InputParameters": [
			{
				"Name": "HostDwg",
				"LocalFileName": "$(HostDwg)"
			},
			{
				"Name": "ToCompareWith",
				"LocalFileName": "ToCompareWith.dwg"
			}
		],
		"OutputParameters": [
			{
				"Name": "Result",
				"LocalFileName": "output.dwg"
			}
		]
	},
	"Instruction": {
		"CommandLineParameters": null,
		"Script": "COMPAREINPLACE\nON\n-COMPARE\n\nToCompareWith.dwg\n_SAVEAS\n\noutput.dwg\n"
	},
	"Id": "FPDCompare"
}
```

