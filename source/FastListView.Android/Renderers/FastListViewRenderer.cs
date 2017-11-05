using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using Android.Support.V7.Widget;
using FastListView.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FastListView.Forms.VisualElements.FastListView), typeof(FastListViewRenderer))]
namespace FastListView.Renderers
{
	public class FastListViewRenderer : ViewRenderer<Forms.VisualElements.FastListView, RecyclerView>
	{
		public FastListViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Forms.VisualElements.FastListView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var itemsSource = e.OldElement.ItemsSource as INotifyCollectionChanged;
				if (itemsSource != null)
				{
					itemsSource.CollectionChanged -= ItemsSourceOnCollectionChanged;
				}
			}

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var recyclerView = new RecyclerView(Context);
					recyclerView.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
					SetNativeControl(recyclerView);

					var linearLayout = new LinearLayoutManager(Context, OrientationHelper.Vertical, false);

					recyclerView.SetLayoutManager(linearLayout);

					UpdateAdapter();
				}

				var itemsSource = e.NewElement.ItemsSource as INotifyCollectionChanged;
				if (itemsSource != null)
				{
					itemsSource.CollectionChanged += ItemsSourceOnCollectionChanged;
				}
				Control.GetAdapter().NotifyDataSetChanged();

				Control.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
				Control.HasFixedSize = true;

				Control.SetClipToPadding(false);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Element.ItemsSource))
			{
				UpdateAdapter();
			}
		}

		private void UpdateAdapter()
		{
			var newItemsSource = this.Element.ItemsSource as INotifyCollectionChanged;
			if (newItemsSource != null)
			{
				newItemsSource.CollectionChanged += ItemsSourceOnCollectionChanged;
			}
			var adapter = new RecyclerViewAdapter(Element) { HasStableIds = true };
			Control.SetAdapter(adapter);
		}

		private void ItemsSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			var adapter = Control.GetAdapter();
			Console.WriteLine("Action : {0}", e.Action);
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					adapter.NotifyItemRangeInserted(
						positionStart: e.NewStartingIndex,
						itemCount: e.NewItems.Count
					);
					break;
				case NotifyCollectionChangedAction.Remove:
					if (((IList)Element.ItemsSource).Count == 0)
					{
						adapter.NotifyDataSetChanged();
						return;
					}
					((RecyclerViewAdapter)adapter).ClearRowHeight(e.OldStartingIndex);
					adapter.NotifyItemRangeRemoved(
						positionStart: e.OldStartingIndex,
						itemCount: e.OldItems.Count
					);
					break;
				case NotifyCollectionChangedAction.Replace:
					((RecyclerViewAdapter)adapter).ClearRowHeight(e.OldStartingIndex);
					adapter.NotifyItemRangeChanged(
						positionStart: e.OldStartingIndex,
						itemCount: e.OldItems.Count
					);
					break;
				case NotifyCollectionChangedAction.Move:
					for (var i = 0; i < e.NewItems.Count; i++)
						adapter.NotifyItemMoved(
							fromPosition: e.OldStartingIndex + i,
							toPosition: e.NewStartingIndex + i
						);
					break;
				case NotifyCollectionChangedAction.Reset:
					adapter.NotifyDataSetChanged();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
