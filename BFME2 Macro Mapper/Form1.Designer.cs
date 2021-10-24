
namespace BFME2_Macro_Mapper
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonStart = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSelectInput = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSelectOutput = new System.Windows.Forms.Button();
            this.checkBoxPlayIntro = new System.Windows.Forms.CheckBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(256, 299);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 50);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Location = new System.Drawing.Point(57, 146);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(400, 20);
            this.textBoxInput.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Input";
            // 
            // buttonSelectInput
            // 
            this.buttonSelectInput.Location = new System.Drawing.Point(463, 144);
            this.buttonSelectInput.Name = "buttonSelectInput";
            this.buttonSelectInput.Size = new System.Drawing.Size(111, 23);
            this.buttonSelectInput.TabIndex = 3;
            this.buttonSelectInput.Text = "Select Input";
            this.buttonSelectInput.UseVisualStyleBackColor = true;
            this.buttonSelectInput.Click += new System.EventHandler(this.buttonSelectInput_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Location = new System.Drawing.Point(57, 172);
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.Size = new System.Drawing.Size(400, 20);
            this.textBoxOutput.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 172);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output";
            // 
            // buttonSelectOutput
            // 
            this.buttonSelectOutput.Location = new System.Drawing.Point(463, 170);
            this.buttonSelectOutput.Name = "buttonSelectOutput";
            this.buttonSelectOutput.Size = new System.Drawing.Size(111, 23);
            this.buttonSelectOutput.TabIndex = 6;
            this.buttonSelectOutput.Text = "Select Output";
            this.buttonSelectOutput.UseVisualStyleBackColor = true;
            this.buttonSelectOutput.Click += new System.EventHandler(this.buttonSelectOutput_Click);
            // 
            // checkBoxPlayIntro
            // 
            this.checkBoxPlayIntro.AutoSize = true;
            this.checkBoxPlayIntro.Checked = true;
            this.checkBoxPlayIntro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxPlayIntro.Location = new System.Drawing.Point(387, 198);
            this.checkBoxPlayIntro.Name = "checkBoxPlayIntro";
            this.checkBoxPlayIntro.Size = new System.Drawing.Size(70, 17);
            this.checkBoxPlayIntro.TabIndex = 7;
            this.checkBoxPlayIntro.Text = "Play Intro";
            this.checkBoxPlayIntro.UseVisualStyleBackColor = true;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(300, 65);
            this.labelInfo.TabIndex = 8;
            this.labelInfo.Text = "Macros will be renamed starting with M_\r\ngamedata.ini will be seperated into:\r\n  " +
    " _gamedata.inc\r\n   /object/gamedata.ini\r\nCreate a shortcut when done and point t" +
    "o the gamedata folder\r\n";
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.checkBoxPlayIntro);
            this.Controls.Add(this.buttonSelectOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.buttonSelectInput);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.buttonStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSelectInput;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSelectOutput;
        private System.Windows.Forms.CheckBox checkBoxPlayIntro;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Timer timer;
    }
}

