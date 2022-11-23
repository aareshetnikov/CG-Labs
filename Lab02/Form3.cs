using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab02
{
    public partial class Form3 : Form
    {
        private int x1, y1, x2, y2;
        Graphics g;
        Bitmap pt;
        Color c;
  
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            x1 = Int32.Parse(textBox1.Text);
            y1 = Int32.Parse(textBox2.Text);
            x2 = Int32.Parse(textBox3.Text);
            y2 = Int32.Parse(textBox4.Text);

            textBox1.Dispose();
            textBox2.Dispose();
            textBox3.Dispose();
            textBox4.Dispose();

            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            label4.Dispose();

            button1.Dispose();

            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Size = this.Size;

            g = pictureBox1.CreateGraphics();

            BresenhamAlgorithm(x1, y1, x2, y2);
            ByAlgorithm(x1, y1, x2, y2);
        }

        //целочисленный алгоритм Брезенхема
        private void BresenhamAlgorithm(int x1,int y1,int x2,int y2)
        {
            pt = new Bitmap(1, 1);
            pt.SetPixel(0, 0, Color.Black);
            g = pictureBox1.CreateGraphics();

            var dy = Math.Abs(y2 - y1);
            var dx = Math.Abs(x2 - x1);
            if (dy <= dx)
            {
                var d = 2 * dy - dx;                
                if(y1<y2)
                {
                    g.DrawImageUnscaled(pt, x1, y1);
                    int y = y1;
                    for (int x = x1 + 1; x <= x2; x++)
                    {
                        if (d > 0)
                        {
                            y++;
                            d = d + 2 * (dy - dx);
                        }
                        else
                            d = d + 2 * dy;
                        g.DrawImageUnscaled(pt, x, y);
                    }
                }
                else
                {
                    g.DrawImageUnscaled(pt, x2, y2);
                    int y = y2;
                    for (int x = x2 - 1; x >= x1; x--)
                    {
                        if (d > 0)
                        {
                            y++;
                            d = d + 2 * (dy - dx);
                        }
                        else
                            d = d + 2 * dy;
                        g.DrawImageUnscaled(pt, x, y);                  
                    }
                }
            }
            else
            {
                var d = 2 * dx - dy;
               
                if(y1<y2)
                {
                    int x = x1;
                    g.DrawImageUnscaled(pt, x1, y1);                    
                    for (int y = y1 + 1; y <= y2; y++)
                    {
                        if (d > 0)
                        {
                            x++;
                            d = d + 2 * (dx - dy);
                        }
                        else
                            d = d + 2 * dx;
                        g.DrawImageUnscaled(pt, x, y);
                    }
                }
                else
                {
                    int x = x2;
                    g.DrawImageUnscaled(pt, x2, y2);                   
                    for (int y = y2 + 1; y <= y1; y++)
                    {
                        if (d > 0)
                        {
                            x--;
                            d = d + 2 * (dx - dy);
                        }
                        else
                            d = d + 2 * dx;
                        g.DrawImageUnscaled(pt, x, y);
                    }
                }
            }
        }

        //алгоритм ВУ
        private void ByAlgorithm(double x1, double y1, double x2, double y2)
        {
            x1 = x1 + 350;
            x2 = x2 + 350;

            pt = new Bitmap(this.Width, this.Height);
            c = Color.Black;
            pt.SetPixel(0, 0, c);
            g = pictureBox1.CreateGraphics();

            var deltax = Math.Abs(x2 - x1);
            var deltay = Math.Abs(y2 - y1);

            var dx = x2 - x1;
            var dy = y2 - y1;
            var gradient = dy / dx;

            if (deltay <= deltax)
            {
                if (y1 < y2)
                {
                    pt.SetPixel((int)x1, (int)y1, c);
                    g.DrawImageUnscaled(pt, (int)x1, (int)y1);
                    var y = y1 + gradient;
                    for (var x = x1 + 1; x <= x2 - 1; x++)
                    {
                        var sat = Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();

                        Color c2 = Color.FromArgb((int)(sat * (1.0 - (y - (int)y))), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)y);

                        c2 = Color.FromArgb((int)(sat * (y - (int)y)), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)(y + 1), c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)(y + 1));

                        y = y + gradient;
                    }
                    pt.SetPixel((int)x2, (int)y2, c);
                    g.DrawImageUnscaled(pt, (int)x2, (int)y2);
                }
                else
                {
                    pt.SetPixel((int)x1, (int)y1, c);
                    g.DrawImageUnscaled(pt, (int)x1, (int)y1);
                    var y = y1 + gradient;
                    for (var x = x1 + 1; x <= x2 - 1; x++)
                    {
                        var sat = Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();

                        Color c2 = Color.FromArgb((int)(sat * (1.0 - (y - (int)y))), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)y);

                        c2 = Color.FromArgb((int)(sat * (y - (int)y)), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)(y - 1), c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)(y - 1));

                        y = y + gradient;

                    }
                    pt.SetPixel((int)x2, (int)y2, c);
                    g.DrawImageUnscaled(pt, (int)x2, (int)y2);
                }
            }
            else
            {
                if (y1 < y2)
                {
                    gradient = dx / dy;
                    pt.SetPixel((int)x1, (int)y1, c);
                    g.DrawImageUnscaled(pt, (int)x1, (int)y1);
                    var x = x1 + gradient;
                    for (var y = y1 + 1; y <= y2 - 1; y++)
                    {
                        var sat = Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();

                        Color c2 = Color.FromArgb((int)(sat * (1.0 - (x - (int)x))), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)y);

                        c2 = Color.FromArgb((int)(sat * (x - (int)x)), c.R, c.G, c.B);
                        pt.SetPixel((int)(x + 1), (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)(x + 1), (int)y);

                        x = x + gradient;
                    }
                    pt.SetPixel((int)x2, (int)y2, c);
                    g.DrawImageUnscaled(pt, (int)x2, (int)y2);
                }
                else
                {
                    gradient = dx / dy;
                    pt.SetPixel((int)x2, (int)y2, c);
                    g.DrawImageUnscaled(pt, (int)x2, (int)y2);
                    var x = x2 + gradient;
                    for (var y = y2 + 1; y <= y1 - 1; y++)
                    {
                        var sat = Color.FromArgb(c.A, c.R, c.G, c.B).GetSaturation();

                        Color c2 = Color.FromArgb((int)(sat * (1.0 - (x - (int)x))), c.R, c.G, c.B);
                        pt.SetPixel((int)x, (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)x, (int)y);

                        c2 = Color.FromArgb((int)(sat * (x - (int)x)), c.R, c.G, c.B);
                        pt.SetPixel((int)(x - 1), (int)y, c2);
                        g.DrawImageUnscaled(pt, (int)(x - 1), (int)y);

                        x = x + gradient;
                    }
                    pt.SetPixel((int)x1, (int)y1, c);
                    g.DrawImageUnscaled(pt, (int)x1, (int)y1);
                }
            }
        }
    }
}
