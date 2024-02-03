using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewFingerPrint
{
    public partial class Form1 : Form, DPFP.Capture.EventHandler
    {
        private DPFP.Capture.Capture Capturer;
        //DPFP.Verification.Verification();
        private Label StatusText;
        public Form1()
        {
            InitializeComponent();
        }


        protected void MakeReport(string message)
        {
            this.Invoke(new Action(delegate ()
            {
                StatusText.Text = message;
            }));

        }


        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                Capturer = new DPFP.Capture.Capture();
               
                if(Capturer != null)
                {
                    Capturer.EventHandler = this;
                    MakeReport("Press start capture button to start scanning");
                }
                else
                {
                    MakeReport("Cannot initiate capture operation");
                }

            }
            catch
            {
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void OnReaderConnect(object Capture, string ReaderSerialNumber )
        {
            MakeReport("Finger print reader successfully connected");
        }


        public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
        {
            MakeReport("Finger print reader disconnected");
        }

        public void OnFingerGone(object Capture, string ReaderSerialNumber)
        {
            MakeReport("Finger was removed from the fingerprint reader");
        }


        public void OnFingerTouch(object Capture, string ReaderSerialNumber)
        {
            MakeReport("Finger was placed on the fingerprint reader");
        }



        public void OnComplete(object Capture, string ReaderSerialNumber,DPFP.Sample Sample)
        {
            MakeReport("Finger sample was captured");
            Process(Sample);
        }


        public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
        {
            if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
            {
                MakeReport("Quality of the finger print sample is good ");
            }
            else
            {
                MakeReport("Quality of the finger print sample is poor ");
            }
           

        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            DrawPicture(ConvertSampleToBitmap(Sample));
        }

        protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
        {
            DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();
            Bitmap bitmap = null;
            Convertor.ConvertToPicture( Sample, ref bitmap );

            return bitmap;
        }

        private void DrawPicture(Bitmap bitmap)
        {
            fImage.Image = new Bitmap( bitmap,fImage.Size );
        }










        private void InitializeComponent()
        {
            this.StatusText = new System.Windows.Forms.Label();
            this.fImage = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fImage)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusText
            // 
            this.StatusText.AutoSize = true;
            this.StatusText.Location = new System.Drawing.Point(29, 643);
            this.StatusText.Name = "StatusText";
            this.StatusText.Size = new System.Drawing.Size(115, 25);
            this.StatusText.TabIndex = 0;
            this.StatusText.Text = "StatusText";
            // 
            // fImage
            // 
            this.fImage.Location = new System.Drawing.Point(24, 12);
            this.fImage.Name = "fImage";
            this.fImage.Size = new System.Drawing.Size(621, 500);
            this.fImage.TabIndex = 1;
            this.fImage.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(476, 643);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 46);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Capture";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(657, 729);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fImage);
            this.Controls.Add(this.StatusText);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private PictureBox fImage;
        private Button button1;

        private void button1_Click(object sender, EventArgs e)
        {
            if (Capturer != null)
            {

                try
                {
                    Capturer.StartCapture();
                    MakeReport("Using the fingerprint reader, scan your fingerprint");
                }
                catch {
                    MakeReport("Can't initiate capture!");
                }



            }

        }







    }
}
