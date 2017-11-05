using System;
using CoreGraphics;
using FastListView.Forms.VisualElements;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FastListView.Renderers
{
	public class FastListViewCell : UICollectionViewCell
	{
		public const string Key = "FastListViewCell";

		ViewCell _viewCell;

		public ViewCell ViewCell
		{
			get { return _viewCell; }
			set
			{
				if (_viewCell == value)
					return;
				UpdateCell(value);
			}
		}

		[Export("initWithFrame:")]
		public FastListViewCell(CGRect frame) : base(frame)
		{
			Console.WriteLine("(initWithFrame) Frame {0}", frame);

			this.ContentView.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var contentFrame = ContentView.Frame;
			var view = ViewCell.View;

			Layout.LayoutChildIntoBoundingRegion(view, contentFrame.ToRectangle());

			if (_rendererRef == null)
				return;

			IVisualElementRenderer renderer;
			if (_rendererRef.TryGetTarget(out renderer))
				renderer.NativeView.Frame = view.Bounds.ToRectangleF();
		}

		WeakReference<IVisualElementRenderer> _rendererRef;

		IVisualElementRenderer GetNewRenderer()
		{
			if (_viewCell.View == null)
				throw new InvalidOperationException($"ViewCell must have a {nameof(_viewCell.View)}");

			var newRenderer = Platform.CreateRenderer(_viewCell.View);
			_rendererRef = new WeakReference<IVisualElementRenderer>(newRenderer);
			ContentView.AddSubview(newRenderer.NativeView);
			return newRenderer;
		}

		public void UpdateCell(ViewCell cell)
		{
			Console.WriteLine("(Update) Frame {0}", Frame);

			Device.BeginInvokeOnMainThread(() =>
			{
				var _cell = _viewCell as FastViewCell;
				_cell?.SendDisappearing();
			});

			if (cell != null)
			{
				this._viewCell = cell;
				_viewCell = cell;
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				var _cell = _viewCell as FastViewCell;
				_cell?.SendAppearing();
			});

			IVisualElementRenderer renderer;
			if (_rendererRef == null || !_rendererRef.TryGetTarget(out renderer))
				renderer = GetNewRenderer();
			else
				renderer.SetElement(_viewCell.View);

			Platform.SetRenderer(this._viewCell.View, renderer);
			SetNeedsLayout();
		}

		public override CGSize SizeThatFits(CGSize size)
		{
			IVisualElementRenderer renderer;
			if (!_rendererRef.TryGetTarget(out renderer))
				return base.SizeThatFits(size);

			if (renderer.Element == null)
				return CGSize.Empty;

			double width = size.Width;
			var height = size.Height > 0 ? size.Height : double.PositiveInfinity;
			var result = renderer.Element.Measure(width, height);

			var finalheight = (float)result.Request.Height;
			return new CGSize(size.Width, finalheight);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_rendererRef = null;
				_viewCell = null;
			}
			base.Dispose(disposing);
		}
	}}
