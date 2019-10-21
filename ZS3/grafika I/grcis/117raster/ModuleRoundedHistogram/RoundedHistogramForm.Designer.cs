namespace Modules
{
  partial class RoundedHistogramForm
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
            this.SuspendLayout();
            // 
            // RoundedHistogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 661);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RoundedHistogramForm";
            this.ShowIcon = false;
            this.Text = "Histogram";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HistogramForm_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HistogramForm_Paint);
            this.Resize += new System.EventHandler(this.HistogramForm_Resize);
            this.ResumeLayout(false);

    }

    #endregion
  }
}
