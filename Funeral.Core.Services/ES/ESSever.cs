using AutoMapper.Configuration;
using Elasticsearch.Net;
using Funeral.Core.IServices.ES;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funeral.Core.Services.ES
{
    public class ESSever: IESSever
    {
        /// <summary>
        /// Linq查询的官方Client
        /// </summary>
        public IElasticClient ElasticLinqClient { get; set; }
        /// <summary>
        /// Js查询的官方Client
        /// </summary>
        public IElasticLowLevelClient ElasticJsonClient { get; set; }

        public ESSever()
        {

            var nodes = "http://localhost/:8081/".Split(';').Select(t => new Uri(t));
            var pool = new StaticConnectionPool(nodes);

            //var uris = configuration["ElasticSearchContext:Url"].Split(",").ToList().ConvertAll(x => new Uri(x));//配置节点地址，以，分开
            //var uris = new Uri("");
            //var connectionPool = new StaticConnectionPool(uris);//配置请求池

            var settings = new ConnectionSettings(pool).RequestTimeout(TimeSpan.FromSeconds(30));//请求配置参数
            this.ElasticJsonClient = new ElasticLowLevelClient(settings);//json请求客户端初始化
            this.ElasticLinqClient = new ElasticClient(settings);//linq请求客户端初始化
        }
    }
}
