// SPDX-FileCopyrightText: 2025 Demerzel Solutions Limited
// SPDX-License-Identifier: MIT

namespace Nethermind.MclBindings.Tests;

using Xunit.Internal;
using static Mcl;

public class BN254Tests
{
    static BN254Tests()
    {
        if (mclBn_init(MCL_BN254, MCLBN_COMPILED_TIME_VAR) != 0)
            throw new InvalidOperationException("MCL initialization failed");
    }

    [Fact]
    public void Should_get_curve_type() => Assert.Equal(MCL_BN254, mclBn_getCurveType());

    [Fact]
    public void Should_get_op_unit_size() => Assert.Equal(4, mclBn_getOpUnitSize());

    [Fact]
    public void Should_get_G1_size() => Assert.Equal(32, mclBn_getG1ByteSize());

    [Fact]
    public void Should_get_G2_size() => Assert.Equal(64, mclBn_getG2ByteSize());

    [Fact]
    public void Should_get_Fr_size() => Assert.Equal(32, mclBn_getFrByteSize());

    [Fact]
    public void Should_get_Fp_size() => Assert.Equal(32, mclBn_getFpByteSize());
}
