using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongTDev.EventManagers
{
    public sealed class CheckBox : ObjectHolder<bool>
    {
        public CheckBox() 
        {
            value = false;
        }
        public void Check()
        {
            value = true;
        }

        public void Uncheck()
        {
            value = false;
        }

        public void ClearContext()
        {
            value = false;
        }

        public bool HasChecked() => value;
    }
}