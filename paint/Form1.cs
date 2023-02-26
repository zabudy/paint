using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace paint
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);
            graphics.Clear(Color.White);
            pictureBox1.Image = map;
            pictureBox1.BackColor = Color.Pink;

        }

        /*private class ArrayPoints
        {
            private int index = 0;
            private Point[] points;
            
            public ArrayPoints(int size)
            {
                if (size <= 0) { size = 2; }
                points = new Point[size];
            }

            public void SetPoint(int x, int y)
            {
                if (index >= points.Length)
                {
                    index = 0;
                }
                points[index] = new Point(x, y);
                index++;
            }

            public void ResetPoint()
            {
                index = 0;
            }

            public int GetCountPoints()
            {
                return index;
            }

            public Point[] GetPoints()
            {
                return points;
            }
        }*/


        Bitmap map = new Bitmap(100, 100);
        Graphics graphics;
        Pen pen = new Pen(Color.Black, 2);
        Pen Eraser = new Pen(Color.White, 10);

        bool is_mouse = false;
        Point px, py;

        int index = 0;
        int x, y, sX, sY, cX, cY;

        Color new_clr;


        private void Form1_Load(object sender, EventArgs e)
        {
           // MouseHover
        }

        

        private void button1_Click(object sender, EventArgs e)//cercle button
        {
            index = 3;
        }


        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            is_mouse = true;
            py = e.Location;

            cX = e.X;
            cY = e.Y;
            
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            is_mouse = false;

            sX = x - cX;
            sY = y - cY;

            if (index == 3)
                graphics.DrawEllipse(pen, cX, cY, sX, sY);
            if (index == 4)
                graphics.DrawRectangle(pen, cX, cY, sX, sY);
            if (index == 5)
                graphics.DrawLine(pen, cX, cY, x, y);
            

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (is_mouse)
            {
                if (index == 1)
                {
                    px = e.Location;
                    graphics.DrawLine(pen, px, py);
                    py = px;
                }
                if (index == 2)
                {
                    px = e.Location;
                    graphics.DrawLine(Eraser, px, py);
                    py = px;
                }

                pictureBox1.Refresh();

                x = e.X;
                y = e.Y;
                sX = e.X - cX;
                sY = e.Y - cY;
            }
            /*if (!isMouse) { return; } 

            arrayPoints.SetPoint(e.X, e.Y);
            if (arrayPoints.GetCountPoints() >= 2 && is_Pen)
            {
                graphics.DrawLines(pen, arrayPoints.GetPoints());
                pictureBox1.Image = map;
                arrayPoints.SetPoint(e.X, e.Y);
            }
            if (arrayPoints.GetCountPoints() >= 2 && is_eraser)
            {
                graphics.DrawLines(Eraser, arrayPoints.GetPoints());
                pictureBox1.Image = map;
                arrayPoints.SetPoint(e.X, e.Y);
            }*/
        }

        private void Bcol1_Click(object sender, EventArgs e)
        {
           pen.Color = ((Button)sender).BackColor;
           new_clr = ((Button)sender).BackColor;

        }//Color

        private void clearB_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            pictureBox1.Image = map;
        }

        

        private void rectanglB_Click(object sender, EventArgs e)
        {
            index = 4;
        }//rectangle button

        private void lineB_Click(object sender, EventArgs e)
        {
            index = 5;
        }//line button

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            if (is_mouse)
            {
                if (index == 3)
                    graphics.DrawEllipse(pen, cX, cY, sX, sY);
                if (index == 4)
                    graphics.DrawRectangle(pen, cX, cY, sX, sY);
                if (index == 5)
                    graphics.DrawLine(pen, cX, cY, x, y);
            }
        }//Отрисовка фигур во время движения мыши

        private void validate(Bitmap bitmap, Stack<Point>sp, int x, int y, Color old_c, Color new_c)
        {
            Color cx = bitmap.GetPixel(x, y);

            if (cx == old_c)
            {
                sp.Push(new Point(x, y));
                bitmap.SetPixel(x, y, new_c);
            }
        }

        private void fillB_MouseClick(object sender, MouseEventArgs e)
        {
            index = 7;
        }

        private void fillB_Click_1(object sender, EventArgs e)
        {
            index = 7;
        }

        private void heartB_Click(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            pictureBox1.Image = map;
        }

        private void Fill(Bitmap bitmap, int x, int y, Color new_c)
        {
            Color old_c = bitmap.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bitmap.SetPixel(x, y, new_c);// покарсили нашу точку в новый цвет
            if (old_c == new_c) return;//если наша точка того же цвета, что и цвет пера, выходим

            while (pixel.Count > 0)
            {
                Point p = pixel.Pop();// достали одну точку???
                if (p.X >0 && p.Y > 0 && p.X < bitmap.Width - 1 && p.Y < bitmap.Height - 1)
                {
                    validate(bitmap, pixel, p.X - 1, p.Y, old_c, new_c);
                    validate(bitmap, pixel, p.X + 1, p.Y, old_c, new_c);
                    validate(bitmap, pixel, p.X, p.Y - 1, old_c, new_c);
                    validate(bitmap, pixel, p.X, p.Y + 1, old_c, new_c);
                }
            }
        }

        private void fillB_Click(object sender, EventArgs e)
        {
            index = 7;
        }

        private Point set_point(PictureBox pb, Point pt)
        {
            float pX = 1f * pb.Width / pb.Width;
            float pY = 1f * pb.Height / pb.Height;
            return new Point((int)(pt.X * pX), (int)(pt.Y * pY));
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (index == 7)
            {
                Point point = set_point(pictureBox1, e.Location);
                Fill(map, point.X, point.Y, pen.Color);
                pictureBox1.Refresh();
            }
        }

       

        private void penB_Click(object sender, EventArgs e)
        {
            index = 1;
        }

        private void eraserB_Click(object sender, EventArgs e)
        {
            index = 2;
        }
    }
}