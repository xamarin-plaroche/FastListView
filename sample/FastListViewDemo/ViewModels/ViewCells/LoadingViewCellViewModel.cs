using System;
using FastListViewDemo.Views.ViewCells;
using Xamarin.Forms;

namespace FastListViewDemo.ViewModels.ViewCells
{
    public class LoadingViewCellViewModel : BaseCellViewModel
	{
		public LoadingViewCellViewModel()
		{
			ItemTemplate = new DataTemplate(typeof(LoadingViewCell));
		}
	}
}

