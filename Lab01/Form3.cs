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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private static Bitmap image2, image3, image4;

		/*private void Form1_Load(object sender, EventArgs e)
        {

        }*/

		List<int> lr, lg, lb;

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void ShowPictures()
        {
            pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;

            image2 = new Bitmap(openFileDialog1.FileName, true);
            pictureBox2.Image = image2;
            image3 = new Bitmap(openFileDialog1.FileName, true);
            pictureBox3.Image = image3;
            image4 = new Bitmap(openFileDialog1.FileName, true);
            pictureBox4.Image = image4;

			lr = new List<int>();
			lg = new List<int>();
			lb = new List<int>();

			for (int i = 0; i < 256; ++i)
			{
				lr.Add(0);
				lg.Add(0);
				lb.Add(0);
			}


            for (int x = 0; x < image2.Width; x++)
            {
                for (int y = 0; y < image2.Height; y++)
                {
                    Color pixelColor = image2.GetPixel(x, y);

                    Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    image2.SetPixel(x, y, newColor);

                    newColor = Color.FromArgb(0, pixelColor.G, 0);
                    image3.SetPixel(x, y, newColor);

                    newColor = Color.FromArgb(0, 0, pixelColor.B);
                    image4.SetPixel(x, y, newColor);

					++lr[pixelColor.R];
					++lg[pixelColor.G];
					++lb[pixelColor.B];
                }
            }
			chart3.Series["Series1"].Points.Clear();
			chart4.Series["Series1"].Points.Clear();
			chart5.Series["Series1"].Points.Clear();

            chart3.Series["Series1"].Color = Color.Red;
            chart4.Series["Series1"].Color = Color.Green;
            chart5.Series["Series1"].Color = Color.Blue;

            for (int i = 0; i < 256; ++i)
			{
				chart3.Series["Series1"].Points.AddY(lr[i]);
				chart3.Series["Series1"].Points[i].Color = Color.Red;
				chart4.Series["Series1"].Points.AddY(lg[i]);
				chart4.Series["Series1"].Points[i].Color = Color.Green;
				chart5.Series["Series1"].Points.AddY(lb[i]);
				chart5.Series["Series1"].Points[i].Color = Color.Blue;
			}
			chart3.Update();
			chart4.Update();
			chart5.Update();

		}

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            ShowPictures();
        }
       
    }
}
