using Milky.Net.Client;
using MModel = Milky.Net.Model;
using ShiroBot.MilkyAdapter.Milky;
using ShiroBot.SDK.Adapter;

namespace ShiroBot.MilkyAdapter.AdapterImpl;

public class EventService : IEventService
{
    public event Func<GroupIncomingMessage, Task>? GroupMessageReceived;
    public event Func<FriendIncomingMessage, Task>? FriendMessageReceived;
    public event Func<MessageRecallEvent, Task>? MessageRecall;
    public event Func<FriendRequestEvent, Task>? FriendRequest;
    public event Func<GroupJoinRequestEvent, Task>? GroupJoinRequest;
    public event Func<GroupInvitedJoinRequestEvent, Task>? GroupInvitedJoinRequest;
    public event Func<GroupInvitationEvent, Task>? GroupInvitation;
    public event Func<FriendNudgeEvent, Task>? FriendNudge;
    public event Func<FriendFileUploadEvent, Task>? FriendFileUpload;
    public event Func<GroupAdminChangeEvent, Task>? GroupAdminChange;
    public event Func<GroupEssenceMessageChangeEvent, Task>? GroupEssenceMessageChange;
    public event Func<GroupMemberIncreaseEvent, Task>? GroupMemberIncrease;
    public event Func<GroupMemberDecreaseEvent, Task>? GroupMemberDecrease;
    public event Func<GroupNameChangeEvent, Task>? GroupNameChange;
    public event Func<GroupMessageReactionEvent, Task>? GroupMessageReaction;
    public event Func<GroupMuteEvent, Task>? GroupMute;
    public event Func<GroupWholeMuteEvent, Task>? GroupWholeMute;
    public event Func<GroupNudgeEvent, Task>? GroupNudge;
    public event Func<GroupFileUploadEvent, Task>? GroupFileUpload;
    
    private static MilkyClient Milky => MilkyClientManager.Instance;

    public void AttachEvent()
    {
        Milky.Events.MessageReceive += async (_, e) =>
        {
            switch (e)
            {
                case MModel.GroupIncomingMessage groupMessage when GroupMessageReceived is not null:
                    await GroupMessageReceived(ModelMapper.Convert<GroupIncomingMessage>(groupMessage));
                    break;
                case MModel.FriendIncomingMessage friendMessage when FriendMessageReceived is not null:
                    await FriendMessageReceived(ModelMapper.Convert<FriendIncomingMessage>(friendMessage));
                    break;
            }
        };

        Milky.Events.MessageRecall += async (_, e) =>
        {
            if (MessageRecall is not null)
            {
                await MessageRecall(ModelMapper.Convert<MessageRecallEvent>(e));
            }
        };

        Milky.Events.FriendRequest += async (_, e) =>
        {
            if (FriendRequest is not null)
            {
                await FriendRequest(ModelMapper.Convert<FriendRequestEvent>(e));
            }
        };

        Milky.Events.GroupJoinRequest += async (_, e) =>
        {
            if (GroupJoinRequest is not null)
            {
                await GroupJoinRequest(ModelMapper.Convert<GroupJoinRequestEvent>(e));
            }
        };

        Milky.Events.GroupInvitedJoinRequest += async (_, e) =>
        {
            if (GroupInvitedJoinRequest is not null)
            {
                await GroupInvitedJoinRequest(ModelMapper.Convert<GroupInvitedJoinRequestEvent>(e));
            }
        };

        Milky.Events.GroupInvitation += async (_, e) =>
        {
            if (GroupInvitation is not null)
            {
                await GroupInvitation(ModelMapper.Convert<GroupInvitationEvent>(e));
            }
        };

        Milky.Events.FriendNudge += async (_, e) =>
        {
            if (FriendNudge is not null)
            {
                await FriendNudge(ModelMapper.Convert<FriendNudgeEvent>(e));
            }
        };

        Milky.Events.FriendFileUpload += async (_, e) =>
        {
            if (FriendFileUpload is not null)
            {
                await FriendFileUpload(ModelMapper.Convert<FriendFileUploadEvent>(e));
            }
        };

        Milky.Events.GroupAdminChange += async (_, e) =>
        {
            if (GroupAdminChange is not null)
            {
                await GroupAdminChange(ModelMapper.Convert<GroupAdminChangeEvent>(e));
            }
        };

        Milky.Events.GroupEssenceMessageChange += async (_, e) =>
        {
            if (GroupEssenceMessageChange is not null)
            {
                await GroupEssenceMessageChange(ModelMapper.Convert<GroupEssenceMessageChangeEvent>(e));
            }
        };

        Milky.Events.GroupMemberIncrease += async (_, e) =>
        {
            if (GroupMemberIncrease is not null)
            {
                await GroupMemberIncrease(ModelMapper.Convert<GroupMemberIncreaseEvent>(e));
            }
        };

        Milky.Events.GroupMemberDecrease += async (_, e) =>
        {
            if (GroupMemberDecrease is not null)
            {
                await GroupMemberDecrease(ModelMapper.Convert<GroupMemberDecreaseEvent>(e));
            }
        };

        Milky.Events.GroupNameChange += async (_, e) =>
        {
            if (GroupNameChange is not null)
            {
                await GroupNameChange(ModelMapper.Convert<GroupNameChangeEvent>(e));
            }
        };

        Milky.Events.GroupMessageReaction += async (_, e) =>
        {
            if (GroupMessageReaction is not null)
            {
                await GroupMessageReaction(ModelMapper.Convert<GroupMessageReactionEvent>(e));
            }
        };

        Milky.Events.GroupMute += async (_, e) =>
        {
            if (GroupMute is not null)
            {
                await GroupMute(ModelMapper.Convert<GroupMuteEvent>(e));
            }
        };

        Milky.Events.GroupWholeMute += async (_, e) =>
        {
            if (GroupWholeMute is not null)
            {
                await GroupWholeMute(ModelMapper.Convert<GroupWholeMuteEvent>(e));
            }
        };

        Milky.Events.GroupNudge += async (_, e) =>
        {
            if (GroupNudge is not null)
            {
                await GroupNudge(ModelMapper.Convert<GroupNudgeEvent>(e));
            }
        };

        Milky.Events.GroupFileUpload += async (_, e) =>
        {
            if (GroupFileUpload is not null)
            {
                await GroupFileUpload(ModelMapper.Convert<GroupFileUploadEvent>(e));
            }
        };
    }
}
