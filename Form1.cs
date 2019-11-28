using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using WindowsInput;
using WindowsInput.Native;

namespace Xbox_Controller
{
    public partial class Form1 : Form
    {
        static private XInputController controller = new XInputController();
        private System.Timers.Timer timer = new System.Timers.Timer();
        private Mouse[] mouse;
        private Keyboard[] keyboard;
        private CAD[] cad;
        private InputSimulator simulator = new InputSimulator();

        private delegate void SetTextCallback(TextBox textBox, string text);
        private delegate void SetLabelCallback(Label label, string text);

        public Form1()
        {
            InitializeComponent();
            initializeGUI();
            mouse = InitializeArray<Mouse>(10);
            keyboard = InitializeArray<Keyboard>(10);
            cad = InitializeArray<CAD>(1);
            SetTimer();
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            this.timer = new System.Timers.Timer(10);
            // Hook up the Elapsed event for the timer. 
            this.timer.Elapsed += OnTimedEvent;
            this.timer.AutoReset = true;
            this.timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            controller.Update();
            mouse[0].moveRelative(controller.joystick[0]);

            cad[0].panCAD(controller.joystick[1]);

          /*  output[1].buttonScroll(controller.button[0], "down");            
            output[2].buttonScroll(controller.button[2], "up");
            output[3].buttonKeyScroll(controller.button[1], "up" , VirtualKeyCode.SHIFT);
            output[4].buttonKeyScroll(controller.button[3], "down", VirtualKeyCode.SHIFT);
            output[5].mousePress(simulator.Mouse.LeftButtonClick);*/

            keyboard[0].key = VirtualKeyCode.ESCAPE;
            keyboard[0].keySingle(controller.button[0]);

            keyboard[1].key = VirtualKeyCode.CONTROL;
            keyboard[1].keyToggle(controller.button[1]);
            updateGUI();
        }

        private void updateGUI()
        {
            foreach (XInputController.Button obj in controller.button)
            {
                SetText(obj.text, obj.valueCurrent.ToString());
            }

            foreach (XInputController.Joystick obj in controller.joystick)
            {
                SetText(obj.text, obj.value.ToString());
            }

            foreach (XInputController.Trigger obj in controller.trigger)
            {
                SetText(obj.text, obj.value.ToString());
            }
        }

        private void initializeGUI()
        {
            foreach (XInputController.Button obj in controller.button)
            {
                this.Controls.Add(obj.label);
                this.Controls.Add(obj.text);
            }

            foreach (XInputController.Joystick obj in controller.joystick)
            {
                this.Controls.Add(obj.label);
                this.Controls.Add(obj.text);
            }

            foreach (XInputController.Trigger obj in controller.trigger)
            {
                this.Controls.Add(obj.label);
                this.Controls.Add(obj.text);
            }
        }

        private void SetText(TextBox textBox, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (textBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { textBox, text });
            }
            else
            {
                textBox.Text = text;
            }
        }

        private void SetLabel(Label label, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label.InvokeRequired)
            {
                SetLabelCallback d = new SetLabelCallback(SetLabel);
                this.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
