using System;
using FastListViewDemo.Models;
using FastListViewDemo.Views.ViewCells;
using Xamarin.Forms;

namespace FastListViewDemo.ViewModels.ViewCells
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
				_visuelImageSource = value;
				OnPropertyChanged(nameof(VisuelImageSource));

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
				_titleText = value;
				OnPropertyChanged(nameof(TitleText));
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
				_detailText = value;
				OnPropertyChanged(nameof(DetailText));
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
				_visuelWidth = value;
				OnPropertyChanged(nameof(VisuelWidth));
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
				_visuelHeight = value;
				OnPropertyChanged(nameof(VisuelHeight));
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