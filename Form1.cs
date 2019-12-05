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
using System.IO;
using System.Runtime.Serialization;

namespace Xbox_Controller
{
    public partial class Form1 : Form
    {
        static private XInputController controller = new XInputController();
        private System.Timers.Timer timer = new System.Timers.Timer();
        private MovePointer pointer = new MovePointer();
        private Scroll verticalScroll = new Scroll();
        private Click click = new Click();
        private KeyRepeat[] arrowPad;

        private delegate void SetTextCallback(TextBox textBox, string text);
        private delegate void SetLabelCallback(Label label, string text);

        public Form1()
        {
            InitializeComponent();
            initializeGUI();

            arrowPad = InitializeArray<KeyRepeat>(4);
            arrowPad[0].key = VirtualKeyCode.UP;
            arrowPad[1].key = VirtualKeyCode.DOWN;
            arrowPad[2].key = VirtualKeyCode.LEFT;
            arrowPad[3].key = VirtualKeyCode.RIGHT;


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
            pointer.execute(ref controller.joystick[0]);
            verticalScroll.execute(ref controller.joystick[1]);
            click.execute(ref controller.button[0]);

            arrowPad[0].execute(ref controller.button[11]);
            arrowPad[1].execute(ref controller.button[10]);
            arrowPad[2].execute(ref controller.button[12]);
            arrowPad[3].execute(ref controller.button[13]);
            //updateGUI();
        }

        private void updateGUI()
        {
            for ( int i = 0; i < controller.button.Length; i++)
            {
                if ( controller.button[i].valueCurrent != controller.button[i].valuePast)
                {
                    SetLabel(controller.button[i].text, controller.button[i].valueCurrent.ToString());
                }
                
            }

            for (int i = 0; i < controller.joystick.Length; i++)
            {
                if (!controller.joystick[i].value.ToString().Equals(controller.joystick[i].text.Text))
                {
                    SetLabel(controller.joystick[i].text, controller.joystick[i].value.ToString());
                }

            }

            for (int i = 0; i < controller.trigger.Length; i++)
            {
                if (!controller.trigger[i].value.ToString().Equals(controller.trigger[i].text.Text))
                {
                    SetLabel(controller.trigger[i].text, controller.trigger[i].value.ToString());
                }                
            }
        }

        private void initializeGUI()
        {
            foreach (Button obj in controller.button)
            {
                this.Controls.Add(obj.label);
                this.Controls.Add(obj.text);
            }

            foreach (Joystick obj in controller.joystick)
            {
                this.Controls.Add(obj.label);
                this.Controls.Add(obj.text);
            }

            foreach (Trigger obj in controller.trigger)
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
