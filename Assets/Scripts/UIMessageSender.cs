
/// <summary>
/// an interface for any class that sends requests to display a message 
/// that require feedback to the player. The class using this interface 
/// must be able to recieve the player response to the message
/// </summary>

public interface UIMessageSender
{
    /// <summary>
    /// to recieve the player's response to the message 
    /// </summary>
    /// <param name="response"></param>
    public abstract void messageResponse(bool response);
}
