using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastListViewDemo.Models
{
	public interface IDataStore<T>
	{
		Task<IList<T>> GetRecordsAsync(string postal_code);
	}
}
