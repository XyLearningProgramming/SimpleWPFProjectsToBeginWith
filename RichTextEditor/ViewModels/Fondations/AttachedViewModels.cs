using System;
using System.Collections.Generic;
using System.Text;

namespace RichTextEditor.ViewModels
{
	public class AttachedViewModels: BaseViewModel
	{
		public RootViewModel Root { get; }

		protected AttachedViewModels(RootViewModel root)
		{
			Root = root;
		}

		internal virtual void OnApplicationLoaded() { }
		
	}
}
