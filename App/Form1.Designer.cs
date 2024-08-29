namespace App;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        btnStart = new Button();
        btnStop = new Button();
        txtLogs = new RichTextBox();
        numInterval = new NumericUpDown();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)numInterval).BeginInit();
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.Location = new Point(16, 17);
        btnStart.Margin = new Padding(4);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(130, 40);
        btnStart.TabIndex = 0;
        btnStart.Text = "Start";
        btnStart.UseVisualStyleBackColor = true;
        // 
        // btnStop
        // 
        btnStop.Location = new Point(154, 17);
        btnStop.Margin = new Padding(4);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(130, 40);
        btnStop.TabIndex = 1;
        btnStop.Text = "Stop";
        btnStop.UseVisualStyleBackColor = true;
        // 
        // txtLogs
        // 
        txtLogs.BackColor = Color.Linen;
        txtLogs.Location = new Point(12, 121);
        txtLogs.Name = "txtLogs";
        txtLogs.ReadOnly = true;
        txtLogs.ScrollBars = RichTextBoxScrollBars.Vertical;
        txtLogs.Size = new Size(642, 221);
        txtLogs.TabIndex = 2;
        txtLogs.Text = "";
        // 
        // numInterval
        // 
        numInterval.Location = new Point(341, 69);
        numInterval.Maximum = new decimal(new int[] { 600, 0, 0, 0 });
        numInterval.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numInterval.Name = "numInterval";
        numInterval.Size = new Size(150, 34);
        numInterval.TabIndex = 3;
        numInterval.Value = new decimal(new int[] { 60, 0, 0, 0 });
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(16, 69);
        label1.Name = "label1";
        label1.Size = new Size(302, 28);
        label1.TabIndex = 4;
        label1.Text = "Time between events (in seconds)";
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(11F, 28F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(666, 354);
        Controls.Add(label1);
        Controls.Add(numInterval);
        Controls.Add(txtLogs);
        Controls.Add(btnStop);
        Controls.Add(btnStart);
        Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(4);
        MaximizeBox = false;
        Name = "Form1";
        Text = "Just Awake";
        ((System.ComponentModel.ISupportInitialize)numInterval).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button btnStart;
    private Button btnStop;
    private RichTextBox txtLogs;
    private NumericUpDown numInterval;
    private Label label1;
}
