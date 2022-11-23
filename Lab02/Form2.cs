using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02
{
    public partial class Form2 : Form
    {
        bool isBordering = false;
        bool isFilling = false;
        bool isPicFilling = false;
        String picPath = "";
        Bitmap bmp;
        Bitmap picBmp;
        Bitmap bordBmp;
        public static Color backColor;
        public static Color borderColor;
        int centX;
        int centY;

        static int leftBorder;
        //Graphics graf;

        Point lastPoint = Point.Empty;
        bool isMouseDown = new Boolean();

        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && !isFilling && !isPicFilling && !isBordering)
                if (lastPoint != null)
                {
                    if (pictureBox1.Image == null)
                    {
                        Bitmap bmp1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        pictureBox1.Image = bmp1;
                    }
                    using (Graphics g = Graphics.FromImage(pictureBox1.Image))
                    {
                        g.DrawLine(new Pen(Color.Black), lastPoint, e.Location);
                        g.SmoothingMode = SmoothingMode.None;
                    }
                    pictureBox1.Invalidate();
                    lastPoint = e.Location;
                }
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            lastPoint = Point.Empty;

            if (isFilling)
            {
                backColor = bmp.GetPixel(e.X, e.Y);
                fillColor(e.X, e.Y);
                //pictureBox1.Update();
                button4.Enabled = true;
                isFilling = false;
            }
            if (isPicFilling)
            {
                backColor = bmp.GetPixel(e.X, e.Y);
                centX = e.X;
                centY = e.Y;
                picBmp = new Bitmap(Image.FromFile(picPath));
                fillPicture(e.X, e.Y);
                button4.Enabled = true;
                isPicFilling = false;
            }
            if (isBordering)
            {
                bordBmp = new Bitmap(pictureBox1.Image);
                borderColor = bordBmp.GetPixel(e.X, e.Y);
                drawBorder(e.X, e.Y);
                button6.Enabled = true;
                isBordering = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
                Invalidate();
            }
            button6.Enabled = false;
            label5.Text = "Нет катринки";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*graf = pictureBox1.CreateGraphics();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.BackColor = colorDialog1.Color;
            }
            this.Update();
            pictureBox3.Update();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = openFileDialog1.FileName;
                picPath = path;
                //pictureBox1.Image = Image.FromFile(path);
                String text = "";
                if (path.Length > 15)
                    text = $"...{path.Substring(path.Length - 12, 12)}";
                else
                    text = path;
                label2.Text = text;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                button2.Enabled = true;
                button3.Enabled = false;
                button4.Enabled = true;
            }
                
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                button2.Enabled = false;
                button3.Enabled = true;
                button4.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Image);
            //pictureBox1.Cursor = Cursors.
            if (radioButton1.Checked)
            {
                isFilling = true;
                button4.Enabled = false;
            }
            else if (radioButton2.Checked)
            {
                isPicFilling = true;
                button4.Enabled = false;
            }
        }

        public void fillColor(int x, int y)
        {

            while (bmp.GetPixel(x, y) == backColor && x > 0)
                x--;
            Color bord = bmp.GetPixel(x, y);
            leftBorder = ++x;
            while (bmp.GetPixel(x, y) == backColor && x < pictureBox1.Width - 1)
            {
                bmp.SetPixel(x, y, pictureBox3.BackColor);
                pictureBox1.Image = bmp;
                pictureBox1.Update();
                x++;
            }
            x = leftBorder;
            while (bmp.GetPixel(x, y) != Color.FromArgb(255, 0, 0, 0)/*== pictureBox3.BackColor*/ && y > 0)
            {
                if (bmp.GetPixel(x, y - 1) == backColor)
                    fillColor(x, y - 1);

                if (bmp.GetPixel(x, y + 1) == backColor)
                    fillColor(x, y + 1);

                ++x;
            }
            //pictureBox1.Image = bmp;

            //pictureBox1.Update();
            /*x = leftBorder;
            while (bmp.GetPixel(x, y) == pictureBox3.BackColor && y < pictureBox1.Height - 1)
            {
                if (bmp.GetPixel(x, y + 1) == backColor)
                    fillColor(x, y + 1);
                ++x;
            }*/

        }

        public void fillPicture(int x, int y)
        {
            while (bmp.GetPixel(x, y) == backColor && x > 0)
                x--;
            Color bord = bmp.GetPixel(x, y);
            leftBorder = ++x;
            while (bmp.GetPixel(x, y) == backColor && x < pictureBox1.Width - 1)
            {
                try { bmp.SetPixel(x, y, picBmp.GetPixel(x - centX + picBmp.Width / 2, y - centY + picBmp.Height / 2)); }
                catch (Exception e) { bmp.SetPixel(x, y, pictureBox3.BackColor); }
                pictureBox1.Image = bmp;
                pictureBox1.Update();
                x++;
            }
            x = leftBorder;

            while ((bmp.GetPixel(x, y) == pictureBox3.BackColor) || (bmp.GetPixel(x, y) == picBmp.GetPixel(x - centX + picBmp.Width / 2, y - centY + picBmp.Height / 2)) && y > 0)
            {
                if (bmp.GetPixel(x, y - 1) == backColor)
                    fillPicture(x, y - 1);

                if (bmp.GetPixel(x, y + 1) == backColor)
                    fillPicture(x, y + 1);

                ++x;
            }
            /*if (bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0))
            {
                while (bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0) && x > 0)
                    x--;
                Color bord = bmp.GetPixel(x, y);
                leftBorder = ++x;
                while (bmp.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0) && x < pictureBox1.Width - 1)
                {
                    bmp.SetPixel(x, y, picBmp.GetPixel(x, y));
                    pictureBox1.Image = bmp;
                    pictureBox1.Update();
                    x++;
                }
                x = leftBorder;
                while (bmp.GetPixel(x, y) != Color.FromArgb(255, 0, 0, 0) && y > 0)
                {
                    fillPicture(x, y - 1);
                    x++;
                }
                x = leftBorder;
                while (bmp.GetPixel(x, y) != Color.FromArgb(255, 0, 0, 0) && y < pictureBox1.Height - 1)
                {
                    fillPicture(x, y + 1);
                    x++;
                }
            }*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String path = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(path);
                String text = "";
                if (path.Length > 36)
                    text = $"...{path.Substring(path.Length - 33, 33)}";
                else
                    text = path;
                label5.Text = text;
                button6.Enabled = true;
            }
        }

        public void drawBorder(int x, int y)
        {
            List<Point> points = new List<Point>();
            
            while (bordBmp.GetPixel(x, y) == borderColor)
                x--;
            ++x;

            int now = 4;

            while (true)
            {
                switch (now)
                {
                    case 0:
                        if (bordBmp.GetPixel(x + 1, y) == borderColor)
                        {
                            points.Add(new Point(x + 1, y));
                            x = x + 1;
                            now = 6;
                        }
                        else
                            now = 1;
                        break;
                    case 1:
                        if (bordBmp.GetPixel(x + 1, y - 1) == borderColor)
                        {
                            points.Add(new Point(x + 1, y - 1));
                            x = x + 1;
                            y = y - 1;
                            now = 7;
                        }
                        else
                            now = 2;
                        break;
                    case 2:
                        if (bordBmp.GetPixel(x, y - 1) == borderColor)
                        {
                            points.Add(new Point(x, y - 1));
                            y = y - 1;
                            now = 0;
                        }
                        else
                            now = 3;
                        break;
                    case 3:
                        if (bordBmp.GetPixel(x - 1, y - 1) == borderColor)
                        {
                            points.Add(new Point(x - 1, y - 1));
                            x = x - 1;
                            y = y - 1;
                            now = 1;
                        }
                        else
                            now = 4;
                        break;
                    case 4:
                        if (bordBmp.GetPixel(x - 1, y) == borderColor)
                        {
                            points.Add(new Point(x - 1, y));
                            x = x - 1;
                            now = 2;
                        }
                        else
                            now = 5;
                        break;
                    case 5:
                        if (bordBmp.GetPixel(x - 1, y + 1) == borderColor)
                        {
                            points.Add(new Point(x - 1, y + 1));
                            x = x - 1;
                            y = y + 1;
                            now = 3;
                        }
                        else
                            now = 6;
                        break;
                    case 6:
                        if (bordBmp.GetPixel(x, y + 1) == borderColor)
                        {
                            points.Add(new Point(x, y + 1));
                            y = y + 1;
                            now = 4;
                        }
                        else
                            now = 7;
                        break;
                    case 7:
                        if (bordBmp.GetPixel(x + 1, y + 1) == borderColor)
                        {
                            points.Add(new Point(x + 1, y + 1));
                            x = x + 1;
                            y = y + 1;
                            now = 5;
                        }
                        else
                            now = 0;
                        break;
                }
                HashSet<Point> hs = new HashSet<Point>(points);
                if (points.Count > hs.Count)
                    break;
            }

            for (int i = 0; i < points.Count; i++)
            {
                bordBmp.SetPixel(points[i].X, points[i].Y, Color.Black);
                pictureBox1.Image = bordBmp;
                pictureBox1.Update();
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            button6.Enabled = false;
            isBordering = true;
        }
    }
}
