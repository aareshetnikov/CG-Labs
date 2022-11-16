using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

        OpenFileDialog open_dialog;
        List<int> l1, l2;

        private void ntsc(Bitmap bmp) 
		{
			pictureBox2.Image = bmp;
			pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;

            for (int i = 0; i < bmp.Width; ++i)
                for (int j = 0; j < bmp.Height; ++j)
                {
                    Color pixelColor = bmp.GetPixel(i, j); 
                    int c = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B); 
                    l1[c]++; 
                    Color newColor = Color.FromArgb(c, c, c);
                    bmp.SetPixel(i, j, newColor); 
                }
        }

        private void hdtv(Bitmap bmp)
        {
            pictureBox3.Image = bmp;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;

            for (int i = 0; i < bmp.Width; ++i)
                for (int j = 0; j < bmp.Height; ++j)
                {
                    Color pixelColor = bmp.GetPixel(i, j); 
                    int c = (int)(0.2126 * pixelColor.R + 0.7152 * pixelColor.G + 0.0722 * pixelColor.B); 
                    l2[c]++;
                    Color newColor = Color.FromArgb(c, c, c);
                    bmp.SetPixel(i, j, newColor); 
                }
        }


        private void imageDifference(Bitmap bmp1, Bitmap bmp2)
        {
            Bitmap bmp3 = (Bitmap)bmp2.Clone();
            pictureBox4.Image = bmp3;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;

            for (int i = 0; i < bmp3.Width; ++i)
                for (int j = 0; j < bmp3.Height; ++j)
                {
                    Color pixC1 = bmp1.GetPixel(i, j);
                    Color pixC2 = bmp2.GetPixel(i, j);
                    int diff = pixC2.R - pixC1.R + pixC2.G - pixC1.G + pixC2.B - pixC1.B;                 
                    byte d = (byte)Math.Abs(diff);
                    Color newColor = Color.FromArgb(d, d, d);
                    bmp3.SetPixel(i, j, newColor);
                }
        }

        private void Histogram()
        {
            
            chart1.Series["Series1"].Points.Clear(); 
            chart1.Series["Series2"].Points.Clear();

            chart1.Series["Series1"].Color = Color.Green;
            chart1.Series["Series2"].Color = Color.Yellow;

            for (int i = 0; i < 256; ++i)
            {
                chart1.Series["Series1"].Points.AddY(l1[i]);
                chart1.Series["Series1"].Points[i].Color = Color.Green;
                chart1.Series["Series2"].Points.AddY(l2[i]);
                chart1.Series["Series2"].Points[i].Color = Color.Yellow;
            }
            chart1.Update();
        }

        private void ChangePicture(DialogResult res)
		{

			if (res == DialogResult.OK) 
			{
				
			    Bitmap bmp = new Bitmap(open_dialog.FileName); 
                    

			    pictureBox1.Image = bmp;
			    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                Bitmap bmp1 = (Bitmap)bmp.Clone(); 
                Bitmap bmp2 = (Bitmap)bmp.Clone(); 

                l1 = new List<int>();
                l2 = new List<int>();
 
                for (int i = 0; i < 256; ++i)
                {
                    l1.Add(0);
                    l2.Add(0);
                }

                ntsc(bmp1);
                hdtv(bmp2);
                imageDifference((Bitmap)bmp1.Clone(), (Bitmap)bmp2.Clone());
                Histogram();
           
            }
		}

		private void button1_Click(object sender, EventArgs e)
		{
            open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;**.PNG)|*.BMP;*.JPG;**.PNG|All files (*.*)|*.*"; 
            DialogResult dr = open_dialog.ShowDialog();
         
            ChangePicture(dr);
		}

    }
}
