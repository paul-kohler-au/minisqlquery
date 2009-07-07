using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace MiniSqlQuery.Core
{
	/// <summary>
	/// Manages a list of database connections.
	/// </summary>
	[Serializable]
	public class DbConnectionDefinitionList
	{
		// store internally as a list
		private List<DbConnectionDefinition> _definitions;

		/// <summary>
		/// Initializes a new instance of the <see cref="DbConnectionDefinitionList"/> class.
		/// </summary>
		public DbConnectionDefinitionList()
		{
			_definitions = new List<DbConnectionDefinition>();
		}

		/// <summary>
		/// Gets or sets the connection definitions.
		/// </summary>
		/// <value>The definitions.</value>
		public DbConnectionDefinition[] Definitions
		{
			get { return _definitions.ToArray(); }
			set
			{
				_definitions.Clear();
				_definitions.AddRange(value);
			}
		}

		/// <summary>
		/// Gets or sets the default name of a connection definition from the list of <see cref="Definitions"/>.
		/// </summary>
		/// <value>The default name.</value>
		public string DefaultName { get; set; }

		/// <summary>
		/// Creates a <see cref="DbConnectionDefinitionList"/> from a string of <paramref name="xml"/>.
		/// </summary>
		/// <param name="xml">The XML.</param>
		/// <returns>An instance of <see cref="DbConnectionDefinitionList"/>.</returns>
		public static DbConnectionDefinitionList FromXml(string xml)
		{
			using (StringReader sr = new StringReader(xml))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(DbConnectionDefinitionList));
				return (DbConnectionDefinitionList) serializer.Deserialize(sr);
			}
		}

		/// <summary>
		/// Serialize the list to XML.
		/// </summary>
		/// <returns>An XML string.</returns>
		public string ToXml()
		{
			return Utility.ToXml(this);
		}

		/// <summary>
		/// Upgrades the old definitions to a <see cref="DbConnectionDefinitionList"/>.
		/// </summary>
		/// <param name="oldDefinitions">The old definitions.</param>
		/// <param name="defaultName">The default name to use.</param>
		/// <returns>An instance of <see cref="DbConnectionDefinitionList"/>.</returns>
		public static DbConnectionDefinitionList Upgrade(ConnectionDefinition[] oldDefinitions, string defaultName)
		{
			DbConnectionDefinitionList definitionList = new DbConnectionDefinitionList();
			List<DbConnectionDefinition> newDefList = new List<DbConnectionDefinition>();

			definitionList.DefaultName = defaultName;
			foreach (ConnectionDefinition oldConDef in oldDefinitions)
			{
				DbConnectionDefinition dbConnectionDefinition = new DbConnectionDefinition
				                                                {
				                                                	ConnectionString = oldConDef.ConnectionString,
				                                                	Name = oldConDef.Name,
				                                                	ProviderName = oldConDef.ProviderName,
				                                                };
				newDefList.Add(dbConnectionDefinition);
			}
			definitionList.Definitions = newDefList.ToArray();

			if (definitionList.DefaultName == null && definitionList.Definitions.Length > 0)
			{
				definitionList.DefaultName = definitionList.Definitions[0].Name;
			}

			return definitionList;
		}

		/// <summary>
		/// Adds the definition from the list.
		/// </summary>
		/// <param name="connectionDefinition">The connection definition.</param>
		public void AddDefinition(DbConnectionDefinition connectionDefinition)
		{
			_definitions.Add(connectionDefinition);
		}

		/// <summary>
		/// Removes the definition from the list.
		/// </summary>
		/// <param name="connectionDefinition">The connection definition.</param>
		/// <returns>True if the item was removed.</returns>
		public bool RemoveDefinition(DbConnectionDefinition connectionDefinition)
		{
			return _definitions.Remove(connectionDefinition);
		}
	}
}