using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using System.Drawing;
using System.Windows.Forms;

namespace Xbox_Controller
{
    public class Trigger
    {
        public float max { get; } = byte.MaxValue;
        public float min { get; } = byte.MinValue;
        public float value { get; set; }

        public string id { get; set; }

        public Point origin = new Point(0, 100);
        public Label label = new Label();
        public Label text = new Label();
    }
}
