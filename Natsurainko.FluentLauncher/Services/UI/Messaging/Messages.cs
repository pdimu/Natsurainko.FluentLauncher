﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using Nrk.FluentCore.Authentication;

namespace Natsurainko.FluentLauncher.Services.UI.Messaging;

/// <summary>
/// 当激活账户发生改变时触发的消息
/// </summary>
internal class ActiveAccountChangedMessage : ValueChangedMessage<Account>
{
    public ActiveAccountChangedMessage(Account value) : base(value) { }
}
