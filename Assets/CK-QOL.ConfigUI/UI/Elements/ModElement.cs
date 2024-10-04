using System.Collections.Generic;
using UnityEngine;

namespace CK_QOL.ConfigUI.UI.Elements
{
	public class ModElement : MonoBehaviour
	{
		[Header("UI Elements")]
		public GameObject nameElement;
		public GameObject sectionContainerElement;
		
		internal List<SectionElement> SectionElements = new List<SectionElement>();
	}
}