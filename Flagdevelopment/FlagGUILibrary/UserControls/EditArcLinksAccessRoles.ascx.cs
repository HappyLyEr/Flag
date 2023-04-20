using System.Collections;
using System.Text;
using GASystem;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using GASystem.GAExceptions;
using GASystem.DataModel;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace GASystem.UserControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for EditUserRoles.
	/// </summary>
	public class EditArcLinksAccessRoles : System.Web.UI.UserControl
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(EditUserRoles));


		protected System.Web.UI.WebControls.DropDownList ddlRoles;
		protected System.Web.UI.WebControls.DataGrid grdDataRecordRolePermissions;
		

		protected  System.Web.UI.WebControls.DropDownList dropdownRoles = new DropDownList();
		protected System.Web.UI.WebControls.PlaceHolder UserMessagePlaceHolder;
		protected System.Web.UI.WebControls.PlaceHolder TestOutput;
		protected System.Web.UI.WebControls.CheckBox SaveCascadingCheckBox;
		

		private ArrayList _roles = new ArrayList();

		private void Page_Load(object sender, System.EventArgs e)
		{
			UserMessagePlaceHolder.Controls.Clear();
			if (!Page.IsPostBack) 
			{
				SaveCascadingCheckBox.Text = Localization.GetGuiElementText("SaveCascadingSecurity");
			}

		
		}



		private void testSqlGenerate()
		{
			string sql = @"SELECT * FROM dbo.GASuperClass WHERE dbo.GASuperClass.MemberClass = N'{0}'\n AND ({1})\n AND ({2}) ";
			
			string[] engagements = new string[] {"GAProject-1", "GAProject-3"};
			string[] userRoles = new string[] {"11","1"};

			StringBuilder pathConstraints = new StringBuilder();
			foreach (string engagement in engagements)
			{
				pathConstraints.Append("(Path + '/' + MemberClass + '-' + CAST(MemberClassRowId as nvarChar(20)))");
				pathConstraints.Append(" LIKE '%").Append(engagement).Append("%' \n");
				pathConstraints.Append(" OR ");
			}
			pathConstraints.Remove(pathConstraints.Length-5,4); //remove last OR statement

			StringBuilder roleConstraints = new StringBuilder();
			foreach (string roleId in userRoles)
			{
				roleConstraints.Append("';'+dbo.GASuperClass.ReadRoles+';' LIKE '%").Append(roleId).Append("%' \n");
				roleConstraints.Append(" OR ");
			}
			roleConstraints.Remove(roleConstraints.Length-5,4); //remove last OR statement

			sql = string.Format(sql, "GAProject", pathConstraints.ToString(), roleConstraints.ToString());
			_logger.Debug(sql);
			
		}


		private void DisplayUserMessage(UserMessage.UserMessageType MessageType, string Message)
		{
			try
			{
				UserMessage userMessageControl = (UserMessage) LoadControl("../UserControls/UserMessage.ascx");
				userMessageControl.MessageType = MessageType;
				userMessageControl.MessageText = Message;
				UserMessagePlaceHolder.Controls.Add(userMessageControl);
			}
			catch (Exception e)
			{
				_logger.Error(Localization.GetExceptionMessage("ErrorDisplayUserMessage"), e);
			}
		}

		/// <summary>
		/// Public method for refreshinbg grid. first set DataClass property, then cakk RefreshControl
		/// </summary>
		public void RefreshControl() 
		{
			SetupGrid();
		}

		private void SetupGrid()
		{
			try
			{
				RolesDS rolesDataSet = Security.GetAllSecurityRoles();
				foreach (RolesDS.RolesRow row in rolesDataSet.Roles)
				{
					RolesValueObject vo = new RolesValueObject();
					vo.roleId = row.RoleID;
//					vo.roleName = row.RoleName;
                    vo.roleName = row.Description; // Tor 20130914 Display Role Description (GAListDescription) instead of Role Value (GAListValue)
					_roles.Add(vo);
				}

				DataRecordRolePermissionsDS rolePermissionsSet = Security.GetSecurityRolesAccessForArcLink(Owner, Member); //DataClass(DataClass);
				grdDataRecordRolePermissions.DataSource = rolePermissionsSet;
				grdDataRecordRolePermissions.DataKeyField = "RoleId";
				grdDataRecordRolePermissions.DataBind();
			}
			catch (GAException gaEx)
			{
				_logger.Error(gaEx.Message + gaEx.DebugMessage, gaEx);
				DisplayUserMessage(UserMessage.UserMessageType.Error, gaEx.Message);	
			}
			catch (Exception ex)
			{
				_logger.Error(ex.Message, ex);
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}
		}

		


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
		
			XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/bin/Log4NetConfig.xml")));

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
			this.grdDataRecordRolePermissions.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdDataRecordRolePermissions_ItemCommand);
			this.grdDataRecordRolePermissions.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdDataRecordRolePermissions_CancelCommand);
			this.grdDataRecordRolePermissions.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdDataRecordRolePermissions_EditCommand);
			this.grdDataRecordRolePermissions.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdDataRecordRolePermissions_UpdateCommand);
			this.grdDataRecordRolePermissions.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.grdDataRecordRolePermissions_DeleteCommand);
			this.grdDataRecordRolePermissions.ItemDataBound += new System.Web.UI.WebControls.DataGridItemEventHandler(this.grdDataRecordRolePermissions_ItemDataBound);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void grdDataRecordRolePermissions_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			
			try 
			{
				foreach (TableCell cell in e.Item.Cells)
				{
				
					//Localize header and footer, and last column (command buttons)
					if (e.Item.ItemType == ListItemType.Header || e.Item.ItemType == ListItemType.Footer || e.Item.Cells.GetCellIndex(cell)== e.Item.Cells.Count-1)
						Localization.LocalizeControls(cell.Controls);	
				}
			
				if (e.Item.ItemType == ListItemType.Footer)
				{
					DropDownList rolesDropDown = e.Item.FindControl("ddlRoles") as DropDownList;
					if (rolesDropDown!=null)
					{
						rolesDropDown.Items.Add(new ListItem(AppUtils.Localization.GetGuiElementText("All"), "-1"));
						rolesDropDown.Items.Add(new ListItem(AppUtils.Localization.GetGuiElementText("Company Users"), "-2"));
						foreach (RolesValueObject vo in _roles)
						{
							ListItem item = new ListItem(vo.roleName, vo.roleId.ToString());
							rolesDropDown.Items.Add(item);
							
						}
					
					}
				}
			}
			catch (GAException GAex)
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, GAex.Message);		
			}
			catch (Exception ex)
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}
			
			
			//Localization.LocalizeControls(e.Item.Cells[2].Controls);
			//Localization.LocalizeControls(e.Item.Cells[3].Controls);
		}
	
	

		private void grdDataRecordRolePermissions_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			if (e.CommandName == "Insert")
			{
				try 
				{
					DropDownList rolesDropDown = e.Item.FindControl("ddlRoles") as DropDownList;
					if (rolesDropDown!=null && null!=rolesDropDown.SelectedItem)
					{
						bool hasRead = false;
						bool hasUpdate = false;
						bool hasCreate = false;
						bool hasDelete = false;

						ListItem ddlItem  = rolesDropDown.SelectedItem;
						
						CheckBox tmpCheck = e.Item.FindControl("CheckReadInsert") as CheckBox ;
						if (null!=tmpCheck) 
							hasRead = tmpCheck.Checked;

						tmpCheck = e.Item.FindControl("CheckUpdateInsert") as CheckBox ;
						if (null!=tmpCheck) 
							hasUpdate = tmpCheck.Checked;
			
						tmpCheck = e.Item.FindControl("CheckCreateInsert") as CheckBox ;
						if (null!=tmpCheck) 
							hasCreate = tmpCheck.Checked;

						tmpCheck = e.Item.FindControl("CheckDeleteInsert") as CheckBox ;
						if (null!=tmpCheck) 
							hasDelete = tmpCheck.Checked;
							
						Security.AddArcLinkRolePermissions(Owner, Member, ddlItem.Value, hasRead, hasUpdate, hasCreate, hasDelete);
						if (SaveCascadingCheckBox.Checked) 
						{
							_arcLinksUpdated = new ArrayList();
							AddArcLinkRoleCascading(Member, ddlItem.Value, hasRead, hasUpdate, hasCreate, hasDelete);

						}
					}
					SetupGrid();
				}
				catch (GAException gaEx) 
				{
					DisplayUserMessage(UserMessage.UserMessageType.Error, gaEx.Message);
				}
				catch (Exception ex) 
				{
					_logger.Error(Localization.GetExceptionMessage("UnhandledError"), ex);
					DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
				}
			}
			
		}

		//keep track of which arcLinks that have been updated during a cascading update. Helps stop the recursion
		//from going down the same path twice
		private ArrayList _arcLinksUpdated = new ArrayList();

		//ArrayList _arcLinksUpdated must be an empty arraylist when calling this recursive method first time!
		private void AddArcLinkRoleCascading(GADataClass OwnerClass, string roleId, bool hasRead, bool hasUpdate, bool hasCreate, bool hasDelete) 
		{
			ArrayList childClasses = DataClassRelations.GetNextLevelDataClasses(OwnerClass);
			if (childClasses.Count==0)
				return;

			foreach (string childClassName in childClasses) 
			{
				GADataClass childClass = GADataRecord.ParseGADataClass(childClassName);
				
				//string that uniquely identified the arclink. Keep track of visited arclinks and don't revisit
				string ownerChildRelationKey = OwnerClass.ToString()+childClassName;
				if (_arcLinksUpdated.Contains(ownerChildRelationKey))
					continue;

				Security.AddArcLinkRolePermissions(OwnerClass, childClass, roleId, hasRead, hasUpdate, hasCreate, hasDelete);
				_arcLinksUpdated.Add(ownerChildRelationKey);
				AddArcLinkRoleCascading(childClass, roleId, hasRead, hasUpdate, hasCreate, hasDelete);
			}
		}

		//ArrayList _arcLinksUpdated must be an empty arraylist when calling this recursive method first time!
		private void UpdateArcLinkRoleCascading(DataRecordRolePermissionsDS rolePermissionsData, GADataClass OwnerClass) 
		{
			ArrayList childClasses = DataClassRelations.GetNextLevelDataClasses(OwnerClass);
			if (childClasses.Count==0)
				return;

			foreach (string childClassName in childClasses) 
			{
				GADataClass childClass = GADataRecord.ParseGADataClass(childClassName);
				
				//string that uniquely identified the arclink. Keep track of visited arclinks and don't revisit
				string ownerChildRelationKey = OwnerClass.ToString()+childClassName;
				if (_arcLinksUpdated.Contains(ownerChildRelationKey))
					continue;

				Security.UpdateArcLinkRolePermissions(rolePermissionsData, OwnerClass, childClass);
				_arcLinksUpdated.Add(ownerChildRelationKey);
				UpdateArcLinkRoleCascading(rolePermissionsData, childClass);
			}
		}

		//ArrayList _arcLinksUpdated must be an empty arraylist when calling this recursive method first time!
		private void UpdateArcLinkRoleCascading(GADataClass OwnerClass, string roleId, bool hasRead, bool hasUpdate, bool hasCreate, bool hasDelete) 
		{
			ArrayList childClasses = DataClassRelations.GetNextLevelDataClasses(OwnerClass);
			if (childClasses.Count==0)
				return;

			foreach (string childClassName in childClasses) 
			{
				GADataClass childClass = GADataRecord.ParseGADataClass(childClassName);
				
				//string that uniquely identified the arclink. Keep track of visited arclinks and don't revisit
				string ownerChildRelationKey = OwnerClass.ToString()+childClassName;
				if (_arcLinksUpdated.Contains(ownerChildRelationKey))
					continue;

				Security.UpdateArcLinkRolePermissions(OwnerClass, childClass, roleId, hasRead, hasUpdate, hasCreate, hasDelete);
				_arcLinksUpdated.Add(ownerChildRelationKey);
				UpdateArcLinkRoleCascading(childClass, roleId, hasRead, hasUpdate, hasCreate, hasDelete);
			}
		}



		private void grdDataRecordRolePermissions_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			String key = grdDataRecordRolePermissions.DataKeys[e.Item.ItemIndex].ToString();
			
			try
			{
				DataRecordRolePermissionsDS rolePermissionsData = Security.GetSecurityRolesAccessForArcLink(Owner, Member);
				DataRecordRolePermissionsDS.DataRecordRolePermissionsRow row = (DataRecordRolePermissionsDS.DataRecordRolePermissionsRow) rolePermissionsData.DataRecordRolePermissions.Rows.Find(key);
				rolePermissionsData.DataRecordRolePermissions.RemoveDataRecordRolePermissionsRow(row);
				Security.UpdateArcLinkRolePermissions(rolePermissionsData, Owner, Member);
				if (SaveCascadingCheckBox.Checked) 
				{
					_arcLinksUpdated = new ArrayList();
					UpdateArcLinkRoleCascading(rolePermissionsData, Member);
				}
				SetupGrid();
			}
			catch (GAException GAex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, GAex.Message);
			}
			catch (Exception ex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}
		
		}

		private void grdDataRecordRolePermissions_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			
			try
			{
				String key = grdDataRecordRolePermissions.DataKeys[e.Item.ItemIndex].ToString();
				//DataRecordRoleAccessDS roleAccessData = Security.GetSecurityRolesAccessForDataRecord(_dataRecord);
				//DataRecordRoleAccessDS.DataRecordRoleAccessRow row = (DataRecordRoleAccessDS.DataRecordRoleAccessRow) roleAccessData.DataRecordRoleAccess.Rows.Find(key);
				
				bool hasRead = false;
				bool hasUpdate = false;
				bool hasCreate = false;
				bool hasDelete = false;


				CheckBox tmpCheck = e.Item.FindControl("CheckReadEdit") as CheckBox ;
				if (null!=tmpCheck) 
					hasRead = tmpCheck.Checked;

				tmpCheck = e.Item.FindControl("CheckUpdateEdit") as CheckBox ;
				if (null!=tmpCheck) 
					hasUpdate = tmpCheck.Checked;
				
				tmpCheck = e.Item.FindControl("CheckCreateEdit") as CheckBox ;
				if (null!=tmpCheck) 
					hasCreate = tmpCheck.Checked;

				tmpCheck = e.Item.FindControl("CheckDeleteEdit") as CheckBox ;
				if (null!=tmpCheck) 
					hasDelete = tmpCheck.Checked;


				Security.UpdateArcLinkRolePermissions(Owner, Member, key, hasRead, hasUpdate, hasCreate, hasDelete);
				if (SaveCascadingCheckBox.Checked) 
				{
					_arcLinksUpdated = new ArrayList();
					UpdateArcLinkRoleCascading(Member, key, hasRead, hasUpdate, hasCreate, hasDelete);
				}
				grdDataRecordRolePermissions.EditItemIndex = -1;	
				SetupGrid();
						
			}
			catch (GAException GAex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, GAex.Message);
			}
			catch (Exception ex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}
		
		}

		private void grdDataRecordRolePermissions_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			try
			{
				grdDataRecordRolePermissions.EditItemIndex = e.Item.ItemIndex;
				SetupGrid();
			}
			catch (GAException GAex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, GAex.Message);
			}
			catch (Exception ex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}

		}

		private void grdDataRecordRolePermissions_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			try
			{
				grdDataRecordRolePermissions.EditItemIndex = -1;	
				SetupGrid();
			}
			catch (GAException GAex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, GAex.Message);
			}
			catch (Exception ex) 
			{
				DisplayUserMessage(UserMessage.UserMessageType.Error, Localization.GetExceptionMessage("UnhandledError"));
			}
			
		}

		public GADataClass Owner 
		{
		
			get { return null==ViewState["Owner"] ? GADataClass.NullClass : (GADataClass) ViewState["Owner"]; }
			set { ViewState["Owner"] = value; }
		}

		public GADataClass Member 
		{
		
			get { return null==ViewState["Member"] ? GADataClass.NullClass : (GADataClass) ViewState["Member"]; }
			set { ViewState["Member"] = value; }
		}



		class RolesValueObject
		{
			private String _roleName;
			private int _roleId;


			public string roleName
			{
				get { return _roleName; }
				set { _roleName = value; }
			}

			public int roleId
			{
				get { return _roleId; }
				set { _roleId = value; }
			}

		}
	}
}
