using System;
using FastListView.Forms.Contracts;
using FastListViewDemo.Base;
using Xamarin.Forms;

namespace FastListViewDemo.ViewModels.ViewCells
{
    public class BaseCellViewModel : ViewModelBase, IFastViewCell
	{
		public DataTemplate ItemTemplate { get; set; }
	}
}
