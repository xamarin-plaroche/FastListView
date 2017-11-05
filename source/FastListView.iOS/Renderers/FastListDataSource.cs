using System;
using System.Collections;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace FastListView.Renderers
{
	public class FastListDataSource : UICollectionViewSource
	{
		Forms.VisualElements.FastListView _fastListView;

		public FastListDataSource(Forms.VisualElements.FastListView fastListView)
		{
			this._fastListView = fastListView;
		}

		#region implemented abstract members of UICollectionViewDataSource

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return ((ICollection)this._fastListView.ItemsSource).Count;
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			//var item = this._fastListView.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
			//this.Element.InvokeItemSelectedEvent(this, item);
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = this._fastListView.ItemsSource.Cast<object>().ElementAt(indexPath.Row);

			var _viewCell = this._fastListView.CreateDefault(item);

			if (_viewCell != null)
			{
				_viewCell.BindingContext = item;
				_viewCell.Parent = this._fastListView;

				if (indexPath != null)
				{
					var collectionCell = collectionView.DequeueReusableCell(new NSString(FastListViewCell.Key), indexPath) as FastListViewCell;

					if (collectionCell == null)
					{
						return null;
					}

					_viewCell.Appearing += (sender, e) => {
						Console.WriteLine("Cell => Appearing !");
						_fastListView.SendCellAppearing((Cell)sender);
					};
					_viewCell.Disappearing += (sender, e) => {
						Console.WriteLine("Cell => Disappearing !");
						_fastListView.SendCellDisappearing((Cell)sender);
					};
					collectionCell.UpdateCell(_viewCell);
					return collectionCell;
				}
				else
				{
					return null;
				}
			}

			return null;
		}

		#endregion

		//ConcurrentDictionary<NSIndexPath, nfloat> _rowHeights = new ConcurrentDictionary<NSIndexPath, nfloat>();

		internal void CacheDefinedRowHeights()
		{
			//Task.Run(() =>
			//{
			//	var templatedItems = _fastListView.ItemsSource as IList<BaseCellViewModel>;

			//	foreach (var item in templatedItems)
			//	{
			//		//if (_disposed)
			//		//return;
			//		var viewCell = (BaseCellViewModel)item;
			//		var template = viewCell.ItemTemplate;
			//		if (template != null)
			//		{
			//			var templateInstance = template.CreateContent();
			//			// see if it's a view or a cell
			//			var templateView = templateInstance as View;
			//			var viewCellBinded = templateInstance as ViewCell;
			//			if (templateView == null && viewCellBinded == null)
			//				throw new InvalidOperationException("DataTemplate must be either a Cell or a View");
			//			if (templateView != null) // we got a view, wrap in a cell
			//				viewCellBinded = new ViewCell { View = templateView };
			//			viewCellBinded.BindingContext = item;
			//			viewCellBinded.Parent = _fastListView;
			//		}

			//		double? cellRenderHeight = cell?.RenderHeight;
			//		if (cellRenderHeight > 0)
			//			_rowHeights[cell.GetIndexPath()] = (nfloat)cellRenderHeight;
			//	}
			//});
		}
	}
}
