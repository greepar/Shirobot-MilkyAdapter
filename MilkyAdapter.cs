using ShiroBot.MilkyAdapter.AdapterImpl;
using ShiroBot.MilkyAdapter.Milky;
using ShiroBot.SDK.Abstractions;
using ShiroBot.SDK.Adapter;
using ShiroBot.SDK.Config;
using ShiroBot.SDK.Core;
using ShiroBot.SDK.Plugin;

namespace ShiroBot.MilkyAdapter;

public class MilkyAdapter : IBotAdapter
{
    public string Name => "MilkyAdapter";
    public BotComponentMetadata Metadata { get; } = new()
    {
        Name = "MilkyAdapter",
        Version = "1.1.0",
        Description = "Milky Adapter for ShiroBot"
    };

    private readonly EventService _eventService = new();

    public IFileService File { get; } = new FileService();
    public IFriendService Friend { get; } = new FriendService();
    public IGroupService Group { get; } = new GroupService();
    public IMessageService Message { get; } = new MessageService();
    public ISystemService System { get; } = new SystemService();
    public IEventService Event => _eventService;
    public IConfigContext Config { get; set; } = null!;
    public IConsoleLogger Logger { get; set; } = null!;

    private CancellationTokenSource? _eventTokenSource;

    public async Task StartAsync()
    {
        var config = Config.Load<MilkyAdapterConfig>();
        Config.Save(config);

        MilkyClientManager.Initialize(config.BaseUrl, config.AccessToken);
        var milky = MilkyClientManager.Instance;
        _eventService.AttachEvent();
        
        Logger.Info("开始连接 Milky...");
        try
        {
            var loginInfo = await milky.System.GetLoginInfoAsync();
            var result = await milky.System.GetImplInfoAsync();
            Logger.Success($"Milky 登录成功 - Nickname: {loginInfo.Nickname},Milky Impl: {result.ImplName} {result.ImplVersion}");
        }
        catch (Exception)
        {
            Logger.Error("Milky连接失败,请检查Adapter配置是否正确。");
            throw;
        }

        switch (config.Protocol)
        {
            case var s when s.Equals("sse", StringComparison.OrdinalIgnoreCase):
                _eventTokenSource = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    var retryCount = 0;
                    while (!_eventTokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            retryCount++;
                            Logger.Info($"正在尝试连接 SSE 事件流，第 {retryCount} 次。");
                            await milky.ReceivingEventUsingSSEAsync(_eventTokenSource.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            Logger.Warning("SSE 事件接收已取消。");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"SSE 事件接收异常: {ex.GetType().Name}: {ex.Message}");
                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(5), _eventTokenSource.Token);
                            }
                            catch (TaskCanceledException)
                            {
                                break;
                            }
                        }
                    }
                });
                break;
            case var s when s.Equals("ws", StringComparison.OrdinalIgnoreCase):
                _eventTokenSource = new CancellationTokenSource();
                _ = Task.Run(async () =>
                {
                    var retryCount = 0;
                    while (!_eventTokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            retryCount++;
                            Logger.Info($"正在尝试连接 WebSocket 事件流，第 {retryCount} 次。");
                            await milky.ReceivingEventUsingSSEAsync(_eventTokenSource.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            Logger.Warning("SSE 事件接收已取消。");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"SSE 事件接收异常: {ex.GetType().Name}: {ex.Message}");
                            try
                            {
                                await Task.Delay(TimeSpan.FromSeconds(5), _eventTokenSource.Token);
                            }
                            catch (TaskCanceledException)
                            {
                                break;
                            }
                        }
                    }
                });
                break;
            case var s when s.Equals("webhook", StringComparison.OrdinalIgnoreCase):
                BotLog.Error("暂不支持 Webhook 协议");
                throw new NotSupportedException("暂不支持 Webhook 协议");
            default:
                BotLog.Error("请配置正确的协议,支持的协议有Sse,WebSocket");
                throw new ArgumentOutOfRangeException();
        }
    }
}

