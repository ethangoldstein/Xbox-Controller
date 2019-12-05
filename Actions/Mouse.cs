using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using System.Timers;
using System.Drawing;
using System.Threading;
using System.Runtime.Serialization;

namespace Xbox_Controller
{
    [Serializable()]
    public class MovePointer : ISerializable
    {
        public float speed = 10;
        public float acceleration = 6;
        InputSimulator simulator = new InputSimulator();

        public void execute(ref Joystick joystick)
        {
            float x = joystick.value.X - joystick.offset.X;
            float y = joystick.value.Y - joystick.offset.Y;

            if (x > 0)
            {
                x = (x * speed) + (acceleration * (float)Math.Pow(x, 2));
            }
            else
            {
                x = (x * speed) + (-1 * acceleration * (float)Math.Pow(x, 2));
            }

            if (y > 0)
            {
                y = (y * speed) + (acceleration * (float)Math.Pow(y, 2));
            }
            else
            {
                y = (y * speed) + (-1 * acceleration * (float)Math.Pow(y, 2));
            }

            simulator.Mouse.MoveMouseBy((int)(x), (int)(-1 * y));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("acceleration", acceleration);
            info.AddValue("speed", speed);
        }

        public void LoadObjectData(SerializationInfo info, StreamingContext context)
        {
            acceleration = (float)info.GetValue("cadScrolling", typeof(float));
            speed = (float)info.GetValue("verticalScroll", typeof(float));
        }
    }

    public class Click
    {
        public InputSimulator simulator = new InputSimulator();

        public void execute(ref Button button)
        {
            if (button.state == Button.State.Pressed)
            {
                simulator.Mouse.LeftButtonClick();
            }
            
        }        
    }

    public class Scroll
    {
        public System.Timers.Timer timer = new System.Timers.Timer();
        public InputSimulator simulator = new InputSimulator();
        public int direction;
        public int intervalSlowest = 350;
        public int intervalFastest = 50;
        public int intervalCurrent;
        public float value;

        [Flags]
        public enum Axis : short 
        { 
            X = 0,
            Y = 1,
            Both = 2
        }

        public Axis axis = Axis.Y;

        public void execute(ref Joystick joystick)
        {
            if (axis == Axis.Y)
            {
                this.value = joystick.value.Y;
            }
            else if (axis == Axis.X)
            {
                this.value = joystick.value.X;
            }
            else if (axis == Axis.Both)
            {
                throw new Exception("Cannot initialize both axes for this class.");
            }

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




