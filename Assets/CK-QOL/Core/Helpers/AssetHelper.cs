using System.Linq;
using UnityEngine;

namespace CK_QOL.Core.Helpers
{
	internal static class AssetHelper
	{
		internal static GameObject LoadAndInstantiatePrefab(string assetName, bool dontDestroyOnLoad = true)
		{
			var assets = Entry.AssetBundle.LoadAllAssets();
			
			var prefab = assets.First(a => a.name == assetName);
			var gameObject = Object.Instantiate(prefab);
			
			if (dontDestroyOnLoad)
			{
				Object.DontDestroyOnLoad(gameObject);
			}

			return gameObject as GameObject;
		}
	}
}