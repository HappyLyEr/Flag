using System;
using GASystem;
using GASystem.DataModel;
using GASystem.DataAccess;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
    /// <summary>
    /// Agent for writing text to console
    /// </summary>
    public class DebugAgent : AbstractAgent
    {
        public const string WORKFLOW = "GAWorkflow";
        public const string HEADING = "GAHeading";
        public const string TEXT = "GAText";

        public DebugAgent()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
        {
            string Message = "Debug: WF: " + utils.AttributeHelper.GetAttribute(wi, WORKFLOW) 
                + "Heading: " + utils.AttributeHelper.GetAttribute(wi, HEADING) 
                + " Text: " + utils.AttributeHelper.GetAttribute(wi, TEXT);
            System.Console.WriteLine(Message);
            return wi;
        }
    }
}

