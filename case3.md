# [Custom plot with pc3](https://stackoverflow.com/questions/51383783/custom-plot-with-pc3)

## Create Activity
```json
{
    "HostApplication": "",
    "RequiredEngineVersion": "22.0",
    "Parameters": {
        "InputParameters": [
            {
                "Name": "HostDwg",
                "LocalFileName": "$(HostDwg)"
            },
            {
                "Name": "CustomPC3",
                "LocalFileName": "custom.pc3"
            }
        ],
        "OutputParameters": [
            {
                "Name": "Result",
                "LocalFileName": "result.pdf"
            }
        ]
    },
    "Instruction": {
        "CommandLineParameters": null,
        "Script": "FPDPLOT\ncustom.pc3\nresult.pdf\n"
    },
    "AppPackages":["CustomPlotter"],
    "Version": 1,
    "Id": "CustomPlotter"
}
```
## Post WorkItem
```json
{
    "Arguments": {
        "InputArguments": [
            {
                "Resource":"https://madhukar-fda.s3.us-west-2.amazonaws.com/Kitchens.dwg",
                "Name": "HostDwg"
            },
            {
                "Resource": "https://madhukar-fda.s3.us-west-2.amazonaws.com/Custom.pc3",
                "Name": "CustomPC3"
            }
        ],
        "OutputArguments": [
            {
                "Name": "Result",
                "HttpVerb": "POST"
            }
        ]
    },
    "ActivityId": "CustomPlotter"
}
```
## Custom Plotter >NET CSharp Code
```csharp
public static void PlotLayout()
{
    // Get the current document and database, and start a transaction

    Document acDoc = Application.DocumentManager.MdiActiveDocument;
    Database acCurDb = acDoc.Database;
    PromptResult result = acDoc.Editor.GetString("Enter PC3 File:");
    if (result.Status != PromptStatus.OK) return;
    string pc3FileName = result.StringResult;
    result = acDoc.Editor.GetString("Enter PDF Name :");
    if (result.Status != PromptStatus.OK) return;
    string pdfFileName = result.StringResult;

    using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
    {
        // Reference the Layout Manager
        LayoutManager acLayoutMgr = LayoutManager.Current;

        // Get the current layout and output its name in the Command Line window
        Layout acLayout = acTrans.GetObject(acLayoutMgr.GetLayoutId(acLayoutMgr.CurrentLayout),
                                            OpenMode.ForRead) as Layout;
        //Here we are setting custom config file
        PlotConfig acPlCfg = PlotConfigManager.SetCurrentConfig(pc3FileName);
        // Get the PlotInfo from the layout
        using (PlotInfo acPlInfo = new PlotInfo())
        {
            acPlInfo.Layout = acLayout.ObjectId;

            // Get a copy of the PlotSettings from the layout
            using (PlotSettings acPlSet = new PlotSettings(acLayout.ModelType))
            {
                acPlSet.CopyFrom(acLayout);

                // Update the PlotSettings object
                PlotSettingsValidator acPlSetVdr = PlotSettingsValidator.Current;

                // Set the plot type
                acPlSetVdr.SetPlotType(acPlSet, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);

                // Set the plot scale
                acPlSetVdr.SetUseStandardScale(acPlSet, true);
                acPlSetVdr.SetStdScaleType(acPlSet, StdScaleType.ScaleToFit);

                // Center the plot
                acPlSetVdr.SetPlotCentered(acPlSet, true);

                // Set the plot device to use
                acPlSetVdr.SetPlotConfigurationName(acPlSet, "custom.pc3", "ANSI_A_(8.50_x_11.00_Inches)");

                // Set the plot info as an override since it will
                // not be saved back to the layout
                acPlInfo.OverrideSettings = acPlSet;

                // Validate the plot info
                using (PlotInfoValidator acPlInfoVdr = new PlotInfoValidator())
                {
                    acPlInfoVdr.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;
                    acPlInfoVdr.Validate(acPlInfo);

                    // Check to see if a plot is already in progress
                    if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                    {
                        using (PlotEngine acPlEng = PlotFactory.CreatePublishEngine())
                        {
                            // Track the plot progress with a Progress dialog
                            using (PlotProgressDialog acPlProgDlg = new PlotProgressDialog(false, 1, true))
                            {
                                using ((acPlProgDlg))
                                {
                                    // Define the status messages to display 
                                    // when plotting starts
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Plot Progress");
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");

                                    // Set the plot progress range
                                    acPlProgDlg.LowerPlotProgressRange = 0;
                                    acPlProgDlg.UpperPlotProgressRange = 100;
                                    acPlProgDlg.PlotProgressPos = 0;

                                    // Display the Progress dialog
                                    acPlProgDlg.OnBeginPlot();
                                    acPlProgDlg.IsVisible = true;

                                    // Start to plot the layout
                                    acPlEng.BeginPlot(acPlProgDlg, null);

                                    // Define the plot output
                                    acPlEng.BeginDocument(acPlInfo, acDoc.Name, null, 1, true, pdfFileName);

                                    // Display information about the current plot
                                    acPlProgDlg.set_PlotMsgString(PlotMessageIndex.Status, "Plotting: " + acDoc.Name + " - " + acLayout.LayoutName);

                                    // Set the sheet progress range
                                    acPlProgDlg.OnBeginSheet();
                                    acPlProgDlg.LowerSheetProgressRange = 0;
                                    acPlProgDlg.UpperSheetProgressRange = 100;
                                    acPlProgDlg.SheetProgressPos = 0;

                                    // Plot the first sheet/layout
                                    using (PlotPageInfo acPlPageInfo = new PlotPageInfo())
                                    {
                                        acPlEng.BeginPage(acPlPageInfo, acPlInfo, true, null);
                                    }

                                    acPlEng.BeginGenerateGraphics(null);
                                    acPlEng.EndGenerateGraphics(null);

                                    // Finish plotting the sheet/layout
                                    acPlEng.EndPage(null);
                                    acPlProgDlg.SheetProgressPos = 100;
                                    acPlProgDlg.OnEndSheet();

                                    // Finish plotting the document
                                    acPlEng.EndDocument(null);

                                    // Finish the plot
                                    acPlProgDlg.PlotProgressPos = 100;
                                    acPlProgDlg.OnEndPlot();
                                    acPlEng.EndPlot(null);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
```