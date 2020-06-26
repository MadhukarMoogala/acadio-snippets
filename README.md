### AcadIO-Snippets
This repo consists of my answers on Stackoverflow for question regarding Forge Design Automation.
And, other snippets.
To use the snippets, first get comfortable with Forge 
[Forge WorkFlow](https://github.com/szilvaa/acadio-tutorials/blob/master/tutorial1/readme.md)

These snippets consists of mainly 
- Workitem JSON
- Activity JSON
- Source of custom ARX | NET Module if any

### Prerequisites
1. **Forge Account**: Learn how to create a Forge Account, activate subscription and create an app at [this tutorial](http://learnforge.autodesk.io/#/account/). 
2. **Visual Code**: Visual Code (Windows or MacOS)
3. **.netcore 3.1**: [dotnet core SDK](https://dotnet.microsoft.com/download/dotnet-core/current/runtime) 
4. **7z** [7z Zip Archive](https://www.7-zip.org/download.html)
5. Make sure `7z.exe` is available on your system path env.

`git clone --single-branch --branch v3 https://github.com/MadhukarMoogala/acadio-snippets.git`



### Instructions To Build  and Test Forge DA Client

This is Design Automation client is used to run forge Design Application, for this converting a drawing file to dxf is taken as an example to demonstrate the working of DA client, DA client is highly customizable CLI utility can be used for various DA apps.
At a broad level,
- inputs are uploaded to OSS, 
- generates a readable signed url
- output objects are created writable signed url
- urls are set in workItem spec


```bash
cd acadio-snippets\NETCore\ClientV3
touch appsettings.users.json `feed with your Forge Credentials`
dotnet build
dotnet run -c RELEASE -i "<inputDrawing>" -o "<outputFolder>"
```
`appsettings.users.json`

```
{
  "Forge": {
    "ClientId": "ForgeClientId",
    "ClientSecret": "ForgeClientSecret"
  }
}
```

`launchsettings.json`

```
{
  "profiles": {
    "ClientV3": {
      "commandName": "Project",
      "commandLineArgs": "-i \"D:\\Work\\Forge\\acadio-snippets\\Drawings\\blocks_and_tables_-_metric.dwg\" -o D:\\Work\\Forge\\acadio-snippets\\Drawings\"",
      "workingDirectory": "D:\\Work\\Forge\\acadio-snippets\\NETCore\\ClientV3",
      "environmentVariables": {
        "FORGE_CLIENT_SECRET": "",
        "FORGE_CLIENT_ID": ""
      }
    }
  }
}
```




#### Instructions To Debug

##### .NETCore
```
edit launchsettings.json
launch ClientV3 profile.

```

### License
This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.

### Written by
Madhukar Moogala, [Forge Partner Development](http://forge.autodesk.com)  @galakar







