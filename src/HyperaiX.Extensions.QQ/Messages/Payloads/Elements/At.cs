﻿using HyperaiX.Abstractions.Messages.Payloads.Elements;

namespace HyperaiX.Extensions.QQ.Messages.Payloads.Elements;

public record At(ulong MemberId, string Display) : IMessageElement;