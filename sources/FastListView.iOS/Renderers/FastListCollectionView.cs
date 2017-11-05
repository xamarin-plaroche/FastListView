using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FastListView.Renderers
{
	public class FastListCollectionView : UICollectionView
	{
		public FastListCollectionView(Forms.VisualElements.FastListView fastListView) : base(default(CGRect), new FastListViewFlowLayout())
		{
			AutoresizingMask = UIViewAutoresizing.All;
			ContentMode = UIViewContentMode.ScaleToFill;
			TranslatesAutoresizingMaskIntoConstraints = false;
			ContentInset = UIEdgeInsets.Zero;
			this.CollectionViewLayout = new FastListViewFlowLayout(fastListView);
			RegisterClassForCell(typeof(FastListViewCell), new NSString(FastListViewCell.Key));
		}

		public FastListCollectionView(IntPtr handle) : base(handle)
		{

		}
	}
}
