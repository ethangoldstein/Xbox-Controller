using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Xbox_Controller
{
    
    public class Joystick
    {
        public string id { get; set; }
        public float deadband { get; set; }
        public PointF offset;
        public float max { get; } = Int16.MaxValue;
        public float min { get; } = Int16.MinValue;
        public PointF value { get; set; } = new PointF(0, 0);

        public Point origin = new Point(0, 0);
        public Label label = new Label();
        public Label text = new Label();

        public void update()
        {
            if (Math.Abs(this.value.X) < this.deadband)
            {
                this.value = new PointF(0, this.value.Y);
            }
            else
            {
                if (this.value.X < 0)
                {
                    this.value = new PointF((this.value.X + this.deadband), this.value.Y);
                }
                else if (this.value.X > 0)
                {
                    this.value = new PointF((this.value.X - this.deadband), this.value.Y);
                }
            }

            if (Math.Abs(this.value.Y) < this.deadband)
            {
                this.value = new PointF(this.value.X, 0);
            }
            else
            {
                if (this.value.Y < 0)
                {
                    this.value = new PointF(this.value.X, (this.value.Y + this.deadband));
                }
                else if (this.value.Y > 0)
                {
                    this.value = new PointF(this.value.X, (this.value.Y - this.deadband));
                }
            }

            this.value = new PointF(this.value.X, this.value.Y);
        }
    }
}
