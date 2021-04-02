using System;
using System.Collections.Generic;
using FastListViewDemo.Base;
using Xamarin.Forms;

namespace FastListViewDemo.Views
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
