using System;
using System.Collections.Generic;
using System.Text;
using SharpDX.XInput;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace Xbox_Controller
{
    public class XInputController
    {
        public Button[] button;
        public Joystick[] joystick;
        public Trigger[] trigger;        

        private Controller controller;
        private Gamepad state;
        private bool connected = false;

        private T[] InitializeArray<T>(int length) where T : new()
        {
            T[] array = new T[length];
            for (int i = 0; i < length; ++i)
            {
                array[i] = new T();
            }

            return array;
        }

        public XInputController()
        {
            button = InitializeArray<Button>(14);
            joystick = InitializeArray<Joystick>(2);
            trigger = InitializeArray<Trigger>(2);

            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;

            if (!connected)
                throw new System.ArgumentException("Failed to connect to the controller.");

            Update();

            button[0].id = "A";
            button[0].gamepad = GamepadButtonFlags.A;
            button[1].id = "B";
            button[1].gamepad = GamepadButtonFlags.B;
            button[2].id = "X";
            button[2].gamepad = GamepadButtonFlags.X;
            button[3].id = "Y";
            button[3].gamepad = GamepadButtonFlags.Y;
            button[4].id = "leftShoulder";
            button[4].gamepad = GamepadButtonFlags.RightShoulder;
            button[5].id = "rightShoulder";
            button[5].gamepad = GamepadButtonFlags.LeftShoulder;
            button[6].id = "leftJoystick";
            button[6].gamepad = GamepadButtonFlags.LeftThumb;
            button[7].id = "rightJoystick";
            button[7].gamepad = GamepadButtonFlags.RightThumb;
            button[8].id = "Back";
            button[8].gamepad = GamepadButtonFlags.Back;
            button[9].id = "Start";
            button[9].gamepad = GamepadButtonFlags.Start;
            button[10].id = "Down";
            button[10].gamepad = GamepadButtonFlags.DPadDown;
            button[11].id = "Up";
            button[11].gamepad = GamepadButtonFlags.DPadUp;
            button[12].id = "Left";
            button[12].gamepad = GamepadButtonFlags.DPadLeft;
            button[13].id = "Right";
            button[13].gamepad = GamepadButtonFlags.DPadRight;


            for (int i = 0; i<button.Length; i++)
            {
                button[i].label.AutoSize = false;
                button[i].label.Text = button[i].id;
                button[i].label.Location = new Point(0, i * 20) + (Size)button[i].origin;
                button[i].label.TextAlign = ContentAlignment.MiddleRight;

                button[i].text.Enabled = true;
                button[i].text.Location = new Point(0, i * 20) + (Size)button[i].origin + new Size(100, 0);
                button[i].text.TextAlign = ContentAlignment.MiddleLeft;
            }

            joystick[0].id = "leftJoystick";
            joystick[1].id = "rightJoystick";

            for(int i = 0; i<joystick.Length; i++)
            {
                joystick[i].label.AutoSize = false;
                joystick[i].label.Text = joystick[i].id;
                joystick[i].label.Location = new Point(0, i * 20) + (Size)joystick[i].origin;
                joystick[i].label.TextAlign = ContentAlignment.MiddleRight;

                joystick[i].text.Enabled = true;
                joystick[i].text.Location = new Point(0, i * 20) + (Size)joystick[i].origin + new Size( 100 , 0);
                joystick[i].text.Size = new Size(200, 20);
                joystick[i].text.TextAlign = ContentAlignment.MiddleLeft;

                joystick[i].deadband = (float)0.08;
                joystick[i].offset = joystick[i].value;
            }

            trigger[0].id = "leftTrigger";
            trigger[1].id = "rightTrigger";

            for(int i = 0; i<trigger.Length; i++)
            {
                trigger[i].label.AutoSize = true;
                trigger[i].label.Text = trigger[i].id;
                trigger[i].label.Location = new Point(0, i * 20) + (Size)trigger[i].origin;
                trigger[i].label.TextAlign = ContentAlignment.MiddleRight;

                trigger[i].text.Enabled = true;
                trigger[i].text.Location = new Point(0, i * 20) + (Size)trigger[i].origin + new Size(100, 0);
                trigger[i].text.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        // Call this method to update all class values
        public void Update()
        {
            if (!connected)
                throw new System.ArgumentException("The controller is not connected.");

            try
            {
                state = controller.GetState().Gamepad;
            }
            catch
            {
                throw new System.ArgumentException("Cannot update the controller.");
            }
           

            joystick[0].value = new PointF(state.LeftThumbX/joystick[0].max, state.LeftThumbY/joystick[0].max);
            joystick[1].value = new PointF(state.RightThumbX/joystick[1].max, state.RightThumbY/joystick[1].max);

            joystick[0].update();
            joystick[1].update();

            trigger[0].value = ((float)state.LeftTrigger)/trigger[0].max;
            trigger[1].value = ((float)state.RightTrigger) / trigger[1].max;

            for (int i = 0; i < button.Length; i++)
            {
                button[i].valuePast = button[i].valueCurrent;
                button[i].valueCurrent = state.Buttons.HasFlag(button[i].gamepad);
                button[i].update();
            }
        }

        

    }

}
