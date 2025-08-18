using System.IO;

namespace Weth.External;

/**
ver.0.1

*/

public class LiveValue
{
    private static int _checkTimer = 0;
    private static int _retryTimer = 0;
    public static int CheckTimer
    {
        get => _checkTimer > 0 ? _checkTimer-- : 0;
        set => _checkTimer = value;
    }

    public static bool PendingCheck { get => CheckTimer == 0; }
    public static int RetryTimer
    {
        get => _retryTimer > 0 ? _retryTimer-- : 0;
        set => _retryTimer = value;
    }
    public static bool PendingRetry { get => RetryTimer == 0; }
    public const int CHECK_INTERVAL = 180;  // Check every 180 frames
    public const int RETRY_INTERVAL = 30;  // Retry every 30 frames if an error occurred
}