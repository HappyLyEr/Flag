using System;
using System.Xml;
using DataDynamics.Analysis.DataSources;
//using DataDynamics.Analysis.Viewer;

using DataDynamics.Analysis.Web;
using DataDynamics.Analysis;


namespace GASystem.GAGUI.GAControls.Analysis
{
	/// <summary>
	/// The implementation helpers.
	/// </summary>
	public static class SamplesImpl
	{
		/// <summary>
		/// Loads the layout from the specified file.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="pivotView"></param>
		public static void LoadLayout(XmlNode node, PivotView pivotView)
		{
			if(node == null)
				throw new ArgumentNullException("node");
			if(pivotView == null)
				throw new ArgumentNullException("pivotView");

			XmlNode dataSourceNode = node.SelectSingleNode("DataSource");
			if(dataSourceNode == null)
				throw new InvalidOperationException("DataSource node is expected");

			// backward compatibility mode ON
			XmlNode sdsNode = dataSourceNode.SelectSingleNode("//SharedDataSourceName");
			if (sdsNode != null)
			{
				XmlDocument document = dataSourceNode.OwnerDocument;

				XmlNode dataSourceType = dataSourceNode.AppendChild(
					document.CreateNode(XmlNodeType.Element, "DataSourceType", string.Empty));
				dataSourceType.InnerText = "SharedLC";

				XmlNode settingsNode = dataSourceNode.AppendChild(document.CreateNode(XmlNodeType.Element, "Settings", string.Empty));

				settingsNode
					.AppendChild(document.CreateNode(XmlNodeType.Element, "Name", string.Empty))
					.InnerText = sdsNode.InnerText;

				XmlNode cubeNameNode = dataSourceNode.SelectSingleNode("//CubeName");
				if (cubeNameNode != null)
				{
					settingsNode
						.AppendChild(document.CreateNode(XmlNodeType.Element, "Perspective", string.Empty))
						.InnerText = cubeNameNode.InnerText;
					dataSourceNode.RemoveChild(cubeNameNode);
					dataSourceType.InnerText = "SharedMP";
				}

				dataSourceNode.RemoveChild(sdsNode);
			}

			SharedDataSourceBase.RegisterSharedInFactory();
			IDataSource dataSource = DataSourceFactory.ReadDataSource(
				new XmlNodeReader(node.SelectSingleNode("DataSource")), "DataSource");

			if (dataSource == null)
				throw new DataSourceException("Failed to read data source settings");

			SharedDataSourceBase dataSourceAsShared = dataSource as SharedDataSourceBase;
			if (dataSourceAsShared == null)
			{
				throw new InvalidOperationException("Expected shared data source");
			}

			if (pivotView.DataSource != null)
				pivotView.DataSource.Disconnect();
			pivotView.DataSource = dataSourceAsShared;
			pivotView.DataSource.Connect();

			using (XmlNodeReader reader = new XmlNodeReader(node))
			{
				pivotView.Read(reader, PersistSettings.DataSource | PersistSettings.Layout | PersistSettings.CardLayout);
			}
		}
	}
}
