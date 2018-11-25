using CoreAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreAPI.DataProvider
{
    public interface ITelephoneExtensionDataProvider
    {
        Task<IEnumerable<TelephoneExtension>> GetExtensions();

        Task<TelephoneExtension> GetExtension(int RecordId);

        Task<int> AddExtension(TelephoneExtension TelephoneExtension);

        Task UpdateExtension(TelephoneExtension TelephoneExtension);

        Task DeleteExtension(int RecordId);
    }
}
