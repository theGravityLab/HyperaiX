﻿using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Messages.Payloads;

namespace HyperaiX.Extensions.QQ.Messages.Payloads;

public record Forward(IReadOnlyList<MessageEntity> Messages) : IMessagePayload;