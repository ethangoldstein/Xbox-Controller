using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using System.Timers;
using System.Drawing;

namespace Xbox_Controller
{
    class CAD
    {
        double a = 0.8;
        double b = 0.2;

        float max_interval = 100;
        float min_interval = 1000;
        PointF interval;
        Point direction = new Point();
        
        public System.Timers.Timer timer = new System.Timers.Timer();
        public UInt32 intervalInitial = 350;
        public UInt32 intervalFinal = 100;

        InputSimulator simulator = new InputSimulator();

        public void panCAD(XInputController.Joystick joystick)
        {
            double y = (double)joystick.value.Y;
            double x = (double)joystick.value.X;

            interval.X = (float)(max_interval * (a * Math.Abs(x) + b * Math.Pow(Math.Abs(y), 2)));
            interval.Y = (float)(max_interval * (a * Math.Abs(y) + b * Math.Pow(Math.Abs(y), 2)));

            if (interval.X < min_interval)
            {
                interval.X = min_interval;
            }
            else if (interval.X > max_interval)
            {
                interval.X = max_interval;
            }

            if (timer.Enabled)
            {
                if (x > 0)
                {
                    direction.X = 1;
                    timer.Interval = interval.X;
                }
                else if (x < 0)
                {
                    direction.X = -1;
                    timer.Interval = interval.X;
                }
                else
                {
                    timer.Enabled = false;
                }
            }
            else 
            {
                if (x > 0)
                {
                    direction.X = 1;
                    SetTimer((uint)interval.X);
                    timer.Enabled = true;
                }
                else if (x < 0)
                {
                    direction.X = -1;
                    SetTimer((uint)interval.X);
                    timer.Enabled = true;
                }
            }

        }
        public void SetTimer(uint interval)
        {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(interval);
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
            simulator.Mouse.VerticalScroll(direction.X);
        }
    }


}
