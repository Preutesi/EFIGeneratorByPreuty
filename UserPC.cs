using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EFIGeneratorByPreuty
{
    internal class UserPC
    {
        private string _cpuName = "";
        private string _cpuClassName = "";
        private string _cpuFullName = "";
        private string _cpuType = "";
        private string _pcType = "";

        public string CpuName 
        {
            get { return _cpuFullName; }
            set 
            { 
                if (value != "")
                {
                    _cpuName = value.Split(" | ")[0];
                    _cpuClassName = value.Split(" | ")[1];
                }
            }
        }

        public string CpuType
        {
            get { return _cpuType; }
            set { _cpuType = value; }
        }

        public string PCType
        {
            get { return _pcType; }
            set { _pcType = value; }
        }

        public bool CheckCorrectValuesPage1(List<string> names)
        {
            if (_cpuType == "" ||  _cpuType == "" || names.Contains(_cpuFullName))
                return false;
            else
                return true;
        }
    }
}
