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
        private string _pcType = "";
        private string _pcBrand = "";
        private string _ocVersion = "";

        public string PCBrand { get { return _pcBrand; } set { _pcBrand = value; } }
        public string OCVersion { get { return _ocVersion; } set { _ocVersion = value; } }

        public string CpuName 
        {
            get { return _cpuFullName; }
            set 
            { 
                if (value != "")
                {
                    _cpuName = value.Split("|")[0];
                    _cpuClassName = value.Split("|")[1];
                }
            }
        }

        public string PCType
        {
            get { return _pcType; }
            set { _pcType = value; }
        }

        public bool CheckCorrectValuesPage1(List<string> names)
        {
            if (_pcType == "" || names.Contains(_cpuFullName) || _ocVersion == "" || _pcBrand == "")
                return false;
            else
                return true;
        }
    }
}
