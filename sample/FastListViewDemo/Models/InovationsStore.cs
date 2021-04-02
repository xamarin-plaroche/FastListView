using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using FastListViewDemo.Extensions;
using FastListViewDemo.ViewModels.ViewCells;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace FastListViewDemo.Models
{
    public class InovationsStore : IDataStore<InovationViewModel>
	{
		private static readonly Lazy<InovationsStore> lazy = new Lazy<InovationsStore>(() => new InovationsStore());

		private InovationsStore()
		{
		}

		public static InovationsStore Instance
		{
			get
			{
				return lazy.Value;
			}
		}

		public async Task<IList<InovationViewModel>> GetRecordsAsync(string postal_code = "75011")
		{
			try
			{
				if (Connectivity.NetworkAccess == NetworkAccess.Internet)
				{
					var base_url = App.OpenDataUrl;
					var uri = base_url + "api/records/1.0/search/?dataset=arc_innovation";

					var client = new HttpClient();
					var response = await client.GetAsync(uri);

					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var json = await response.Content.ReadAsStringAsync();
						var innovations = JsonConvert.DeserializeObject<RootInovations>(json);
						return innovations?.Records?.ToInovationViewModels();
					}
				}
			}
			catch (Exception ex)
            {
				Debug.WriteLine(ex.Message);
            }

			return null;
		}
	}
}
