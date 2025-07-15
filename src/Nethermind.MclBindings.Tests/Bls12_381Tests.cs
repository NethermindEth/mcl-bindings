// SPDX-FileCopyrightText: 2025 Demerzel Solutions Limited
// SPDX-License-Identifier: MIT

namespace Nethermind.MclBindings.Tests;

using static Mcl;

public class BLs12_381Tests
{
    public BLs12_381Tests()
    {
        if (mclBn_init(MCL_BLS12_381, MCLBN_COMPILED_TIME_VAR) != 0)
            throw new InvalidOperationException("MCL initialization failed");
    }

    [Fact]
    public void Should_get_curve_type() => Assert.Equal(MCL_BLS12_381, mclBn_getCurveType());

    [Fact]
    public void Should_get_op_unit_size() => Assert.Equal(6, mclBn_getOpUnitSize());

    [Fact]
    public void Should_get_G1_size() => Assert.Equal(48, mclBn_getG1ByteSize());

    [Fact]
    public void Should_get_G2_size() => Assert.Equal(96, mclBn_getG2ByteSize());

    [Fact]
    public void Should_get_Fr_size() => Assert.Equal(32, mclBn_getFrByteSize());

    [Fact]
    public void Should_get_Fp_size() => Assert.Equal(48, mclBn_getFpByteSize());
}
