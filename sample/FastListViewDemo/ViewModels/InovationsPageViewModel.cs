using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using FastListViewDemo.Base;
using FastListViewDemo.Models;
using FastListViewDemo.ViewModels.ViewCells;
using Xamarin.Forms;

namespace FastListViewDemo.ViewModels
{
    public class InovationsPageViewModel : ViewModelBase
	{
		INavigation _nav;

		IList<BaseCellViewModel> _innovations;
		public IList<BaseCellViewModel> Innovations
		{
			get
			{
				return _innovations;
			}
			set
			{
				_innovations = value;
				OnPropertyChanged(nameof(Innovations));
			}
		}

		bool _isRefreshing;
		public bool IsRefreshing
		{
			get
			{
				return _isRefreshing;
			}
			set
			{
				_isRefreshing = value;
				OnPropertyChanged(nameof(IsRefreshing));
			}
		}

		public ICommand RefreshCommand { get; private set; }
		public ICommand LoadMoreCommand { get; private set; }

		public ICommand SelectedCommand { get; set; }

		public InovationsPageViewModel(INavigation nav)
		{
			_nav = nav;

			var list = new ObservableCollection<BaseCellViewModel>();
			list.Add(new LoadingViewCellViewModel());

			Device.BeginInvokeOnMainThread(() =>
			{
				Innovations = list;
			});


			LoadDataAsync().FireAndForgetSafeAsync();


			RefreshCommand = new Command(async () => { await RefreshAsync(); });

			LoadMoreCommand = new Command(async () => {
				var feeds = await LoadMoreAsync();
				if (feeds != null && feeds.Count > 0)
				{
					Innovations = feeds;
				}
			});

			SelectedCommand = new Command((obj) => {

				var vm = (BaseCellViewModel)obj;

				if (vm is InovationViewModel)
				{
					var inovationVM = (InovationViewModel)vm;
					if (!string.IsNullOrEmpty(inovationVM.Record.Fields.SiteInternet))
					{
						Device.OpenUri(new Uri(inovationVM.Record.Fields.SiteInternet));
					}
				}

				Debug.WriteLine("Selected!");
			});

		}

		async Task LoadDataAsync()
        {
			var feeds = await InovationsStore.Instance.GetRecordsAsync();
			var start = DateTime.Now;

			Device.BeginInvokeOnMainThread(() =>
			{
				Innovations = new ObservableCollection<BaseCellViewModel>(feeds);
				Innovations.Add(new LoadingViewCellViewModel());
			});

			var stop = DateTime.Now;
			Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
		}

		async Task<ObservableCollection<BaseCellViewModel>> LoadMoreAsync()
        {
			var list = new ObservableCollection<BaseCellViewModel>();
			var feeds = await InovationsStore.Instance.GetRecordsAsync("75013");
			list = new ObservableCollection<BaseCellViewModel>(Innovations);
			list.RemoveAt(list.Count - 1);
			foreach (var f in feeds)
			{
				list.Add(f);
			}
			list.Add(new LoadingViewCellViewModel());

			return list;
		}

		async Task RefreshAsync()
		{
			IsRefreshing = true;

			var feeds = await InovationsStore.Instance.GetRecordsAsync();

            var start = DateTime.Now;

            Innovations = new ObservableCollection<BaseCellViewModel>(feeds);
            Innovations.Add(new LoadingViewCellViewModel());

            var stop = DateTime.Now;
            Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());

        }
	}
}
