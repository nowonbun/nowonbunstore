using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowExample
{
    public class ControlFactory
    {
        private static ControlFactory singleton = null;
        private Dictionary<Type, Form> flyweigth_form;
        private Dictionary<Type, Control> flyweight_control;

        private static ControlFactory Instance()
        {
            if (singleton == null)
            {
                singleton = new ControlFactory();
            }
            return singleton;
        }

        private ControlFactory()
        {
            flyweigth_form = new Dictionary<Type, Form>();
            flyweight_control = new Dictionary<Type, Control>();
        }

        public static void SetForm(Form form)
        {
            if (!Instance().flyweigth_form.ContainsKey(form.GetType()))
            {
                Instance().flyweigth_form.Add(form.GetType(), form);
            }
            else
            {
                Instance().flyweigth_form[form.GetType()] = form;
            }
        }

        public static void SetControl(Control control)
        {
            if (!Instance().flyweight_control.ContainsKey(control.GetType()))
            {
                Instance().flyweight_control.Add(control.GetType(), control);
            }
            else
            {
                Instance().flyweight_control[control.GetType()] = control;
            }
        }

        public static T GetForm<T>() where T : Form
        {
            Type type = typeof(T);
            if (!Instance().flyweigth_form.ContainsKey(type))
            {
                return null;
            }
            return Instance().flyweigth_form[type] as T;
        }

        public static T GetControl<T>() where T : Control
        {
            Type type = typeof(T);
            if (!Instance().flyweight_control.ContainsKey(type))
            {
                return null;
            }
            return Instance().flyweight_control[type] as T;
        }
    }
}
