using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.DataModel.View;
using Telerik.WebControls;

namespace GASystem.UserControls
{
	/// <summary>
	/// Displays a value as a MultiPartCheckList
	/// </summary>
	public class MultiPartCheckListTemplate : ITemplate
	{
		/*private Label lbl;//Table
		private RadioButton radio1;
		private RadioButton radio2;
		private RadioButton radio3;
		private string _columnName;
		private bool _editable;
		private bool _naOption;
		private int _partNum;*/
        Control _ctrl = null;
        private Control PrarentControl = null;

		public MultiPartCheckListTemplate(Control ctrl)
        {
            _ctrl = ctrl;
        }

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
            container.Controls.Add(_ctrl);
        }

		#endregion

        public static TableRow GetRow(string questionText,  string result,  bool _naOption)
        {
            TableRow row = new TableRow();

            TableCell tdLabel = new TableCell();
            tdLabel.Text = questionText;
            row.Cells.Add(tdLabel);

            TableCell tdRadios = new TableCell();

            RadioButtonList radios = new RadioButtonList();
            //radios.SelectedIndexChanged += Radios_SelectedIndexChanged;
            radios.RepeatDirection = RepeatDirection.Horizontal;
            radios.Items.Add(new ListItem("Yes","y"));
            radios.Items.Add(new ListItem("No", "n"));

            if (_naOption)
            {
                radios.Items.Add(new ListItem("N/A","a"));
            }
            ListItem select = radios.Items.FindByText(result);
            if (select != null)
                //radios.SelectedIndex = select.i;
                select.Selected = true;
            if (_naOption && string.IsNullOrEmpty(result))
                radios.Items.FindByText("N/A").Selected = true;
            radios.SelectedIndexChanged += Radios_SelectedIndexChanged1;
            tdRadios.Controls.Add(radios);

            row.Cells.Add(tdRadios);
            //table.Rows.Add(row);

            return row;
        }

        private static void Radios_SelectedIndexChanged1(object sender, EventArgs e)
        {
            RadioButtonList radio = sender as RadioButtonList;
            if (radio != null)
            {
                radio.Parent.ID = radio.SelectedItem.Text;
            }
        }
     

        public static RadPanelItem GetSeparatorItem(string message, bool border)
        {
            RadPanelItem separator = new RadPanelItem();
            separator.IsSeparator = true;

            if (border)
            {
                separator.ExpandedCssClass = "multicheckboder";
                separator.Expanded = true;
                Panel pnl = new Panel();
                pnl.CssClass = "multicheckboder";
                pnl.Controls.Add(new LiteralControl(message));

                RadPanelItem itemTemplate = new RadPanelItem();
                itemTemplate.IsSeparator = true;

                MultiPartCheckListTemplate mutlti = new MultiPartCheckListTemplate(pnl);
                mutlti.InstantiateIn(itemTemplate);
                itemTemplate.DataBind();

                separator.Items.Add(itemTemplate);
            }
            else
            {
                separator.Text = message;
            }

            return separator;
        }
    }

    public class MultiPartCheckListTableUtil
    {
        public static void AddPartTemplate(Table table, RadPanelItem itemTemplate, RadPanelItem part, ref RadPanelbar radPanel)
        {
            MultiPartCheckListTemplate mutlti = new MultiPartCheckListTemplate(table);

            mutlti.InstantiateIn(itemTemplate);
            itemTemplate.DataBind();
            radPanel.Items.Add(part);
        }

    }

    public class MultiPartCheckListScript
    {
        public static void RegisterScript(string tablePrefixName,string panelPrefixName, string columnName,
            string onClientLoadEventName, Type csType, ClientScriptManager cs)
        {
            StringBuilder SBScript = new StringBuilder();
            string panelBarVarName = "panelBar" + columnName;
            String csName = panelBarVarName+"Script"; 


            SBScript.AppendLine(
                @" var " + panelBarVarName + "; " +
                "function " + onClientLoadEventName + "(sender) " +
                "{" + panelBarVarName + " = sender; }");


            SBScript.AppendLine(
                @"
            $(document).ready(function() {
                Sys.Application.add_load(function() {
                    handlePart1();
                    handlePart2();
                });
            });

            function expand(element) {
                var part = panelBarMeetingOwner.FindItemByValue(element);

                part.Enable();
                setTimeout(function() {
                    part.Expand();
                });
            }

            function collapse(element) {
                var part = panelBarMeetingOwner.FindItemByValue(element);

                part.Collapse();
                part.Disable();
            }

            function handlePart1() {
                var inputsRdo = $(""[id$=_" + tablePrefixName + @"1]"").find(""input[type='radio']"");

                inputsRdo.change(function(a, b, c) {

                    panelBarMeetingOwner.RecordState();

                    var values = [];
                    var lastGroupName = '';

                    for (var i = 0; i < inputsRdo.length; i++) {
                        var rdoGpoName = $(inputsRdo[i]).attr('name');

                        if (lastGroupName !== rdoGpoName) {
                            lastGroupName = rdoGpoName;

                            var value = $(""input:radio[name ='"" + rdoGpoName + ""']:checked"").val();
                            
                            values.push(value);

                            if (value === 'y') {
                                expand('" + panelPrefixName + @"2'); 
                                panelBarMeetingOwner.PersistState();

                                return;
                            }
                        }
                    }

                    if (values.every(function(val) { return val === 'n' })) {
                        collapse('" + panelPrefixName + @"2');
                        expand('" + panelPrefixName + @"3');
                    } 
                    panelBarMeetingOwner.PersistState();
                });
            }


            function handlePart2() { 
                var inputsRdo = $(""[id$=_" + tablePrefixName + @"2]"").find(""input[type='radio']"");

                inputsRdo.change(function(a, b, c) {

                    panelBarMeetingOwner.RecordState();

                    var values = [];
                    var lastGroupName = '';

                    for (var i = 0; i < inputsRdo.length; i++) {
                        var rdoGpoName = $(inputsRdo[i]).attr('name');

                        if (lastGroupName !== rdoGpoName) {
                            lastGroupName = rdoGpoName;

                            var value = $(""input:radio[name ='"" + rdoGpoName + ""']:checked"").val();
                            
                            values.push(value);

                            if (value === ""n"") {
                                collapse('" + panelPrefixName + @"3');
                                panelBarMeetingOwner.PersistState();

                                return;
                            }
                        }
                    }

                    if (values.every(function(val) { return val === 'y' || val === 'a' })) {
                        expand('" + panelPrefixName + @"3');
                    }

                    panelBarMeetingOwner.PersistState();
                });
            }
"

            );



            if(!cs.IsClientScriptBlockRegistered(csType, csName))
                cs.RegisterClientScriptBlock(csType, csName, SBScript.ToString(),true);

        }
    }
}
