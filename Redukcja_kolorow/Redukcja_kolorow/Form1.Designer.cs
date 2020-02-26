namespace Redukcja_kolorow
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.picMain = new System.Windows.Forms.PictureBox();
            this.picAfterConstruction = new System.Windows.Forms.PictureBox();
            this.picAlongConstruction = new System.Windows.Forms.PictureBox();
            this.btnReduce = new System.Windows.Forms.Button();
            this.barAfterConstruction = new System.Windows.Forms.ProgressBar();
            this.trcMain = new System.Windows.Forms.TrackBar();
            this.barAlongConstruction = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAfterConstruction = new System.Windows.Forms.Label();
            this.lblAlongConstruction = new System.Windows.Forms.Label();
            this.btnChangeBitmap = new System.Windows.Forms.Button();
            this.btnGenerateTexture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAfterConstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAlongConstruction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcMain)).BeginInit();
            this.SuspendLayout();
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(59, 55);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(658, 353);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            // 
            // picAfterConstruction
            // 
            this.picAfterConstruction.Location = new System.Drawing.Point(814, 55);
            this.picAfterConstruction.Name = "picAfterConstruction";
            this.picAfterConstruction.Size = new System.Drawing.Size(518, 244);
            this.picAfterConstruction.TabIndex = 1;
            this.picAfterConstruction.TabStop = false;
            this.picAfterConstruction.Paint += new System.Windows.Forms.PaintEventHandler(this.picAfterConstruction_Paint);
            // 
            // picAlongConstruction
            // 
            this.picAlongConstruction.Location = new System.Drawing.Point(814, 390);
            this.picAlongConstruction.Name = "picAlongConstruction";
            this.picAlongConstruction.Size = new System.Drawing.Size(518, 244);
            this.picAlongConstruction.TabIndex = 2;
            this.picAlongConstruction.TabStop = false;
            this.picAlongConstruction.Paint += new System.Windows.Forms.PaintEventHandler(this.picAlongConstruction_Paint);
            // 
            // btnReduce
            // 
            this.btnReduce.Location = new System.Drawing.Point(132, 476);
            this.btnReduce.Name = "btnReduce";
            this.btnReduce.Size = new System.Drawing.Size(204, 62);
            this.btnReduce.TabIndex = 3;
            this.btnReduce.Text = "Reduce to 1";
            this.btnReduce.UseVisualStyleBackColor = true;
            this.btnReduce.Click += new System.EventHandler(this.btnReduce_Click);
            // 
            // barAfterConstruction
            // 
            this.barAfterConstruction.Location = new System.Drawing.Point(814, 314);
            this.barAfterConstruction.Maximum = 153648;
            this.barAfterConstruction.Name = "barAfterConstruction";
            this.barAfterConstruction.Size = new System.Drawing.Size(518, 23);
            this.barAfterConstruction.Step = 1;
            this.barAfterConstruction.TabIndex = 4;
            // 
            // trcMain
            // 
            this.trcMain.Location = new System.Drawing.Point(59, 414);
            this.trcMain.Maximum = 1024;
            this.trcMain.Minimum = 1;
            this.trcMain.Name = "trcMain";
            this.trcMain.Size = new System.Drawing.Size(658, 56);
            this.trcMain.TabIndex = 5;
            this.trcMain.TickFrequency = 25;
            this.trcMain.Value = 1;
            this.trcMain.Scroll += new System.EventHandler(this.trcMain_Scroll);
            // 
            // barAlongConstruction
            // 
            this.barAlongConstruction.Location = new System.Drawing.Point(814, 649);
            this.barAlongConstruction.Maximum = 153648;
            this.barAlongConstruction.Name = "barAlongConstruction";
            this.barAlongConstruction.Size = new System.Drawing.Size(518, 23);
            this.barAlongConstruction.Step = 1;
            this.barAlongConstruction.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label1.Location = new System.Drawing.Point(809, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Reduce after octree construction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.label2.Location = new System.Drawing.Point(809, 351);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Reduce along octree construction";
            // 
            // lblAfterConstruction
            // 
            this.lblAfterConstruction.AutoSize = true;
            this.lblAfterConstruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblAfterConstruction.Location = new System.Drawing.Point(1143, 22);
            this.lblAfterConstruction.Name = "lblAfterConstruction";
            this.lblAfterConstruction.Size = new System.Drawing.Size(127, 20);
            this.lblAfterConstruction.TabIndex = 10;
            this.lblAfterConstruction.Text = "using: all colors";
            // 
            // lblAlongConstruction
            // 
            this.lblAlongConstruction.AutoSize = true;
            this.lblAlongConstruction.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblAlongConstruction.Location = new System.Drawing.Point(1143, 355);
            this.lblAlongConstruction.Name = "lblAlongConstruction";
            this.lblAlongConstruction.Size = new System.Drawing.Size(127, 20);
            this.lblAlongConstruction.TabIndex = 11;
            this.lblAlongConstruction.Text = "using: all colors";
            // 
            // btnChangeBitmap
            // 
            this.btnChangeBitmap.Location = new System.Drawing.Point(463, 476);
            this.btnChangeBitmap.Name = "btnChangeBitmap";
            this.btnChangeBitmap.Size = new System.Drawing.Size(204, 62);
            this.btnChangeBitmap.TabIndex = 12;
            this.btnChangeBitmap.Text = "Change bitmap";
            this.btnChangeBitmap.UseVisualStyleBackColor = true;
            this.btnChangeBitmap.Click += new System.EventHandler(this.btnChangeBitmap_Click);
            // 
            // btnGenerateTexture
            // 
            this.btnGenerateTexture.Location = new System.Drawing.Point(290, 572);
            this.btnGenerateTexture.Name = "btnGenerateTexture";
            this.btnGenerateTexture.Size = new System.Drawing.Size(204, 62);
            this.btnGenerateTexture.TabIndex = 13;
            this.btnGenerateTexture.Text = "Generate texture";
            this.btnGenerateTexture.UseVisualStyleBackColor = true;
            this.btnGenerateTexture.Click += new System.EventHandler(this.btnGenerateTexture_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1397, 684);
            this.Controls.Add(this.btnGenerateTexture);
            this.Controls.Add(this.btnChangeBitmap);
            this.Controls.Add(this.lblAlongConstruction);
            this.Controls.Add(this.lblAfterConstruction);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.barAlongConstruction);
            this.Controls.Add(this.trcMain);
            this.Controls.Add(this.barAfterConstruction);
            this.Controls.Add(this.btnReduce);
            this.Controls.Add(this.picAlongConstruction);
            this.Controls.Add(this.picAfterConstruction);
            this.Controls.Add(this.picMain);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Color reduction";
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAfterConstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAlongConstruction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trcMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.PictureBox picAfterConstruction;
        private System.Windows.Forms.PictureBox picAlongConstruction;
        private System.Windows.Forms.Button btnReduce;
        private System.Windows.Forms.ProgressBar barAfterConstruction;
        private System.Windows.Forms.TrackBar trcMain;
        private System.Windows.Forms.ProgressBar barAlongConstruction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAfterConstruction;
        private System.Windows.Forms.Label lblAlongConstruction;
        private System.Windows.Forms.Button btnChangeBitmap;
        private System.Windows.Forms.Button btnGenerateTexture;
    }
}

