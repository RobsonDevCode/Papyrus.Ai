﻿using Papyrus.Domain.Clients;

namespace Papyrus.Domain.Models.Client;

public record Candidates
{
    public Content Content { get; init; }
}