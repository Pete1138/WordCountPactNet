using System;
using System.Fabric;
using System.Fabric.Query;
using System.Threading.Tasks;

namespace WordCount.WebService
{
    public class FabricClientQueryManager : IFabricClientQueryManager
    {
        public async Task<ServicePartitionList> GetPartitionListAsync(Uri serviceUri)
        {
            var partitions = await new FabricClient().QueryManager.GetPartitionListAsync(serviceUri);
            return partitions;
        }
    }
}
