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
using System.Text.RegularExpressions;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Runtime.InteropServices;

namespace BFME2_Macro_Mapper
{
    public partial class Form1 : Form
    {
        CommonOpenFileDialog dialog;
        StringBuilder builder;
        StringBuilder builderPath;
        string gamedataPath = "gamedata.ini";
        string inputPath;
        DirectoryInfo currentDirectory;

        public string selectedDirectory;

        public string[] files;
        public string directory;
        public List<string> defines = new List<string>();
        public List<string> m_defines = new List<string>();
        public List<string> macros = new List<string>();
        public List<string> m_macros = new List<string>();

        /// <summary>Text parsed from gamedata.ini</summary>
        string[] gamedataText;
        int defineIndexStart = 0;
        int defineIndexEnd = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dialog = new CommonOpenFileDialog();
            builder = new StringBuilder();
            builderPath = new StringBuilder();
            dialog.IsFolderPicker = true;
            dialog.InitialDirectory = Directory.GetCurrentDirectory();

            textBoxOutput.Text = Path.Combine(Directory.GetCurrentDirectory(), "data", "ini");


        }

        void Run()
        {
            //GetWorkingDirectory();

            //folders = currentDirectory.GetDirectories();

            LoadMacros();
            ReplaceMacros();


            MessageBox.Show("Process Complete");
            Application.Exit();
        }

        void GetWorkingDirectory()
        {
            //selectedDirectory = Directory.GetCurrentDirectory();
            //folderBrowserDialog.SelectedPath = selectedDirectory; // check in data if this needs to be called
            //folderBrowserDialog.ShowDialog();
            //selectedDirectory = folderBrowserDialog.SelectedPath;

            currentDirectory = new DirectoryInfo(textBoxInput.Text);
            //subDirectories = Directory.GetDirectories(currentDirectory.FullName, "*", SearchOption.AllDirectories);
            files = Directory.GetFiles(currentDirectory.FullName, "*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine(files[i]);
            }
        }

        bool GetGameData()
        {
            if (File.Exists(Path.Combine(currentDirectory.FullName, gamedataPath)))
            {
                gamedataText = File.ReadAllLines(Path.Combine(currentDirectory.FullName, gamedataPath));
                Console.WriteLine("Gamedata.ini found, ready to begin");
                return true;
            }
            else
                Console.WriteLine("Gamedata.ini not found");
            return false;
        }

        void LoadMacros()
        { 
            // Seperate #defines into array
            for (int i = 0; i < gamedataText.Length; i++)
            {
                if (gamedataText[i].StartsWith("#define"))
                {
                    if (defineIndexStart == 0) defineIndexStart = i; // Get beginning index for copying
                    defines.Add(gamedataText[i]);
                    m_defines.Add(gamedataText[i].Insert(8, "M_"));
                }
                if(gamedataText[i].Contains("GAME DATA"))
                {
                    defineIndexEnd = i; // get ending index for copying
                }
            }

            // Adjust for headers
            defineIndexStart -= 1;
            defineIndexEnd -= 2;

            string str;
            for (int i = 0; i < defines.Count; i++)
            {
                str = defines[i];
                //Console.WriteLine(defines[i]);
                str = str.Remove(0, 8); // Remove "#define "
                str = str.Trim(); // Remove whitespace if any
                int index = Array.FindIndex(str.ToCharArray(), x => char.IsWhiteSpace(x)); // Get first whitespace after macro //(str.Skip(3).First(c => !char.IsWhiteSpace(c)))
                str = str.Substring(0, index); // Remove any text after macro

                // Add macros to lists
                macros.Add(str);
                m_macros.Add(str.Insert(0, "M_"));

                Console.WriteLine(macros[i] + " | TO | " + m_macros[i]);
            }
        }


        void ReplaceMacros()
        {
            // Search files in base (later further folders) to see if any strings contain any of the default macros, then replace with same index of m_defines
            string str = "";
            string subFile = "";

            if (!Directory.Exists(textBoxOutput.Text)) Directory.CreateDirectory(textBoxOutput.Text);

            string[] dirs = Directory.GetDirectories(textBoxInput.Text, "*", SearchOption.AllDirectories);
            for (int i = 0; i < dirs.Length; i++)
            {
                builder.Append(dirs[i]);
                Directory.CreateDirectory(builder.Replace(textBoxInput.Text, textBoxOutput.Text).ToString());
                Console.WriteLine(dirs[i]);
                builder.Clear();
            }

            // Replace in all files
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Contains("gamedata.ini"))
                {
                    // Create new file for /default/gamedata.ini & _gamedata.inc
                    CreateGameData(i);

                    Console.WriteLine("Skipping " + files[i]);
                    continue;
                }
                if (files[i].Contains("_gamedata.inc"))
                {
                    Console.WriteLine("Skipping " + files[i]);
                    continue;
                }
                if (files[i].Contains(Path.Combine("default","water.ini")))
                {
                    AppendWater(i);
                    continue;
                }
                Console.WriteLine("Opening " + files[i]);

                builder.Clear();
                builderPath.Clear();

                builder.Append(File.ReadAllText(files[i]));
                for (int j = 0; j < macros.Count; j++)
                {
                    if (str.Contains(macros[j]))
                    {
                        builder.Replace(macros[j], m_macros[j]);
                        //str = str.Replace(macros[j], m_macros[j]);

                        Console.WriteLine("Replacing " + macros[j] + " > " + m_macros[j]);
                        //Thread.Sleep(500);
                    }
                }

                builderPath.Append(files[i]);
                File.WriteAllText(builderPath.Replace(textBoxInput.Text, textBoxOutput.Text).ToString(), builder.ToString());
                Console.WriteLine("Creating " + builderPath.ToString());
                Console.WriteLine("Closing " + files[i]);
            }

            void CreateGameData(int i)
            {
                builder.Clear();
                builderPath.Clear();

                INC();

                builder.Clear();
                builderPath.Clear();

                INI();

                void INI()
                {
                    // Put Gamedata block into "object/gamedata.ini"
                    for (int j = defineIndexEnd; j < gamedataText.Length; j++)
                    {
                        builder.AppendLine(gamedataText[j]);
                        if (gamedataText[j].Contains("  WaterType = 0"))
                        {
                            //builder.Insert(j - 1, "  PlayIntro = " + (checkBoxNoIntro.Checked ? "Yes" : "No"));
                            builder.AppendLine("  PlayIntro = " + (checkBoxPlayIntro.Checked ? "Yes" : "No"));
                        }
                    }

                    // Create gamedata.ini in root
                    File.WriteAllText(Path.Combine(textBoxOutput.Text, "object", "gamedata.ini"), builder.ToString()); // Output directory to gamedata.ini
                }
                void INC()
                {
                    // Put defines block in "_gamedata.inc"
                    for (int j = defineIndexStart; j < defineIndexEnd; j++)
                    {
                        builder.AppendLine(gamedataText[j]);
                    }

                    // Create _gamedata.inc in objects/gamedata
                    File.WriteAllText(Path.Combine(textBoxOutput.Text, "_gamedata.inc"), builder.ToString()); // Output directory to gamedata.ini
                }
            }

            void AppendWater(int i)
            {
                Console.WriteLine("Appending " + files[i]);
                builder.Clear();
                builderPath.Clear();

                builder.Append(File.ReadAllText(files[i]));
                builder.AppendLine();
                builder.AppendLine();
                builder.AppendLine("#include \"..\\_gamedata.inc\" ;all macros are now in that file!");

                builderPath.Append(files[i]);
                File.WriteAllText(builderPath.Replace(textBoxInput.Text, textBoxOutput.Text).ToString(), builder.ToString());
            }
        }

       

        private void buttonStart_Click(object sender, EventArgs e)
        {
            textBoxInput.Enabled = false;
            textBoxOutput.Enabled = false;
            Run();
        }


        private void buttonSelectInput_Click(object sender, EventArgs e)
        {
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                selectedDirectory = dialog.FileName;

                textBoxInput.Text = dialog.FileName;


                GetWorkingDirectory();
                buttonStart.Enabled = GetGameData();
            }



        }

        private void buttonSelectOutput_Click(object sender, EventArgs e)
        {
            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
                textBoxOutput.Text = dialog.FileName;
        }
    }
}
