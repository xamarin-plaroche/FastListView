using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FLVDemo.Extensions;
using FLVDemo.Models;
using FLVDemo.ViewModels.ViewCells;
using GalaSoft.MvvmLight;
using ReactiveUI;
using Xamarin.Forms;

namespace FLVDemo.ViewModels
{
	public class InovationsPageViewModel : ViewModelBase
	{
		INavigation _nav;

		ObservableCollection<BaseCellViewModel> _innovations;
		public ObservableCollection<BaseCellViewModel> Innovations
		{
			get
			{
				return _innovations;
			}
			set
			{
				Set(() => Innovations, ref _innovations, value);
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
				Set(() => IsRefreshing, ref _isRefreshing, value);
			}
		}

		public ReactiveCommand RefreshCommand { get; private set; }
		public ReactiveCommand<string, ObservableCollection<BaseCellViewModel>> LoadMoreCommand { get; private set; }

		public InovationsPageViewModel(INavigation nav)
		{
			_nav = nav;

			var list = new ObservableCollection<BaseCellViewModel>();
			list.Add(new LoadingViewCellViewModel());

			Device.BeginInvokeOnMainThread(() =>
			{
				Innovations = list;
			});

			Observable.FromAsync<IList<InovationViewModel>>(async () => await InovationsStore.Instance.GetRecordsAsync())
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(feeds =>
				{
					var start = DateTime.Now;

					Innovations = new ObservableCollection<BaseCellViewModel>(feeds);
					Innovations.Add(new LoadingViewCellViewModel());

					var stop = DateTime.Now;
					Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
				});

			RefreshCommand = ReactiveCommand.Create(() => RefreshAction());

			LoadMoreCommand = ReactiveCommand.CreateFromTask<string, ObservableCollection<BaseCellViewModel>>(
				async _ =>
				{
					var feeds = await InovationsStore.Instance.GetRecordsAsync("75013");
					list = new ObservableCollection<BaseCellViewModel>(Innovations);
					list.RemoveAt(list.Count - 1);
					foreach (var f in feeds)
					{
						list.Add(f);
					}
					list.Add(new LoadingViewCellViewModel());

					return list;
				});

			LoadMoreCommand
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(feeds =>
				{
					if (feeds != null && feeds.Count > 0)
					{
						Innovations = feeds;
					}
				});

		}

		void RefreshAction()
		{
			IsRefreshing = true;

			Observable.FromAsync<IList<InovationViewModel>>(async () => await InovationsStore.Instance.GetRecordsAsync())
				.ObserveOn(RxApp.MainThreadScheduler)
				.Subscribe(feeds =>
				{
					var start = DateTime.Now;

					Innovations = new ObservableCollection<BaseCellViewModel>(feeds);
					Innovations.Add(new LoadingViewCellViewModel());

					var stop = DateTime.Now;
					Debug.WriteLine("Diff : " + (stop - start).TotalMilliseconds.ToString());
				});
		}
	}
}
