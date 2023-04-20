using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.AppUtils
{
    public interface IWorkflowStarterNextRun
    {
        string Filter
        {
            get;
            set;
        }

        string FilterDescription
        {
            get;
        }

        bool CanDisableFilter
        {
            get;
        }


        //GADataRecord Owner 
        //{
        //    get;
        //    set;
        //}

    }
}
