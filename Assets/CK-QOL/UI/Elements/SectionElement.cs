using System.Collections.Generic;
using UnityEngine;

namespace CK_QOL.UI.Elements
{
	public class SectionElement : MonoBehaviour
	{
		[Header("UI Elements")]
		public GameObject nameElement;
		public GameObject configContainerElement;
		
		internal List<ConfigElement> ConfigElements = new List<ConfigElement>();
	}
}