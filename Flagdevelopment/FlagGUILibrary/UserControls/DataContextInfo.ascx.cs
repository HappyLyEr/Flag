namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.AppUtils;
	using GASystem.DataModel;
	using GASystem.BusinessLayer;
    using GASystem.GUIUtils;

	/// <summary>
	///		Summary description for DataClassOwner.
	/// </summary>
	public class DataContextInfo : System.Web.UI.UserControl
	{
		//protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.Label LabelCaption;
		protected System.Web.UI.WebControls.LinkButton ListAllLinkButton;
        protected System.Web.UI.WebControls.Label LabelSubCaptionFlagClass;
		protected System.Web.UI.WebControls.Label LabelSubCaption;
		protected System.Web.UI.WebControls.LinkButton ListAllWithinLinkButton;
		protected System.Web.UI.WebControls.Label LabelSubCaptionAllWithin;
		protected System.Web.UI.WebControls.LinkButton LinkSetContext;
		protected System.Web.UI.WebControls.HyperLink TopLevelLink;
		protected System.Web.UI.WebControls.HyperLink SecurityAdminLink;
		//protected System.Web.UI.WebControls.Label SecuirtyAdminLinkSeparator;
		protected System.Web.UI.WebControls.LinkButton ContextLinkButton;
        

		private void Page_Load(object sender, System.EventArgs e)
		{
			//if (!Page.IsPostBack)
				
				//	SetupContextInfo();
			LinkSetContext.Text = String.Format(Localization.GetGuiElementText("UseAsHome"));
		
		}

		private void SetupAdminLink() 
		{
            // Tor 20171115
            if (Security.IsGAAdministrator() || Security.IsAdministrators() || HttpContext.Current.User.Identity.Name == "host")
            //if (Security.IsGAAdministrator() || HttpContext.Current.User.Identity.Name == "host") 
			{
				string arcLinkAdminLink = "~/gagui/WebForms/EditArcLinkRolePermission.aspx?Owner={0}&Member={1}";
				string owner = OwnerDataRecord!=null ? OwnerDataRecord.DataClass.ToString() : "";
				string member = ContextDataRecord!=null ? ContextDataRecord.DataClass.ToString() : "";
				
				SecurityAdminLink.NavigateUrl = String.Format(arcLinkAdminLink, owner, member);
				SecurityAdminLink.Visible = owner.Length>0 && member.Length>0;
                SecurityAdminLink.Text = AppUtils.Localization.GetGuiElementText("SecuritySettings");
				//SecuirtyAdminLinkSeparator.Visible = LinkSetContext.Visible; //if setcontext link is visible, display separator bar. looks nicer
			} 
			else 
			{
				//SecurityAdminLink.Visible = SecuirtyAdminLinkSeparator.Visible = false;
                // Tor 20140708 try to fix error
                SecurityAdminLink.Visible = false;
			}
		}


		private string GetOwnerName()
		{
			if (ViewState["OwnerNameString"] != null)
				return (string)ViewState["OwnerNameString"];
			
			string ownerName = getNameForDataRecord(OwnerDataRecord);
//			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(OwnerDataRecord);
//			AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(OwnerDataRecord.DataClass);
//			
//			if (cd.ObjectName != string.Empty) 
//			{
//				if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(cd.ObjectName))
//					ownerName = ds.Tables[0].Rows[0]["Name"].ToString();
//				else
//					ownerName = ""; 
//			
//			} 
//			else 
//			{
//				ownerName = "";
//			}
//			//TODO: what should ownername be if it is not defined in gaclass?


			ViewState["OwnerNameString"] = ownerName;
			return ownerName;
		}

		private string getNameForDataRecord(GADataRecord DataRecord)
		{
			string ownerName;
			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(DataRecord);
			AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
			
			if (cd.ObjectName != string.Empty) 
			{
				if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(cd.ObjectName))
					ownerName = ds.Tables[0].Rows[0][cd.ObjectName].ToString();
				else
					ownerName = ""; 
			
			} 
			else 
			{
				ownerName = "";
			}
			return ownerName;
		}

		private void SetOwnerName(GADataRecord Owner)
		{
			OwnerName = getNameForDataRecord(Owner);


			//get and set top level
			//get path for owner
			string path = string.Empty;
			SuperClassDS superClassDS = BusinessLayer.DataClassRelations.GetByMember(Owner);
			if (superClassDS.GASuperClass.Rows.Count > 0 && !((SuperClassDS.GASuperClassRow)superClassDS.GASuperClass.Rows[0]).IsPathNull())
				path = ((SuperClassDS.GASuperClassRow)superClassDS.GASuperClass.Rows[0]).Path;
			
			if (path != string.Empty && path != "/")   //ignore if path is empty or we are already at a top node
			{
				//set name for top owner
				//remove first slash
				path = path.Substring(1);
				//remove first record if it is of type gaflag
				string firstRecord = path.Substring(0, path.IndexOf("/"));
				if (firstRecord.ToUpper().IndexOf(GADataClass.GAFlag.ToString().ToUpper()) > -1)
					path = path.Substring(path.IndexOf("/")+1);


			
				if (path != string.Empty && path != "/")   //ignore if path is empty or we are now at level 2
				{
					if (path.IndexOf("/") != -1)	//there must be an error if there are no more slashes, should be one at the end, 
					{								//silently ignore it, if this is the case
						path = path.Substring(0, path.IndexOf("/"));
						GADataRecord topLevel = GADataRecord.ParseGADataRecord(path);
						if (topLevel != null)
							TopLevelDataRecord = topLevel;
						TopLevelName = getNameForDataRecord(topLevel);
					}
				}
			}



//
//			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(Owner);
//			if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("name"))
//				OwnerName = ds.Tables[0].Rows[0]["Name"].ToString();
//			else
//				OwnerName = string.Empty; //TODO make sure all tables has a name column or implement a method for getting columns with purpose name;
		}

		private void SetContextName(GADataRecord ContextRecord)
		{
			ContextName = getNameForDataRecord(ContextRecord);

//			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(ContextRecord);
//			if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains("name"))
//				ContextName = ds.Tables[0].Rows[0]["Name"].ToString();
//			else
//				ContextName = string.Empty; //TODO make sure all tables has a name column or implement a method for getting columns with purpose name;
		}

		private string OwnerName 
		{
			set 
			{
				ViewState["OwnerNameString"] = value;
			}
			get 
			{
				return ViewState["OwnerNameString"] == null ? string.Empty : ViewState["OwnerNameString"].ToString();
			}
		}

		private string TopLevelName 
		{
			set 
			{
				ViewState["TopLevelName"] = value;
			}
			get 
			{
				return ViewState["TopLevelName"] == null ? string.Empty : ViewState["TopLevelName"].ToString();
			}
		}

		private GADataRecord TopLevelDataRecord 
		{
			set 
			{
				ViewState["TopLevelDataRecord"] = value;
			}
			get 
			{
				return ViewState["TopLevelDataRecord"] == null ? null : (GADataRecord)ViewState["TopLevelDataRecord"];
			}
		}

		private string ContextName 
		{
			set 
			{
				ViewState["ContextNameString"] = value;
			}
			get 
			{
				return ViewState["ContextNameString"] == null ? string.Empty : ViewState["ContextNameString"].ToString();
			}
		}


		

		public void SetupContextInfo()
		{
		
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			
			LabelSubCaptionAllWithin.Visible = false;
			ListAllWithinLinkButton.Visible = false;
			LinkSetContext.Visible = false;
			//setup context
			
            // Tor 20140711 Handle OwnerDataRecord GAFlag in the same way as OwnerDataRecord==null
			//if (null!=OwnerDataRecord)
            if (null != OwnerDataRecord && OwnerDataRecord.DataClass.ToString()!="GAFlag")
                {
				//ContextLinkButton.Text = Localization.GetGuiElementText("Goto") + " " + Localization.GetGuiElementText(OwnerDataRecord.DataClass.ToString());
				ContextLinkButton.Text = Localization.GetGuiElementText(OwnerDataRecord.DataClass.ToString()) + ": " + OwnerName;
				//LabelCaption.Text =  Localization.GetGuiElementText(OwnerDataRecord.DataClass.ToString()) + ": " + GetOwnerName();
				LabelCaption.Text = "";
			} 
			else
			{
				ContextLinkButton.Text = string.Empty;
				LabelCaption.Text = string.Empty;
			}

			if (null != TopLevelDataRecord && TopLevelName != string.Empty) 
			{
                TopLevelLink.Text = TopLevelName;
				TopLevelLink.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(TopLevelDataRecord.DataClass.ToString(), TopLevelDataRecord.RowId.ToString());
			} 
			else 
			{
				TopLevelLink.Visible = false;
			}

			//setup subcontext
			if (CurrentIsSingleRecord)
			{
                if (NewRecord)
                {
                    //LabelSubCaption.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(CurrentDataClass.ToString()));
                    LabelSubCaptionFlagClass.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(CurrentDataClass.ToString()));
                    LabelSubCaptionFlagClass.Text = LabelSubCaptionFlagClass.Text.ToUpper();
                    LabelSubCaption.Text = string.Empty;
                }
                else
                {
                    LabelSubCaption.Text = ContextName;
                    LabelSubCaptionFlagClass.Text = Localization.GetGuiElementText(CurrentDataClass.ToString()) + ": ";
                    LabelSubCaptionFlagClass.Text = LabelSubCaptionFlagClass.Text.ToUpper();

                }
				
				
				
				if (null!=OwnerDataRecord) 
				{
					ListAllLinkButton.Text = String.Format(Localization.GetGuiElementText("ListAllIn"), Localization.GetGuiElementTextPlural(CurrentDataClass.ToString()), Localization.GetGuiElementText(OwnerDataRecord.DataClass.ToString()));
				} 
				else
				{
					ListAllLinkButton.Text = String.Format(Localization.GetGuiElementText("ListAll"), Localization.GetGuiElementTextPlural(CurrentDataClass.ToString()));
				}
				ListAllLinkButton.Visible = true;
                
                // Tor 20140708 Set LinkSetContex.Visible=true when specified in GAClass, else false
                LinkSetContext.Visible = false;       //TODO control this by checking display status in gaclass
                if (CurrentIsSingleRecord && !(CurrentDataClass.ToString() == string.Empty) )
                {
                    LinkSetContext.Visible = GASystem.BusinessLayer.Class.GetClassIsUseAsHomeClass(CurrentDataClass);
                }
			}
			else //Plural form in gui text
			{
				if (!ViewAllRecordsWithin) 
				{
					//LabelSubCaption.Text = Localization.GetGuiElementTextPlural(CurrentDataClass.ToString());
                    LabelSubCaptionFlagClass.Text = Localization.GetGuiElementTextPlural(CurrentDataClass.ToString()).ToUpper();
                    // Tor 20140711 Handle class = GAFlag and class==null in the same way
                    // if (ContextDataRecord != null)
                        
                    if (ContextDataRecord != null && ContextDataRecord.DataClass.ToString() != "GAFlag")
					{
                        ContextLinkButton.Text = Localization.GetGuiElementText(ContextDataRecord.DataClass.ToString()) + ": " + ContextName;
                        ContextLinkButton.Visible = true;
					}
					ListAllLinkButton.Visible = false;
                    LabelSubCaption.Visible = false;
				} 
				else
				{
					LabelSubCaptionAllWithin.Text = String.Format(Localization.GetGuiElementText("AllRecordsWithinOwner"), 
						new object[] {Localization.GetGuiElementTextPlural(CurrentDataClass.ToString()),
										 Localization.GetGuiElementText(ContextDataRecord.DataClass.ToString()), ContextName});
					//ListAllWithinLinkButton.Text = string.Format(Localization.GetGuiElementText("ViewRecordsForDifferenContext"), 
					//	Localization.GetGuiElementTextPlural(CurrentDataClass.ToString()));
					ListAllLinkButton.Visible = false;
					ListAllWithinLinkButton.Visible = false;
					LabelSubCaption.Visible = false;
                    LabelSubCaptionFlagClass.Visible = false;
					LabelSubCaptionAllWithin.Visible = true;
 

				}
			}
			
			SetupAdminLink();
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ContextLinkButton.Click += new System.EventHandler(this.ContextLinkButton_Click);
			this.ListAllLinkButton.Click += new System.EventHandler(this.ListAllLinkButton_Click);
			this.LinkSetContext.Click += new System.EventHandler(this.LinkSetContext_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ContextLinkButton_Click(object sender, System.EventArgs e)
		{
			GADataRecord dataRecord ;
			if (CurrentIsSingleRecord) 
				dataRecord = OwnerDataRecord;
			else
				dataRecord = ContextDataRecord;


			GADataRecord OwnerOfOwner = BusinessLayer.DataClassRelations.GetOwner(dataRecord);
			
			//TODO quick fix, how do we handle records that does not have a owner
			//if (null == OwnerOfOwner) 
			//	OwnerOfOwner = OwnerDataRecord; 
			PageDispatcher.GotoDataRecordViewPage(Response, dataRecord.DataClass, dataRecord.RowId, OwnerOfOwner, this.CurrentDataClass);
		}

		private void ListAllLinkButton_Click(object sender, System.EventArgs e)
		{
			PageDispatcher.GotoDataRecordListPage(Response, CurrentDataClass, OwnerDataRecord);
		}

		private void LinkSetContext_Click(object sender, System.EventArgs e)
		{
			SessionManagement.SetCurrentInitialContext(ContextDataRecord);
            // Tor 20151020 Update left pane shortcut links when home context changes
            GASystem.GUIUtils.ShortcutLinks.getAllLinks();
            
			//SessionManagementGetCurrentDataContext();
		}

		private void SecurityAdmin_Click(object sender, System.EventArgs e)
		{
		
		}

		public GADataClass CurrentDataClass
		{
			get
			{
				return (GADataClass) ViewState["CurrentDataClass"+this.ID];
			}
			set
			{
				ViewState["CurrentDataClass"+this.ID] = value;
			}
		}


		public GADataRecord OwnerDataRecord
		{
			get
			{
				return (GADataRecord) ViewState["OwnerDataRecord"+this.ID];
			}
			set
			{
				ViewState["OwnerDataRecord"+this.ID] = value;
				if (value != null)
					SetOwnerName(value);
			}
		}

		public GADataRecord ContextDataRecord
		{
			get
			{
				return (GADataRecord) ViewState["ContextDataRecord"+this.ID];
			}
			set
			{
				ViewState["ContextDataRecord"+this.ID] = value;
				if (value != null) 
				{
					OwnerDataRecord = GASystem.BusinessLayer.DataClassRelations.GetOwner(value);
					SetContextName(value);
					
				
				}
			}
		}
		

		public bool CurrentIsSingleRecord
		{
			get
			{
				return null == ViewState["CurrentIsSingleRecord"+this.ID] ? false : bool.Parse(ViewState["CurrentIsSingleRecord"+this.ID].ToString());
			}
			set
			{
				ViewState["CurrentIsSingleRecord"+this.ID] = value;
			}
		}

		public bool ViewAllRecordsWithin 
		{
			get
			{
				return null == ViewState["viewallwithin"+this.ID] ? false : bool.Parse(ViewState["viewallwithin"+this.ID].ToString());
			}
			set
			{
				ViewState["viewallwithin"+this.ID] = value;
			}
		}

		public bool NewRecord 
		{
			get
			{
				return null == ViewState["NewRecord"+this.ID] ? false : bool.Parse(ViewState["NewRecord"+this.ID].ToString());
			}
			set
			{
				ViewState["NewRecord"+this.ID] = value;
			}
		}

		/*public GADataClass CurrentDataClass
		{
			get
			{
				return (GADataClass) ViewState["CurrentDataClass"+this.ID];
			}
			set
			{
				ViewState["CurrentDataClass"+this.ID] = value;
			}
		}

		*/
	}
}
