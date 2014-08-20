using System;
using System.Runtime.Caching;
using Manufacturing.Framework.Datasource;
using Manufacturing.Framework.Dto;

namespace Manufacturing.Api.Hubs.ChannelResolvers
{
    /// <summary>
    ///     Responsible for resolving the correct channel
    /// </summary>
    public class DatasourceRecordChannelResolver<T> : IChannelResolver<T> where T : DatasourceRecord
    {
        #region Fields

        private readonly MemoryCache _cache = MemoryCache.Default;

        #endregion

        #region IChannelResolver<T> Members

        public string GetChannelId(T message)
        {
            // TODO: this obviously would need to a.) be expanded on and b.) not use only memcache
            return _cache.Get(message.DatasourceId.ToString()) as string;
        }

        public void SetChannelId(string domainId, string channelId)
        {
            _cache.Set(domainId, channelId, new CacheItemPolicy
            {
                SlidingExpiration = new TimeSpan(1, 0, 0, 0),
            });
        }

        #endregion
    }
}