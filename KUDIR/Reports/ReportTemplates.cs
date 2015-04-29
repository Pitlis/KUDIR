using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports
{
    public interface IReportTemplates
    {
        byte[] GetTemplate(string name); 
    }
}
