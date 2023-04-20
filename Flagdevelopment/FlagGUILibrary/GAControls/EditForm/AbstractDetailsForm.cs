using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.DataAccess;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.WebControls.EditControls.EditForm;
using log4net;
using GASystem.AppUtils;
using GASystem.UserControls;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Abstract class for use as superclass for all GA editforms. Specific edit forms implementations
	/// for GA must inherit from this class
	/// </summary>
	public abstract class AbstractDetailsForm : System.Web.UI.WebControls.WebControl
	{
		private static readonly ILog _logger = LogManager.GetLogger(typeof(AbstractDetailsForm));

		protected EditDataRecord myEditDataRecord;

        protected SelectTemplate mySelectTemplate;
   

		protected PlaceHolder PlaceHolderTop;
		private GADataRecord myOwner;
		private GADataRecord myDataRecord;


		/// <summary>
		/// Constructor. Record reference for record to be edited must be set here. Use a gadatarecord with rowid 0 for 
		/// adding a new record. 
		/// </summary>
		/// <param name="DataRecord">GADataRecord reference for record to be edited</param>
		public AbstractDetailsForm(GADataRecord DataRecord) 
		{
			myDataRecord = DataRecord;
			PlaceHolderTop = new PlaceHolder();
		}

		/// <summary>
		/// Current datarecord
		/// </summary>
		public GADataRecord DataRecord 
		{
			get {return myDataRecord;}
			
		}

		/// <summary>
		/// Owner of current datarecord
		/// </summary>
		public GADataRecord OwnerRecord 
		{
			get {return myOwner;}
			set {myOwner = value;}
		}

		/// <summary>
		/// Returns the current dataset for this form
		/// </summary>
		protected DataSet RecordDataSet
		{
			get {return myEditDataRecord.RecordDataSet;}
		}

		/// <summary>
		/// Load data and initilize the usercontrol EditDataRecord. 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			myEditDataRecord = GetEditFormControl();
			
			//handle events from editdatarecord
			myEditDataRecord.EditRecordSave += new GASystem.GAGUIEvents.GACommandEventHandler(EditRecordSave);
			myEditDataRecord.EditRecordCancel += new GASystem.GAGUIEvents.GACommandEventHandler(EditRecordCancel);
            myEditDataRecord.EditRecordDelete += new GASystem.GAGUIEvents.GACommandEventHandler(EditRecordDelete);

			this.Controls.Add(PlaceHolderTop);
			this.Controls.Add(myEditDataRecord);
			if (DataRecord.RowId == 0 && DataRecord.DataClass == GADataClass.GACrisis && OwnerRecord.DataClass != GADataClass.GATemplate)
			{
				mySelectTemplate = GetSelectTemplateControl();
				if (mySelectTemplate != null)
				{
					mySelectTemplate.TemplateSelected += new GASystem.GAGUIEvents.GACommandEventHandler(TemplateSelected);
					ClassDescription classDesc = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(GADataClass.GACrisis);
					mySelectTemplate.TemplateRootRecord = classDesc.ClassTemplateRootNode;
					mySelectTemplate.TemplateDataClass = GADataClass.GACrisis;
					this.Controls.Add(mySelectTemplate);
                    myEditDataRecord.Visible = false;
				}
			}
            
        
			//if (!this.Page.IsPostBack) 
			//{
				myEditDataRecord.DataClass = myDataRecord.DataClass.ToString();
                myEditDataRecord.Owner = this.OwnerRecord;


				GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(DataRecord.DataClass);		
				if (DataRecord.RowId == 0) 
				{
					//newrecord
                    // Tor 20140827 If owner==null and dataclass is owned by GAFlag, set owner data record = GAFlag/1
                    GADataRecord classOwner = this.OwnerRecord;
                    if (this.OwnerRecord == null)
                    {
                        SuperClassLinksDS Owner = SuperClassDb.GetSuperClassLinksByMember(DataRecord.DataClass);
                        string a = Owner.Tables[0].Rows[0]["OwnerClass"].ToString();

                        //if (Owner.Tables[0].Rows[0].Table.ToString() == "GAFlag")
                        if (Owner.Tables.Count == 1 && a == "GAFlag")
                        {
                            this.OwnerRecord = new GADataRecord(1, GADataClass.GAFlag);
                            classOwner = this.OwnerRecord;// new GADataRecord(1, GADataClass.GAFlag);
                        }
                    }

                    myEditDataRecord.RecordDataSet = bc.GetNewRecord(classOwner);
                    //myEditDataRecord.RecordDataSet = bc.GetNewRecord(this.OwnerRecord);
                    // Tor end 20140827
                    
                    //add extra info tables to dataset
                    myEditDataRecord.RecordDataSet.Tables.Add(new GASystem.DataModel.ListsSelectedDS().Tables[0].Copy());
				}
				else
				{
					//myEditDataRecord.RecordDataSet = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(myDataRecord);
					myEditDataRecord.RecordDataSet = bc.GetByRowId(myDataRecord.RowId);

                    //get lists selected for this control
                    GASystem.BusinessLayer.BusinessClass bcListsSelected = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);
                    DataSet dsListsSelected = bcListsSelected.GetByOwner(this.DataRecord, null);
                    
                    /* Lars Heskje, 05112010: Added a test to make sure GAListsSelected is not added to the dataset multiple times, 
                        as it could be when dataset is fetched from datacache. */
                    if (!myEditDataRecord.RecordDataSet.Tables.Contains("GAListsSelected")) 
                        myEditDataRecord.RecordDataSet.Tables.Add(dsListsSelected.Tables[GADataClass.GAListsSelected.ToString()].Copy());
				}
                myEditDataRecord.SetupForm(); 

            
            //}
			
            if (DataRecord.RowId == 0)
			{
				AddSubClassesForm();  //new record add subforms
			}
		}

		private void AddSubClassesForm()
		{
			StoreObjectDS dsMembers = StoreObjectDb.GetStoreObjectsByOwnerClass(this.DataRecord.DataClass.ToString());
			
			if (dsMembers.GAStoreObject.Rows.Count == 0)
				return; //no members to add
			
			foreach (StoreObjectDS.GAStoreObjectRow memberRow in dsMembers.GAStoreObject.Rows) 
			{
				if (!memberRow.IsSwitchFree1Null() && memberRow.SwitchFree1 == true)
				{
					WebControls.EditControls.EditForm.GeneralEditForm subForm = new GeneralEditForm();
					subForm.DataClass = memberRow.MemberClass;
					
					GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(memberRow.MemberClass));		
					subForm.RecordDataSet = bc.GetNewRecord(null);
					//limit columns to display
					StoreAttributeDS dsAttributes = StoreAttributeDb.GetStoreAttributesByOwner(new GADataRecord(memberRow.StoreObjectRowId, GADataClass.GAStoreObject)); 
					//create new row
					//BusinessClass bc =  RecordsetFactory.Make(GADataRecord.ParseGADataClass(memberRow.MemberClass));

					//add attributes
					foreach (StoreAttributeDS.GAStoreAttributeRow attributeRow in dsAttributes.GAStoreAttribute.Rows ) 
					{
						//check whether user should supply value
						if (!attributeRow.IsSwitchFree1Null() && attributeRow.SwitchFree1 == true && attributeRow.IsAtrributeValueNull())    
						{
							subForm.AddColumnToDisplay(attributeRow.AttributeName);
						}
					}

					myEditDataRecord.AddSubClassForm(subForm);
						
				}
			}
			
			
//			if (DataRecord.DataClass == GADataClass.GASafetyObservation)
//			{
//				WebControls.EditControls.EditForm.GeneralEditForm subForm = new GeneralEditForm();
//				subForm.DataClass = GADataClass.GAAction.ToString();
//				GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAAction);		
//				subForm.RecordDataSet = bc.GetNewRecord();
//				//subForm.SetupForm();
//				myEditDataRecord.AddSubClassForm(subForm);
//			}
		}

        
        /// <summary>
        /// Method for getting selectTemplateControl
        /// </summary>
        /// <returns></returns>
        protected virtual SelectTemplate GetSelectTemplateControl()
        {
            return (SelectTemplate)this.Page.LoadControl("~/gagui/UserControls/SelectTemplate.ascx");
        }

		/// <summary>
		/// Method for getting editform.
		/// Refactored to seperate method for ease of overriding
		/// </summary>
		/// <returns></returns>
		protected virtual EditDataRecord GetEditFormControl() 
		{
			return (EditDataRecord)this.Page.LoadControl("~/gagui/UserControls/EditDataRecord.ascx");
		}

		/// <summary>
		/// Edit record save button click. Subclasses must implement this method for saving data.
		/// </summary>
		/// <param name="sender">EditDataRecord</param>
		/// <param name="e">GACommandEventArgs</param>
		protected abstract void EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e);

		/// <summary>
		/// Cancel record save button clicked. Subclasses must implement this method.
		/// </summary>
		/// <param name="sender">EditDataRecord</param>
		/// <param name="e">GACommandEventArgs</param>
		protected abstract void EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e);

        /// <summary>
        /// Edit record delete button clicked. Subclasses must implement this method.
        /// </summary>
        /// <param name="sender">EditDataRecord</param>
        /// <param name="e">GACommandEventArgs</param>
        protected abstract void EditRecordDelete(object sender, GASystem.GAGUIEvents.GACommandEventArgs e);

		/// <summary>
		/// Select templae button was clicked (only applies if SelectTemplate control is loaded)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected abstract void TemplateSelected(object sender, GASystem.GAGUIEvents.GACommandEventArgs e);


		protected void DisplayUserMessage(string Message, UserMessage.UserMessageType MessageType)
		{
			try
			{
				UserMessage userMessageControl = (UserMessage) this.Page.LoadControl("~/gagui/UserControls/UserMessage.ascx");
				userMessageControl.MessageType = MessageType;
				userMessageControl.MessageText = Message;
				this.Controls.Add(userMessageControl);
			}
			catch (Exception e)
			{
				_logger.Error(e.Message, e);	
			}
		}
		
	}
}
