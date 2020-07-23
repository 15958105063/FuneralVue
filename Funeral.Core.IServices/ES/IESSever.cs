
using Elasticsearch.Net;
using Nest;

namespace Funeral.Core.IServices.ES
{
   public interface IESSever
    {
        /// <summary>
        /// Linq查询的官方Client
        /// </summary>
        IElasticClient ElasticLinqClient { get; set; }

        /// <summary>
        /// Js查询的官方Client
        /// </summary>
        IElasticLowLevelClient ElasticJsonClient { get; set; }

    }
}
