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
            SetSize();
            /*btnStartDrawing.MouseEnter += (s, e) => {
                btnStartDrawing.BackColor = Color.Pink;
            };
            btnStartDrawing.MouseLeave += (s, e) => {
                btnStartDrawing.BackColor = Color.FromKnownColor(KnownColor.Control);
            };*/
        }

        private class ArrayPoints
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
        }

        private bool isMouse = false;
        private ArrayPoints arrayPoints = new ArrayPoints(2);

        Bitmap map = new Bitmap(100, 100);
        Graphics graphics;

        Pen pen = new Pen(Color.Black, 3f);

        private void SetSize()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // MouseHover
        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            is_cerc = true;
            is_eraser = false;
            is_Pen = false;
            is_rectangle = false;
            is_line = false;
            is_fill = false;
        }


        int x, y, sX, sY, cX, cY;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouse = true;


            //cerc
            cX = e.X;
            cY = e.Y;
        }

        /*private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (is_fill)
            {
                Point p = set_point(pictureBox1, e.Location);
                Fill(map, p.X, p.Y, pen.Color);
            }
        }*/

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouse = false;
            arrayPoints.ResetPoint();


            sX = x - cX;
            sY = y - cY;

            if (is_cerc)
            {
                graphics.DrawEllipse(pen, cX, cY, sX, sY);
                pictureBox1.Image = map;
                arrayPoints.SetPoint(e.X, e.Y);
            }
            
            if (is_rectangle)
            {
                graphics.DrawRectangle(pen, cX, cY, sX, sY);
                pictureBox1.Image = map;
                arrayPoints.SetPoint(e.X, e.Y);
            }

            if (is_line)
            {
                graphics.DrawLine(pen, cX, cY, x, y);
                pictureBox1.Image = map;
                arrayPoints.SetPoint(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouse) { return; } 

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
            }

            x = e.X;
            y = e.Y;
            sX = e.X - cX;
            sY = e.Y - cY;
        }

        private void Bcol1_Click(object sender, EventArgs e)
        {
            pen.Color = ((Button)sender).BackColor;

        }

        private void clearB_Click(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            pictureBox1.Image = map;
        }

        bool is_Pen = false;
        Pen Eraser = new Pen(Color.White, 10);
        bool is_eraser = false;
        bool is_line = false;
        bool is_cerc = false;

        private void rectanglB_Click(object sender, EventArgs e)
        {
            is_Pen = false;
            is_eraser = false;
            is_cerc = false;
            is_rectangle = true;
            is_line = false;
            is_fill = false;
        }

        private void lineB_Click(object sender, EventArgs e)
        {
            is_Pen = false;
            is_eraser = false;
            is_cerc = false;
            is_rectangle = false;
            is_line = true;
            is_fill = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)///??????не работает обновление при рисовании круга, а в видео работает
        {
            Graphics graphics = e.Graphics;


            /*if (isMouse)
            {
                if (is_cerc)
                {
                    graphics.DrawEllipse(pen, cX, cY, sX, sY);
                    pictureBox1.Image = map;
                    arrayPoints.ResetPoint();
                }

                if (is_rectangle)
                {
                    graphics.DrawRectangle(pen, cX, cY, sX, sY);
                    pictureBox1.Image = map;
                    arrayPoints.ResetPoint();
                }

                if (is_line)
                {
                    graphics.DrawLine(pen, cX, cY, x, y);
                    pictureBox1.Image = map;
                    arrayPoints.ResetPoint();
                }
            }*/
        }

        private void validate(Bitmap bitmap, Stack<Point>sp, int x, int y, Color old_c, Color new_c)
        {
            Color cx = bitmap.GetPixel(x, y);

            if (cx == old_c)
            {
                sp.Push(new Point(x, y));
                bitmap.SetPixel(x, y, new_c);
            }
        }

        private void Fill(Bitmap bitmap, int x, int y, Color new_c)
        {
            Color old_c = bitmap.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bitmap.SetPixel(x, y, new_c);
            if (old_c == new_c) return;

            while (pixel.Count > 0)
            {
                Point p =(Point)pixel.Pop();
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
            is_Pen = false;
            is_eraser = false;
            is_cerc = false;
            is_rectangle = false;
            is_line = false;
            is_fill = true;
        }

        private Point set_point(PictureBox pb, Point pt)
        {
            float px = 1f * pb.Width / pb.Width;
            float py = 1f * pb.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * py));
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (is_fill)
            {
                Point p = set_point(pictureBox1, e.Location);
                Fill(map, p.X, p.Y, pen.Color);
            }
        }

        /*private void fillB_MouseClick(object sender, MouseEventArgs e)
        {
            is_fill = true;
        }*/

        bool is_rectangle = false;
        bool is_heart = false;
        bool is_fill = false;
        private void penB_Click(object sender, EventArgs e)
        {
            is_Pen = true;
            is_eraser = false;
            is_cerc = false;
            is_rectangle = false;
            is_line=false;
            is_fill = false;
        }

        private void eraserB_Click(object sender, EventArgs e)
        {
            is_eraser = true;
            is_Pen = false;
            is_cerc = false;
            is_rectangle = false;
            is_line = false;
            is_fill = false;
        }
    }
}