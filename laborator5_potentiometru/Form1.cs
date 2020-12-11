using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace laborator5_potentiometru
{
    public partial class Form1 : Form
    {
        int cnt = 0;
        int rot = 0;
        Bitmap img = new Bitmap(@"C:\Users\student\Pictures\lena30.jpg");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Porneste operatia asincrona
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch(Exception ex)
            {
                
            }
                
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while(true)
            {
                // Asteapta o cuanta de timp si raporteaza progresul
                System.Threading.Thread.Sleep(50);
                worker.ReportProgress(cnt++);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
            {

                backgroundWorker1.ReportProgress(0);
                return;
            }
            if (serialPort1.IsOpen)
                serialPort1.Close();
            serialPort1.Open();
            serialPort1.Write("r\r");
            string res = serialPort1.ReadLine();
            while (!res.Contains("<"))
                res += serialPort1.ReadLine();
            serialPort1.Close();
            // Exemplu de valori: 0.00 4999.12
            try
            {
                int value = int.Parse(res.Substring(res.IndexOf("=") + 1, res.IndexOf(".") - 1 - res.IndexOf("="))); // preia ce e intre egal si punct
                textBox1.Text = value + " " + e.ProgressPercentage.ToString();
                textBox2.Text = value.ToString();
                rot = Convert.ToInt32(value / 100 * 7.2);
                textBox3.Text = rot.ToString();
            }
            catch(Exception ex)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                textBox1.Text = "Anulat!";
            }
            else
            {
                textBox1.Text = "Complet!";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(img, new Size(100, 100));
            pictureBox1.Image=bitmap;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(img, new Size(100, 100));
            Bitmap returnBitmap = new Bitmap(bitmap.Height, bitmap.Width);
            Graphics g = Graphics.FromImage(returnBitmap);
            g.TranslateTransform((float)bitmap.Width / 2, (float)bitmap.Height / 2);
            g.RotateTransform(rot);
            g.TranslateTransform(-(float)bitmap.Width / 2, -(float)bitmap.Height / 2);
            g.DrawImage(bitmap, new Point(0, 0));
            pictureBox1.Image = returnBitmap;
            
        }
    }
}