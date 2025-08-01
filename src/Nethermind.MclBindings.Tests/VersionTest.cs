// SPDX-FileCopyrightText: 2025 Demerzel Solutions Limited
// SPDX-License-Identifier: MIT

namespace Nethermind.MclBindings.Tests;

public class VersionTest
{
    [Fact]
    public void Should_match_version() => Assert.Equal(0x301, Mcl.mclBn_getVersion());
}
