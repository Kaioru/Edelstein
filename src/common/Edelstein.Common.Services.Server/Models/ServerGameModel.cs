namespace Edelstein.Common.Services.Server.Models;

public record ServerGameModel : ServerModel
{
    public int WorldID { get; set; }
    public int ChannelID { get; set; }
    public bool IsAdultChannel { get; set; }
}
