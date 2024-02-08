namespace Run_WinForms {
    partial class RunGameForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            _menuStrip = new MenuStrip();
            _menuFile = new ToolStripMenuItem();
            _menuFileNewGame = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            _menuFileSaveGame = new ToolStripMenuItem();
            _menuFileLoadGame = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            _menuFileQuit = new ToolStripMenuItem();
            _menuSettings = new ToolStripMenuItem();
            _menuSettingsSize = new ToolStripMenuItem();
            _menuSize11 = new ToolStripMenuItem();
            _menuSize15 = new ToolStripMenuItem();
            _menuSize21 = new ToolStripMenuItem();
            _menuSettingsDifficulty = new ToolStripMenuItem();
            _menuDifficultyEasy = new ToolStripMenuItem();
            _menuDifficultyMedium = new ToolStripMenuItem();
            _menuDifficultyHard = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            _toolLabel1 = new ToolStripStatusLabel();
            _toolLabelGameSteps = new ToolStripStatusLabel();
            _toolLabel2 = new ToolStripStatusLabel();
            _toolLabelGameTime = new ToolStripStatusLabel();
            _openFileDialog = new OpenFileDialog();
            _saveFileDialog = new SaveFileDialog();
            _menuStrip.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // _menuStrip
            // 
            _menuStrip.Items.AddRange(new ToolStripItem[] { _menuFile, _menuSettings });
            _menuStrip.Location = new Point(0, 0);
            _menuStrip.Name = "_menuStrip";
            _menuStrip.Size = new Size(560, 24);
            _menuStrip.TabIndex = 0;
            _menuStrip.Text = "menuStrip1";
            // 
            // _menuFile
            // 
            _menuFile.DropDownItems.AddRange(new ToolStripItem[] { _menuFileNewGame, toolStripMenuItem1, _menuFileSaveGame, _menuFileLoadGame, toolStripMenuItem2, _menuFileQuit });
            _menuFile.Name = "_menuFile";
            _menuFile.Size = new Size(37, 20);
            _menuFile.Text = "File";
            // 
            // _menuFileNewGame
            // 
            _menuFileNewGame.Name = "_menuFileNewGame";
            _menuFileNewGame.Size = new Size(180, 22);
            _menuFileNewGame.Text = "Új Játék";
            _menuFileNewGame.Click += _menuFileNewGame_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(177, 6);
            // 
            // _menuFileSaveGame
            // 
            _menuFileSaveGame.Enabled = false;
            _menuFileSaveGame.Name = "_menuFileSaveGame";
            _menuFileSaveGame.Size = new Size(180, 22);
            _menuFileSaveGame.Text = "Mentés...";
            _menuFileSaveGame.Click += _menuFileSaveGame_Click;
            // 
            // _menuFileLoadGame
            // 
            _menuFileLoadGame.Enabled = false;
            _menuFileLoadGame.Name = "_menuFileLoadGame";
            _menuFileLoadGame.Size = new Size(180, 22);
            _menuFileLoadGame.Text = "Betöltés...";
            _menuFileLoadGame.Click += _menuFileLoadGame_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(177, 6);
            // 
            // _menuFileQuit
            // 
            _menuFileQuit.Name = "_menuFileQuit";
            _menuFileQuit.Size = new Size(180, 22);
            _menuFileQuit.Text = "Kilépés";
            _menuFileQuit.Click += _menuFileQuit_Click;
            // 
            // _menuSettings
            // 
            _menuSettings.DropDownItems.AddRange(new ToolStripItem[] { _menuSettingsSize, _menuSettingsDifficulty });
            _menuSettings.Name = "_menuSettings";
            _menuSettings.Size = new Size(75, 20);
            _menuSettings.Text = "Beállítások";
            // 
            // _menuSettingsSize
            // 
            _menuSettingsSize.DropDownItems.AddRange(new ToolStripItem[] { _menuSize11, _menuSize15, _menuSize21 });
            _menuSettingsSize.Name = "_menuSettingsSize";
            _menuSettingsSize.Size = new Size(125, 22);
            _menuSettingsSize.Text = "Méret";
            // 
            // _menuSize11
            // 
            _menuSize11.Checked = true;
            _menuSize11.CheckState = CheckState.Checked;
            _menuSize11.Name = "_menuSize11";
            _menuSize11.Size = new Size(180, 22);
            _menuSize11.Text = "11 x 11";
            _menuSize11.Click += _menuSize11_Click;
            // 
            // _menuSize15
            // 
            _menuSize15.Name = "_menuSize15";
            _menuSize15.Size = new Size(180, 22);
            _menuSize15.Text = "15 x 15";
            _menuSize15.Click += _menuSize15_Click;
            // 
            // _menuSize21
            // 
            _menuSize21.Name = "_menuSize21";
            _menuSize21.Size = new Size(180, 22);
            _menuSize21.Text = "21 x 21";
            _menuSize21.Click += _menuSize21_Click;
            // 
            // _menuSettingsDifficulty
            // 
            _menuSettingsDifficulty.DropDownItems.AddRange(new ToolStripItem[] { _menuDifficultyEasy, _menuDifficultyMedium, _menuDifficultyHard });
            _menuSettingsDifficulty.Name = "_menuSettingsDifficulty";
            _menuSettingsDifficulty.Size = new Size(125, 22);
            _menuSettingsDifficulty.Text = "Nehézség";
            // 
            // _menuDifficultyEasy
            // 
            _menuDifficultyEasy.Name = "_menuDifficultyEasy";
            _menuDifficultyEasy.Size = new Size(180, 22);
            _menuDifficultyEasy.Text = "Könnyű";
            _menuDifficultyEasy.Click += _menuDifficultyEasy_Click;
            // 
            // _menuDifficultyMedium
            // 
            _menuDifficultyMedium.Checked = true;
            _menuDifficultyMedium.CheckState = CheckState.Checked;
            _menuDifficultyMedium.Name = "_menuDifficultyMedium";
            _menuDifficultyMedium.Size = new Size(180, 22);
            _menuDifficultyMedium.Text = "Közepes";
            _menuDifficultyMedium.Click += _menuDifficultyMedium_Click;
            // 
            // _menuDifficultyHard
            // 
            _menuDifficultyHard.Name = "_menuDifficultyHard";
            _menuDifficultyHard.Size = new Size(180, 22);
            _menuDifficultyHard.Text = "Nehéz";
            _menuDifficultyHard.Click += _menuDifficultyHard_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { _toolLabel1, _toolLabelGameSteps, _toolLabel2, _toolLabelGameTime });
            statusStrip1.Location = new Point(0, 447);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(560, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // _toolLabel1
            // 
            _toolLabel1.Name = "_toolLabel1";
            _toolLabel1.Size = new Size(67, 17);
            _toolLabel1.Text = "Lépésszám:";
            // 
            // _toolLabelGameSteps
            // 
            _toolLabelGameSteps.Name = "_toolLabelGameSteps";
            _toolLabelGameSteps.Size = new Size(13, 17);
            _toolLabelGameSteps.Text = "0";
            // 
            // _toolLabel2
            // 
            _toolLabel2.Name = "_toolLabel2";
            _toolLabel2.Size = new Size(59, 17);
            _toolLabel2.Text = "Eltelt idő: ";
            // 
            // _toolLabelGameTime
            // 
            _toolLabelGameTime.Name = "_toolLabelGameTime";
            _toolLabelGameTime.Size = new Size(43, 17);
            _toolLabelGameTime.Text = "0:00:00";
            // 
            // _openFileDialog
            // 
            _openFileDialog.DefaultExt = "tzt";
            _openFileDialog.FileName = "savefile";
            // 
            // _saveFileDialog
            // 
            _saveFileDialog.DefaultExt = "txt";
            _saveFileDialog.FileName = "savefile";
            // 
            // RunGameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 469);
            Controls.Add(statusStrip1);
            Controls.Add(_menuStrip);
            MainMenuStrip = _menuStrip;
            Name = "RunGameForm";
            Text = "Menekülj!";
            KeyDown += Form1_KeyDown;
            _menuStrip.ResumeLayout(false);
            _menuStrip.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip _menuStrip;
        private ToolStripMenuItem _menuFile;
        private ToolStripMenuItem _menuFileNewGame;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem _menuFileSaveGame;
        private ToolStripMenuItem _menuFileLoadGame;
        private ToolStripMenuItem _menuSettings;
        private ToolStripMenuItem _menuSettingsSize;
        private ToolStripMenuItem _menuSize11;
        private ToolStripMenuItem _menuSize15;
        private ToolStripMenuItem _menuSize21;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem _menuFileQuit;
        private StatusStrip statusStrip1;
        private ToolStripMenuItem _menuSettingsDifficulty;
        private ToolStripMenuItem _menuDifficultyEasy;
        private ToolStripMenuItem _menuDifficultyMedium;
        private ToolStripMenuItem _menuDifficultyHard;
        private ToolStripStatusLabel _toolLabel1;
        private ToolStripStatusLabel _toolLabelGameSteps;
        private ToolStripStatusLabel _toolLabel2;
        private ToolStripStatusLabel _toolLabelGameTime;
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;
    }
}