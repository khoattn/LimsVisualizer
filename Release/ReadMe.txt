-------------------------------------------------------------
-------------------------------------------------------------
| |                Lims Visualizer v1.1.0.0               | |
| |      Compatible with Davis 5 v 2.10.310 Releases      | |
-------------------------------------------------------------
-------------------------------------------------------------

PREREQUISITES:
- .NET Framework 4
- Microsoft Excel 2010
-------------------------------------------------------------
INSTRUCTIONS:
- Setup Data Monitoring within Davis 5
- Copy the "File Location" Path from Davis 5 and insert it in
  the Textbox within Lims Visualizer
- Select the time range, how often Lims Visualizer should
  check for new files
- Press [Start]
-> As soon as a new file will be added to the specified
  folder a instance of Microsoft Excel will appear and the
  measurement data will be added.
-------------------------------------------------------------
NOTES:
- If there are allready files stored in the specified folder
  they will all be added as soon as you press [Start]
- Old files will be deleted after they were parsed
  successfully
- Lims Visualizer can currently handle the data of one
  measuring line only
- Never define the same folder for two different lines,
  otherwise a new instance of Microsoft Excel will be opened
  for each file.
- Log Files can be found under "%TEMP%\LimsVisualizer"
-------------------------------------------------------------
CHANGE LOG:
v1.1.0.0
Compatible with Davis 5 v 2.10.310 Releases
- Adapted Type Document to new XML file format

v1.0.1.0
Compatible with Davis 5 v 2.10.310 Releases
- Adapted XML-Paths
- Added key shortcuts for [Start], [Stop] and [Browse]

v1.0.0.0
Compatible with Davis 5 v 2.10.300 Releases
- Initial Release