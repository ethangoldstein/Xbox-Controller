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
        public Pad pad;
        public Joystick[] joystick;
        public Trigger[] trigger;

        private Controller controller;
        private Gamepad state;
        private bool connected = false;

        public class Button
        {
           //public const int length = 5;
            public string id { get; set; }
            public bool valueCurrent;
            public bool valuePast;

            //GUI
            public Point origin = new Point(300, 0);
            public Label label = new Label();
            public TextBox text = new TextBox();
        }
    
        public class Pad
        {
            public const int length = 5;
            public string id { get; set; }
            public bool up { get; set; }
            public bool down { get; set; }
            public bool left { get; set; }
            public bool right { get; set; }

        }
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
            public TextBox text = new TextBox();
        }
        
        public class Trigger
        {
            public float max { get; } = byte.MaxValue;
            public float min { get; } = byte.MinValue;
            public float value { get; set; }

            public string id { get; set; }

            public Point origin = new Point(0, 100);
            public Label label = new Label();
            public TextBox text = new TextBox();
        }

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
            button = InitializeArray<Button>(10);
            joystick = InitializeArray<Joystick>(2);
            trigger = InitializeArray<Trigger>(2);
            pad = new Pad();

            controller = new Controller(UserIndex.One);
            connected = controller.IsConnected;

            if (!connected)
                throw new System.ArgumentException("Failed to connect to the controller.");

            Update();

            button[0].id = "A";
            button[1].id = "B";
            button[2].id = "X";
            button[3].id = "Y";
            button[4].id = "leftShoulder";
            button[5].id = "rightShoulder";
            button[6].id = "leftJoystick";
            button[7].id = "rightJoystick";
            button[8].id = "Back";
            button[9].id = "Start";

            for(int i = 0; i<button.Length; i++)
            {
                button[i].label.AutoSize = false;
                button[i].label.Text = button[i].id;
                button[i].label.Location = new Point(0, i * 20) + (Size)button[i].origin;
                button[i].label.TextAlign = ContentAlignment.MiddleRight;

                button[i].text.Enabled = false;
                button[i].text.Location = new Point(0, i * 20) + (Size)button[i].origin + new Size(100, 0);
            }

            pad.id = "Pad";

            joystick[0].id = "leftJoystick";
            joystick[1].id = "rightJoystick";

            for(int i = 0; i<joystick.Length; i++)
            {
                joystick[i].label.AutoSize = false;
                joystick[i].label.Text = joystick[i].id;
                joystick[i].label.Location = new Point(0, i * 20) + (Size)joystick[i].origin;
                joystick[i].label.TextAlign = ContentAlignment.MiddleRight;

                joystick[i].text.Enabled = false;
                joystick[i].text.Location = new Point(0, i * 20) + (Size)joystick[i].origin + new Size( 100 , 0);
                joystick[i].text.Size = new Size(200, 20);

                joystick[i].deadband = (float)0.06;
                joystick[i].offset = joystick[i].value;
            }

            trigger[0].id = "leftTrigger";
            trigger[1].id = "rightTrigger";

            for(int i = 0; i<trigger.Length; i++)
            {
                trigger[i].label.AutoSize = false;
                trigger[i].label.Text = trigger[i].id;
                trigger[i].label.Location = new Point(0, i * 20) + (Size)trigger[i].origin;
                trigger[i].label.TextAlign = ContentAlignment.MiddleRight;

                trigger[i].text.Enabled = false;
                trigger[i].text.Location = new Point(0, i * 20) + (Size)trigger[i].origin + new Size(100, 0);
            }
        }

        // Call this method to update all class values
        public void Update()
        {
            if (!connected)
                throw new System.ArgumentException("The controller is not connected.");
            
            foreach(Button obj in button)
            {
                obj.valuePast = obj.valueCurrent;
            }

            state = controller.GetState().Gamepad;

            joystick[0].value = new PointF(state.LeftThumbX/joystick[0].max, state.LeftThumbY/joystick[0].max);
            joystick[1].value = new PointF(state.RightThumbX/joystick[1].max, state.RightThumbY/joystick[1].min);
            
            trigger[0].value = ((float)state.LeftTrigger)/trigger[0].max;
            trigger[1].value = ((float)state.RightTrigger) / trigger[1].max;

            button[0].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.A);
            button[1].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.B);
            button[2].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.X);
            button[3].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.Y);
            button[4].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
            button[5].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);
            button[6].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
            button[7].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.RightThumb);
            button[8].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.Back);
            button[9].valueCurrent = state.Buttons.HasFlag(GamepadButtonFlags.Start);

            pad.up = state.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
            pad.down = state.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
            pad.left = state.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
            pad.right = state.Buttons.HasFlag(GamepadButtonFlags.DPadRight);

        }
    }

}
