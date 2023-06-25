using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using CCEditor.ReflectionData;
using CCEditor.Classes.Master_File_Pieces;
using CCEditor.CC;
using System.IO.Pipes;

namespace CCEditor
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            this.materialMultiLineTextBox1.AppendText("==============[CC Editor Started]=============");
        }

        private void languageBtn_Click(object sender, EventArgs e)
        {

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "mtb files (*.mtb)|*.mtb|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    // Determine the length of the file
                    int fileSize = (int)fileStream.Length;

                    // Create a byte array to store the file data
                    byte[] fileData = new byte[fileSize];

                    // Read all bytes from the file stream into the byte array
                    fileStream.Read(fileData, 0, fileSize);

                    LazySerializableSequence<FileComponent> lazySerializableSequence = SerializeManager.LoadRawLazyFile<FileComponent>(fileData);
                    foreach (LazySerializableElement<FileComponent> element in lazySerializableSequence.contentElements)
                    {
                        this.materialMultiLineTextBox1.AppendText(element.ToString());
                    }
                    // Close the file stream
                    fileStream.Close();
                }
            }
            

        }
    }
}
