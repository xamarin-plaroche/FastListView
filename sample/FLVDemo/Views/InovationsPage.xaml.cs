using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace FLVDemo.Views
{
	public partial class InovationsPage : ContentPage
	{
		public InovationsPage(ViewModelBase vm)
		{
			Title = "Inovations";
			InitializeComponent();
			BindingContext = vm;
		}
	}
}
