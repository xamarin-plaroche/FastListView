using System;
using FastListView.Forms.Contracts;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace FLVDemo.ViewModels.ViewCells
{
	public class BaseCellViewModel : ViewModelBase, IFastViewCell
	{
		public DataTemplate ItemTemplate { get; set; }
	}
}
