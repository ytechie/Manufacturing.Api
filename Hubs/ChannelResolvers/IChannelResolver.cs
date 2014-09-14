namespace Manufacturing.Api.Hubs.ChannelResolvers
{
    public interface IChannelResolver<in T> where T : class
    {
        #region public

        string GetChannelId(T message);

        void SetChannelId(int domainId, string channelId);

        #endregion
    }
}