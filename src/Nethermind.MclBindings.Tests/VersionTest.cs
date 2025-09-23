// SPDX-FileCopyrightText: 2025 Demerzel Solutions Limited
// SPDX-License-Identifier: MIT

namespace Nethermind.MclBindings.Tests;

public class VersionTest
{
    [Test]
    public async Task Should_match_version() => await Assert.That(Mcl.mclBn_getVersion()).IsEqualTo(0x304);
}
