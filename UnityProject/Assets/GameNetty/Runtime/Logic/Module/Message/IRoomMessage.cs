namespace ET
{
    public interface IRoomMessage: IMessage
    {
        long PlayerId { get; set; }
        long TargetPlayerId { get; set; }
    }
    
    public interface IRoomRequest: IRoomMessage, IRequest
    {
    }
    
    public interface IRoomResponse: IRoomMessage, IResponse
    {

    }
}