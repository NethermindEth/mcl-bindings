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

    [Test]
    public async Task Should_use_span_buffers()
    {
        Span<byte> order = stackalloc byte[128];
        nuint orderSize = mclBn_getCurveOrder(order, (nuint)order.Length);

        mclBnFr value = default;
        ReadOnlySpan<byte> source = "1"u8;
        int setResult = mclBnFr_setStr(ref value, source, (nuint)source.Length, 10);

        Span<byte> destination = stackalloc byte[4];
        nuint written = mclBnFr_getStr(destination, (nuint)destination.Length, in value, 10);
        byte firstByte = destination[0];

        await Assert.That(orderSize).IsGreaterThan(0u);
        await Assert.That(setResult).IsZero();
        await Assert.That(written).IsEqualTo((nuint)1);
        await Assert.That(firstByte).IsEqualTo((byte)'1');
    }
}
