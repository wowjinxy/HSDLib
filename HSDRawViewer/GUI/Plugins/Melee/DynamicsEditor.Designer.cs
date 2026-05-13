namespace HSDRawViewer.GUI.Plugins.Melee
{
    partial class DynamicsEditor
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
            arrayEditor = new ArrayMemberEditor();
            tabControl = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            arrayGroup = new ArrayMemberEditor();
            tabPage2 = new System.Windows.Forms.TabPage();
            arrayHitbubble = new ArrayMemberEditor();
            groupBoxViewport = new System.Windows.Forms.GroupBox();
            toolStripViewport = new System.Windows.Forms.ToolStrip();
            toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            loadModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            loadAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            clearAnimationToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            dropDownView = new System.Windows.Forms.ToolStripDropDownButton();
            toggleViewBones = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            toggleBoneChain = new System.Windows.Forms.ToolStripMenuItem();
            toggleHitbubbles = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            toggleControlGravity = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            buttonPlay = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            buttonBubbleTransparency = new System.Windows.Forms.ToolStripButton();
            buttonResetGravity = new System.Windows.Forms.ToolStripButton();
            groupBox1 = new System.Windows.Forms.GroupBox();
            splitter1 = new System.Windows.Forms.Splitter();
            splitter2 = new System.Windows.Forms.Splitter();
            toolStrip2 = new System.Windows.Forms.ToolStrip();
            buttonSave = new System.Windows.Forms.ToolStripButton();
            tabControl.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBoxViewport.SuspendLayout();
            toolStripViewport.SuspendLayout();
            groupBox1.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // arrayEditor
            // 
            arrayEditor.DisplayItemImages = false;
            arrayEditor.DisplayItemIndices = true;
            arrayEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            arrayEditor.EnablePropertyViewDescription = true;
            arrayEditor.ImageHeight = (ushort)24;
            arrayEditor.ImageWidth = (ushort)24;
            arrayEditor.InsertCloneAfterSelected = false;
            arrayEditor.ItemHeight = 13;
            arrayEditor.ItemIndexOffset = 0;
            arrayEditor.Location = new System.Drawing.Point(3, 23);
            arrayEditor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            arrayEditor.Name = "arrayEditor";
            arrayEditor.SelectionMode = System.Windows.Forms.SelectionMode.One;
            arrayEditor.Size = new System.Drawing.Size(282, 531);
            arrayEditor.TabIndex = 0;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPage1);
            tabControl.Controls.Add(tabPage2);
            tabControl.Dock = System.Windows.Forms.DockStyle.Left;
            tabControl.Location = new System.Drawing.Point(0, 27);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new System.Drawing.Size(299, 557);
            tabControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(arrayGroup);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(291, 524);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Bone Chains";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // arrayGroup
            // 
            arrayGroup.DisplayItemImages = false;
            arrayGroup.DisplayItemIndices = true;
            arrayGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            arrayGroup.EnablePropertyViewDescription = true;
            arrayGroup.ImageHeight = (ushort)24;
            arrayGroup.ImageWidth = (ushort)24;
            arrayGroup.InsertCloneAfterSelected = false;
            arrayGroup.ItemHeight = 13;
            arrayGroup.ItemIndexOffset = 0;
            arrayGroup.Location = new System.Drawing.Point(3, 3);
            arrayGroup.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            arrayGroup.Name = "arrayGroup";
            arrayGroup.SelectionMode = System.Windows.Forms.SelectionMode.One;
            arrayGroup.Size = new System.Drawing.Size(285, 518);
            arrayGroup.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(arrayHitbubble);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(291, 524);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Hit Bubbles";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // arrayHitbubble
            // 
            arrayHitbubble.DisplayItemImages = false;
            arrayHitbubble.DisplayItemIndices = true;
            arrayHitbubble.Dock = System.Windows.Forms.DockStyle.Fill;
            arrayHitbubble.EnablePropertyViewDescription = true;
            arrayHitbubble.ImageHeight = (ushort)24;
            arrayHitbubble.ImageWidth = (ushort)24;
            arrayHitbubble.InsertCloneAfterSelected = false;
            arrayHitbubble.ItemHeight = 13;
            arrayHitbubble.ItemIndexOffset = 0;
            arrayHitbubble.Location = new System.Drawing.Point(3, 3);
            arrayHitbubble.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            arrayHitbubble.Name = "arrayHitbubble";
            arrayHitbubble.SelectionMode = System.Windows.Forms.SelectionMode.One;
            arrayHitbubble.Size = new System.Drawing.Size(285, 518);
            arrayHitbubble.TabIndex = 0;
            // 
            // groupBoxViewport
            // 
            groupBoxViewport.Controls.Add(toolStripViewport);
            groupBoxViewport.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxViewport.Location = new System.Drawing.Point(299, 27);
            groupBoxViewport.Name = "groupBoxViewport";
            groupBoxViewport.Size = new System.Drawing.Size(696, 557);
            groupBoxViewport.TabIndex = 2;
            groupBoxViewport.TabStop = false;
            groupBoxViewport.Text = "Preview";
            // 
            // toolStripViewport
            // 
            toolStripViewport.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStripViewport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripDropDownButton2, toolStripSeparator2, dropDownView, toolStripSeparator1, buttonPlay, toolStripSeparator5, buttonBubbleTransparency, buttonResetGravity });
            toolStripViewport.Location = new System.Drawing.Point(3, 23);
            toolStripViewport.Name = "toolStripViewport";
            toolStripViewport.Size = new System.Drawing.Size(690, 27);
            toolStripViewport.TabIndex = 1;
            toolStripViewport.Text = "toolStrip1";
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { loadModelToolStripMenuItem, toolStripSeparator6, loadAnimationToolStripMenuItem, clearAnimationToolStripItem });
            toolStripDropDownButton2.Image = Properties.Resources.ts_importfile;
            toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new System.Drawing.Size(66, 24);
            toolStripDropDownButton2.Text = "File";
            // 
            // loadModelToolStripMenuItem
            // 
            loadModelToolStripMenuItem.Name = "loadModelToolStripMenuItem";
            loadModelToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            loadModelToolStripMenuItem.Text = "Load Model";
            loadModelToolStripMenuItem.Click += buttonLoadModel_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(221, 6);
            // 
            // loadAnimationToolStripMenuItem
            // 
            loadAnimationToolStripMenuItem.Enabled = false;
            loadAnimationToolStripMenuItem.Name = "loadAnimationToolStripMenuItem";
            loadAnimationToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            loadAnimationToolStripMenuItem.Text = "Load Animation";
            loadAnimationToolStripMenuItem.Click += loadAnimationToolStripMenuItem_Click;
            // 
            // clearAnimationToolStripItem
            // 
            clearAnimationToolStripItem.Enabled = false;
            clearAnimationToolStripItem.Name = "clearAnimationToolStripItem";
            clearAnimationToolStripItem.Size = new System.Drawing.Size(224, 26);
            clearAnimationToolStripItem.Text = "Clear Animation";
            clearAnimationToolStripItem.Click += clearAnimationToolStripItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // dropDownView
            // 
            dropDownView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toggleViewBones, toolStripSeparator3, toggleBoneChain, toggleHitbubbles, toolStripSeparator4, toggleControlGravity });
            dropDownView.Image = Properties.Resources.ts_visible;
            dropDownView.ImageTransparentColor = System.Drawing.Color.Magenta;
            dropDownView.Name = "dropDownView";
            dropDownView.Size = new System.Drawing.Size(75, 24);
            dropDownView.Text = "View";
            // 
            // toggleViewBones
            // 
            toggleViewBones.Checked = true;
            toggleViewBones.CheckOnClick = true;
            toggleViewBones.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleViewBones.Name = "toggleViewBones";
            toggleViewBones.Size = new System.Drawing.Size(191, 26);
            toggleViewBones.Text = "Bones";
            toggleViewBones.Click += Button_UpdateVisibility;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // toggleBoneChain
            // 
            toggleBoneChain.Checked = true;
            toggleBoneChain.CheckOnClick = true;
            toggleBoneChain.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleBoneChain.Name = "toggleBoneChain";
            toggleBoneChain.Size = new System.Drawing.Size(191, 26);
            toggleBoneChain.Text = "Bone Chain";
            toggleBoneChain.Click += Button_UpdateVisibility;
            // 
            // toggleHitbubbles
            // 
            toggleHitbubbles.Checked = true;
            toggleHitbubbles.CheckOnClick = true;
            toggleHitbubbles.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleHitbubbles.Name = "toggleHitbubbles";
            toggleHitbubbles.Size = new System.Drawing.Size(191, 26);
            toggleHitbubbles.Text = "Hit Bubbles";
            toggleHitbubbles.Click += Button_UpdateVisibility;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(188, 6);
            // 
            // toggleControlGravity
            // 
            toggleControlGravity.Checked = true;
            toggleControlGravity.CheckOnClick = true;
            toggleControlGravity.CheckState = System.Windows.Forms.CheckState.Checked;
            toggleControlGravity.Name = "toggleControlGravity";
            toggleControlGravity.Size = new System.Drawing.Size(191, 26);
            toggleControlGravity.Text = "Gravity Control";
            toggleControlGravity.Click += Button_UpdateVisibility;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // buttonPlay
            // 
            buttonPlay.Image = Properties.Resources.ts_play;
            buttonPlay.ImageTransparentColor = System.Drawing.Color.Magenta;
            buttonPlay.Name = "buttonPlay";
            buttonPlay.Size = new System.Drawing.Size(60, 24);
            buttonPlay.Text = "Play";
            buttonPlay.Click += buttonPlay_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // buttonBubbleTransparency
            // 
            buttonBubbleTransparency.Image = Properties.Resources.ts_transparent;
            buttonBubbleTransparency.ImageTransparentColor = System.Drawing.Color.Magenta;
            buttonBubbleTransparency.Name = "buttonBubbleTransparency";
            buttonBubbleTransparency.Size = new System.Drawing.Size(80, 24);
            buttonBubbleTransparency.Text = "Bubble";
            buttonBubbleTransparency.ToolTipText = "Toggle Hitbubble Transparency";
            buttonBubbleTransparency.Click += buttonBubbleTransparency_Click;
            // 
            // buttonResetGravity
            // 
            buttonResetGravity.Image = Properties.Resources.ts_down;
            buttonResetGravity.ImageTransparentColor = System.Drawing.Color.Magenta;
            buttonResetGravity.Name = "buttonResetGravity";
            buttonResetGravity.Size = new System.Drawing.Size(119, 24);
            buttonResetGravity.Text = "Reset Gravity";
            buttonResetGravity.Click += buttonResetGravity_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(arrayEditor);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            groupBox1.Location = new System.Drawing.Point(995, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(288, 557);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Properties";
            // 
            // splitter1
            // 
            splitter1.Location = new System.Drawing.Point(299, 27);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(4, 557);
            splitter1.TabIndex = 4;
            splitter1.TabStop = false;
            // 
            // splitter2
            // 
            splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            splitter2.Location = new System.Drawing.Point(991, 27);
            splitter2.Name = "splitter2";
            splitter2.Size = new System.Drawing.Size(4, 557);
            splitter2.TabIndex = 5;
            splitter2.TabStop = false;
            // 
            // toolStrip2
            // 
            toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { buttonSave });
            toolStrip2.Location = new System.Drawing.Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new System.Drawing.Size(1283, 27);
            toolStrip2.TabIndex = 6;
            toolStrip2.Text = "toolStrip2";
            // 
            // buttonSave
            // 
            buttonSave.Image = Properties.Resources.ico_save;
            buttonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new System.Drawing.Size(124, 24);
            buttonSave.Text = "Save Changes";
            buttonSave.Click += buttonSave_Click;
            // 
            // DynamicsEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1283, 584);
            Controls.Add(splitter2);
            Controls.Add(splitter1);
            Controls.Add(groupBoxViewport);
            Controls.Add(groupBox1);
            Controls.Add(tabControl);
            Controls.Add(toolStrip2);
            Name = "DynamicsEditor";
            Text = "DynamicsEditor";
            tabControl.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            groupBoxViewport.ResumeLayout(false);
            groupBoxViewport.PerformLayout();
            toolStripViewport.ResumeLayout(false);
            toolStripViewport.PerformLayout();
            groupBox1.ResumeLayout(false);
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ArrayMemberEditor arrayEditor;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBoxViewport;
        private System.Windows.Forms.ToolStrip toolStripViewport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private ArrayMemberEditor arrayGroup;
        private ArrayMemberEditor arrayHitbubble;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton buttonSave;
        private System.Windows.Forms.ToolStripButton buttonBubbleTransparency;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton dropDownView;
        private System.Windows.Forms.ToolStripMenuItem toggleViewBones;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toggleHitbubbles;
        private System.Windows.Forms.ToolStripMenuItem toggleBoneChain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem loadModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleControlGravity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton buttonPlay;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton buttonResetGravity;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem clearAnimationToolStripItem;
    }
}