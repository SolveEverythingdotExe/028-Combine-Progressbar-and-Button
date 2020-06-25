using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MainApplication
{
    public class CustomProgressBar : UserControl
    {
        private Color TextColor = Color.FromArgb(88, 88, 88);
        private Color ProgressBarColorLight = Color.FromArgb(0, 255, 0);
        private Color ProgressBarColorDark = Color.FromArgb(50, 100, 100);
        private Point MouseClickLocation;
        private int CircleBrushWidth = 20;
        private int SizeOffset = 21;
        private string StandardFont = "Trebuchet MS";
        private int Max = 100;

        private string _DefaultText = "RUN";
        public string DefaultText
        {
            get { return _DefaultText; }
            set
            {
                _DefaultText = value;
                Invalidate();
            }
        }

        public string TextOnSucess { get; set; } = "RERUN";

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                if (value > Max)
                    value = Max;
                else if (value < 0)
                    value = 0;

                _Value = value;
            }
        }

        //constructor
        public CustomProgressBar()
        {
            //set default and minimum size
            Size = new Size(200, 200);
            MinimumSize = new Size(200, 200);

            //remove flickering
            DoubleBuffered = true;

            //set default font family
            Font = new Font(StandardFont, Font.Size, Font.Style);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //on resize make sure 1:1 ratio of width and height and call invalidate to update the screen
            StabilizeSize();
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            //on resize make sure 1:1 ratio of width and height and call invalidate to update the screen
            StabilizeSize();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //we set the font on every repaint to calculate the right font size 
            Font = new Font(Font.FontFamily, (int)(Convert.ToDouble(Width) / 4.5));

            //remove pixelation on drawing of shapes and text
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //embossed effect, dark color
            using (SolidBrush brush = new SolidBrush(SystemColors.ActiveBorder))
            {
                using (Pen pen = new Pen(brush, 1))
                {
                    e.Graphics.DrawArc(pen, 2.5f, 2.5f, this.Width - SizeOffset, this.Height - SizeOffset, 0, 360);
                }
            }

            //embossed effect, light color
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                using (Pen pen = new Pen(brush, 1))
                {
                    e.Graphics.DrawArc(pen, 17.5f, 17.5f, this.Width - SizeOffset, this.Height - SizeOffset, 0, 360);
                }
            }

            //base circle
            using (SolidBrush brush = new SolidBrush(Color.Gainsboro))
            {
                using (Pen pen = new Pen(brush, CircleBrushWidth))
                {
                    e.Graphics.DrawArc(pen, 10, 10, this.Width - SizeOffset, this.Height - SizeOffset, 0, 360);
                }
            }

            //progress motion
            if (Value > 0 && Value <= 100)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, this.ProgressBarColorDark, this.ProgressBarColorLight, LinearGradientMode.Horizontal))
                {
                    using (Pen pen = new Pen(brush, CircleBrushWidth))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        e.Graphics.DrawArc(pen, 10, 10, this.Width - SizeOffset, this.Height - SizeOffset, 90, (int)Math.Round((double)((360.0 / ((double)this.Max)) * this._Value)));
                    }
                }
            }

            //progress value and text in the center
            string textDisplay = (Value == 0) ? DefaultText : Value.ToString();

            //calculate the appropriate font size to make sure the text will not elapsed on the control
            AdjustFontSize(e.Graphics, textDisplay);

            SizeF textWidth = e.Graphics.MeasureString(textDisplay, Font);
            using (SolidBrush brush = new SolidBrush(TextColor))
            {
                //display "VALUE" or "RUN"
                e.Graphics.DrawString(textDisplay, Font, brush, (Width - textWidth.Width) / 2, (Height - textWidth.Height) / 2);

                //display "RERUN"
                if (Value == Max)
                {
                    //calculate font based on Width / 15
                    Font smallFont = new Font(Font.FontFamily, (int)(Convert.ToDouble(Width) / 15));
                    SizeF successTextMeasurement = e.Graphics.MeasureString(TextOnSucess, smallFont);

                    e.Graphics.DrawString(TextOnSucess, smallFont, brush, (Width - successTextMeasurement.Width) / 2, ((Height - successTextMeasurement.Height) / 2) + (Height / 4));
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            //get the location of cursor and store it
            MouseClickLocation = e.Location;
        }

        protected override void OnClick(EventArgs e)
        {
            //check if the cursor was inside the circle if not stop the click event
            if (!InsideCircle(Width / 2, Height / 2, Width / 2, MouseClickLocation.X, MouseClickLocation.Y))
                return;

            base.OnClick(e);

            MouseClickLocation = new Point(0, 0);
        }

        private void StabilizeSize()
        {
            //we will just make sure that width and height is always equal
            int size = Math.Max(Width, Height);

            Size = new Size(size, size);
        }

        //algorithm from https://stackoverflow.com/ to check if the mouse is inside the circle
        private bool InsideCircle(int xCircle, int yCircle, int radiusCircle, int xMouse, int yMouse)
        {
            int dx = xCircle - xMouse;
            int dy = yCircle - yMouse;
            return dx * dx + dy * dy <= radiusCircle * radiusCircle;
        }

        //method to calculate the font size and to make it sure it will not elapsed on the usercontrol
        private void AdjustFontSize(Graphics g, string textToDisplay)
        {
            while (true)
            {
                SizeF size = g.MeasureString(textToDisplay, Font);

                if (size.Width >= Width - (CircleBrushWidth * 2))
                    Font = new Font(Font.FontFamily, Font.Size - 1);
                else
                    break;
            }
        }

        //call this method to run the progress after setting the "Value"
        public void PerformStep()
        {
            Invalidate();
        }

        //method to reset the progress
        public void Reset()
        {
            Value = 0;
            Invalidate();
        }
    }
}