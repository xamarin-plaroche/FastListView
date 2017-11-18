using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace FLVDemo.Views
{
	public partial class MainPage : ContentPage
	{
		public MainPage(ViewModelBase vm)
		{
			Title = "FLV Demo";
			InitializeComponent();
			BindingContext = vm;
		}
	}
}
