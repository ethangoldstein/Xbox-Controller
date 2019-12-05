using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;

namespace Xbox_Controller
{
    public class CADScrolling
    {
        public WindowsInput.Native.VirtualKeyCode key;
        private Scroll vertical = new Scroll();
        private Scroll horizontal = new Scroll();
        private InputSimulator simulator = new InputSimulator();

        public CADScrolling()
        { 
            vertical.axis = Scroll.Axis.Y;
            horizontal.axis = Scroll.Axis.X;
        }

        public void execute(ref Joystick joystick)
        {
            if (Math.Abs(joystick.value.Y) > Math.Abs(joystick.value.X))
            {
                simulator.Keyboard.KeyUp(key);
                vertical.execute(ref joystick);
                horizontal.StopTimer();
            }
            else if (Math.Abs(joystick.value.Y) < Math.Abs(joystick.value.X))
            {
                simulator.Keyboard.KeyDown(key);
                horizontal.execute(ref joystick);
                vertical.StopTimer();
            }
            else
            {
                simulator.Keyboard.KeyUp(key);
                vertical.StopTimer();
                horizontal.StopTimer();
            }
        }
    }
}
