using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Game.Combat;

public class DamageCalculator : IDamageCalculator
{
    public uint InitSeed1 { get; }
    public uint InitSeed2 { get; }
    public uint InitSeed3 { get; }

    private readonly DamageRand _rndGenForCharacter;
    private readonly DamageRand _rndForCheckDamageMiss;
    private readonly DamageRand _rndGenForMob;
    
    public DamageCalculator()
    {
        var random = new Random();
        var s1 = (uint)random.Next();
        var s2 = (uint)random.Next();
        var s3 = (uint)random.Next();
        
        InitSeed1 = s1;
        InitSeed2 = s2;
        InitSeed3 = s3;

        s1 |= 0x100000;
        s2 |= 0x1000;
        s3 |= 0x10;

        _rndGenForCharacter = new DamageRand(s1, s2, s3);
        _rndForCheckDamageMiss = new DamageRand(s1, s2, s3);
        _rndGenForMob = new DamageRand(s1, s2, s3);
    }

    public Task<IUserAttackDamage[]> CalculatePDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack) => throw new NotImplementedException();
    public Task<IUserAttackDamage[]> CalculateMDamage(IFieldUserStats attacker, IFieldMobStats target, IUserAttack attack) => throw new NotImplementedException();
}
