﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace GASystem.DataModel {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.ComponentModel.ToolboxItem(true)]
    [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [System.Xml.Serialization.XmlRootAttribute("ManyToManyLinksDS")]
    [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class ManyToManyLinksDS : System.Data.DataSet {
        
        private GAManyToManyLinksDataTable tableGAManyToManyLinks;
        
        private System.Data.SchemaSerializationMode _schemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public ManyToManyLinksDS() {
            this.BeginInit();
            this.InitClass();
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected ManyToManyLinksDS(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
                if ((ds.Tables["GAManyToManyLinks"] != null)) {
                    base.Tables.Add(new GAManyToManyLinksDataTable(ds.Tables["GAManyToManyLinks"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXmlSchema(new System.Xml.XmlTextReader(new System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Content)]
        public GAManyToManyLinksDataTable GAManyToManyLinks {
            get {
                return this.tableGAManyToManyLinks;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.BrowsableAttribute(true)]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override System.Data.DataSet Clone() {
            ManyToManyLinksDS cln = ((ManyToManyLinksDS)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["GAManyToManyLinks"] != null)) {
                    base.Tables.Add(new GAManyToManyLinksDataTable(ds.Tables["GAManyToManyLinks"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            this.WriteXmlSchema(new System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return System.Xml.Schema.XmlSchema.Read(new System.Xml.XmlTextReader(stream), null);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tableGAManyToManyLinks = ((GAManyToManyLinksDataTable)(base.Tables["GAManyToManyLinks"]));
            if ((initTable == true)) {
                if ((this.tableGAManyToManyLinks != null)) {
                    this.tableGAManyToManyLinks.InitVars();
                }
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "ManyToManyLinksDS";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/ManyToManyLinksDS.xsd";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            this.tableGAManyToManyLinks = new GAManyToManyLinksDataTable();
            base.Tables.Add(this.tableGAManyToManyLinks);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializeGAManyToManyLinks() {
            return false;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet xs) {
            ManyToManyLinksDS ds = new ManyToManyLinksDS();
            System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
            System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
            xs.Add(ds.GetSchemaSerializable());
            System.Xml.Schema.XmlSchemaAny any = new System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            return type;
        }
        
        public delegate void GAManyToManyLinksRowChangeEventHandler(object sender, GAManyToManyLinksRowChangeEvent e);
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [System.Serializable()]
        [System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class GAManyToManyLinksDataTable : System.Data.DataTable, System.Collections.IEnumerable {
            
            private System.Data.DataColumn columnLinkOwnerClass;
            
            private System.Data.DataColumn columnLinkOwnerClassField;
            
            private System.Data.DataColumn columnLinkMemberClass;
            
            private System.Data.DataColumn columnLinkMemberClassField;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksDataTable() {
                this.TableName = "GAManyToManyLinks";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal GAManyToManyLinksDataTable(System.Data.DataTable table) {
                this.TableName = table.TableName;
                if ((table.CaseSensitive != table.DataSet.CaseSensitive)) {
                    this.CaseSensitive = table.CaseSensitive;
                }
                if ((table.Locale.ToString() != table.DataSet.Locale.ToString())) {
                    this.Locale = table.Locale;
                }
                if ((table.Namespace != table.DataSet.Namespace)) {
                    this.Namespace = table.Namespace;
                }
                this.Prefix = table.Prefix;
                this.MinimumCapacity = table.MinimumCapacity;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected GAManyToManyLinksDataTable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LinkOwnerClassColumn {
                get {
                    return this.columnLinkOwnerClass;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LinkOwnerClassFieldColumn {
                get {
                    return this.columnLinkOwnerClassField;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LinkMemberClassColumn {
                get {
                    return this.columnLinkMemberClass;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataColumn LinkMemberClassFieldColumn {
                get {
                    return this.columnLinkMemberClassField;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksRow this[int index] {
                get {
                    return ((GAManyToManyLinksRow)(this.Rows[index]));
                }
            }
            
            public event GAManyToManyLinksRowChangeEventHandler GAManyToManyLinksRowChanging;
            
            public event GAManyToManyLinksRowChangeEventHandler GAManyToManyLinksRowChanged;
            
            public event GAManyToManyLinksRowChangeEventHandler GAManyToManyLinksRowDeleting;
            
            public event GAManyToManyLinksRowChangeEventHandler GAManyToManyLinksRowDeleted;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddGAManyToManyLinksRow(GAManyToManyLinksRow row) {
                this.Rows.Add(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksRow AddGAManyToManyLinksRow(string LinkOwnerClass, string LinkOwnerClassField, string LinkMemberClass, string LinkMemberClassField) {
                GAManyToManyLinksRow rowGAManyToManyLinksRow = ((GAManyToManyLinksRow)(this.NewRow()));
                rowGAManyToManyLinksRow.ItemArray = new object[] {
                        LinkOwnerClass,
                        LinkOwnerClassField,
                        LinkMemberClass,
                        LinkMemberClassField};
                this.Rows.Add(rowGAManyToManyLinksRow);
                return rowGAManyToManyLinksRow;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override System.Data.DataTable Clone() {
                GAManyToManyLinksDataTable cln = ((GAManyToManyLinksDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataTable CreateInstance() {
                return new GAManyToManyLinksDataTable();
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnLinkOwnerClass = base.Columns["LinkOwnerClass"];
                this.columnLinkOwnerClassField = base.Columns["LinkOwnerClassField"];
                this.columnLinkMemberClass = base.Columns["LinkMemberClass"];
                this.columnLinkMemberClassField = base.Columns["LinkMemberClassField"];
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnLinkOwnerClass = new System.Data.DataColumn("LinkOwnerClass", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLinkOwnerClass);
                this.columnLinkOwnerClassField = new System.Data.DataColumn("LinkOwnerClassField", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLinkOwnerClassField);
                this.columnLinkMemberClass = new System.Data.DataColumn("LinkMemberClass", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLinkMemberClass);
                this.columnLinkMemberClassField = new System.Data.DataColumn("LinkMemberClassField", typeof(string), null, System.Data.MappingType.Element);
                base.Columns.Add(this.columnLinkMemberClassField);
                this.columnLinkOwnerClass.MaxLength = 50;
                this.columnLinkOwnerClassField.MaxLength = 100;
                this.columnLinkMemberClass.MaxLength = 100;
                this.columnLinkMemberClassField.MaxLength = 100;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksRow NewGAManyToManyLinksRow() {
                return ((GAManyToManyLinksRow)(this.NewRow()));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Data.DataRow NewRowFromBuilder(System.Data.DataRowBuilder builder) {
                return new GAManyToManyLinksRow(builder);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override System.Type GetRowType() {
                return typeof(GAManyToManyLinksRow);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.GAManyToManyLinksRowChanged != null)) {
                    this.GAManyToManyLinksRowChanged(this, new GAManyToManyLinksRowChangeEvent(((GAManyToManyLinksRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.GAManyToManyLinksRowChanging != null)) {
                    this.GAManyToManyLinksRowChanging(this, new GAManyToManyLinksRowChangeEvent(((GAManyToManyLinksRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.GAManyToManyLinksRowDeleted != null)) {
                    this.GAManyToManyLinksRowDeleted(this, new GAManyToManyLinksRowChangeEvent(((GAManyToManyLinksRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.GAManyToManyLinksRowDeleting != null)) {
                    this.GAManyToManyLinksRowDeleting(this, new GAManyToManyLinksRowChangeEvent(((GAManyToManyLinksRow)(e.Row)), e.Action));
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemoveGAManyToManyLinksRow(GAManyToManyLinksRow row) {
                this.Rows.Remove(row);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet xs) {
                System.Xml.Schema.XmlSchemaComplexType type = new System.Xml.Schema.XmlSchemaComplexType();
                System.Xml.Schema.XmlSchemaSequence sequence = new System.Xml.Schema.XmlSchemaSequence();
                ManyToManyLinksDS ds = new ManyToManyLinksDS();
                xs.Add(ds.GetSchemaSerializable());
                System.Xml.Schema.XmlSchemaAny any1 = new System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                System.Xml.Schema.XmlSchemaAny any2 = new System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                System.Xml.Schema.XmlSchemaAttribute attribute1 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                System.Xml.Schema.XmlSchemaAttribute attribute2 = new System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "GAManyToManyLinksDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                return type;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class GAManyToManyLinksRow : System.Data.DataRow {
            
            private GAManyToManyLinksDataTable tableGAManyToManyLinks;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal GAManyToManyLinksRow(System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tableGAManyToManyLinks = ((GAManyToManyLinksDataTable)(this.Table));
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string LinkOwnerClass {
                get {
                    try {
                        return ((string)(this[this.tableGAManyToManyLinks.LinkOwnerClassColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'LinkOwnerClass\' in table \'GAManyToManyLinks\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAManyToManyLinks.LinkOwnerClassColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string LinkOwnerClassField {
                get {
                    try {
                        return ((string)(this[this.tableGAManyToManyLinks.LinkOwnerClassFieldColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'LinkOwnerClassField\' in table \'GAManyToManyLinks\' is DBNull" +
                                ".", e);
                    }
                }
                set {
                    this[this.tableGAManyToManyLinks.LinkOwnerClassFieldColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string LinkMemberClass {
                get {
                    try {
                        return ((string)(this[this.tableGAManyToManyLinks.LinkMemberClassColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'LinkMemberClass\' in table \'GAManyToManyLinks\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAManyToManyLinks.LinkMemberClassColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string LinkMemberClassField {
                get {
                    try {
                        return ((string)(this[this.tableGAManyToManyLinks.LinkMemberClassFieldColumn]));
                    }
                    catch (System.InvalidCastException e) {
                        throw new System.Data.StrongTypingException("The value for column \'LinkMemberClassField\' in table \'GAManyToManyLinks\' is DBNul" +
                                "l.", e);
                    }
                }
                set {
                    this[this.tableGAManyToManyLinks.LinkMemberClassFieldColumn] = value;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsLinkOwnerClassNull() {
                return this.IsNull(this.tableGAManyToManyLinks.LinkOwnerClassColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetLinkOwnerClassNull() {
                this[this.tableGAManyToManyLinks.LinkOwnerClassColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsLinkOwnerClassFieldNull() {
                return this.IsNull(this.tableGAManyToManyLinks.LinkOwnerClassFieldColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetLinkOwnerClassFieldNull() {
                this[this.tableGAManyToManyLinks.LinkOwnerClassFieldColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsLinkMemberClassNull() {
                return this.IsNull(this.tableGAManyToManyLinks.LinkMemberClassColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetLinkMemberClassNull() {
                this[this.tableGAManyToManyLinks.LinkMemberClassColumn] = System.Convert.DBNull;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsLinkMemberClassFieldNull() {
                return this.IsNull(this.tableGAManyToManyLinks.LinkMemberClassFieldColumn);
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetLinkMemberClassFieldNull() {
                this[this.tableGAManyToManyLinks.LinkMemberClassFieldColumn] = System.Convert.DBNull;
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class GAManyToManyLinksRowChangeEvent : System.EventArgs {
            
            private GAManyToManyLinksRow eventRow;
            
            private System.Data.DataRowAction eventAction;
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksRowChangeEvent(GAManyToManyLinksRow row, System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAManyToManyLinksRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}
namespace GASystem.DataModel.ManyToManyLinksDSTableAdapters {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DataObjectAttribute(true)]
    [System.ComponentModel.DesignerAttribute("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner" +
        ", Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
    public partial class GAManyToManyLinksTableAdapter : System.ComponentModel.Component {
        
        private System.Data.SqlClient.SqlDataAdapter _adapter;
        
        private System.Data.SqlClient.SqlConnection _connection;
        
        private System.Data.SqlClient.SqlCommand[] _commandCollection;
        
        private bool _clearBeforeFill;
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public GAManyToManyLinksTableAdapter() {
            this.ClearBeforeFill = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private System.Data.SqlClient.SqlDataAdapter Adapter {
            get {
                if ((this._adapter == null)) {
                    this.InitAdapter();
                }
                return this._adapter;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal System.Data.SqlClient.SqlConnection Connection {
            get {
                if ((this._connection == null)) {
                    this.InitConnection();
                }
                return this._connection;
            }
            set {
                this._connection = value;
                if ((this.Adapter.InsertCommand != null)) {
                    this.Adapter.InsertCommand.Connection = value;
                }
                if ((this.Adapter.DeleteCommand != null)) {
                    this.Adapter.DeleteCommand.Connection = value;
                }
                if ((this.Adapter.UpdateCommand != null)) {
                    this.Adapter.UpdateCommand.Connection = value;
                }
                for (int i = 0; (i < this.CommandCollection.Length); i = (i + 1)) {
                    if ((this.CommandCollection[i] != null)) {
                        ((System.Data.SqlClient.SqlCommand)(this.CommandCollection[i])).Connection = value;
                    }
                }
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected System.Data.SqlClient.SqlCommand[] CommandCollection {
            get {
                if ((this._commandCollection == null)) {
                    this.InitCommandCollection();
                }
                return this._commandCollection;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public bool ClearBeforeFill {
            get {
                return this._clearBeforeFill;
            }
            set {
                this._clearBeforeFill = value;
            }
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitAdapter() {
            this._adapter = new System.Data.SqlClient.SqlDataAdapter();
            System.Data.Common.DataTableMapping tableMapping = new System.Data.Common.DataTableMapping();
            tableMapping.SourceTable = "Table";
            tableMapping.DataSetTable = "GAManyToManyLinks";
            tableMapping.ColumnMappings.Add("LinkOwnerClass", "LinkOwnerClass");
            tableMapping.ColumnMappings.Add("LinkOwnerClassField", "LinkOwnerClassField");
            tableMapping.ColumnMappings.Add("LinkMemberClass", "LinkMemberClass");
            tableMapping.ColumnMappings.Add("LinkMemberClassField", "LinkMemberClassField");
            this._adapter.TableMappings.Add(tableMapping);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitConnection() {
            this._connection = new System.Data.SqlClient.SqlConnection();
            this._connection.ConnectionString = global::GASystem.Properties.Settings.Default.flagdata071025ConnectionString;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitCommandCollection() {
            this._commandCollection = new System.Data.SqlClient.SqlCommand[1];
            this._commandCollection[0] = new System.Data.SqlClient.SqlCommand();
            this._commandCollection[0].Connection = this.Connection;
            this._commandCollection[0].CommandText = "SELECT LinkOwnerClass, LinkOwnerClassField, LinkMemberClass, LinkMemberClassField" +
                " FROM dbo.GAManyToManyLinks";
            this._commandCollection[0].CommandType = System.Data.CommandType.Text;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Fill, true)]
        public virtual int Fill(ManyToManyLinksDS.GAManyToManyLinksDataTable dataTable) {
            this.Adapter.SelectCommand = this.CommandCollection[0];
            if ((this.ClearBeforeFill == true)) {
                dataTable.Clear();
            }
            int returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public virtual ManyToManyLinksDS.GAManyToManyLinksDataTable GetData() {
            this.Adapter.SelectCommand = this.CommandCollection[0];
            ManyToManyLinksDS.GAManyToManyLinksDataTable dataTable = new ManyToManyLinksDS.GAManyToManyLinksDataTable();
            this.Adapter.Fill(dataTable);
            return dataTable;
        }
    }
}

#pragma warning restore 1591