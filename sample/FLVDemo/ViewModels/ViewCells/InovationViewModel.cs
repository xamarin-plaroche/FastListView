using System;
using FLVDemo.Models;
using FLVDemo.Views.ViewCells;
using Xamarin.Forms;

namespace FLVDemo.ViewModels.ViewCells
{
	public class InovationViewModel : BaseCellViewModel
	{
		public Record Record { get; private set; }

		ImageSource _visuelImageSource;
		public ImageSource VisuelImageSource
		{
			get
			{
				return _visuelImageSource;
			}
			set
			{
				Set(() => VisuelImageSource, ref _visuelImageSource, value);
			}
		}

		string _titleText;
		public string TitleText
		{
			get
			{
				return _titleText;
			}
			set
			{
				Set(() => TitleText, ref _titleText, value);
			}
		}

		string _detailText;
		public string DetailText
		{
			get
			{
				return _detailText;
			}
			set
			{
				Set(() => DetailText, ref _detailText, value);
			}
		}

		double _visuelWidth;
		public double VisuelWidth
		{
			get
			{
				return _visuelWidth;
			}
			set
			{
				Set(() => VisuelWidth, ref _visuelWidth, value);
			}
		}

		double _visuelHeight;
		public double VisuelHeight
		{
			get
			{
				return _visuelHeight;
			}
			set
			{
				Set(() => VisuelHeight, ref _visuelHeight, value);
			}
		}


		public InovationViewModel(Record record)
		{
			Record = record;

			if (Record.Fields.Image != null && !string.IsNullOrEmpty(Record.Fields.Image.Filename))
			{
				ItemTemplate = new DataTemplate(typeof(InovationWithImageViewCell));
				VisuelImageSource = App.OpenDataImageUrl + Record.Fields.Image.Id + "/300";
				VisuelWidth = App.ScreenSize.Width;
				VisuelHeight = App.ScreenSize.Width / 2;
			}
			else
			{
				ItemTemplate = new DataTemplate(typeof(InovationViewCell));
			}

			TitleText = Record.Fields.Nom;

			DetailText = Record.Fields.TexteDescriptif;

		}
	}
}
