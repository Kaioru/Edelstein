using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Utilities;
using Microsoft.Extensions.Logging;
using Stateless;

namespace Edelstein.Common.Gameplay.Game.Continent;

public class ContiMove : FieldSet, IContiMove
{
    private readonly ILogger<ContiMove> _logger;
    private readonly IDateTimeProvider _dateTime;
    private readonly StateMachine<ContiMoveState, ContiMoveStateTrigger> _stateMachine;
    
    public IContiMoveTemplate Template { get; }

    public ContiMoveState State => _stateMachine.State;
    
    public IField? StartShipMoveField { get; private set; }
    public IField? WaitField { get; private set; }
    public IField? MoveField { get; private set; }
    public IField? CabinField { get; private set; }
    public IField? EndField { get; private set; }
    public IField? EndShipMoveField { get; private set; }
    
    protected IFieldReactor? StartReactor { get; private set; }
    protected IFieldReactor? EndReactor { get; private set; }
    
    public DateTime NextBoarding { get; private set; }
    public DateTime NextStart => NextBoarding.AddMinutes(Template.Wait);
    public DateTime NextEnd => NextStart.AddMinutes(Template.Required);
    public DateTime? NextEvent { get; private set; }
    public DateTime? NextEventEnd => NextBoarding.AddMinutes(Template.Wait).AddMinutes(Template.EventEnd);

    public ContiMove(
        ILogger<ContiMove> logger,
        IDateTimeProvider dateTime,
        IContiMoveTemplate template
    ) : base(template.Name)
    {
        _logger = logger;
        _dateTime = dateTime;
        Template = template;
        
        _stateMachine = new StateMachine<ContiMoveState, ContiMoveStateTrigger>(ContiMoveState.Dormant);
        _stateMachine
            .Configure(ContiMoveState.Dormant)
            .Permit(ContiMoveStateTrigger.Board, ContiMoveState.Wait);
        _stateMachine
            .Configure(ContiMoveState.Wait)
            .Permit(ContiMoveStateTrigger.Start, ContiMoveState.Move);
        _stateMachine
            .Configure(ContiMoveState.Move)
            .OnEntryFromAsync(ContiMoveStateTrigger.Start, async () =>
            {
                if (WaitField != null && MoveField != null) 
                    await Move(WaitField, MoveField);
                if (StartShipMoveField != null) 
                    await StartShipMoveField.Dispatch(new CONTIMOVE
                    {
                        Target = ContiMoveTarget.TargetStartShipMoveField,
                        Trigger = ContiMoveStateTrigger.Start
                    });
                if (Template.Reactor != null && StartReactor != null)
                    await StartReactor.SetState((byte)Template.Reactor.StateOnStart);
            })
            .OnExitAsync(async () =>
            {
                if (MoveField != null && EndField != null) 
                    await Move(MoveField, EndField);
                if (CabinField != null && EndField != null)
                    await Move(CabinField, EndField);
                if (EndShipMoveField != null) 
                    await EndShipMoveField.Dispatch(new CONTIMOVE
                    {
                        Target = ContiMoveTarget.TargetEndShipMoveField,
                        Trigger = ContiMoveStateTrigger.End
                    });
                if (Template.Reactor != null && EndReactor != null)
                    await EndReactor.SetState((byte)Template.Reactor.StateOnEnd);

                NextBoarding = NextBoarding.AddMinutes(Template.Term);
                ResetEvent();
            })
            .Permit(ContiMoveStateTrigger.MobGen, ContiMoveState.Event)
            .Permit(ContiMoveStateTrigger.End, ContiMoveState.Dormant);
        _stateMachine
            .Configure(ContiMoveState.Event)
            .SubstateOf(ContiMoveState.Move)
            .OnEntryAsync(async () =>
            {
                NextEvent = null;

                _logger.LogContiMoveEventStarted(template.Name, NextEventEnd);

                // TODO: Mobspawns
                if (MoveField != null) 
                    await MoveField.Dispatch(new CONTIMOVE
                    {
                        Target = ContiMoveTarget.TargetMoveField,
                        Trigger = ContiMoveStateTrigger.MobGen
                    });
            })
            .OnExitAsync(async () =>
            {
                _logger.LogContiMoveEventEnded(template.Name);

                // TODO: Mobspawns
                if (MoveField != null) 
                    await MoveField.Dispatch(new CONTIMOVE
                    {
                        Target = ContiMoveTarget.TargetMoveField,
                        Trigger = ContiMoveStateTrigger.MobDestroy
                    });
            })
            .Permit(ContiMoveStateTrigger.MobDestroy, ContiMoveState.Move);

        _stateMachine.OnTransitioned(t => _logger.LogContiMoveStateTrigger(
                Template.Name, t.Trigger, t.Destination, t.Trigger switch
                {
                    ContiMoveStateTrigger.Board => NextStart,
                    ContiMoveStateTrigger.Start => NextEvent ?? NextEnd,
                    ContiMoveStateTrigger.MobGen => NextEventEnd,
                    ContiMoveStateTrigger.MobDestroy => NextBoarding,
                    ContiMoveStateTrigger.End => NextBoarding,
                    _ => NextBoarding
                }
            )
        );
    }
    
    public override async Task Initialize(IFieldManager manager)
    {
        await base.Initialize(manager);

        StartShipMoveField = await manager.Retrieve(Template.StartShipMoveFieldID);
        WaitField = Register(await manager.Retrieve(Template.WaitFieldID));
        MoveField = Register(await manager.Retrieve(Template.MoveFieldID));
        if (Template.CabinFieldID.HasValue)
            CabinField = Register(await manager.Retrieve(Template.CabinFieldID.Value));
        EndField = await manager.Retrieve(Template.EndFieldID);
        EndShipMoveField = await manager.Retrieve(Template.EndShipMoveFieldID);

        if (Template.Reactor != null)
        {
            StartReactor = StartShipMoveField?
                .GetPool(FieldObjectType.Reactor)?
                .GetObjects()
                .OfType<IFieldReactor>()
                .FirstOrDefault(r => r.Name == Template.Reactor.Name);
            EndReactor = EndShipMoveField?
                .GetPool(FieldObjectType.Reactor)?
                .GetObjects()
                .OfType<IFieldReactor>()
                .FirstOrDefault(r => r.Name == Template.Reactor.Name);
        }

        var now = _dateTime.Now;
        
        NextBoarding = now
            .AddMinutes(now.Minute % Template.Term == 0
                ? 0
                : Template.Term - now.Minute % Template.Term)
            .AddMinutes(Template.Delay)
            .AddSeconds(-now.Second);
        
        _logger.LogContiMoveScheduled(Template.Name, NextBoarding);
        
        ResetEvent();
    }

    public override async Task Enter(IFieldObject obj)
    {
        var field = State switch
        {
            ContiMoveState.Wait => WaitField,
            ContiMoveState.Move => MoveField,
            ContiMoveState.Event => MoveField,
            _ => StartShipMoveField
        };

        if (field != null)
            await field.Enter(obj);
    }

    public override async Task Leave(IFieldObject obj)
    {
        if (WaitField != null)
            await WaitField.Enter(obj);
    }

    public Task Trigger(ContiMoveStateTrigger trigger)
        => _stateMachine.FireAsync(trigger);
    
    private static Task Move(IField from, IField to) =>
        Task.WhenAll(from.GetObjects()
            .OfType<IFieldUser>()
            .Select(u => to.Enter(u, 0)));
    
    private void ResetEvent()
    {
        var random = new Random(
            NextBoarding.Year +
            NextBoarding.Month +
            NextBoarding.Day +
            NextBoarding.Hour +
            NextBoarding.Minute
        );

        if (!Template.Event || random.Next(100) > 30) return;

        NextEvent = NextBoarding
            .AddMinutes(Template.Wait)
            .AddMinutes(random.Next(Template.Required - 5))
            .AddMinutes(2);
        
        _logger.LogContiMoveScheduledEvent(Template.Name, NextEvent, NextEventEnd);
    }
}
