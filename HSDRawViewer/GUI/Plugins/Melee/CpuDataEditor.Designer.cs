namespace HSDRawViewer.GUI.Plugins.Melee
{
    partial class CpuDataEditor
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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewportBox = new System.Windows.Forms.GroupBox();
            editor = new ArrayMemberEditor();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadModelToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadModelToolStripMenuItem
            // 
            loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            loadModelToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            loadModelToolStripMenuItem.Text = "Load Model";
            loadModelToolStripMenuItem.Click += buttonLoadModel_Click;
            // 
            // viewportBox
            // 
            viewportBox.Dock = System.Windows.Forms.DockStyle.Fill;
            viewportBox.Location = new System.Drawing.Point(346, 28);
            viewportBox.Name = "viewportBox";
            viewportBox.Size = new System.Drawing.Size(454, 422);
            viewportBox.TabIndex = 1;
            viewportBox.TabStop = false;
            viewportBox.Text = "Viewport";
            // 
            // editor
            // 
            editor.DisplayItemImages = false;
            editor.DisplayItemIndices = false;
            editor.Dock = System.Windows.Forms.DockStyle.Left;
            editor.EnablePropertyViewDescription = true;
            editor.ImageHeight = (ushort)24;
            editor.ImageWidth = (ushort)24;
            editor.InsertCloneAfterSelected = false;
            editor.ItemHeight = 13;
            editor.ItemIndexOffset = 0;
            editor.Location = new System.Drawing.Point(0, 28);
            editor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            editor.Name = "editor";
            editor.SelectionMode = System.Windows.Forms.SelectionMode.One;
            editor.Size = new System.Drawing.Size(346, 422);
            editor.TabIndex = 0;
            // 
            // CpuDataEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(viewportBox);
            Controls.Add(editor);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "CpuDataEditor";
            Text = "CpuDataEditor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.GroupBox viewportBox;
        private ArrayMemberEditor editor;
    }
}