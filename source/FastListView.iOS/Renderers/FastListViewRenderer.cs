using System;
using System.Collections;
using System.Collections.Specialized;
using FastListView.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FastListView.Forms.VisualElements.FastListView), typeof(FastListViewRenderer))]
namespace FastListView.Renderers
{
	public class FastListViewRenderer : ViewRenderer<Forms.VisualElements.FastListView, UICollectionView>
	{
		private FastListDataSource _dataSource;
		private FastListViewDataDelegateFlowLayout _dataDelegateFlowLayout;

		private FastListCollectionView _collectionView;

		private UICollectionViewSource DataSource
		{
			get
			{
				return _dataSource ?? (_dataSource = new FastListDataSource(Element));
			}
		}

		private UICollectionViewDelegateFlowLayout DataDelegateFlowLayout
		{
			get
			{
				return _dataDelegateFlowLayout ?? (_dataDelegateFlowLayout = new FastListViewDataDelegateFlowLayout(Element));
			}
		}

		public FastListViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Forms.VisualElements.FastListView> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null)
			{
				Unbind(e.OldElement);
			}
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					_collectionView = new FastListCollectionView(Element)
					{
						AllowsMultipleSelection = false,
						//SelectionEnable = e.NewElement.SelectionEnabled,
						BackgroundColor = this.Element.BackgroundColor.ToUIColor(),
						AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight,
					};

					_collectionView?.SizeToFit();

					Bind(e.NewElement);

					_collectionView.Source = this.DataSource;
					_collectionView.Delegate = this.DataDelegateFlowLayout;

					if (e.NewElement.IsPullToRefreshEnabled && e.NewElement.RefreshCommand != null)
					{
						var refreshCtrl = new UIRefreshControl();
						refreshCtrl.ValueChanged += (sender, _e) =>
						{
							Element.IsRefreshing = true; // -- OK

							if (e.NewElement.RefreshCommand.CanExecute(null))
							{
								e.NewElement.RefreshCommand.Execute(null);
							}

							Element.IsRefreshing = false; // -- ??

							refreshCtrl.EndRefreshing();
						};
						_collectionView.RefreshControl = refreshCtrl;
					}

					SetNativeControl(_collectionView);
				}
			}
		}

		private void Unbind(Forms.VisualElements.FastListView oldElement)
		{
			if (oldElement == null) return;

			oldElement.PropertyChanging -= this.ElementPropertyChanging;
			oldElement.PropertyChanged -= this.ElementPropertyChanged;

			var itemsSource = oldElement.ItemsSource as INotifyCollectionChanged;
			if (itemsSource != null)
			{
				itemsSource.CollectionChanged -= this.DataCollectionChanged;
			}
		}

		private void Bind(Forms.VisualElements.FastListView newElement)
		{
			if (newElement == null) return;

			newElement.PropertyChanging += this.ElementPropertyChanging;
			newElement.PropertyChanged += this.ElementPropertyChanged;

			var source = newElement.ItemsSource as INotifyCollectionChanged;
			if (source != null)
			{
				source.CollectionChanged += this.DataCollectionChanged;
			}
		}

		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Forms.VisualElements.FastListView.ItemsSourceProperty.PropertyName)
			{
				var newItemsSource = this.Element.ItemsSource as INotifyCollectionChanged;
				if (newItemsSource != null)
				{
					newItemsSource.CollectionChanged += DataCollectionChanged;
					this.Control.ReloadData();
				}
			}
		}

		private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
		{
			if (e.PropertyName == "ItemsSource")
			{
				var oldItemsSource = this.Element.ItemsSource as INotifyCollectionChanged;
				if (oldItemsSource != null)
				{
					oldItemsSource.CollectionChanged -= DataCollectionChanged;
				}
			}
		}

		//static int nbItems = 0;

		private void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			//InvokeOnMainThread(() =>
			//{
			try
			{
				if (this.Control == null)
					return;

				Console.WriteLine("Nb items in DataSource : {0}", ((ICollection)Element.ItemsSource).Count);

				//if (((ICollection)Element.ItemsSource).Count > 0)
				//{
				//	nbItems = ((ICollection)Element.ItemsSource).Count;
				//}

				// TODO: try to handle add or remove operations gracefully, just reload the whole collection for other changes
				// InsertItems, DeleteItems or ReloadItems can cause
				// *** Assertion failure in -[XLabs_Forms_Controls_GridCollectionView _endItemAnimationsWithInvalidationContext:tentativelyForReordering:],
				// BuildRoot/Library/Caches/com.apple.xbs/Sources/UIKit_Sim/UIKit-3512.30.14/UICollectionView.m:4324

				//var indexes = new List<NSIndexPath>();
				//switch (e.Action) {
				//    case NotifyCollectionChangedAction.Add:
				//        for (int i = 0; i < e.NewItems.Count; i++) {
				//            indexes.Add(NSIndexPath.FromRowSection((nint)(e.NewStartingIndex + i),0));
				//        }
				//        this.Control.InsertItems(indexes.ToArray());
				//        break;
				//    case NotifyCollectionChangedAction.Remove:
				//        for (int i = 0; i< e.OldItems.Count; i++) {
				//            indexes.Add(NSIndexPath.FromRowSection((nint)(e.OldStartingIndex + i),0));
				//        }
				//        this.Control.DeleteItems(indexes.ToArray());
				//        break;
				//    default:
				//        this.Control.ReloadData();
				//        break;
				//}
				Console.WriteLine("Action : {0}", e.Action);
				switch (e.Action)
				{
					case NotifyCollectionChangedAction.Add:

						//UpdateEstimatedRowHeight();
						if (e.NewStartingIndex == -1 /* || groupReset */)
							goto case NotifyCollectionChangedAction.Reset;

						Console.WriteLine("IsSynchronized : {0}", ((ICollection)Element.ItemsSource).IsSynchronized);
						//if (((ICollection)Element.ItemsSource).IsSynchronized)
						{
							Control.PerformBatchUpdatesAsync(() =>
							{
								Control.InsertItems(GetPaths(0, e.NewStartingIndex, e.NewItems.Count));
								//Control.SetNeedsLayout();
							});
						}
						break;

					case NotifyCollectionChangedAction.Remove:
						if (e.OldStartingIndex == -1 /* || groupReset */)
							goto case NotifyCollectionChangedAction.Reset;

						this._dataDelegateFlowLayout.ClearRowHeight(NSIndexPath.FromRowSection(e.OldStartingIndex, 0));

						Control.PerformBatchUpdatesAsync(() =>
						{
							Control.DeleteItems(GetPaths(0, e.OldStartingIndex, e.OldItems.Count));
							//Control.SetNeedsLayout();
						});
						break;

					case NotifyCollectionChangedAction.Move:
						//if (e.OldStartingIndex == -1 || e.NewStartingIndex == -1 || groupReset)
						//	goto case NotifyCollectionChangedAction.Reset;
						//Control.BeginUpdates();
						//for (var i = 0; i < e.OldItems.Count; i++)
						//{
						//	var oldi = e.OldStartingIndex;
						//	var newi = e.NewStartingIndex;

						//	if (e.NewStartingIndex < e.OldStartingIndex)
						//	{
						//		oldi += i;
						//		newi += i;
						//	}

						//	Control.MoveRow(NSIndexPath.FromRowSection(oldi, section), NSIndexPath.FromRowSection(newi, section));
						//}
						//Control.EndUpdates();

						//if (_estimatedRowHeight && e.OldStartingIndex == 0)
						//_estimatedRowHeight = false;

						break;

					case NotifyCollectionChangedAction.Replace:
						if (e.OldStartingIndex == -1 /* || groupReset */)
							goto case NotifyCollectionChangedAction.Reset;

						this._dataDelegateFlowLayout.ClearRowHeight(NSIndexPath.FromRowSection(e.OldStartingIndex, 0));

						Control.PerformBatchUpdatesAsync(() =>
						{
							Control.ReloadItems(GetPaths(0, e.OldStartingIndex, e.OldItems.Count));
						});
						break;

					case NotifyCollectionChangedAction.Reset:
						var nb = Control.NumberOfItemsInSection(0);
						Console.WriteLine("NumberOfItemsInSection : {0}", nb);
						Control.ReloadData();
						return;
				}
			}
			catch { } // todo: determine why we are hiding a possible exception here
					  //});
		}

		NSIndexPath[] GetPaths(int section, int index, int count)
		{
			var paths = new NSIndexPath[count];
			for (var i = 0; i < paths.Length; i++)
				paths[i] = NSIndexPath.FromRowSection(index + i, section);

			return paths;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing && _dataSource != null)
			{
				Unbind(Element);
				_dataSource.Dispose();
				_dataSource = null;
			}
		}
	}
}
