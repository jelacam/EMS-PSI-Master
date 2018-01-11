using System.Collections.Generic;

namespace CIM.Model
{
    public class CPackage
    {
        public string name;
        public List<CClass> classes = new List<CClass>();
    }
}
