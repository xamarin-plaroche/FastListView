using System;
using System.Collections.Generic;
using System.Linq;
using FastListViewDemo.Models;
using FastListViewDemo.ViewModels.ViewCells;

namespace FastListViewDemo.Extensions
{
	public static class InovationsExtensions
	{
		public static IList<InovationViewModel> ToInovationViewModels(this IList<Record> list)
		{
			if (list == null)
			{
				return null;
			}
			return list.Select(i => new InovationViewModel(i)).ToList();
		}
	}
}
