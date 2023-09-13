namespace Edelstein.Protocol.Gameplay.Models.Characters;

public interface ICharacterWildHunterInfo
{
    byte RidingType { get; set; }
    int[] CaptureMob { get; }
}
