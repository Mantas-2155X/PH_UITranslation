using System;

namespace UnityEngine.UI.Translation
{
	internal interface ISubtitle
	{
		string this[TextPosition anchor]
		{
			get;
		}
	}
}
