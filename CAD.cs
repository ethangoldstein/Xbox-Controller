using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using System.Timers;
using System.Drawing;
using System.Threading;

namespace Xbox_Controller
{
    class CADScrolling
    {
        public WindowsInput.Native.VirtualKeyCode key;
        private Scroll vertical = new Scroll();
        private Scroll horizontal = new Scroll();
        private InputSimulator simulator = new InputSimulator();

        public void execute(XInputController.Joystick joystick)
        {
            if (Math.Abs(joystick.value.Y) > Math.Abs(joystick.value.X))
            {
                simulator.Keyboard.KeyUp(key);
                vertical.execute(joystick.value.Y);
                horizontal.StopTimer();
            }
            else if (Math.Abs(joystick.value.Y) < Math.Abs(joystick.value.X))
            {
                simulator.Keyboard.KeyDown(key);
                horizontal.execute(joystick.value.X*-1);   
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


    class Scroll
    {
        public System.Timers.Timer timer = new System.Timers.Timer();
        public InputSimulator simulator = new InputSimulator();
        public int direction;
        public int intervalSlowest = 350;
        public int intervalFastest = 50;
        public int intervalCurrent;
        public float value;

        public void execute(float value)
        {
            this.value = value;
            if (!timer.Enabled)
            {
                if (value != 0)
                {
                    if (value > 0)
                    {
                        direction = 1;
                        simulator.Mouse.VerticalScroll(direction);
                    }
                    else if (value < 0)
                    {
                        direction = -1;
                        simulator.Mouse.VerticalScroll(direction);
                    }

                    SetTimer();
                }
            }
            else
            {
                if (value == 0)
                {
                    StopTimer();
                }
                else if (value < 0 & direction > 0)
                {
                    StopTimer();
                }
                else if (value > 0 & direction < 0)
                {
                    StopTimer();
                }
            }
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(intervalSlowest);
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
            intervalCurrent = (int)(Math.Abs(1 - Math.Abs(value)) * intervalSlowest);

            if (intervalCurrent < intervalFastest)
            {
                intervalCurrent = intervalFastest;

            }
            timer.Interval = intervalCurrent;
            simulator.Mouse.VerticalScroll(direction);
        }
    }
}




