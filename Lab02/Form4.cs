using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Lab02
{
    public partial class Form4 : Form
    {
        Point[] points = new Point[3];

        int step, steps;
        int CounterSetPoint;
        int deltaX, deltaY;
        int signX, signY;
        int e1, e2;

        Bitmap bmp;
        PictureBox pbox;
        Dictionary<int, GradientColors> dictionary = new Dictionary<int, GradientColors>();

        private class GradientColors
        {
            public int leftX;
            public int rightX;
            public Color leftColor;
            public Color rightColor;

            public GradientColors(int leftX, int rightX, Color leftColor, Color rightColor)
            {
                this.leftX = leftX;
                this.rightX = rightX;
                this.leftColor = leftColor;
                this.rightColor = rightColor;
            }
        }
        private Color LerpRGB(Color color1, Color color2)
        {
            return Color.FromArgb(CalculateParametrColor(color1.R, color2.R), CalculateParametrColor(color1.G, color2.G), CalculateParametrColor(color1.B, color2.B));
        }

        private int CalculateParametrColor(int parametrColor1, int parametrColor2)
        {
            return parametrColor1 + (parametrColor2 - parametrColor1) * step / steps;
        }
        private int CalculateDelta(int a, int b) => Math.Abs(b - a);
        private int CalculateSign(int a, int b) => a < b ? 1 : -1;
        private int CalculateCountSteps(Point point1, Point point2)
        {
            int result = 0;


            Point pointForWork = point1;

            signX = CalculateSign(point1.X, point2.X);
            signY = CalculateSign(point1.Y, point2.Y);

            deltaX = CalculateDelta(point1.X, point2.X);
            deltaY = CalculateDelta(point1.Y, point2.Y);

            e1 = (deltaX > deltaY ? deltaX : -deltaY) / 2;

            while (pointForWork.X != point2.X || pointForWork.Y != point2.Y)
            {
                result++;
                e2 = e1;
                if (e2 > -deltaX)
                {
                    e1 -= deltaY;
                    pointForWork.X += signX;
                }
                if (e2 < deltaY)
                {
                    e1 += deltaX;
                    pointForWork.Y += signY;
                }
            }

            return result;
        }

        private void DrawBordersGradient(Point point1, Point point2, Color color1, Color color2, Dictionary<int, GradientColors> dict)
        {
            step = 0;
            steps = CalculateCountSteps(point1, point2);

            while (point1.X != point2.X || point1.Y != point2.Y)
            {
                Color colorForWork = LerpRGB(color1, color2);
                if (dict.ContainsKey(point1.Y))
                {

                    if (dict[point1.Y].leftX > point1.X)
                    {
                        dict[point1.Y].leftX = point1.X;
                        dict[point1.Y].leftColor = colorForWork;
                    }
                    if (dict[point1.Y].rightX < point1.X)
                    {
                        dict[point1.Y].rightX = point1.X;
                        dict[point1.Y].rightColor = colorForWork;
                    }

                }
                else
                    dict.Add(point1.Y, new GradientColors(point1.X, point1.X, colorForWork, colorForWork));
                step++;

                bmp.SetPixel(point1.X, point1.Y, colorForWork);
                pictureBox1.Invalidate();

                e2 = e1;

                if (e2 > -deltaX)
                {
                    e1 -= deltaY;
                    point1.X += signX;
                }
                if (e2 < deltaY)
                {
                    e1 += deltaX;
                    point1.Y += signY;
                }
            }
        }
        private void DrawOneLineGradient(Point point1, Point point2, Color color1, Color color2)
        {
            step = 0;
            steps = CalculateCountSteps(point1, point2);

            while (point1.X != point2.X || point1.Y != point2.Y)
            {
                Color colorForWork = LerpRGB(color1, color2);
                bmp.SetPixel(point1.X, point1.Y, colorForWork);
                pictureBox1.Invalidate();

                e2 = e1;
                if (e2 > -deltaX)
                {
                    e1 -= deltaY;
                    point1.X += signX;
                }
                if (e2 < deltaY)
                {
                    e1 += deltaX;
                    point1.Y += signY;
                }
                step++;
            }
        }
        private void CreateGradient(Color color1, Color color2, Color color3)
        {
            Dictionary<int, GradientColors> dict = new Dictionary<int, GradientColors>();

            DrawBordersGradient(points[0], points[1], color1, color2, dict);
            DrawBordersGradient(points[0], points[2], color1, color3, dict);
            DrawBordersGradient(points[1], points[2], color2, color3, dict);

            foreach (var t in dict)
            {
                int y = t.Key;
                Point pt1 = new Point(t.Value.leftX, y);
                Point pt2 = new Point(t.Value.rightX, y);
                DrawOneLineGradient(pt1, pt2, t.Value.leftColor, t.Value.rightColor);
            }
        }

        public Form4()
        {
            InitializeComponent();
            CounterSetPoint = 0;
            pbox = this.pictureBox1;
            bmp = new Bitmap(pbox.Width, pbox.Height);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (CounterSetPoint > 2)
            {
                CounterSetPoint = 0;
                pictureBox1.Invalidate();
            }

            points[CounterSetPoint] = new Point(e.X, e.Y);
            CounterSetPoint++;

            Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            if (CounterSetPoint > 2)
            {
                CreateGradient(Color.Blue, Color.Red, Color.Green);
            }

        }
    }
}