using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FLVDemo.Extensions;
using FLVDemo.ViewModels.ViewCells;
using Plugin.Connectivity;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;

namespace FLVDemo.Models
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
			if (CrossConnectivity.Current.IsConnected)
			{
				var base_url = App.OpenDataUrl;
				var timeout = 30000;
				var client = new RestClient(base_url);
				var request = new RestRequest("api/records/1.0/search/?dataset=arc_innovation", Method.GET);
				request.AddQueryParameter("q", postal_code);
				var cts = new CancellationTokenSource(timeout);
				var result = await client.Execute<RootInovations>(request, cts.Token);
				return result.Data.Records.ToInovationViewModels();
			}
			return null;
		}
	}
}
