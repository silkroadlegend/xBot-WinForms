﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace xBot.Game
{
	/// <summary>
	/// Keep tracking of everything about the Silkroad actually selected.
	/// </summary>
	public class Info
	{
		private static Info _this = null;
		/// <summary>
		/// Unique name from the Silkroad.
		/// </summary>
		public string Silkroad {
			get {
				if (db != null)
					return db.Name;
				return "";
			}
		}
		/// <summary>
		/// Server name.
		/// </summary>
		public string Server { get; set; }
		/// <summary>
		/// Character name. Will be available just right before the character is selected.
		/// </summary>
		public string Charname { get; set; }
		/// <summary>
		/// Silkroad Locale
		/// </summary>
		public byte Locale { get; set; }
		/// <summary>
		/// Silkroad Version
		/// </summary>
		public uint Version { get; set; }
		/// <summary>
		/// Database required to parsing packets & stuffs.
		/// </summary>
		public Database Database { get { return db; } }
		private Database db;
		/// <summary>
		/// Reference to the selected character for playing.
		/// </summary>
		public SRObject Character { get; set; }
		/// <summary>
		/// All characters displayed on character selection.
		/// </summary>
		public List<SRObject> CharacterList { get; }
		/// <summary>
		/// Track any entity that spawn closer.
		/// </summary>
		public List<SRObject> EntityList { get; }
		/// <summary>
		/// Graphic reference used to display the moon.
		/// </summary>
		public ushort Moonphase { get; set; }
		/// <summary>
		/// Graphic reference to display day/night times.
		/// </summary>
		public byte Hour { get; set; }
		/// <summary>
		/// Graphic reference to display day/night times.
		/// </summary>
		public byte Minute { get; set; }
		private Info()
		{
			Character = null;
			CharacterList = new List<SRObject>();
			EntityList = new List<SRObject>();
		}
		/// <summary>
		/// GetInstance. Secures an unique class creation for being used anywhere at the project.
		/// </summary>
		public static Info Get
		{
			get
			{
				if (_this == null)
					_this = new Info();
				return _this;
			}
		}
		/// <summary>
		/// Select the database if exists. Return success.
		/// </summary>
		/// <param name="name">Database unique name</param>
		/// <returns></returns>
		public bool SelectDatabase(string name)
		{
			if (Database.Exists(name))
			{
				db = new Database(name);
				db.Connect();
				return true;
			}
			return false;
		}
		/// <summary>
		/// Gets the maximum exp required for the level specified.
		/// </summary>
		public ulong getMaxExp(byte level)
		{
			string sql = "SELECT exp FROM levels WHERE level=" + level;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if(result.Count > 0)
				return ulong.Parse(result[0]["exp"]);
			return 0;
		}
		/// <summary>
		/// Get model by id, using the current database loaded.
		/// </summary>
		public NameValueCollection getModel(uint id)
		{
			string sql = "SELECT * FROM models WHERE id=" + id;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if (result.Count > 0)
				return result[0];
			return null;
    }
		/// <summary>
		/// Get teleportlink by id, using the current database loaded.
		/// </summary>
		public NameValueCollection getTeleport(uint id)
		{
			string sql = "SELECT * FROM teleportbuildings WHERE id=" + id;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if (result.Count > 0)
				return result[0];
			return null;
		}
		/// <summary>
		/// Get teleportlink by id, using the current database loaded.
		/// </summary>
		public NameValueCollection getTeleportLink(uint id)
		{
			string sql = "SELECT * FROM teleportlinks WHERE id=" + id;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if (result.Count > 0)
				return result[0];
			return null;
		}
		/// <summary>
		/// Get item by id, using the current database loaded.
		/// </summary>
		public NameValueCollection getItem(uint id)
		{
			string sql = "SELECT * FROM items WHERE id=" + id;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if (result.Count > 0)
				return result[0];
			return null;
		}
		/// <summary>
		/// Get skill by id, using the current database loaded.
		/// </summary>
		public NameValueCollection getSkill(uint id)
		{
			string sql = "SELECT * FROM skills WHERE id=" + id;
			Database.ExecuteQuery(sql);
			List<NameValueCollection> result = Database.getResult();
			if (result.Count > 0)
				return result[0];
			return null;
		}
		/// <summary>
		/// Get an entity by his unique ID.
		/// </summary>
		/// <param name="uniqueid">Spawn object reference</param>
		/// <returns><see cref="null"/> if cannot be found</returns>
		public SRObject getEntity(uint uniqueid)
		{
			if ((uint)Character[SRAttribute.UniqueID] == uniqueid)
				return Character;
			return EntityList.Find(spawn => ((uint)spawn[SRAttribute.UniqueID] == uniqueid));
		}
	}
}
