using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace CodeEditor
{
    public partial class FM_CodeEditorMain : Form
    {
        public string FileTextRaw = string.Empty;
        public string CurrentFilePath = string.Empty;
        public string CurrentFileName = string.Empty;
        public string CurrentFileExt = string.Empty;

        public FM_CodeEditorMain()
        {
            InitializeComponent();
        }


        //When the open file option is selected in the editor
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try 
            {
                //Will Open the file Dialog to let the user select thier own file to load into the editor.
                openFileDialog_Editor.Title = "Open File to Edit";
                DialogResult result =  openFileDialog_Editor.ShowDialog();
                //If A File is selected then Process the file to load into the application
                if (result == DialogResult.OK) 
                {
                    //Confirms if the file exist and has access to the file.
                    if (File.Exists(openFileDialog_Editor.FileName))
                    {
                        //Load the text into the RawText value
                        FileTextRaw = File.ReadAllText(openFileDialog_Editor.FileName).ToString();
                        CurrentFilePath = openFileDialog_Editor.FileName;
                        CurrentFileName = openFileDialog_Editor.SafeFileName;         
                        //Load the Raw text into the RTB in the Editor
                        RTB_MainEditor.Text = FileTextRaw;
                    }
                }
            }
            catch { MessageBox.Show("Error Opening File"); }
        }
        //Save to Current file or open the save as window if new file
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try 
            {
                if (CurrentFilePath != string.Empty)
                {
                    File.WriteAllText(CurrentFilePath, FileTextRaw);
                }
                else 
                {
                    SaveFileAS();
                }
            }
            catch { MessageBox.Show("Error Saving File"); }
        }

        //Updates When Text is changed in the Editor
        private void RTB_MainEditor_TextChanged(object sender, EventArgs e)
        {
            try 
            { 
                //Updates the Raw Text Value when updated.
                FileTextRaw = RTB_MainEditor.Text; 
            }
            catch { MessageBox.Show("Error Updating Text"); }
        }

        //Function for Save As to File
        private void SaveFileAS() 
        {
            try 
            {
                saveFileDialog_Editor.Title = "Save File As";
                if (CurrentFilePath != String.Empty) 
                { 
                    saveFileDialog_Editor.FileName = CurrentFileName; 
                }
                DialogResult result = saveFileDialog_Editor.ShowDialog();
                if (result == DialogResult.OK) 
                {
                    File.WriteAllText(saveFileDialog_Editor.FileName, FileTextRaw);
                }
            }
            catch { MessageBox.Show("Error Saving As"); }
        }



        //Opens the Save As Fuction when Selected.
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAS();
        }


        private void Search(string Value) 
        {
            try
            {
                int start = 0;
                //Check for Empty Search Data
                if (Value != string.Empty)
                {
                    //Checks if Search Data is in the Text Editor
                    if (RTB_MainEditor.Text.Contains(Value))
                    {
                        //loops though the Text 
                        while (start < RTB_MainEditor.TextLength)
                        {
                            toolStripStatusLabel_TextStatus.Text = $"Searching for: {Value}";
                            //looks for the value in the document from start point and returns the location start value
                            int wordstart = RTB_MainEditor.Find(Value, start, RichTextBoxFinds.None);
                            if (wordstart != 1 && wordstart != -1)
                            {
                                RTB_MainEditor.SelectionStart = wordstart;
                                RTB_MainEditor.SelectionLength = Value.Length;
                                RTB_MainEditor.SelectionBackColor = Color.Yellow;
                            }
                            else
                            {
                                //Breaks the loop if no value is found
                                break;
                            }
                            //Sets the Start value after last found Word
                            start += wordstart + Value.Length;

                        }



                    }
                }
            }
            catch(Exception ex) { MessageBox.Show("Error Searching" + ex); }
            //Cleans up if it fails or not
            finally 
            {
                toolStripStatusLabel_TextStatus.Text = "";
            }

        }

        //Fuction to Clear Search Data Backcolor
        private void ClearSearch() 
        {
            RTB_MainEditor.SelectionStart = 0;
            RTB_MainEditor.SelectAll();
            RTB_MainEditor.SelectionBackColor = Color.White;
        }
        //Starts the Search Proccess
        private void BTN_Search_Click(object sender, EventArgs e)
        {
            Search(TXB_SearchBox.Text);
        }

        //Clears the Highlighed Text in the document if changed
        private void TXB_SearchBox_TextChanged(object sender, EventArgs e)
        {
            ClearSearch();
        }

    }
}
