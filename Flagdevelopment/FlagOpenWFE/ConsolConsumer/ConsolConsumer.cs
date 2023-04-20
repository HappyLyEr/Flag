using System;
using openwfe.workitem;
using log4net;
using log4net.Config;
using System.Collections.Generic;

namespace GASystem.DotNetApre
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class ConsolConsumer : impl.SimpleConsumer
	{
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ConsolConsumer));
        
        /// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //
            // TODO: Add code to start application here
            //
            bool isOk = true;
            bool isStartWorkflowOK = true;
            bool isWorkitemHeaderOK = true;
            bool isProceedPendingWorkitemsOK = true;
            log4net.Config.XmlConfigurator.Configure();  // BasicConfigurator.Configure();

            ConsolConsumer consumer = new ConsolConsumer();
            String storeName = "Store.ga";

            DateTime start = DateTime.Now;

            _logger.Info("starting consolconsumer");

            System.Console.WriteLine("--starting--");
            System.Console.WriteLine("Executing agent assigned workitems ");
            // Tor 20181115 added do loop to do next step in workflows while headers.count > 0
            int myHeadersCount = 1;
            int foundStartPending = 0;
            do
            {
                try
                {

                    openwfe.rest.worklist.WorkSession ws = new openwfe.rest.worklist.WorkSession(WorkSessionServerAddress, WorkSessionServerPort, OWFEUserName, OWFEPassword);
                    System.Console.WriteLine("Session id >" + ws.GetSessionId() + "<");

                    openwfe.workitem.FlowExpressionId itemId = null;

                    System.Collections.IList headers = ws.GetHeaders(storeName);
                    System.Console.WriteLine("headers.count : " + headers.Count);
                    // Tor 20181115
                    myHeadersCount = headers.Count;
                    foreach (openwfe.workitem.Header h in headers)
                    {
                        itemId = h.flowExpressionId;
                        openwfe.workitem.InFlowWorkitem wi = ws.GetAndLockWorkitem(storeName, itemId);
                        if (wi != null)
                        {
                            System.Console.WriteLine("wi.subject = " + wi.attributes[new StringAttribute("__subject__")]);
                            consumer.UseAgent(wi);

                            try
                            {
                                ws.SaveWorkitem(storeName, wi);
                                ws.ForwardWorkitem(storeName, wi);
                            }
                            catch (Exception ex)
                            {
                                // Tor 20200224
                                System.Console.WriteLine("Error saving and forwarding workitem with storeName: "+storeName+" workflowInstanceId: "
                                    +itemId.workflowInstanceId+" workflowDefinitionUrl: "+itemId.workflowDefinitionUrl
                                    +" workflowDefinitionName: "+itemId.workflowDefinitionName
                                    +" expressionName: "+itemId.expressionName
                                    +" expressionId: "+itemId.expressionId+" Message: " + ex.Message);
                                _logger.Info("Error saving and forwarding workitem with storeName: " + storeName + " workflowInstanceId: "
                                    + itemId.workflowInstanceId + " workflowDefinitionUrl: " + itemId.workflowDefinitionUrl
                                    + " workflowDefinitionName: " + itemId.workflowDefinitionName
                                    + " expressionName: " + itemId.expressionName
                                    + " expressionId: " + itemId.expressionId + " Message: " + ex.Message);
                                // Tor 20190922 added value itemId  (h.flowExpressionId)
                                //System.Console.WriteLine("Error saving and forwarding workitem with flowExpressionId "+itemId+" Message: " + ex.Message);
                                //_logger.Info("Error saving and forwarding workitem with flowExpressionId " + itemId + " Message: " + ex.Message);

                                isWorkitemHeaderOK = false;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("did not find the workitem");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Error Executing agent assigned workitems");
                    System.Console.WriteLine(ex.Message);
                    // Tor 20180411 error opening openwfe - terminate consolconsumer
                    System.Console.WriteLine("ConsolConsumer terminates");
                    isOk = false;
                }
            //    // Tor 20181115 end do loop
            //} while (myHeadersCount > 0 && isOk);

                // Tor 20180411 Proceed if opening openwfe OK
                if (isOk)
                {
                    // Tor 20190520 test to avoid loop if failed in previous instance of the do loop
                    if (isProceedPendingWorkitemsOK)
                    {
                        //proceed workitems
                        System.Console.WriteLine("Proceed pending workitems");
                        try
                        {
                            GASystem.BusinessLayer.Workitem.ProceedPendingWorkitems(getflowExpressions());
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Error proceeding workitems");
                            System.Console.WriteLine(ex.Message);
                            isProceedPendingWorkitemsOK = false; // Tor 20190520 set false to avoid loop if failed in previous instance of the do loop
                        }
                    }
                    //start workflows
                    // Tor 20190520 test to avoid loop if failed in previous instance of the do loop
                    if (isStartWorkflowOK)
                    {
                        System.Console.WriteLine("Start pending workflows");
                        try
                        {
                            // Tor 20181118 return number of workflows started
                            //GAWorkflow.OWFEWorkFlowEngine.startPendingWorkflows();
                            foundStartPending = GAWorkflow.OWFEWorkFlowEngine.startPendingWorkflows();
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Error starting workflows");
                            System.Console.WriteLine(ex.Message);
                            isStartWorkflowOK = false; // Tor 20190520 set false to avoid loop if failed in previous instance of the do loop
                        }
                    }

                    //check for new workitems
                    System.Console.WriteLine("Check for new workitems");
                    try
                    {
                        GASystem.BusinessLayer.Workitem.LoadWorkitems(getflowExpressions());
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Error getting workitems");
                        System.Console.WriteLine(ex.Message);
                    }
                    System.Console.WriteLine("-- Loop Finished --");
                }
            // Tor 20181115 end do loop
            } while ((myHeadersCount>0 || foundStartPending>0) && isOk && isWorkitemHeaderOK);
            System.Console.WriteLine("-- Finished --");
        }

        //test get flowexpressionsid directly from disk
        private static List<openwfe.workitem.FlowExpressionId> getflowExpressions()
        {
            string[] xmlfiles = System.IO.Directory.GetFiles(OWFEGAUserStorePath, "*.xml");
            List<openwfe.workitem.FlowExpressionId> flowExpressions = new List<openwfe.workitem.FlowExpressionId>();

            foreach (string xmlfileorg in xmlfiles)
            {
                string xmlfile = xmlfileorg.Replace(".xml", "");
                xmlfile = xmlfile.Replace(OWFEGAUserStorePath, "");
                xmlfile = xmlfile.Replace("--", "/");  //replacing with slash because of split not handling double characters
                string[] fileParts = xmlfile.Split('/');
                if (fileParts.Length == 5)
                {
                    openwfe.workitem.FlowExpressionId flowEpxression = new openwfe.workitem.FlowExpressionId();
                    flowEpxression.engineId = fileParts[0];
                    flowEpxression.expressionName = "participant";
                    flowEpxression.expressionId = fileParts[4];
                    flowEpxression.initialEngineId = fileParts[0];
                    flowEpxression.workflowDefinitionName = fileParts[1];
                    flowEpxression.workflowDefinitionRevision = fileParts[2];
                    flowEpxression.workflowDefinitionUrl = "http://localhost:7079/" + fileParts[1] + ".xml";
                    flowEpxression.workflowInstanceId = fileParts[3];
                    flowExpressions.Add(flowEpxression);

                }
                else
                {
                    //something wrong, how to handle?? LOG!!
                }
            }


            return flowExpressions;
        }

        public static string OWFEGAUserStorePath
        {
            get
            {
                //return "Store.gauser" has to be stored in .config file because path might be different on different servers ; 
                return System.Configuration.ConfigurationManager.AppSettings.Get("OWFEGAUserStorePath");
            }
        }
	}
}
