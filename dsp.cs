using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WindowsInput;
using System.Timers;
using System.Threading;

namespace Xbox_Controller
{
    class Mouse
    {
        public float mouse_speed = 10;
        public float mouse_acceleration = 6;

        public string vertical_scroll_direction = "";
        InputSimulator simulator = new InputSimulator();

        public System.Timers.Timer timer = new System.Timers.Timer();
        public UInt32 intervalInitial = 350;
        public UInt32 intervalFinal = 100;

        public void moveRelative(XInputController.Joystick joystick)
        {
            float x = joystick.value.X - joystick.offset.X;
            float y = joystick.value.Y - joystick.offset.Y;

           

            if (x > 0)
            {
                x = (x * mouse_speed) + (mouse_acceleration * (float)Math.Pow(x, 2));
            }
            else
            {
                x = (x * mouse_speed) + (-1 * mouse_acceleration * (float)Math.Pow(x, 2));
            }

            if (y > 0)
            {
                y = (y * mouse_speed) + (mouse_acceleration * (float)Math.Pow(y, 2));
            }
            else
            {
                y = (y * mouse_speed) + (-1 * mouse_acceleration * (float)Math.Pow(y, 2));
            }

            simulator.Mouse.MoveMouseBy((int)(x), (int)(-1 * y));
        }
        public void scrollVertical(XInputController.Button button)
        {            
            if((button.valuePast == false) & button.valueCurrent)
            {
                scrollVertical(vertical_scroll_direction);
                SetTimer();
            }
            else if (button.valuePast & (button.valueCurrent == false))
            {
                StopTimer();
            }
        }        

        private void scrollVertical(string direction)
        {                    
            if(direction == "up")
            {
                simulator.Mouse.VerticalScroll(1);
            }
            else if(direction == "down")
            {
                simulator.Mouse.VerticalScroll(-1);
            }
            else
            {
                throw new Exception("Invalid scroll Direction"); 
            }
        }

        public void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(intervalInitial);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public void StopTimer()
        {
            timer.Stop();
            timer.Dispose();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timer.Interval = intervalFinal;
        }
    }
}
