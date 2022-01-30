using System;
using System.Drawing;
using System.Windows.Forms;

namespace OSINTBrowser
{
    //Canvas used on top of web browser for Screen Snipping.
    public class Canvas : Form
    {
        Point start;      // mouse-down position
        Point current;    // current mouse position
        bool drawing;
        
        public Canvas()
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.Opacity = 0.75;
            this.Cursor = Cursors.Cross;
            this.MouseDown += Canvas_MouseDown;
            this.MouseMove += Canvas_MouseMove;
            this.MouseUp += Canvas_MouseUp;
            this.Paint += Canvas_Paint;
            this.KeyDown += Canvas_KeyDown;
            this.DoubleBuffered = true;
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            current = start = e.Location;
            drawing = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            current = e.Location;
            if (drawing) this.Invalidate();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            if (drawing) e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                Math.Min(start.X, current.X),
                Math.Min(start.Y, current.Y),
                Math.Abs(start.X - current.X),
                Math.Abs(start.Y - current.Y));
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // Canvas
        //    // 
        //    this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "Canvas";
        //    this.ShowInTaskbar = false;
        //    this.ResumeLayout(false);
        //}
    }
}