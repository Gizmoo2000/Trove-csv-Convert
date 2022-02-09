# Trove-JSON-Convert
Trove json to cvs data converter. NOTE: This is a 64 bit build. if you need a 32 bit build, let me know.
This is intended to convert the data in the Trove json file to a cvs file format so that the data can be imported into any standard database or spreadsheet.
There is a check box to include a column header in the cvs output, by default is on. The output cvs file is placed in the same directory as the json file that was used. Under the Convert button it will display the file name and location when it is done.
This is a VS2022 wpf project using .NET 6, C# 10 and Newtonsoft.Json.
It has been tested it on windows 10 and 11 with no problems
it will ask you to download the .Net Desktop Runtime 6 (x64) from Microsoft on install or the first time you run it.
The cvs output includes all fields except for the icon image, (this is a json encoded field) and the account hash is replace with the nickname you enter the first time the program encounters a new account hash. If you have a need for either of these fields, let me know.
If you have any questions or problems, you can email me at gizmoo2000@tutanota.com.
