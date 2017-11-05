using System;
using System.Collections.Concurrent;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FastListView.Renderers
{
	public class FastListViewDataDelegateFlowLayout : UICollectionViewDelegateFlowLayout
	{

		ConcurrentDictionary<NSIndexPath, nfloat> _rowHeights = new ConcurrentDictionary<NSIndexPath, nfloat>();

		Forms.VisualElements.FastListView _fastListView;

		public FastListViewDataDelegateFlowLayout(Forms.VisualElements.FastListView fastListView)
		{
			this._fastListView = fastListView;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = this._fastListView.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
			if (_fastListView.SelectedCommand != null && _fastListView.SelectedCommand.CanExecute(item))
			{
				_fastListView.SelectedCommand.Execute(item);
			}
		}

		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
		{
		}

		public void ClearRowsHeight()
		{
			_rowHeights.Clear();
		}

		public void ClearRowHeight(NSIndexPath indexpath)
		{
			_rowHeights[indexpath] = 0;
		}

		public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			nfloat specifiedRowHeight;

			// -- Gestion du Cache
			if (_rowHeights.TryGetValue(indexPath, out specifiedRowHeight) && specifiedRowHeight > 0)
			{
				return new CGSize(collectionView.Frame.Width, specifiedRowHeight);
			}
			else
			{
				var item = this._fastListView.ItemsSource.Cast<object>().ElementAt(indexPath.Row);

				var _viewCellBinded = this._fastListView.CreateDefault(item);

				if (_viewCellBinded != null)
				{
					_viewCellBinded.BindingContext = item;
					_viewCellBinded.Parent = this._fastListView;
					var height = this.CalculateHeightForCell(collectionView, _viewCellBinded);
					//Console.WriteLine("Height : {0}", height);
					_rowHeights[indexPath] = (nfloat)height;
					return new CGSize(collectionView.Frame.Width, height);
				}

				return new CGSize(1, 1);
			}
		}

		IVisualElementRenderer _renderer = null;

		public nfloat CalculateHeightForCell(UICollectionView collectionView, ViewCell cell)
		{
			var target = cell.View;

			if (_renderer == null)
				_renderer = Platform.CreateRenderer(target);
			else
				_renderer.SetElement(target);

			Platform.SetRenderer(target, _renderer);

			var req = target.Measure(collectionView.Frame.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
			return (nfloat)req.Request.Height;
		}

	}}
