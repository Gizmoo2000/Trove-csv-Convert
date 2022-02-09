using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Trove_JSON_Convert__WPF_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string JSON_file { get; private set; }
        

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void select_JSON_file_button_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            string starting_directory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);        // GetEnvironmentVariable("AppDataFolder");
            dlg.InitialDirectory = starting_directory;

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                JSON_file = dlg.FileName;
                file_name_textBlock.Text = JSON_file;
                status_label.Content = "Select Convert to convet to CSV";
                convert_button.IsEnabled = true;
            }
        }

        private void convert_button_Click(object sender, RoutedEventArgs e)
        {
            // Column headers
            string header_string0 = "\"Account\",\"Last Update\",\"OwnerId\",\"ItemId\",\"ItemIdHex\",\"Character Name\",\"Location\",\"Tab\",";
            string header_string1 = "\"Row\",\"Column\",\"WeenieId\",\"Item Type\",\"Bag Name\",\"Quantity\",";
            string header_string2 = "\"Binding\",\"Item Name\",\"Description\",\"Minimum Level\",\"Hardness\",\"Item Type\",\"Proficiency\",";
            string header_string3 = "\"Weapon Type\",\"Armor Type\",\"Item Sub Type\",\"EquipToFlags\",\"Equips To\",\"Plat Value\",\"Gold Value\",";
            string header_string4 = "\"Silver Value\",\"Copper Value\",\"Bonus Set\",\"Set Description\"";

            string account_name = "";
            string world_name = "";

            List<AccountHashObject> account_hash = new List<AccountHashObject>();

            // ++++++++++++++++++++++++++++++
            // opens the CSV file and output the header
            // ++++++++++++++++++++++++++++++
            // create output file names
            string CSV_string = JSON_file.Replace("json", "csv");   // build the path and name fop the csv file
            output_textBlock.Text = CSV_string;                     // display the name in the textbox

            // open output CSV file to output the header if the header box is checked
            try
            {
                if (add_header_checkBox.IsChecked == true)     // create a csv file with the header info
                {
                    StreamWriter output_csv_file_fs = new StreamWriter(CSV_string, false);     
                    output_csv_file_fs.WriteLine(header_string0 + header_string1 + header_string2 + header_string3 + header_string4);  //insert column header
                    output_csv_file_fs.Close();
                }
                else      // create an empty csv file
                {
                    StreamWriter output_csv_file_fs = new StreamWriter(CSV_string, false);
                    output_csv_file_fs.Close();
                }
            }
            catch (Exception)
            {
                write_error_textBox.Text = "Error: CSV create";
                return;
            }

            /* ++++++++++++++++++++++++++++++++++++
            // get json account hash data if it exist
            // ++++++++++++++++++++++++++++++++++++ */
            if (File.Exists("account_hash.json"))     //if account data exist, get it
            {
                string json_account_string = "";
                try
                {
                    StreamReader account_stream_fs = new StreamReader("account_hash.json");
                    json_account_string = account_stream_fs.ReadToEnd();
                    account_stream_fs.Close();
                }
                catch (Exception)
                {
                    write_error_textBox.Text = "Error: account read";
                    Close ();
                }
                if (json_account_string != null)
                {
                    List<AccountHashObject> account_Hash_local = JsonConvert.DeserializeObject<List<AccountHashObject>>(json_account_string);
                    foreach (var hash in account_Hash_local)
                    {
                        account_hash.Add(hash);
                    }
                }
            }
            /* +++++++++++++++++++++++++++++
            get the JSON data
            +++++++++++++++++++++++++++++
            load raw JSON file into JSON_string */
            string JSON_string = "";
            // read JSON file
            try
            {
                StreamReader json_stream_fs = new StreamReader(JSON_file);
                JSON_string = json_stream_fs.ReadToEnd();
                json_stream_fs.Close();
            }
            catch (Exception)
            {
                write_error_textBox.Text = "Error: JSON read";
                Close ();
            }

            // convert JSON string to object
            Rootobject? trove_data = JsonConvert.DeserializeObject<Rootobject>(JSON_string);
            // ++++++++++++++++++++++++++++
            // read and output JSON data to csv format
            // ++++++++++++++++++++++++++++
            if (trove_data != null) {
                // cycle through each charter
                foreach (var items in trove_data.Characters)
                {
                    // check if there is a nick name for the hash number
                    account_name = "";
                    if (account_hash != null)       // make sure the hash table is not empty
                    {
                        foreach (var hash in account_hash)
                        {
                            if(hash.account_hash == items.SubscriptionKeyHash)
                            {
                                account_name = hash.account_name;
                            }
                        }
                    }
                    // if there is not a nick name, ask for one
                    if (account_name == "")
                    {
                        AccountHashObject? temp_holding = new AccountHashObject();
                        // ++++++++++++++++++++++++++
                        //ask for account name if not found
                        // ++++++++++++++++++++++++++
                        Trove_JSON_Convert__WPF_.Window1 window1 = new Trove_JSON_Convert__WPF_.Window1();
                        window1.name_textBox.Text = items.Name;
                        window1.ShowDialog();
                        account_name = window1.return_account;
                        if (account_name == "error")
                        {
                            temp_holding.account_name = "Error";        // user did not enter a nick name
                        }
                        else
                        {
                            temp_holding.account_name = account_name;
                        }
                        temp_holding.account_hash = items.SubscriptionKeyHash;
                        account_hash.Add(temp_holding);            
                    }
                    string last_updated = items.LastUpdated.ToString();

                    // cycle through the inventory items
                    StringBuilder format_string = new StringBuilder();
                    if (items.Inventory != null) {
                        // get the Inventory items
                        foreach(var inventory in items.Inventory)
                        {
                            format_string.Append(format_csv(account_name, false));
                            format_string.Append(format_csv(last_updated,false));
                            format_string.Append(format_csv(inventory.OwnerId.ToString(), false));
                            format_string.Append(format_csv(inventory.ItemId.ToString(), false));
                            format_string.Append(format_csv(inventory.ItemIdHex, false));
                            format_string.Append(format_csv(inventory.CharacterName, false));
                            format_string.Append(format_csv(inventory.Container, false));                //Inventory, Equipped, or inventory
                            format_string.Append(format_csv(inventory.Tab.ToString(), false));
                            format_string.Append(format_csv(inventory.Row.ToString(), false));
                            format_string.Append(format_csv(inventory.Column.ToString(), false));
                            format_string.Append(format_csv(inventory.WeenieId.ToString(), false));
                            format_string.Append(format_csv(inventory.TreasureType, false));
                            format_string.Append(format_csv(inventory.BagName, false));
                            format_string.Append(format_csv(inventory.Quantity.ToString(), false));
                            format_string.Append(format_csv(inventory.Binding, false));
                            format_string.Append(format_csv(inventory.Name, false));
                            format_string.Append(format_csv(inventory.Description, false));
                            format_string.Append(format_csv(inventory.MinimumLevel.ToString(), false));
                            format_string.Append(format_csv(inventory.Hardness.ToString(), false));
                            format_string.Append(format_csv(inventory.ItemType, false));
                            format_string.Append(format_csv(inventory.Proficiency, false));
                            format_string.Append(format_csv(inventory.WeaponType, false));
                            format_string.Append(format_csv(inventory.ArmorType, false));
                            format_string.Append(format_csv(inventory.ItemSubType, false));
                            format_string.Append(format_csv(inventory.EquipsToFlags.ToString(), false));
                            if (inventory.EquipsTo != null)
                            {
                                format_string.Append(format_csv(String.Join(",", inventory.EquipsTo.ToArray()), false));
                            }
                            else
                            {
                                format_string.Append(format_csv("", false));
                            }
                            format_string.Append(format_csv(inventory.PlatValue.ToString(), false));
                            format_string.Append(format_csv(inventory.GoldValue.ToString(), false));
                            format_string.Append(format_csv(inventory.SilverValue.ToString(), false));
                            format_string.Append(format_csv(inventory.CopperValue.ToString(), false));
                            format_string.Append(format_csv(inventory.SetBonus1Name, false));

                            if (inventory.SetBonus1Description != null)
                            {
                                format_string.Append(format_csv(String.Join(",", inventory.SetBonus1Description.ToArray()), true));
                            }
                            else
                            {
                                format_string.Append(format_csv("", true));
                            }
                            // end of line, insert a CR
                            format_string.Append("\r\n");
                        }
                    }
                    
                    // cycle through the charters Bank items
                    if (items.Bank != null)
                    {
                        foreach (var bank in items.Bank)
                        {
                            format_string.Append(format_csv(account_name, false));              // 1
                            format_string.Append(format_csv(last_updated, false));              // 2
                            format_string.Append(format_csv(bank.OwnerId.ToString(), false));   // 3
                            format_string.Append(format_csv(bank.ItemId.ToString(), false));    // 4
                            format_string.Append(format_csv(bank.ItemIdHex, false));            // 5
                            format_string.Append(format_csv(bank.CharacterName, false));        // 6
                            format_string.Append(format_csv(bank.Container, false));            // 7 bank, Equipped, or bank
                            format_string.Append(format_csv(bank.Tab.ToString(), false));       // 8
                            format_string.Append(format_csv(bank.Row.ToString(), false));       // 9
                            format_string.Append(format_csv(bank.Column.ToString(), false));    // 10
                            format_string.Append(format_csv(bank.WeenieId.ToString(), false));  // 11
                            format_string.Append(format_csv(bank.TreasureType, false));         // 12
                            format_string.Append(format_csv(bank.BagName, false));              // 13
                            format_string.Append(format_csv(bank.Quantity.ToString(), false));  // 14
                            format_string.Append(format_csv(bank.Binding, false));              // 15
                            format_string.Append(format_csv(bank.Name, false));                 // 16
                            format_string.Append(format_csv(bank.Description, false));          // 17
                            format_string.Append(format_csv(bank.MinimumLevel.ToString(), false));  // 18
                            format_string.Append(format_csv(bank.Hardness.ToString(), false));  // 19
                            format_string.Append(format_csv(bank.ItemType, false));             // 20
                            format_string.Append(format_csv(bank.Proficiency, false));          // 21
                            format_string.Append(format_csv(bank.WeaponType, false));           // 22
                            format_string.Append(format_csv(bank.ArmorType, false));            // 23
                            format_string.Append(format_csv(bank.ItemSubType, false));          // 24
                            format_string.Append(format_csv(bank.EquipsToFlags.ToString(), false));     // 25
                            if (bank.EquipsTo != null)
                            {
                                format_string.Append(format_csv(String.Join(",", bank.EquipsTo.ToArray()), false));
                            }
                            else
                            {
                                format_string.Append(format_csv("", false));
                            }
                            format_string.Append(format_csv(bank.PlatValue.ToString(), false));     // 26
                            format_string.Append(format_csv(bank.GoldValue.ToString(), false));     // 27
                            format_string.Append(format_csv(bank.SilverValue.ToString(), false));   // 28
                            format_string.Append(format_csv(bank.CopperValue.ToString(), false));   // 29
                            format_string.Append(format_csv(bank.SetBonus1Name, false));            // 30
                            if (bank.SetBonus1Description != null)                                  // 31
                            {
                                format_string.Append(format_csv(String.Join(",", bank.SetBonus1Description.ToArray()), true));
                            }
                            else
                            {
                                format_string.Append(format_csv("", true));
                            }
                            // end of line, insert a CR
                            format_string.Append("\r\n");
                            }
                        }
                     
                    try
                    {
                        StreamWriter output_csv_file_fs = new StreamWriter(CSV_string, true);
                        output_csv_file_fs.Write(format_string.ToString());  // output a charters items from the bank and inventory
                        output_csv_file_fs.Close();
                    }
                    catch (Exception)
                    {
                        write_error_textBox.Text = "Error: Write CSV";
                        return;
                    }
                }
                
            }

            status_label.Content = "CSV File created";

            // ++++++++++++++++
            // add clear the account json file and down load the new hash account name info
            // ++++++++++++++++
            try
            {
                StreamWriter account_stream3_fs = new StreamWriter("account_hash.json", false);
                var account_json = JsonConvert.SerializeObject(account_hash);
                account_stream3_fs.WriteLine(account_json.ToString());
                account_stream3_fs.Close();
            }
            catch (Exception)
            {
                write_error_textBox.Text = "Error: account write";
                return;
            }

        }

        public string format_csv(string item_info, bool ending)
        {
            string format_string = "";
            if (ending == false)
            {
                format_string = "\"" + item_info + "\",";
            }
            else
            {
                format_string = "\"" + item_info + "\"";
            }
            return (format_string);
        }

        public class AccountHashObject
        {
            public string? account_hash { get; set; }
            public string? account_name { get; set; }
        }

        public class Rootobject
        {
            public Character[]? Characters { get; set; }
            public string? Server { get; set; }
        }

        public class Character
        {
            public string? SubscriptionKeyHash { get; set; }
            public string? Name { get; set; }
            public DateTime? LastUpdated { get; set; }
            public Inventory[]? Inventory { get; set; }
            public Bank[]? Bank { get; set; }
        }

        public class Inventory
        {
            public long OwnerId { get; set; }
            public string? CharacterName { get; set; }
            public long ItemId { get; set; }
            public string? ItemIdHex { get; set; }
            public string? Container { get; set; }
            public int Tab { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public int WeenieId { get; set; }
            public int Quantity { get; set; }
            public string? TreasureType { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int MinimumLevel { get; set; }
            public string? Binding { get; set; }
            public string? ItemType { get; set; }
            public int Hardness { get; set; }
            public int EquipsToFlags { get; set; }
            public string[]? EquipsTo { get; set; }
            public string? IconSource { get; set; }
            public string? Proficiency { get; set; }
            public string? WeaponType { get; set; }
            public string? ArmorType { get; set; }
            public string? ItemSubType { get; set; }
            public string? BagName { get; set; }
            public int PlatValue { get; set; }
            public int GoldValue { get; set; }
            public int SilverValue { get; set; }
            public int CopperValue { get; set; }
            public string? SetBonus1Name { get; set; }
            public string[]? SetBonus1Description { get; set; }
        }

        public class Bank
        {
            public long OwnerId { get; set; }
            public string? CharacterName { get; set; }
            public long ItemId { get; set; }
            public string? ItemIdHex { get; set; }
            public string? Container { get; set; }
            public int Tab { get; set; }
            public int Row { get; set; }
            public int Column { get; set; }
            public int WeenieId { get; set; }
            public int Quantity { get; set; }
            public string? TreasureType { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public int MinimumLevel { get; set; }
            public string? Binding { get; set; }
            public string? ItemType { get; set; }
            public int Hardness { get; set; }
            public int EquipsToFlags { get; set; }
            public string[]? EquipsTo { get; set; }
            public string? IconSource { get; set; }
            public string? Proficiency { get; set; }
            public string? WeaponType { get; set; }
            public string? ArmorType { get; set; }
            public string? ItemSubType { get; set; }
            public string? BagName { get; set; }
            public int PlatValue { get; set; }
            public int GoldValue { get; set; }
            public int SilverValue { get; set; }
            public int CopperValue { get; set; }
            public string? SetBonus1Name { get; set; }
            public string[]? SetBonus1Description { get; set; }
        }

        private void exit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
