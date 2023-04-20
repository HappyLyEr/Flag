using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.GAGUI.GAGUIEvents
{
    public class GAEventArgs<T> : EventArgs
    {
        public GAEventArgs(T value)
        {
            m_value = value;
        }

        private T m_value;

        public T Value
        {
            get { return m_value; }
        }
    }
}
