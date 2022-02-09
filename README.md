# Trove-JSON-Convert
This is a utility to be used in conjunction with the Dungeon Helper Program and the plugin Trove developed by Morrikan & RabidSquirrel. (https://dungeonhelper.com) 

Dungeon Helper is a plugin hosting framework. By itself, it doesn't really do anything. It enables developers to write plugins to do cool things inside the game Dungeons & Dragons Online (https://www.ddo.com). Examples include Trove, an inventory tracking application, which this utility is to be used with.

The Trove plugin collects all the charters inventory items across all there accounts and stores them in a json file on the users computer. (Please read https://dungeonhelper.com to learn how to install and use Dungeon Helper)

The purpose of this utility is for the user to build a spreadsheet or database front end they are familiar with, then use this utility to convert the Trove inventory from a json file format to a csv file format. Then simply import the inventory from the created csv file for their DDO accounts. (Note: This is not a link to the json inventory file for your front end, you will need to purge and reload the inventory anytime you update the Trove json file.) 

Sarlona.csv is a sample csv output file with column headers
Trove CSV (x64).msi is a x64 build install file.

The output csv file is placed in the same directory as the json file that was selected to be converted. Under the Convert button it will display the output csv file name and location when it is done converting the data. Note: this utility is read only for the json file and does not make any changes to the original json file.

The csv output includes all fields from the Trove json file except for the icon image, (this is a json encoded text field) and the account hash is replace with the nickname you enter the first time the program encounters a new account hash. 

NOTE: The included MSI file is a 64 bit build. 
This is a VS2022 WPF project using NET 6, C# 10 and Newtonsoft.Json. 
It has been tested it on windows 10 and 11, it will ask you to download the “.Net Desktop Runtime 6 (x64)” from Microsoft if you do not already have it. 

Thanks to Morrikan & RabidSquirrel for the info used in building this utility and the awesome work they are doing on the Dungeon Helper project. I are not affiliated with the VoK team in any way. This is just a project I build for myself and wanted to share with other users of Trove. Please do not bother Morrikan & RabidSquirrel, They are extremely busy working on the awesome DH project which we all enjoy so much. If you have any questions or problems, Please contact me at gizmoo2000@tutanota.com or DM me on Discord at Gizmoo2000.

Instructions.

1. click the “Select JSON File” button and select your file.
2. check if you want column headers in the output csv file.
3. Click the “Convert JSON to CSV” button.
3a.	When the Utility encounters a new account hash during the conversion, a dialog box will pop up asking you for a nickname to use for that account. This can be anything you want to call the account. The utility will only do this one time for each account and will store the nickname in the 
4. the box below with display the path and file name of the csv file.

