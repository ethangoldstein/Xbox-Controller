using System;
using WindowsInput;
using System.Timers;
using SharpDX.XInput;

namespace Xbox_Controller
{
    public class Keyboard
    {
        private Timer timer = new Timer();
        public UInt32 intervalInitial = 350;
        public UInt32 intervalFinal = 100;

        private InputSimulator simulator = new InputSimulator();
        public WindowsInput.Native.VirtualKeyCode key { set;  get; }
        public void keySingle(XInputController.Button button)
        {
            if ((button.valuePast == false) & button.valueCurrent)
            {
                simulator.Keyboard.KeyPress(key);
            }
        }
        
        public void keyRepeat(XInputController.Button button)
        {
            if ((button.valuePast == false) & button.valueCurrent)
            {
                simulator.Keyboard.KeyPress(key);
                SetTimer();
            }
            else if (button.valuePast & (button.valueCurrent == false))
            {
                StopTimer();
            }
        }

        public void keyToggle(XInputController.Button button)
        {
            if ((button.valuePast == false) & button.valueCurrent)
            {
                if(simulator.InputDeviceState.IsKeyDown(key))
                {
                    simulator.Keyboard.KeyUp(key);
                }
                else
                {
                    simulator.Keyboard.KeyDown(key);
                }
            }
        }
          
        //private function;
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new Timer(intervalInitial);
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
            simulator.Keyboard.KeyPress(key);
        }
    }
}