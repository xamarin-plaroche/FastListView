using System;
using System.Windows.Input;
using FLVDemo.Views;
using GalaSoft.MvvmLight;
using Xamarin.Forms;

namespace FLVDemo.ViewModels
{
	public class MainPageViewModel : ViewModelBase
	{
		INavigation _nav;

		public ICommand LoadCommand { get; set; }

		public MainPageViewModel(INavigation nav)
		{
			_nav = nav;

			LoadCommand = new Command(async () => {
				await _nav.PushAsync(new InovationsPage(new InovationsPageViewModel(_nav)));
			});
		}
	}
}
