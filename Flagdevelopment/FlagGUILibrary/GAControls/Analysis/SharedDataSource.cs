using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DataDynamics.Analysis.Schema;
using DataDynamics.Analysis.DataSources;
using DataDynamics.Analysis;


namespace GASystem.GAGUI.GAControls.Analysis
{
	public sealed class SharedDataSourceWithPerspectives : SharedDataSourceBase, IMultiplePerspectives
	{
		public IMultiplePerspectives MultiplePerspectivesDataSource
		{
			get { return (IMultiplePerspectives)UnderlyingDataSource; }
		}

		#region IMultiplePerspectives Members

		public string Perspective
		{
			get { return MultiplePerspectivesDataSource.Perspective; }
			set { MultiplePerspectivesDataSource.Perspective = value; }
		}

		public IList<string> Perspectives
		{
			get { return MultiplePerspectivesDataSource.Perspectives; }
		}

		/// <summary>
		/// Gets the <see cref="SchemaDefinition"/> by perspective.
		/// </summary>
		/// <param name="perspective"></param>
		/// <returns></returns>
		public SchemaDefinition GetSchema(string perspective)
		{
			return MultiplePerspectivesDataSource.GetSchema(perspective);
		}

		public event EventHandler PerspectiveChanged;

		#endregion

		#region Overrides

		protected override void BeforeConnect()
		{
			base.BeforeConnect();
			MultiplePerspectivesDataSource.PerspectiveChanged += OnPerspectiveChanged;
		}

		protected override void BeforeDisconnect()
		{
			base.BeforeDisconnect();
			MultiplePerspectivesDataSource.PerspectiveChanged -= OnPerspectiveChanged;
		}

		protected override bool ValidateDataSource()
		{
			return UnderlyingDataSource == null || UnderlyingDataSource is IMultiplePerspectives;
		}

		#endregion

		#region Implementation

		private void OnPerspectiveChanged(object sender, EventArgs e)
		{
			if (PerspectiveChanged != null)
				PerspectiveChanged(this, e);
		}

		#endregion
	}

	public class SharedPersistLocalCubeDataSource : SharedDataSourceBase, IPersistLocalCube
	{
		#region IPersistLocalCube Members

		public void SaveLocalCube(FileInfo cubeFileInfo, SaveLocalCubeOptions options)
		{
			((IPersistLocalCube)UnderlyingDataSource).SaveLocalCube(cubeFileInfo, options);
		}

		#endregion

		protected override bool ValidateDataSource()
		{
			return UnderlyingDataSource == null || UnderlyingDataSource is IPersistLocalCube;
		}

		#region ISchemaFactoryProvider Members

		/// <summary>
		/// Gets an object that implements <see cref="IRdSchemaFactory"/>.
		/// </summary>
		public ISchemaFactory SchemaFactory
		{
			get { return ((ISchemaFactoryProvider) UnderlyingDataSource).SchemaFactory; }
		}

		#endregion
	}

	public sealed class SharedPersistLocalCubeDataSourceWithFactory: SharedPersistLocalCubeDataSource, ISchemaFactoryProvider
	{
		protected override bool ValidateDataSource()
		{
			return base.ValidateDataSource() && (UnderlyingDataSource == null || UnderlyingDataSource is ISchemaFactoryProvider);
		}
	}

	public abstract class SharedDataSourceBase : IDataSource, IPersistXmlSettings, 
		IInternalPivotDataProviderAccessor
	{
		private string _name;
		private IDataSource _underlyingDataSource;
		private bool _isConnected;

		/// <summary>
		/// Gets the data source catalog.
		/// </summary>
		/// <value>The data source catalog.</value>
		public static DataSourceCatalog DataSourceCatalog
		{
			get { return DataSourceCatalog.GetInstance(Folder); }
		}

		internal static string _folder;

		/// <summary>
		/// Folder where the data source catalog is placed.
		/// </summary>
		internal static string Folder
		{
			set
			{
				if (string.IsNullOrEmpty(value))
					throw new ArgumentNullException("value");
				_folder = value;
			}
			get
			{
				if(string.IsNullOrEmpty(_folder))
				{
					_folder = System.Configuration.ConfigurationManager.AppSettings["DataSourcesPath"];
				}
				return _folder;
			}
		}

		public string Name
		{
			get { return _name; }
			internal set
			{
				if (UnderlyingDataSource != null && UnderlyingDataSource.IsConnected && IsConnected)
					throw new DataSourceConfigurationException("Name",
						"Connection is expected to be in disconnected/not initialized mode");

				if (_name == value)
					return;

				// Notice we allow to set the name to empty
				//ConnectionState oldState = State;
				_name = value;
				UnderlyingDataSource = DataSourceCatalog.Resolve(_name);

				if (!string.IsNullOrEmpty(_name) && UnderlyingDataSource == null || !ValidateDataSource())
					throw new DataSourceConfigurationException("Name",
						"Failed to find data source for " + _name);

				_isConnected = false;
				if (StateChanged != null)
					StateChanged(this, EventArgs.Empty);
			}
		}

		public IDataSource UnderlyingDataSource
		{
			get { return _underlyingDataSource; }
			private set { _underlyingDataSource = value; }
		}


		#region IDataSource Members

		public void Connect()
		{
			if (UnderlyingDataSource == null || string.IsNullOrEmpty(Name))
				throw new DataSourceConfigurationException(null, "Data source is not initalized yet");

			if (IsConnected && UnderlyingDataSource.IsConnected)
				throw new DataSourceException("Connection is already opened");

			BeforeConnect();

			if (string.IsNullOrEmpty(Name) || UnderlyingDataSource == null)
				throw new DataSourceConfigurationException(null,
					"Underlaying data source is not properly configured.");

			bool oldIsConnected = IsConnected;
			if (!UnderlyingDataSource.IsConnected)
				UnderlyingDataSource.Connect();

			_isConnected = true;

			if (oldIsConnected != UnderlyingDataSource.IsConnected && StateChanged != null)
				StateChanged(this, EventArgs.Empty);
		}

		public void Disconnect()
		{
			BeforeDisconnect();

			bool oldIsConnected = IsConnected;
			_isConnected = false;

			if (oldIsConnected != _isConnected && StateChanged != null)
				StateChanged(this, EventArgs.Empty);
		}

		public bool IsConnected
		{
			get
			{
				return _isConnected;
			}
		}

		public SchemaDefinition Schema
		{
			get
			{
				if (string.IsNullOrEmpty(Name) || UnderlyingDataSource == null || !UnderlyingDataSource.IsConnected)
					return null;

				return UnderlyingDataSource.Schema;
			}
		}

		public event EventHandler StateChanged;

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			throw new NotImplementedException();
		}

		#endregion

		#region IPersistXmlSettings Members

		public const string TagSharedDataSourceName = "Name";
		public const string TagPerspective = "Perspective";

		public void Write(XmlWriter writer)
		{
			writer.WriteElementString(TagSharedDataSourceName, Name);
			IMultiplePerspectives withPerspectives = UnderlyingDataSource as IMultiplePerspectives;
			if (withPerspectives != null)
				writer.WriteElementString(TagPerspective, withPerspectives.Perspective);
		}

		public void Read(XmlReader reader)
		{
			Name = reader.ReadElementString(TagSharedDataSourceName);
			IMultiplePerspectives withPerspectives = UnderlyingDataSource as IMultiplePerspectives;
			if (reader.LocalName == TagPerspective && withPerspectives != null)
				withPerspectives.Perspective = reader.ReadElementString(TagPerspective);
		}

		#endregion

		#region IInternalPivotDataProviderAccessor Members

		DataDynamics.Analysis.Core.Data.PivotDataProvider IInternalPivotDataProviderAccessor.DataProvider
		{
			get
			{
				IInternalPivotDataProviderAccessor p = 
					UnderlyingDataSource as IInternalPivotDataProviderAccessor;
				if (UnderlyingDataSource != null && p == null)
					throw new DataProviderException("Underlaying data source does not implement IInternalPivotDataProviderAccessor as expected");
				return p != null && _isConnected ? p.DataProvider : null;
			}
		}

		#endregion

		#region Overridable

		protected virtual void BeforeConnect() { }
		protected virtual void BeforeDisconnect() { }
		protected abstract bool ValidateDataSource();

		#endregion

		public static IDataSource Create(string dataSourceName)
		{
			IDataSource ds = DataSourceCatalog.Resolve(dataSourceName);
			SharedDataSourceBase newDs;
			if (ds is IMultiplePerspectives)
				newDs = new SharedDataSourceWithPerspectives();
			else if (ds is IPersistLocalCube)
				newDs = (ds is ISchemaFactoryProvider
					? new SharedPersistLocalCubeDataSourceWithFactory()
					: new SharedPersistLocalCubeDataSource());
			else
				throw new NotSupportedException();

			newDs.Name = dataSourceName;
			return newDs;
		}

		public static void RegisterSharedInFactory()
		{
			DataSourceFactory.Instance.RegisterDataSource("SharedLC", typeof(SharedPersistLocalCubeDataSourceWithFactory));
			DataSourceFactory.Instance.RegisterDataSource("SharedLocalCube", typeof(SharedPersistLocalCubeDataSource));
			DataSourceFactory.Instance.RegisterDataSource("SharedMP", typeof(SharedDataSourceWithPerspectives));
		}
	}

	public sealed class DataSourceCatalog : IDisposable
	{
		private readonly List<NamedDataSource> _dataSources;
		private IList<string> _names;
		private string _folder = null;
		private readonly bool _initializing;

		/// <summary>
		/// Data Dynamics Analysis Data Source file extension
		/// </summary>
		internal const string FileExtension = "ddads";
		/// <summary>
		/// Gets the default path to load/save the data sources to
		/// </summary>
		internal static readonly string DefaultPath = Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.MyDocuments), @"Data Dynamics\Analysis\DataSources");

		internal static readonly Dictionary<string, DataSourceCatalog> Catalogs = 
			new Dictionary<string, DataSourceCatalog>();

		private static readonly object _mutex = new object();

		public static DataSourceCatalog GetInstance(string folder)
		{
			lock (_mutex)
			{
				if (string.IsNullOrEmpty(folder))
					folder = DefaultPath;

				if (!Catalogs.ContainsKey(folder))
					Catalogs.Add(folder, new DataSourceCatalog(folder));

				return Catalogs[folder];
			}
		}

		private DataSourceCatalog(string folderPath)
		{
			if (string.IsNullOrEmpty(folderPath))
				throw new ArgumentNullException("folderPath");

			if (!Directory.Exists(folderPath))
				throw new ArgumentException("Folder is not found");

			_dataSources = new List<NamedDataSource>();

			_initializing = true;
			InitFromFolder(folderPath);
			_initializing = false;
		}


		#region Public methods

		/// <summary>
		/// Adds new data source to the catalog
		/// </summary>
		/// <param name="name"></param>
		/// <param name="dataSource"></param>
		public void AddDataSource(string name, IDataSource dataSource)
		{
			if (_dataSources.Exists(new NamedDataSourcePredicate(name).Predicate))//delegate(NamedDataSource ds) { return ds.Name == name; }))
				throw new ArgumentException("DataSource with that name is already added", "name");
			if (dataSource == null)
				throw new ArgumentNullException("dataSource");

			NamedDataSource namedDataSource = new NamedDataSource(name, dataSource);
			AddDataSourceImpl(namedDataSource);
		}

		private class NamedDataSourcePredicate
		{
			private readonly string _dsName;
			public NamedDataSourcePredicate(string dsName)
			{
				_dsName = dsName;
			}
			public bool Predicate(NamedDataSource ds)
			{
				return ds.Name == _dsName;
			}
		}

		/// <summary>
		/// Removes data source from collection, optionally removing it from folder
		/// </summary>
		/// <param name="name"></param>
		public void RemoveDataSource(string name)
		{
			NamedDataSource dataSource =
				_dataSources.Find(new NamedDataSourcePredicate(name).Predicate);//delegate(NamedDataSource ds) { return ds.Name == name; });

			if (dataSource == null)
				throw new ArgumentException(string.Format("Failed to find data source named '{0}'", name));

			RemoveDataSourceImpl(dataSource);

			dataSource.Instance.Disconnect();
		}

		/// <summary>
		/// Gets the names of catalog items
		/// </summary>
		[Browsable(false)]
		public IList<string> Names
		{
			get
			{
				if (_names == null)
				{
					Converter<NamedDataSource, string> NamedDataSourceConvertor = delegate(NamedDataSource ds)
                      	{
							return ds.Name;
                      	};
					_names = _dataSources.ConvertAll<string>(
						NamedDataSourceConvertor).AsReadOnly();
				}
				return _names;
			}
		}

		/// <summary>
		/// Gets list of data sources
		/// </summary>
		internal IEnumerable<NamedDataSource> DataSources
		{
			get { return _dataSources; }
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			// we might decide to not to disconnect in case it was connected already
			foreach (NamedDataSource ds in _dataSources)
			{
				ds.Instance.Disconnect();
			}

			_dataSources.Clear();
		}

		#endregion

		#region IDataSourceResolver Members

		public IDataSource Resolve(string name)
		{
			NamedDataSource dataSource =
				_dataSources.Find(new NamedDataSourcePredicate(name).Predicate);//delegate(NamedDataSource ds) { return ds.Name == name; });

			return dataSource == null ? null : dataSource.Instance;
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Loads all data sources from specific folder
		/// </summary>
		/// <param name="folderPath"></param>
		private void InitFromFolder(string folderPath)
		{
			if (Names.Count > 0)
				throw new InvalidOperationException("Initializing is not allowed when any datasource is specified");

			foreach (string fileName in Directory.GetFiles(folderPath, "*." + FileExtension))
			{
				try
				{
					NamedDataSource dataSource;
					LoadDataSource(fileName, out dataSource);

					if (dataSource == null || string.IsNullOrEmpty(dataSource.Name) || dataSource.Instance == null)
						throw new DataSourceConfigurationException("", "Incorrect file format");

					FixupRelativePaths(folderPath, dataSource.Instance);

					AddDataSourceImpl(dataSource);
					dataSource.Tag = fileName;
				}catch(ArgumentException)
				{}
			}

			_folder = folderPath;
		}

		private static void FixupRelativePaths(string rootPath, IDataSource instance)
		{
			if(instance == null || instance.IsConnected)
				throw new ArgumentException("Data source is not specified or has invalid state");

			LocalCubeDataSource lcds = instance as LocalCubeDataSource;
			if(lcds != null)
			{
				if (!Path.IsPathRooted(lcds.LocalCubeFile))
					lcds.LocalCubeFile = Path.Combine(rootPath, lcds.LocalCubeFile);
			}

			RdDataSource rdds = instance as RdDataSource;
			if(rdds != null)
			{
				rdds.ConnectionString = FixupRelatedPathInConnectionString(rootPath, rdds.ConnectionString);
				if (!Path.IsPathRooted(rdds.CustomSchemaFile))
					rdds.CustomSchemaFile = Path.Combine(rootPath, rdds.CustomSchemaFile);
			}

			XmlDataSource xmlds = instance as XmlDataSource;
			if (xmlds != null)
			{
				if (!Path.IsPathRooted(xmlds.CustomSchemaFile))
					xmlds.CustomSchemaFile = Path.Combine(rootPath, xmlds.CustomSchemaFile);
				if (!Path.IsPathRooted(xmlds.DataFile))
					xmlds.DataFile = Path.Combine(rootPath, xmlds.DataFile);
				if (!string.IsNullOrEmpty(xmlds.TransformationFile) &&
					!Path.IsPathRooted(xmlds.TransformationFile))
					xmlds.TransformationFile = Path.Combine(rootPath, xmlds.TransformationFile);
			}
		}

		static readonly Regex DataSourcePattern = new Regex(@"(?<=data\ssource=)[^;]+",
			RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);
		private static string FixupRelatedPathInConnectionString(string rootPath, string connectionString)
		{
			string dataSourcePath = DataSourcePattern.Match(connectionString).Value;
			if (!string.IsNullOrEmpty(dataSourcePath) && !Path.IsPathRooted(dataSourcePath))
			{
				connectionString = connectionString.Replace(
					dataSourcePath, Path.Combine(rootPath, dataSourcePath));
			}

			return connectionString;
		}

		private const string RootTag = "DataSourceFile";

		private static void LoadDataSource(string fileName, out NamedDataSource dataSource)
		{
			using (XmlReader reader = XmlReader.Create(fileName))
			{
				reader.MoveToContent();
				reader.ReadStartElement(RootTag);
				dataSource = new NamedDataSource(reader);
				reader.ReadEndElement();
			}
		}

		internal static void SaveDataSource(string fileName, NamedDataSource dataSource)
		{
			if (string.IsNullOrEmpty(fileName))
				throw new ArgumentNullException("fileName");
			if (dataSource == null)
				throw new ArgumentNullException("dataSource");

			using (XmlWriter writer = XmlWriter.Create(fileName))
			{
				writer.WriteStartElement(RootTag);
				dataSource.Write(writer);
				writer.WriteEndElement();
			}
			dataSource.Tag = fileName;
		}

		internal static string GenerateFileName(string folderPath, string dataSourceName)
		{
			string result;

			try
			{
				string fileName = dataSourceName.Replace("/", "").
					Replace(@"\", "").
					Replace("..", " ").
					Trim();

				StringBuilder validFileName = new StringBuilder();

				// remove all non-alphanumeric characters
				string validCharacters = " -=[].\"'{{}}~!@#$%^&()_+|";
				foreach (char c in fileName)
				{
					if (char.IsLetterOrDigit(c) || validCharacters.IndexOf(c) >= 0)
						validFileName.Append(c);
				}

				//if name doesn't contains alphanumeric characters
				if (validFileName.Length == 0)
					validFileName.Append("DataSource");

				int num = 0;
				do
				{
					num++;
					string postFix = num == 1 ? "" : num.ToString();
					result = Path.Combine(folderPath, validFileName + postFix + "." + FileExtension);
				} while (File.Exists(result));
			}
			catch
			{
				result = Guid.NewGuid().ToString();
			}
			return result;
		}

		internal void RemoveDataSourceImpl(NamedDataSource dataSource)
		{
			_dataSources.Remove(dataSource);
			_names = null;

			string fileName = (string)dataSource.Tag;
			if (!File.Exists(fileName))
				throw new InvalidOperationException(string.Format(
					"The file named '{0}' that keeps settings for '{1}' data source is not found",
					fileName, dataSource.Name));
			File.Delete(fileName);
		}

		internal void AddDataSourceImpl(NamedDataSource dataSource)
		{
			_dataSources.Add(dataSource);
			_names = null;

			if (!_initializing)
			{
				string fileName = GenerateFileName(_folder, dataSource.Name);
				SaveDataSource(fileName, dataSource);
			}
		}

		#endregion
	}

	/// <summary>
	/// Implements instance of data source associated with a name
	/// </summary>
	internal sealed class NamedDataSource
	{
		private const string TagName = "Name";
		private const string TagDataSource = "DataSource";

		private readonly string _name;
		private object _tag;
		private readonly IDataSource _instance;

		/// <summary>
		/// Gets the data source instance
		/// </summary>
		public IDataSource Instance
		{
			get { return _instance; }
		}

		/// <summary>
		/// Gets or sets user specific information related to this data source instance
		/// </summary>
		public object Tag
		{
			get { return _tag; }
			set { _tag = value; }
		}

		/// <summary>
		/// Gets the name
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		public NamedDataSource(XmlReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			_name = reader.ReadElementString(TagName);
			_instance = DataSourceFactory.ReadDataSource(reader, TagDataSource);
		}

		public NamedDataSource(string name, IDataSource instance)
		{
			if (instance == null)
				throw new ArgumentNullException("instance");
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");

			_name = name;
			_instance = instance;
		}

		public void Write(XmlWriter writer)
		{
			writer.WriteElementString(TagName, _name);
			DataSourceFactory.WriteDataSource(_instance, writer, TagDataSource);
		}
	}
}
