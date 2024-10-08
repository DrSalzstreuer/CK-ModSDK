namespace CK_QOL.ConfigUI.Core
{
	/// <summary>
	///     Contains metadata and constant settings for the CK QOL mod, such as the mod's name, version, author, and
	///     contributors.
	///     This static class holds information about the mod itself, which is used throughout the mod's codebase.
	/// </summary>
	/// <remarks>
	///     This class is primarily used to provide mod information, including the mod's name, version, and author details.
	///     It serves as a centralized reference for basic mod information that can be used in logging, UI displays, and
	///     other areas where mod identification is needed.
	/// </remarks>
	internal static class ModSettings
	{
		/// <summary>
		///     The full name of the mod.
		/// </summary>
		internal const string Name = "CK QOL ConfigUI";

		/// <summary>
		///     The short name of the mod, typically used in logging or as a prefix.
		/// </summary>
		internal const string ShortName = "CK_QOL_ConfigUI";

		/// <summary>
		///     The current version of the mod.
		/// </summary>
		public const string Version = "1.0.2";

		/// <summary>
		///     The author of the mod.
		/// </summary>
		internal const string Author = "DrSalzstreuer";
		
		/// <summary>
		///     The contributors who have worked on or helped with the mod.
		/// </summary>
		internal const string SpecialThanks = "Limoka & I Use Mods, So Sue Me";
	}
}