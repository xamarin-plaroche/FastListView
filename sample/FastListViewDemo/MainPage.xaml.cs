using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastListViewDemo.Base;
using Xamarin.Forms;

namespace FastListViewDemo
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
