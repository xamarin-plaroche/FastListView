using System;
using System.Collections.Generic;
using FastListViewDemo.Base;
using Xamarin.Forms;

namespace FastListViewDemo.Views
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
