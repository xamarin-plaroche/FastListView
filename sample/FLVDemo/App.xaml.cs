using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FLVDemo.ViewModels;
using FLVDemo.Views;
using Xamarin.Forms;

namespace FLVDemo
{
	public partial class App : Application, INavigation
	{
		public static string OpenDataUrl = "https://opendata.paris.fr/";
		public static string OpenDataImageUrl = "https://opendata.paris.fr/explore/dataset/arc_innovation/files/";

		public static Size ScreenSize { get; set; }

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new MainPage(new MainPageViewModel(this)));
		}

		public IReadOnlyList<Page> ModalStack => throw new NotImplementedException();

		public IReadOnlyList<Page> NavigationStack => throw new NotImplementedException();

		public void InsertPageBefore(Page page, Page before)
		{
			throw new NotImplementedException();
		}

		public Task<Page> PopAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Page> PopAsync(bool animated)
		{
			throw new NotImplementedException();
		}

		public Task<Page> PopModalAsync()
		{
			throw new NotImplementedException();
		}

		public Task<Page> PopModalAsync(bool animated)
		{
			throw new NotImplementedException();
		}

		public Task PopToRootAsync()
		{
			throw new NotImplementedException();
		}

		public Task PopToRootAsync(bool animated)
		{
			throw new NotImplementedException();
		}

		public async Task PushAsync(Page page)
		{
			await MainPage.Navigation.PushAsync(page);
		}

		public async Task PushAsync(Page page, bool animated)
		{
			await MainPage.Navigation.PushAsync(page, animated);
		}

		public Task PushModalAsync(Page page)
		{
			throw new NotImplementedException();
		}

		public Task PushModalAsync(Page page, bool animated)
		{
			throw new NotImplementedException();
		}

		public void RemovePage(Page page)
		{
			throw new NotImplementedException();
		}
	}
}
