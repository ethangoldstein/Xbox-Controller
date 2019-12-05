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
    public class Button
    {
        //public const int length = 5;
        public string id { get; set; }
        public bool valueCurrent;
        public bool valuePast;

        [Flags]
        public enum State : short
        {
            Pressed = 0,
            Down = 1,
            Released = 2,
            Up = 4
        };
        public State state = State.Released;
        public GamepadButtonFlags gamepad;

        //GUI
        public Point origin = new Point(300, 0);
        public Label label = new Label();
        public Label text = new Label();

        public void update()
        {
            if (!this.valuePast & !this.valueCurrent)
            {
                this.state = Button.State.Up;
            }
            else if (this.valuePast & this.valueCurrent)
            {
                this.state = Button.State.Down;
            }
            else if ((!this.valuePast) & this.valueCurrent)
            {
                this.state = Button.State.Pressed;
            }
            else if (this.valuePast & (!this.valueCurrent))
            {
                this.state = Button.State.Released;
            }
        }
    }

    
}
