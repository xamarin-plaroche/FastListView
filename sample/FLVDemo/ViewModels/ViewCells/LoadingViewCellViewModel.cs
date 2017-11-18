using System;
using FLVDemo.Views.ViewCells;
using Xamarin.Forms;

namespace FLVDemo.ViewModels.ViewCells
{
	public class LoadingViewCellViewModel : BaseCellViewModel
	{
		public LoadingViewCellViewModel()
		{
			ItemTemplate = new DataTemplate(typeof(LoadingViewCell));
		}
	}
}
