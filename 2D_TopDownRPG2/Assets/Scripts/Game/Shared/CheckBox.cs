using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongTDev.EventManagers
{
    public sealed class CheckBox
    {
        private bool _checked;

        public CheckBox() 
        {
            _checked = false;
        }
        public void Check()
        {
            _checked = true;
        }

        public void Uncheck()
        {
            _checked = false;
        }

        public void ClearContext()
        {
            _checked = false;
        }

        public bool HasChecked() => _checked;
    }
}