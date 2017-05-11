
using System;
using System.Fabric.Query;
using System.Threading.Tasks;

namespace WordCount.WebService
{
    public interface IFabricClientQueryManager
    {
        Task<ServicePartitionList> GetPartitionListAsync(Uri serviceUri);
    }
}
