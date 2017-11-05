using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FastListView.Renderers
{
	public class FastListViewFlowLayout : UICollectionViewFlowLayout
	{
		Forms.VisualElements.FastListView _fastListView;

		static readonly NSString NSSeparatorDecorationView = new NSString("Separator");

		public FastListViewFlowLayout()
		{

		}

		public FastListViewFlowLayout(Forms.VisualElements.FastListView fastListView)
		{
			_fastListView = fastListView;
			EstimatedItemSize = new CGSize(1.0f, 1.0f);
			ScrollDirection = UICollectionViewScrollDirection.Vertical;
			MinimumInteritemSpacing = 0;
			if (_fastListView.SeparatorVisibility == SeparatorVisibility.Default)
			{
				MinimumLineSpacing = 1;
				RegisterClassForDecorationView(typeof(FastListViewSeparator), NSSeparatorDecorationView);
			}
			else
				MinimumLineSpacing = 0;
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
		{
			if (_fastListView != null && _fastListView.SeparatorVisibility == SeparatorVisibility.Default)
			{
				var attributes = new List<UICollectionViewLayoutAttributes>();

				var lineWidth = this.MinimumLineSpacing;

				var layoutAttributes = base.LayoutAttributesForElementsInRect(rect);
				foreach (var layoutAttribute in layoutAttributes)
				{
					// -- skip first cell
					if (layoutAttribute.IndexPath != NSIndexPath.FromRowSection(0, 0))
					{
						var separatorAttribute = LayoutAttributesForDecorationView(NSSeparatorDecorationView, layoutAttribute.IndexPath);
						if (separatorAttribute != null)
						{
							var cellFrame = layoutAttribute.Frame;
							separatorAttribute.Frame = new CGRect(cellFrame.X, cellFrame.Y - lineWidth, cellFrame.Width, lineWidth);
							separatorAttribute.ZIndex = -1;
							attributes.Add(separatorAttribute);
						}
					}
					attributes.Add(layoutAttribute);
				}

				return attributes.ToArray();
			}
			return base.LayoutAttributesForElementsInRect(rect);
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForDecorationView(NSString kind, NSIndexPath indexPath)
		{
			var layoutAttributes = UICollectionViewLayoutAttributes.CreateForDecorationView<FastListViewLayoutAttributes>(kind, indexPath);
			layoutAttributes.Color = _fastListView.SeparatorColor.ToUIColor();
			return layoutAttributes;
		}

		public class FastListViewLayoutAttributes : UICollectionViewLayoutAttributes
		{
			public UIColor Color { get; set; }
			public override NSObject Copy()
			{
				// It is required to override Copy, iOS will call this method to clone your object.
				var copy = (FastListViewLayoutAttributes)base.Copy();
				copy.Color = Color;
				return copy;
			}
		}

		public class FastListViewSeparator : UICollectionReusableView
		{
			public FastListViewSeparator()
			{
			}

			public FastListViewSeparator(CGRect frame) : base(frame)
			{
			}

			public FastListViewSeparator(IntPtr handle) : base(handle)
			{
				this.BackgroundColor = UIColor.Clear;
			}
			public override void ApplyLayoutAttributes(UICollectionViewLayoutAttributes layoutAttributes)
			{
				var attributes = layoutAttributes as FastListViewLayoutAttributes;
				this.Frame = attributes.Frame;
				this.BackgroundColor = attributes.Color;
			}
		}

	}}
