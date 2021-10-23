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
        DirectoryInfo currentDirectory;

        public string selectedDirectory;

        /// <summary>All files and subfiles found within the directory</summary>
        public string[] files;
        public string directory;
        /// <summary>Holds lines for "#define MACRO #"</summary>
        public List<string> defines = new List<string>();
        /// <summary>Holds lines for "#define M_MACRO #"</summary>
        public List<string> m_defines = new List<string>();
        /// <summary>Holds unedited MACRO</summary>
        public List<string> macros = new List<string>();
        /// <summary>Holds edited M_MACRO</summary>
        public List<string> m_macros = new List<string>();

        /// <summary>Text parsed from gamedata.ini</summary>
        List<string> gamedataText = new List<string>();
        /// <summary>Edited text parsed from gamedata.ini, including M_</summary>
        List<string> m_gamedataText = new List<string>();
        int defineIndexStart = 0;
        int defineIndexEnd = 0;

        ProgressBar progressBar = new ProgressBar();
        int startTime = 0;

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
            timer.Start();
            //GetWorkingDirectory();

            //folders = currentDirectory.GetDirectories();

            LoadMacros();
            ReplaceMacros();

            CreateShotcutFile();

            MessageBox.Show("Process Complete. Total Time: " + TimeSpan.FromMilliseconds(Environment.TickCount - startTime).ToString());
            timer.Stop();
            Application.Exit();
        }

        void CreateShotcutFile()
        {
            string shortcut = String.Format("-mod \"{0}\"", Directory.GetCurrentDirectory());
            File.WriteAllText(Path.Combine(textBoxOutput.Text,"_shortcut.txt"), shortcut);
            Console.WriteLine("Creating shortcut file");
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
                Console.WriteLine("Files found: " + files[i]);
            }
        }

        bool GetGameData()
        {
            if (File.Exists(Path.Combine(currentDirectory.FullName, gamedataPath)))
            {
                List<int> indexes = new List<int>();
                gamedataText.AddRange(File.ReadAllLines(Path.Combine(currentDirectory.FullName, gamedataPath)));
                for (int i = 0; i < gamedataText.Count; i++)
                {
                    if (gamedataText[i].StartsWith("#include"))
                    {
                        indexes.Add(i);
                    }
                    Console.WriteLine("Macro found: " + gamedataText[i]);
                }
                for (int i = 0; i < indexes.Count; i++)
                {
                    string sub = gamedataText[indexes[i]].Substring(9); // Get path from #include
                    gamedataText.RemoveAt(indexes[i]); // Remove #include from gamedata.ini
                    gamedataText.InsertRange(indexes[i], File.ReadAllLines(Path.Combine(currentDirectory.FullName, sub.Replace("\"", string.Empty))).Skip(3)); // Insert read file into gamedata.ini, skip header
                }

                Console.WriteLine("Gamedata.ini found, ready to begin");
                return true;
            }
            else
                Console.WriteLine("Gamedata.ini not found");
            return false;
        }

        void LoadMacros()
        {
            m_gamedataText = new List<string>(gamedataText.Count); // Allocate new string for edited gamedataText

            // Seperate #defines into array
            for (int i = 0; i < gamedataText.Count; i++)
            {
                if (gamedataText[i].StartsWith("#define"))
                {
                    if (defineIndexStart == 0) defineIndexStart = i; // Get beginning index for copying macros / defines
                    defines.Add(gamedataText[i]);
                    m_defines.Add(gamedataText[i].Insert(8, "M_"));
                    m_gamedataText.Insert(i,gamedataText[i].Insert(8, "M_"));
                }
                else
                {
                    m_gamedataText.Insert(i,gamedataText[i]); // Copy text
                }

                if(gamedataText[i].Contains("GAME DATA"))
                {
                    defineIndexEnd = i; // get ending index for copying macros / defines
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
                m_macros.Add("M_" + str); //"M_" + 


                Console.WriteLine(macros[i] + " | TO | " + m_macros[i]);
                builder.AppendLine(i.ToString() + " " + macros[i] + " | TO | " + m_macros[i]);
            }
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(),"Log.txt"), builder.ToString());
        }


        void ReplaceMacros()
        {
            builder.Clear();
            // Search files in base (later further folders) to see if any strings contain any of the default macros, then replace with same index of m_defines
            string source = "";
            int searchIndex = 0;
            int oldIndex = 0;
            int count = 0, n = 0;

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

                Console.Write("Searching: ");
                source = File.ReadAllText(files[i]);
                builder.EnsureCapacity(source.Length);
                builder.Append(source);

                searchIndex = 0;
                oldIndex = 0;
                count = n = 0;
                using (var progress = new ProgressBar())
                {
                    for (int j = 0; j < macros.Count; j++)
                    {
                        progress.Report((double)j / (double)macros.Count);
                        // add in contains
                        if (!source.Contains(macros[j])) continue;

                        builder.Replace(macros[j], m_macros[j]);

                        //count = n = 0;
                        //while ((n = source.IndexOf(macros[j], n, StringComparison.InvariantCulture)) != -1)//!= -1)
                        //{
                        //    builder.Insert(n + (2*count), "M_");
                        //    builder.
                        //    n += macros[j].Length; // for M_
                        //    count++;
                        //}
                        
                    }
                    //Second Pass, remove duplicates (not sure why they appear)
                    builder.Replace("M_M_", "M_");
                }

                //using (var progress = new ProgressBar())
                //{

                //        //builder.Replace(macros[j], m_macros[j]);


                //        //if (str.Contains(macros[j]))
                //        //{
                //        //    searchIndex = str.IndexOf(macros[j], oldIndex);

                //        //    oldIndex = searchIndex;
                //        //}
                //        oldIndex = 0;
                //        //builder.Replace(macros[j], "M_" + macros[j]);
                //        progress.Report((double)j / (double)macros.Count);
                //        //Thread.Sleep(500);
                //    }
                //}

                // Eliminate M_M_'s (Don't know why they appear)
                //builder.Replace("M_M_", "M_");

                Console.WriteLine("Done");
                builderPath.Append(files[i]);
                File.WriteAllText(builderPath.Replace(textBoxInput.Text, textBoxOutput.Text).ToString(), builder.ToString());
                Console.WriteLine("Creating " + builderPath.ToString());
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
                    for (int j = defineIndexEnd; j < gamedataText.Count; j++)
                    {
                        builder.AppendLine(gamedataText[j]);
                        if (gamedataText[j].Contains("  WaterType = 0"))
                        {
                            //builder.Insert(j - 1, "  PlayIntro = " + (checkBoxNoIntro.Checked ? "Yes" : "No"));
                            builder.AppendLine("  PlayIntro = " + (checkBoxPlayIntro.Checked ? "Yes" : "No"));
                        }
                        Console.WriteLine("Writing " + gamedataText[j]);
                    }

                    // Create gamedata.ini in root
                    File.WriteAllText(Path.Combine(textBoxOutput.Text, "object", "gamedata.ini"), builder.ToString()); // Output directory to gamedata.ini
                }
                void INC()
                {
                    // Put defines block in "_gamedata.inc"
                    for (int j = defineIndexStart; j < defineIndexEnd; j++)
                    {
                        builder.AppendLine(m_gamedataText[j]);
                        Console.WriteLine("Writing " + m_gamedataText[j]);
                    }

                    //// Replace defines in "_gamedata.inc"
                    //for (int j = 0; j < m_defines.Count; j++)
                    //{
                    //    builder.Replace(macros[j], m_macros[j]);
                    //    Console.WriteLine("Replacing " + macros[j]);
                    //}

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
            startTime = Environment.TickCount;
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

        private void timer_Tick(object sender, EventArgs e)
        {
            //elapsedTime++;
        }
    }
}
