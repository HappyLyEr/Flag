﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5485
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591

namespace GASystem.DataModel {
    
    
    /// <summary>
    ///Represents a strongly typed in-memory cache of data.
    ///</summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
    [global::System.Serializable()]
    [global::System.ComponentModel.DesignerCategoryAttribute("code")]
    [global::System.ComponentModel.ToolboxItem(true)]
    [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedDataSetSchema")]
    [global::System.Xml.Serialization.XmlRootAttribute("ClassRoleAccessDS")]
    [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.DataSet")]
    public partial class ClassRoleAccessDS : global::System.Data.DataSet {
        
        private GAClassRoleAccessDataTable tableGAClassRoleAccess;
        
        private global::System.Data.SchemaSerializationMode _schemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public ClassRoleAccessDS() {
            this.BeginInit();
            this.InitClass();
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            base.Relations.CollectionChanged += schemaChangedHandler;
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected ClassRoleAccessDS(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                base(info, context, false) {
            if ((this.IsBinarySerialized(info, context) == true)) {
                this.InitVars(false);
                global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler1 = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
                this.Tables.CollectionChanged += schemaChangedHandler1;
                this.Relations.CollectionChanged += schemaChangedHandler1;
                return;
            }
            string strSchema = ((string)(info.GetValue("XmlSchema", typeof(string))));
            if ((this.DetermineSchemaSerializationMode(info, context) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
                if ((ds.Tables["GAClassRoleAccess"] != null)) {
                    base.Tables.Add(new GAClassRoleAccessDataTable(ds.Tables["GAClassRoleAccess"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXmlSchema(new global::System.Xml.XmlTextReader(new global::System.IO.StringReader(strSchema)));
            }
            this.GetSerializationData(info, context);
            global::System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new global::System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
            base.Tables.CollectionChanged += schemaChangedHandler;
            this.Relations.CollectionChanged += schemaChangedHandler;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.Browsable(false)]
        [global::System.ComponentModel.DesignerSerializationVisibility(global::System.ComponentModel.DesignerSerializationVisibility.Content)]
        public GAClassRoleAccessDataTable GAClassRoleAccess {
            get {
                return this.tableGAClassRoleAccess;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.BrowsableAttribute(true)]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Visible)]
        public override global::System.Data.SchemaSerializationMode SchemaSerializationMode {
            get {
                return this._schemaSerializationMode;
            }
            set {
                this._schemaSerializationMode = value;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataTableCollection Tables {
            get {
                return base.Tables;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.DesignerSerializationVisibilityAttribute(global::System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public new global::System.Data.DataRelationCollection Relations {
            get {
                return base.Relations;
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void InitializeDerivedDataSet() {
            this.BeginInit();
            this.InitClass();
            this.EndInit();
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public override global::System.Data.DataSet Clone() {
            ClassRoleAccessDS cln = ((ClassRoleAccessDS)(base.Clone()));
            cln.InitVars();
            cln.SchemaSerializationMode = this.SchemaSerializationMode;
            return cln;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeTables() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override bool ShouldSerializeRelations() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override void ReadXmlSerializable(global::System.Xml.XmlReader reader) {
            if ((this.DetermineSchemaSerializationMode(reader) == global::System.Data.SchemaSerializationMode.IncludeSchema)) {
                this.Reset();
                global::System.Data.DataSet ds = new global::System.Data.DataSet();
                ds.ReadXml(reader);
                if ((ds.Tables["GAClassRoleAccess"] != null)) {
                    base.Tables.Add(new GAClassRoleAccessDataTable(ds.Tables["GAClassRoleAccess"]));
                }
                this.DataSetName = ds.DataSetName;
                this.Prefix = ds.Prefix;
                this.Namespace = ds.Namespace;
                this.Locale = ds.Locale;
                this.CaseSensitive = ds.CaseSensitive;
                this.EnforceConstraints = ds.EnforceConstraints;
                this.Merge(ds, false, global::System.Data.MissingSchemaAction.Add);
                this.InitVars();
            }
            else {
                this.ReadXml(reader);
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        protected override global::System.Xml.Schema.XmlSchema GetSchemaSerializable() {
            global::System.IO.MemoryStream stream = new global::System.IO.MemoryStream();
            this.WriteXmlSchema(new global::System.Xml.XmlTextWriter(stream, null));
            stream.Position = 0;
            return global::System.Xml.Schema.XmlSchema.Read(new global::System.Xml.XmlTextReader(stream), null);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars() {
            this.InitVars(true);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        internal void InitVars(bool initTable) {
            this.tableGAClassRoleAccess = ((GAClassRoleAccessDataTable)(base.Tables["GAClassRoleAccess"]));
            if ((initTable == true)) {
                if ((this.tableGAClassRoleAccess != null)) {
                    this.tableGAClassRoleAccess.InitVars();
                }
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void InitClass() {
            this.DataSetName = "ClassRoleAccessDS";
            this.Prefix = "";
            this.Namespace = "http://tempuri.org/ClassRoleAccessDS.xsd";
            this.EnforceConstraints = true;
            this.SchemaSerializationMode = global::System.Data.SchemaSerializationMode.IncludeSchema;
            this.tableGAClassRoleAccess = new GAClassRoleAccessDataTable();
            base.Tables.Add(this.tableGAClassRoleAccess);
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private bool ShouldSerializeGAClassRoleAccess() {
            return false;
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        private void SchemaChanged(object sender, global::System.ComponentModel.CollectionChangeEventArgs e) {
            if ((e.Action == global::System.ComponentModel.CollectionChangeAction.Remove)) {
                this.InitVars();
            }
        }
        
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedDataSetSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
            ClassRoleAccessDS ds = new ClassRoleAccessDS();
            global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
            global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
            global::System.Xml.Schema.XmlSchemaAny any = new global::System.Xml.Schema.XmlSchemaAny();
            any.Namespace = ds.Namespace;
            sequence.Items.Add(any);
            type.Particle = sequence;
            global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
            if (xs.Contains(dsSchema.TargetNamespace)) {
                global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                try {
                    global::System.Xml.Schema.XmlSchema schema = null;
                    dsSchema.Write(s1);
                    for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                        schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                        s2.SetLength(0);
                        schema.Write(s2);
                        if ((s1.Length == s2.Length)) {
                            s1.Position = 0;
                            s2.Position = 0;
                            for (; ((s1.Position != s1.Length) 
                                        && (s1.ReadByte() == s2.ReadByte())); ) {
                                ;
                            }
                            if ((s1.Position == s1.Length)) {
                                return type;
                            }
                        }
                    }
                }
                finally {
                    if ((s1 != null)) {
                        s1.Close();
                    }
                    if ((s2 != null)) {
                        s2.Close();
                    }
                }
            }
            xs.Add(dsSchema);
            return type;
        }
        
        public delegate void GAClassRoleAccessRowChangeEventHandler(object sender, GAClassRoleAccessRowChangeEvent e);
        
        /// <summary>
        ///Represents the strongly named DataTable class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        [global::System.Serializable()]
        [global::System.Xml.Serialization.XmlSchemaProviderAttribute("GetTypedTableSchema")]
        public partial class GAClassRoleAccessDataTable : global::System.Data.DataTable, global::System.Collections.IEnumerable {
            
            private global::System.Data.DataColumn columnClassRowId;
            
            private global::System.Data.DataColumn columnClass;
            
            private global::System.Data.DataColumn columnReadRoles;
            
            private global::System.Data.DataColumn columnUpdateRoles;
            
            private global::System.Data.DataColumn columnCreateRoles;
            
            private global::System.Data.DataColumn columnDeleteRoles;
            
            private global::System.Data.DataColumn columnUpdateWithinRoles;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessDataTable() {
                this.TableName = "GAClassRoleAccess";
                this.BeginInit();
                this.InitClass();
                this.EndInit();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal GAClassRoleAccessDataTable(global::System.Data.DataTable table) {
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
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected GAClassRoleAccessDataTable(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context) : 
                    base(info, context) {
                this.InitVars();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn ClassRowIdColumn {
                get {
                    return this.columnClassRowId;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn ClassColumn {
                get {
                    return this.columnClass;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn ReadRolesColumn {
                get {
                    return this.columnReadRoles;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn UpdateRolesColumn {
                get {
                    return this.columnUpdateRoles;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn CreateRolesColumn {
                get {
                    return this.columnCreateRoles;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn DeleteRolesColumn {
                get {
                    return this.columnDeleteRoles;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataColumn UpdateWithinRolesColumn {
                get {
                    return this.columnUpdateWithinRoles;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.ComponentModel.Browsable(false)]
            public int Count {
                get {
                    return this.Rows.Count;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRow this[int index] {
                get {
                    return ((GAClassRoleAccessRow)(this.Rows[index]));
                }
            }
            
            public event GAClassRoleAccessRowChangeEventHandler GAClassRoleAccessRowChanging;
            
            public event GAClassRoleAccessRowChangeEventHandler GAClassRoleAccessRowChanged;
            
            public event GAClassRoleAccessRowChangeEventHandler GAClassRoleAccessRowDeleting;
            
            public event GAClassRoleAccessRowChangeEventHandler GAClassRoleAccessRowDeleted;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void AddGAClassRoleAccessRow(GAClassRoleAccessRow row) {
                this.Rows.Add(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRow AddGAClassRoleAccessRow(string Class, string ReadRoles, string UpdateRoles, string CreateRoles, string DeleteRoles, string UpdateWithinRoles) {
                GAClassRoleAccessRow rowGAClassRoleAccessRow = ((GAClassRoleAccessRow)(this.NewRow()));
                object[] columnValuesArray = new object[] {
                        null,
                        Class,
                        ReadRoles,
                        UpdateRoles,
                        CreateRoles,
                        DeleteRoles,
                        UpdateWithinRoles};
                rowGAClassRoleAccessRow.ItemArray = columnValuesArray;
                this.Rows.Add(rowGAClassRoleAccessRow);
                return rowGAClassRoleAccessRow;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRow FindByClassRowId(int ClassRowId) {
                return ((GAClassRoleAccessRow)(this.Rows.Find(new object[] {
                            ClassRowId})));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public virtual global::System.Collections.IEnumerator GetEnumerator() {
                return this.Rows.GetEnumerator();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public override global::System.Data.DataTable Clone() {
                GAClassRoleAccessDataTable cln = ((GAClassRoleAccessDataTable)(base.Clone()));
                cln.InitVars();
                return cln;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataTable CreateInstance() {
                return new GAClassRoleAccessDataTable();
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal void InitVars() {
                this.columnClassRowId = base.Columns["ClassRowId"];
                this.columnClass = base.Columns["Class"];
                this.columnReadRoles = base.Columns["ReadRoles"];
                this.columnUpdateRoles = base.Columns["UpdateRoles"];
                this.columnCreateRoles = base.Columns["CreateRoles"];
                this.columnDeleteRoles = base.Columns["DeleteRoles"];
                this.columnUpdateWithinRoles = base.Columns["UpdateWithinRoles"];
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            private void InitClass() {
                this.columnClassRowId = new global::System.Data.DataColumn("ClassRowId", typeof(int), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnClassRowId);
                this.columnClass = new global::System.Data.DataColumn("Class", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnClass);
                this.columnReadRoles = new global::System.Data.DataColumn("ReadRoles", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnReadRoles);
                this.columnUpdateRoles = new global::System.Data.DataColumn("UpdateRoles", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnUpdateRoles);
                this.columnCreateRoles = new global::System.Data.DataColumn("CreateRoles", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnCreateRoles);
                this.columnDeleteRoles = new global::System.Data.DataColumn("DeleteRoles", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnDeleteRoles);
                this.columnUpdateWithinRoles = new global::System.Data.DataColumn("UpdateWithinRoles", typeof(string), null, global::System.Data.MappingType.Element);
                base.Columns.Add(this.columnUpdateWithinRoles);
                this.Constraints.Add(new global::System.Data.UniqueConstraint("Constraint1", new global::System.Data.DataColumn[] {
                                this.columnClassRowId}, true));
                this.columnClassRowId.AutoIncrement = true;
                this.columnClassRowId.AllowDBNull = false;
                this.columnClassRowId.ReadOnly = true;
                this.columnClassRowId.Unique = true;
                this.columnClass.MaxLength = 4000;
                this.columnReadRoles.MaxLength = 1073741823;
                this.columnUpdateRoles.MaxLength = 1073741823;
                this.columnCreateRoles.MaxLength = 1073741823;
                this.columnDeleteRoles.MaxLength = 1073741823;
                this.columnUpdateWithinRoles.MaxLength = 1073741823;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRow NewGAClassRoleAccessRow() {
                return ((GAClassRoleAccessRow)(this.NewRow()));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Data.DataRow NewRowFromBuilder(global::System.Data.DataRowBuilder builder) {
                return new GAClassRoleAccessRow(builder);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override global::System.Type GetRowType() {
                return typeof(GAClassRoleAccessRow);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanged(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanged(e);
                if ((this.GAClassRoleAccessRowChanged != null)) {
                    this.GAClassRoleAccessRowChanged(this, new GAClassRoleAccessRowChangeEvent(((GAClassRoleAccessRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowChanging(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowChanging(e);
                if ((this.GAClassRoleAccessRowChanging != null)) {
                    this.GAClassRoleAccessRowChanging(this, new GAClassRoleAccessRowChangeEvent(((GAClassRoleAccessRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleted(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleted(e);
                if ((this.GAClassRoleAccessRowDeleted != null)) {
                    this.GAClassRoleAccessRowDeleted(this, new GAClassRoleAccessRowChangeEvent(((GAClassRoleAccessRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            protected override void OnRowDeleting(global::System.Data.DataRowChangeEventArgs e) {
                base.OnRowDeleting(e);
                if ((this.GAClassRoleAccessRowDeleting != null)) {
                    this.GAClassRoleAccessRowDeleting(this, new GAClassRoleAccessRowChangeEvent(((GAClassRoleAccessRow)(e.Row)), e.Action));
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void RemoveGAClassRoleAccessRow(GAClassRoleAccessRow row) {
                this.Rows.Remove(row);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public static global::System.Xml.Schema.XmlSchemaComplexType GetTypedTableSchema(global::System.Xml.Schema.XmlSchemaSet xs) {
                global::System.Xml.Schema.XmlSchemaComplexType type = new global::System.Xml.Schema.XmlSchemaComplexType();
                global::System.Xml.Schema.XmlSchemaSequence sequence = new global::System.Xml.Schema.XmlSchemaSequence();
                ClassRoleAccessDS ds = new ClassRoleAccessDS();
                global::System.Xml.Schema.XmlSchemaAny any1 = new global::System.Xml.Schema.XmlSchemaAny();
                any1.Namespace = "http://www.w3.org/2001/XMLSchema";
                any1.MinOccurs = new decimal(0);
                any1.MaxOccurs = decimal.MaxValue;
                any1.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any1);
                global::System.Xml.Schema.XmlSchemaAny any2 = new global::System.Xml.Schema.XmlSchemaAny();
                any2.Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1";
                any2.MinOccurs = new decimal(1);
                any2.ProcessContents = global::System.Xml.Schema.XmlSchemaContentProcessing.Lax;
                sequence.Items.Add(any2);
                global::System.Xml.Schema.XmlSchemaAttribute attribute1 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute1.Name = "namespace";
                attribute1.FixedValue = ds.Namespace;
                type.Attributes.Add(attribute1);
                global::System.Xml.Schema.XmlSchemaAttribute attribute2 = new global::System.Xml.Schema.XmlSchemaAttribute();
                attribute2.Name = "tableTypeName";
                attribute2.FixedValue = "GAClassRoleAccessDataTable";
                type.Attributes.Add(attribute2);
                type.Particle = sequence;
                global::System.Xml.Schema.XmlSchema dsSchema = ds.GetSchemaSerializable();
                if (xs.Contains(dsSchema.TargetNamespace)) {
                    global::System.IO.MemoryStream s1 = new global::System.IO.MemoryStream();
                    global::System.IO.MemoryStream s2 = new global::System.IO.MemoryStream();
                    try {
                        global::System.Xml.Schema.XmlSchema schema = null;
                        dsSchema.Write(s1);
                        for (global::System.Collections.IEnumerator schemas = xs.Schemas(dsSchema.TargetNamespace).GetEnumerator(); schemas.MoveNext(); ) {
                            schema = ((global::System.Xml.Schema.XmlSchema)(schemas.Current));
                            s2.SetLength(0);
                            schema.Write(s2);
                            if ((s1.Length == s2.Length)) {
                                s1.Position = 0;
                                s2.Position = 0;
                                for (; ((s1.Position != s1.Length) 
                                            && (s1.ReadByte() == s2.ReadByte())); ) {
                                    ;
                                }
                                if ((s1.Position == s1.Length)) {
                                    return type;
                                }
                            }
                        }
                    }
                    finally {
                        if ((s1 != null)) {
                            s1.Close();
                        }
                        if ((s2 != null)) {
                            s2.Close();
                        }
                    }
                }
                xs.Add(dsSchema);
                return type;
            }
        }
        
        /// <summary>
        ///Represents strongly named DataRow class.
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public partial class GAClassRoleAccessRow : global::System.Data.DataRow {
            
            private GAClassRoleAccessDataTable tableGAClassRoleAccess;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            internal GAClassRoleAccessRow(global::System.Data.DataRowBuilder rb) : 
                    base(rb) {
                this.tableGAClassRoleAccess = ((GAClassRoleAccessDataTable)(this.Table));
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public int ClassRowId {
                get {
                    return ((int)(this[this.tableGAClassRoleAccess.ClassRowIdColumn]));
                }
                set {
                    this[this.tableGAClassRoleAccess.ClassRowIdColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string Class {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.ClassColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'Class\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.ClassColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string ReadRoles {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.ReadRolesColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'ReadRoles\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.ReadRolesColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string UpdateRoles {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.UpdateRolesColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'UpdateRoles\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.UpdateRolesColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string CreateRoles {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.CreateRolesColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'CreateRoles\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.CreateRolesColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string DeleteRoles {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.DeleteRolesColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'DeleteRoles\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.DeleteRolesColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public string UpdateWithinRoles {
                get {
                    try {
                        return ((string)(this[this.tableGAClassRoleAccess.UpdateWithinRolesColumn]));
                    }
                    catch (global::System.InvalidCastException e) {
                        throw new global::System.Data.StrongTypingException("The value for column \'UpdateWithinRoles\' in table \'GAClassRoleAccess\' is DBNull.", e);
                    }
                }
                set {
                    this[this.tableGAClassRoleAccess.UpdateWithinRolesColumn] = value;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsClassNull() {
                return this.IsNull(this.tableGAClassRoleAccess.ClassColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetClassNull() {
                this[this.tableGAClassRoleAccess.ClassColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsReadRolesNull() {
                return this.IsNull(this.tableGAClassRoleAccess.ReadRolesColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetReadRolesNull() {
                this[this.tableGAClassRoleAccess.ReadRolesColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsUpdateRolesNull() {
                return this.IsNull(this.tableGAClassRoleAccess.UpdateRolesColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetUpdateRolesNull() {
                this[this.tableGAClassRoleAccess.UpdateRolesColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsCreateRolesNull() {
                return this.IsNull(this.tableGAClassRoleAccess.CreateRolesColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetCreateRolesNull() {
                this[this.tableGAClassRoleAccess.CreateRolesColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsDeleteRolesNull() {
                return this.IsNull(this.tableGAClassRoleAccess.DeleteRolesColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetDeleteRolesNull() {
                this[this.tableGAClassRoleAccess.DeleteRolesColumn] = global::System.Convert.DBNull;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public bool IsUpdateWithinRolesNull() {
                return this.IsNull(this.tableGAClassRoleAccess.UpdateWithinRolesColumn);
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public void SetUpdateWithinRolesNull() {
                this[this.tableGAClassRoleAccess.UpdateWithinRolesColumn] = global::System.Convert.DBNull;
            }
        }
        
        /// <summary>
        ///Row event argument class
        ///</summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "2.0.0.0")]
        public class GAClassRoleAccessRowChangeEvent : global::System.EventArgs {
            
            private GAClassRoleAccessRow eventRow;
            
            private global::System.Data.DataRowAction eventAction;
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRowChangeEvent(GAClassRoleAccessRow row, global::System.Data.DataRowAction action) {
                this.eventRow = row;
                this.eventAction = action;
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public GAClassRoleAccessRow Row {
                get {
                    return this.eventRow;
                }
            }
            
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            public global::System.Data.DataRowAction Action {
                get {
                    return this.eventAction;
                }
            }
        }
    }
}

#pragma warning restore 1591