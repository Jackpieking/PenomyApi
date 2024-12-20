using PenomyAPI.Realtime.SignalR.Models.ChatHubs;
using System;

namespace PenomyAPI.Realtime.SignalR.AppConstants.ChatHub;

internal static class ChatHubClientMethods
{
    public static class ReceiveMessageMethod
    {
        public const string MethodName = "receiveMessage";

        public static Type Param1Type = typeof(SendMessageRequestDto);
    }
}
