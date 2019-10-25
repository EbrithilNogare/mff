namespace Modules
{
  partial class FormHSV
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose (bool disposing)
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
    private void InitializeComponent ()
    {
            this.buttonRecompute = new System.Windows.Forms.Button();
            this.buttonDeactivate = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.numericHue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textSaturation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textValue = new System.Windows.Forms.TextBox();
            this.checkParallel = new System.Windows.Forms.CheckBox();
            this.checkSlow = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textGamma = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.minHue = new System.Windows.Forms.NumericUpDown();
            this.maxHue = new System.Windows.Forms.NumericUpDown();
            this.minSaturation = new System.Windows.Forms.NumericUpDown();
            this.maxSaturation = new System.Windows.Forms.NumericUpDown();
            this.minValue = new System.Windows.Forms.NumericUpDown();
            this.maxValue = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonRecompute
            // 
            this.buttonRecompute.Location = new System.Drawing.Point(26, 268);
            this.buttonRecompute.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonRecompute.Name = "buttonRecompute";
            this.buttonRecompute.Size = new System.Drawing.Size(220, 35);
            this.buttonRecompute.TabIndex = 11;
            this.buttonRecompute.Text = "Recompute";
            this.buttonRecompute.UseVisualStyleBackColor = true;
            this.buttonRecompute.Click += new System.EventHandler(this.buttonRecompute_Click);
            // 
            // buttonDeactivate
            // 
            this.buttonDeactivate.Location = new System.Drawing.Point(268, 268);
            this.buttonDeactivate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDeactivate.Name = "buttonDeactivate";
            this.buttonDeactivate.Size = new System.Drawing.Size(164, 35);
            this.buttonDeactivate.TabIndex = 12;
            this.buttonDeactivate.Text = "Deactivate module";
            this.buttonDeactivate.UseVisualStyleBackColor = true;
            this.buttonDeactivate.Click += new System.EventHandler(this.buttonDeactivate_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(302, 26);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(130, 35);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Reset values";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // numericHue
            // 
            this.numericHue.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericHue.Location = new System.Drawing.Point(134, 29);
            this.numericHue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericHue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.numericHue.Name = "numericHue";
            this.numericHue.Size = new System.Drawing.Size(130, 26);
            this.numericHue.TabIndex = 1;
            this.numericHue.ValueChanged += new System.EventHandler(this.numericHue_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 92);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Saturation";
            // 
            // textSaturation
            // 
            this.textSaturation.CausesValidation = false;
            this.textSaturation.Location = new System.Drawing.Point(134, 89);
            this.textSaturation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textSaturation.Name = "textSaturation";
            this.textSaturation.Size = new System.Drawing.Size(128, 26);
            this.textSaturation.TabIndex = 4;
            this.textSaturation.Text = "1.0";
            this.textSaturation.TextChanged += new System.EventHandler(this.textSaturation_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 152);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Value";
            // 
            // textValue
            // 
            this.textValue.CausesValidation = false;
            this.textValue.Location = new System.Drawing.Point(134, 149);
            this.textValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textValue.Name = "textValue";
            this.textValue.Size = new System.Drawing.Size(128, 26);
            this.textValue.TabIndex = 7;
            this.textValue.Text = "1.0";
            this.textValue.TextChanged += new System.EventHandler(this.textValue_TextChanged);
            // 
            // checkParallel
            // 
            this.checkParallel.AutoSize = true;
            this.checkParallel.Checked = true;
            this.checkParallel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkParallel.Location = new System.Drawing.Point(344, 94);
            this.checkParallel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkParallel.Name = "checkParallel";
            this.checkParallel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.checkParallel.Size = new System.Drawing.Size(85, 24);
            this.checkParallel.TabIndex = 5;
            this.checkParallel.Text = "parallel";
            this.checkParallel.UseVisualStyleBackColor = true;
            this.checkParallel.CheckedChanged += new System.EventHandler(this.checkParallel_CheckedChanged);
            // 
            // checkSlow
            // 
            this.checkSlow.AutoSize = true;
            this.checkSlow.Location = new System.Drawing.Point(344, 152);
            this.checkSlow.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkSlow.Name = "checkSlow";
            this.checkSlow.Size = new System.Drawing.Size(66, 24);
            this.checkSlow.TabIndex = 8;
            this.checkSlow.Text = "slow";
            this.checkSlow.UseVisualStyleBackColor = true;
            this.checkSlow.CheckedChanged += new System.EventHandler(this.checkSlow_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 212);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Gamma";
            // 
            // textGamma
            // 
            this.textGamma.CausesValidation = false;
            this.textGamma.Location = new System.Drawing.Point(134, 209);
            this.textGamma.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textGamma.Name = "textGamma";
            this.textGamma.Size = new System.Drawing.Size(128, 26);
            this.textGamma.TabIndex = 10;
            this.textGamma.Text = "1.0";
            this.textGamma.TextChanged += new System.EventHandler(this.textGamma_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 523);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Hue";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 583);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Saturation";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 643);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Value";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(172, 472);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Min";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(336, 472);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "Max";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(66, 342);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(328, 29);
            this.label10.TabIndex = 0;
            this.label10.Text = "range of non-colorable colors";
            // 
            // minHue
            // 
            this.minHue.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.minHue.Location = new System.Drawing.Point(132, 521);
            this.minHue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.minHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.minHue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.minHue.Name = "minHue";
            this.minHue.Size = new System.Drawing.Size(130, 26);
            this.minHue.TabIndex = 1;
            this.minHue.Value = new decimal(new int[] {
            30,
            0,
            0,
            -2147483648});
            this.minHue.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // maxHue
            // 
            this.maxHue.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maxHue.Location = new System.Drawing.Point(290, 521);
            this.maxHue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxHue.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.maxHue.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.maxHue.Name = "maxHue";
            this.maxHue.Size = new System.Drawing.Size(130, 26);
            this.maxHue.TabIndex = 1;
            this.maxHue.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.maxHue.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // minSaturation
            // 
            this.minSaturation.DecimalPlaces = 1;
            this.minSaturation.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.minSaturation.Location = new System.Drawing.Point(132, 581);
            this.minSaturation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.minSaturation.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minSaturation.Name = "minSaturation";
            this.minSaturation.Size = new System.Drawing.Size(130, 26);
            this.minSaturation.TabIndex = 1;
            this.minSaturation.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // maxSaturation
            // 
            this.maxSaturation.DecimalPlaces = 1;
            this.maxSaturation.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.maxSaturation.Location = new System.Drawing.Point(290, 581);
            this.maxSaturation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxSaturation.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxSaturation.Name = "maxSaturation";
            this.maxSaturation.Size = new System.Drawing.Size(130, 26);
            this.maxSaturation.TabIndex = 1;
            this.maxSaturation.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxSaturation.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // minValue
            // 
            this.minValue.DecimalPlaces = 1;
            this.minValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.minValue.Location = new System.Drawing.Point(132, 637);
            this.minValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.minValue.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.minValue.Name = "minValue";
            this.minValue.Size = new System.Drawing.Size(130, 26);
            this.minValue.TabIndex = 1;
            this.minValue.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // maxValue
            // 
            this.maxValue.DecimalPlaces = 1;
            this.maxValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.maxValue.Location = new System.Drawing.Point(290, 637);
            this.maxValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxValue.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxValue.Name = "maxValue";
            this.maxValue.Size = new System.Drawing.Size(130, 26);
            this.maxValue.TabIndex = 1;
            this.maxValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxValue.ValueChanged += new System.EventHandler(this.something_changed);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::_117raster.Properties.Resources.hue_wallpaper;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(12, 374);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(432, 95);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // FormHSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 694);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textGamma);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkSlow);
            this.Controls.Add(this.checkParallel);
            this.Controls.Add(this.textValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textSaturation);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.maxValue);
            this.Controls.Add(this.maxSaturation);
            this.Controls.Add(this.maxHue);
            this.Controls.Add(this.minValue);
            this.Controls.Add(this.minSaturation);
            this.Controls.Add(this.minHue);
            this.Controls.Add(this.numericHue);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonDeactivate);
            this.Controls.Add(this.buttonRecompute);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHSV";
            this.Text = "Module HSV";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormHSV_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.numericHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonRecompute;
    private System.Windows.Forms.Button buttonDeactivate;
    private System.Windows.Forms.Button buttonReset;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    public System.Windows.Forms.TextBox textSaturation;
    public System.Windows.Forms.TextBox textValue;
    public System.Windows.Forms.CheckBox checkParallel;
    public System.Windows.Forms.CheckBox checkSlow;
    public System.Windows.Forms.NumericUpDown numericHue;
    private System.Windows.Forms.Label label4;
    public System.Windows.Forms.TextBox textGamma;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.NumericUpDown minHue;
        public System.Windows.Forms.NumericUpDown maxHue;
        public System.Windows.Forms.NumericUpDown minSaturation;
        public System.Windows.Forms.NumericUpDown maxSaturation;
        public System.Windows.Forms.NumericUpDown minValue;
        public System.Windows.Forms.NumericUpDown maxValue;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
