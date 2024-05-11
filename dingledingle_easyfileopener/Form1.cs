///////////////////////////////////////////////////////////////////////////////////////////////////////
//Made by Pleb8000, CatEarsYT, Rockgeneral copyright blah blah blah fair use blah blah MIT something.//
///////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dingledingle_easyfileopener
{
    //------------------------------------------------------
    // Description: Class containing the WinForms API
    // interface along side the application.
    //------------------------------------------------------
    public partial class Form1 : Form
    {
        int LastSelectedIndex;
        List<folderName> folders = new List<folderName>();

        //------------------------------------------------------
        // Purpose: Form constructor.
        //------------------------------------------------------
        public Form1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------
        // Purpose: Called once WinForms API finishes loading
        // all controls/components, additionally contains the
        // code for loading the configuration.
        //------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("CustomDirectory.cfg"))
            {
                foreach (string folderData in File.ReadAllText("CustomDirectory.cfg").Split('\n'))
                {
                    if (folderData.Split('?').Length > 1)
                    {
                        folders.Add(new folderName() { Path = folderData.Split('?')[1], Name = folderData.Split('?')[0], Selected = Convert.ToBoolean(folderData.Split('?')[2]) });
                    }
                }
                comboBox1.Items.Clear();
                foreach (folderName item in folders)
                {
                    comboBox1.Items.Add(item.Name);
                }
            }
            if (folders.Count == 0)
            {
                button3.Enabled = false;
            }

            int LastSelectedIndex = GetSelectedIndex();

            if (LastSelectedIndex < comboBox1.Items.Count)
            {
                comboBox1.SelectedIndex = LastSelectedIndex;
            }

        }

        //------------------------------------------------------
        // Purpose: This is used to get the index of the item from the list that is currently marked as selected.
        //------------------------------------------------------
        private int GetSelectedIndex()
        {
            int myIndex = comboBox1.Items.Count - 1;

            foreach (folderName item in folders)
            {
                if (item.Selected == true) {
                    myIndex = folders.IndexOf(item);
                }
            }

            return myIndex;
        }

        //------------------------------------------------------
        // Purpose: This is used to set Select = true for the item in the list corresponding to the item selected in the DD
        //------------------------------------------------------
        private void SetSelectedIndex(int NewSelectedIndex)
        {
            foreach (folderName item in folders)
            {
                if (folders.IndexOf(item) == NewSelectedIndex)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }
            }
        }

        private bool FolderAlreadyExists(string DirectoryPath)
        {
            foreach (folderName item in folders)
            {
                if (item.Path == DirectoryPath)
                {
                    // DirectoryPath is already in the list.
                    return true;
                }
            }

            // If we made it here, the DirectoryPath does NOT already exist in the list.
            return false;
        }

        //------------------------------------------------------
        // Purpose: Code for the "Open" button.
        //------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
            {
                Process.Start("explorer.exe", folders[comboBox1.SelectedIndex].Path);
            }
        }

        //------------------------------------------------------
        // Purpose: Code for the "Add" button.
        //------------------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            if (folderBrowserDialog.SelectedPath != string.Empty)
            {
                if (FolderAlreadyExists(folderBrowserDialog.SelectedPath) == true)
                {
                    MessageBox.Show("The folder you selected is already in the list.\n\nFolder addition cancelled", "Duplicate Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                folders.Add(new folderName() { Path = folderBrowserDialog.SelectedPath, Name = new DirectoryInfo(folderBrowserDialog.SelectedPath).Name });
                comboBox1.Items.Clear();
                foreach (folderName item in folders)
                {
                    comboBox1.Items.Add(item.Name);
                }
                comboBox1.SelectedIndex = folders.Count - 1;
            }
            if (folders.Count != 0)
            {
                button3.Enabled = true;
            }
        }

        private void AddFolder(string DirectoryName, string DirectoryPath)
        {
            if (DirectoryPath != string.Empty)
            {
                {
                    folders.Add(new folderName() { Path = DirectoryPath, Name = DirectoryName });
                    comboBox1.Items.Clear();
                    foreach (folderName item in folders)
                    {
                        comboBox1.Items.Add(item.Name);
                    }
                    comboBox1.SelectedIndex = folders.Count - 1;
                }
                if (folders.Count != 0)
                {
                    button3.Enabled = true;
                }
            }
        }

        //------------------------------------------------------
        // Purpose: Called once WinForms API is finished
        // unloading all controls/conponents, additionally
        // contains code for saving configuration and other
        // conditional states.
        //------------------------------------------------------
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.Delete("CustomDirectory.cfg");
            foreach (folderName item in folders)
            {

                File.AppendAllText("CustomDirectory.cfg", item.Name + "?" + item.Path + "?" + item.Selected.ToString() + "\n");

            }
        }

        //------------------------------------------------------
        // Purpose: Code for the "Delete" button.
        //------------------------------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            if (folders.Count > 0)
            {
                folders.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.Items.Clear();
                foreach (folderName item in folders)
                {
                    comboBox1.Items.Add(item.Name);
                }
            }
            if (folders.Count == 0)
            {
                button3.Enabled = false;
            }
            else
            {
                comboBox1.SelectedIndex = folders.Count - 1;
            }
        }

        //------------------------------------------------------
        // Purpose: Code for the "Rename Entry" button.
        //------------------------------------------------------
        private void renameButton_Click(object sender, EventArgs e)
        {
            if (folders.Count > 0 && textBox1.Text != string.Empty)
            {
                folders[comboBox1.SelectedIndex].Name = textBox1.Text;
                comboBox1.Items.Clear();
                foreach (folderName item in folders)
                {
                    comboBox1.Items.Add(item.Name);
                }
                comboBox1.SelectedIndex = folders.Count - 1;
            }
            textBox1.Text = string.Empty;
        }

        //------------------------------------------------------
        // Purpose: Code for the "Edit Directory" button.
        //------------------------------------------------------
        private void editButton_Click(object sender, EventArgs e)
        {
            if (comboBox1.Items.Count > 0)
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.ShowDialog();
                if (folderBrowserDialog.SelectedPath != string.Empty)
                {
                    folders[comboBox1.SelectedIndex].Path = folderBrowserDialog.SelectedPath;
                    comboBox1.Items.Clear();
                    foreach (folderName item in folders)
                    {
                        comboBox1.Items.Add(item.Name);
                    }
                    comboBox1.SelectedIndex = folders.Count - 1;
                }
            }
        }

        //------------------------------------------------------
        // Purpose: Code for the text box.
        //------------------------------------------------------
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                renameButton.Enabled = false;
            }
            else
            {
                renameButton.Enabled = true;
            }

        }

        //------------------------------------------------------
        // Purpose: Saves partial state of combo box.
        //------------------------------------------------------
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LastSelectedIndex = comboBox1.SelectedIndex;

            SetSelectedIndex(LastSelectedIndex);
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            Console.WriteLine(EventString("label1_DragDrop"));
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                // Warn the user if they have dragged more than 1 item
                if (files.Length > 1)
                {
                    MessageBox.Show("You dragged more then one item.  Only one directory will be added:" + "\n" + files[0].ToString(), "Warning");
                }

                string DirectoryPath = GetDirectoryFromPath(files[0].ToString());

                if (FolderAlreadyExists(DirectoryPath) == true)
                {
                    MessageBox.Show("The folder you selected is already in the list.\n\nFolder Addition Cancelled", "Duplicate Folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine(DirectoryPath);

                string DirectoryName = new DirectoryInfo(DirectoryPath).Name;

                AddFolder(DirectoryName, DirectoryPath);

            }
        }

        private void label1_DragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine(EventString("label1_DragEnter"));
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        static string GetDirectoryFromPath(string MyPath)
        {
            FileAttributes attr = File.GetAttributes(MyPath);

            if (attr.HasFlag(FileAttributes.Directory))
                return MyPath;
            else
                MyPath = Path.GetDirectoryName(MyPath);
                return MyPath;
        }

        static string EventString(string EventName)
        {
            string eventstr = DateTime.Now.ToString() + " : " + EventName;
            return eventstr;
        }
    }

    //------------------------------------------------------
    // Purpose: Structure containing directory
    // information.
    //------------------------------------------------------
    class folderName
    {
        public string Name;
        public string Path;
        public bool Selected;
    }
}