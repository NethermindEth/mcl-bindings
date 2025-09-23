// SPDX-FileCopyrightText: 2025 Demerzel Solutions Limited
// SPDX-License-Identifier: MIT

namespace Nethermind.MclBindings.Tests;

using static Mcl;

public class BN254Tests
{
    static BN254Tests()
    {
        if (mclBn_init(MCL_BN254, MCLBN_COMPILED_TIME_VAR) != 0)
            throw new InvalidOperationException("MCL initialization failed");
    }

    [Test]
    public async Task Should_get_curve_type() => await Assert.That(mclBn_getCurveType()).IsEqualTo(MCL_BN254);

    [Test]
    public async Task Should_get_op_unit_size() => await Assert.That(mclBn_getOpUnitSize()).IsEqualTo(4);

    [Test]
    public async Task Should_get_G1_size() => await Assert.That(mclBn_getG1ByteSize()).IsEqualTo(32);

    [Test]
    public async Task Should_get_G2_size() => await Assert.That(mclBn_getG2ByteSize()).IsEqualTo(64);

    [Test]
    public async Task Should_get_Fr_size() => await Assert.That(mclBn_getFrByteSize()).IsEqualTo(32);

    [Test]
    public async Task Should_get_Fp_size() => await Assert.That(mclBn_getFpByteSize()).IsEqualTo(32);
}
