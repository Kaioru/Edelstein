using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextPipelines(
    IPipeline<StageStart> StageStart,
    IPipeline<StageStop> StageStop,
    IPipeline<UserMigrate<IGameStageUser>> UserMigrate,
    IPipeline<UserOnPacket<IGameStageUser>> UserOnPacket,
    IPipeline<UserOnException<IGameStageUser>> UserOnException,
    IPipeline<UserOnDisconnect<IGameStageUser>> UserOnDisconnect,
    
    IPipeline<UserOnPacketAliveAck<IGameStageUser>> UserOnPacketAliveAck,
    IPipeline<UserOnPacketMigrateIn<IGameStageUser>> UserOnPacketMigrateIn,
    
    IPipeline<NotifyPartyMemberUpdateChannelOrField> NotifyPartyMemberUpdateChannelOrField,
    IPipeline<NotifyPartyMemberUpdateLevelOrJob> NotifyPartyMemberUpdateLevelOrJob,

    IPipeline<FieldOnPacketUserTransferFieldRequest> FieldOnPacketUserTransferFieldRequest,
    IPipeline<FieldOnPacketUserTransferChannelRequest> FieldOnPacketUserTransferChannelRequest,
    IPipeline<FieldOnPacketUserMigrateToCashShopRequest> FieldOnPacketUserMigrateToCashShopRequest,
    IPipeline<FieldOnPacketUserMove> FieldOnPacketUserMove,
    IPipeline<FieldOnPacketUserAttack> FieldOnPacketUserAttack,
    IPipeline<FieldOnPacketUserChat> FieldOnPacketUserChat,
    IPipeline<FieldOnPacketUserEmotion> FieldOnPacketUserEmotion,
    IPipeline<FieldOnPacketUserSelectNPC> FieldOnPacketUserSelectNPC,
    IPipeline<FieldOnPacketUserShopBuyRequest> FieldOnPacketUserShopBuyRequest,
    IPipeline<FieldOnPacketUserShopSellRequest> FieldOnPacketUserShopSellRequest,
    IPipeline<FieldOnPacketUserShopRechargeRequest> FieldOnPacketUserShopRechargeRequest,
    IPipeline<FieldOnPacketUserShopCloseRequest> FieldOnPacketUserShopCloseRequest,
    IPipeline<FieldOnPacketUserGatherItemRequest> FieldOnPacketUserGatherItemRequest,
    IPipeline<FieldOnPacketUserSortItemRequest> FieldOnPacketUserSortItemRequest,
    IPipeline<FieldOnPacketUserChangeSlotPositionRequest> FieldOnPacketUserChangeSlotPositionRequest,
    IPipeline<FieldOnPacketUserSkillUpRequest> FieldOnPacketUserSkillUpRequest,
    IPipeline<FieldOnPacketUserSkillUseRequest> FieldOnPacketUserSkillUseRequest,
    IPipeline<FieldOnPacketUserSkillCancelRequest> FieldOnPacketUserSkillCancelRequest,
    IPipeline<FieldOnPacketUserSkillPrepareRequest> FieldOnPacketUserSkillPrepareRequest,
    IPipeline<FieldOnPacketUserDropMoneyRequest> FieldOnPacketUserDropMoneyRequest,
    IPipeline<FieldOnPacketUserCharacterInfoRequest> FieldOnPacketUserCharacterInfoRequest,
    IPipeline<FieldOnPacketUserPortalScriptRequest> FieldOnPacketUserPortalScriptRequest,
    IPipeline<FieldOnPacketUserQuestLostItemRequest> FieldOnPacketUserQuestLostItemRequest,
    IPipeline<FieldOnPacketUserQuestAcceptRequest> FieldOnPacketUserQuestAcceptRequest,
    IPipeline<FieldOnPacketUserQuestCompleteRequest> FieldOnPacketUserQuestCompleteRequest,
    IPipeline<FieldOnPacketUserQuestResignRequest> FieldOnPacketUserQuestResignRequest,
    IPipeline<FieldOnPacketUserQuestScriptStartRequest> FieldOnPacketUserQuestScriptStartRequest,
    IPipeline<FieldOnPacketUserQuestScriptEndRequest> FieldOnPacketUserQuestScriptEndRequest,
    IPipeline<FieldOnPacketUserThrowGrenade> FieldOnPacketUserThrowGrenade,
    IPipeline<FieldOnPacketUserClientTimerEndRequest> FieldOnPacketUserClientTimerEndRequest,

    IPipeline<FieldOnPacketPartyCreateRequest> FieldOnPacketPartyCreateRequest,
    IPipeline<FieldOnPacketPartyLeaveRequest> FieldOnPacketPartyLeaveRequest,
    IPipeline<FieldOnPacketPartyInviteRequest> FieldOnPacketPartyInviteRequest,
    IPipeline<FieldOnPacketPartyKickRequest> FieldOnPacketPartyKickRequest,
    IPipeline<FieldOnPacketPartyChangeLeaderRequest> FieldOnPacketPartyChangeLeaderRequest,
    
    IPipeline<FieldOnPacketPartyInviteAcceptResult> FieldOnPacketPartyInviteAcceptResult,
    IPipeline<FieldOnPacketPartyInviteRejectResult> FieldOnPacketPartyInviteRejectResult,
    
    IPipeline<FieldOnPacketFriendSetRequest> FieldOnPacketFriendSetRequest,
    IPipeline<FieldOnPacketFriendAcceptRequest> FieldOnPacketFriendAcceptRequest,
    IPipeline<FieldOnPacketFriendDeleteRequest> FieldOnPacketFriendDeleteRequest,
    
    IPipeline<FieldOnPacketUserFuncKeyMappedModified> FieldOnPacketUserFuncKeyMappedModified,
    
    IPipeline<FieldOnPacketUserMigrateToITCRequest> FieldOnPacketUserMigrateToITCRequest,
    
    IPipeline<FieldOnPacketSummonedMove> FieldOnPacketSummonedMove,
    IPipeline<FieldOnPacketSummonedAttack> FieldOnPacketSummonedAttack,
    IPipeline<FieldOnPacketSummonedSkill> FieldOnPacketSummonedSkill,
    
    IPipeline<FieldOnPacketDragonMove> FieldOnPacketDragonMove,
    
    IPipeline<FieldOnPacketUserQuickslotKeyMappedModified> FieldOnPacketUserQuickslotKeyMappedModified,
    
    IPipeline<FieldOnPacketMobMove> FieldOnPacketMobMove,
    
    IPipeline<FieldOnPacketNPCMove> FieldOnPacketNPCMove,
    
    IPipeline<FieldOnPacketDropPickupRequest> FieldOnPacketDropPickupRequest
);
