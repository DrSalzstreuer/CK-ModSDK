using UnityEngine;

namespace CK_QOL.Core.Helpers
{
	public static class TransformHelpers
	{
		/// <summary>
		/// Recursively searches for a child Transform by name.
		/// </summary>
		/// <param name="parent">The parent Transform to start the search from.</param>
		/// <param name="childName">The name of the child Transform to find.</param>
		/// <returns>The Transform of the child if found; otherwise, null.</returns>
		public static Transform FindChildRecursive(Transform parent, string childName)
		{
			foreach (Transform child in parent)
			{
				if (child.name == childName)
				{
					return child;
				}

				var found = FindChildRecursive(child, childName);
				if (found != null)
				{
					return found;
				}
			}

			return null;
		}
	}
}