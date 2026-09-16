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
    public async Task Should_use_pointer_buffers()
    {
        (nuint orderSize, int setResult, nuint written, byte firstByte) = UsePointerBuffers();

        await Assert.That(orderSize).IsGreaterThan(0u);
        await Assert.That(setResult).IsZero();
        await Assert.That(written).IsEqualTo((nuint)1);
        await Assert.That(firstByte).IsEqualTo((byte)'1');
    }

    private static unsafe (nuint OrderSize, int SetResult, nuint Written, byte FirstByte) UsePointerBuffers()
    {
        const int orderLength = 128;
        const int destinationLength = 4;

        byte* order = stackalloc byte[orderLength];
        nuint orderSize = mclBn_getCurveOrder(order, orderLength);

        mclBnFr value = default;
        ReadOnlySpan<byte> source = "1"u8;
        int setResult;

        fixed (byte* sourcePtr = source)
            setResult = mclBnFr_setStr(ref value, sourcePtr, (nuint)source.Length, 10);

        byte* destination = stackalloc byte[destinationLength];
        nuint written = mclBnFr_getStr(destination, destinationLength, in value, 10);

        return (orderSize, setResult, written, destination[0]);
    }

    [Test]
    public async Task Should_match_miller_loop_vector_with_pairwise_product()
    {
        (int basePointResult, int hashResult, int isEqual) = MillerLoopVec();

        await Assert.That(basePointResult).IsZero();
        await Assert.That(hashResult).IsZero();
        await Assert.That(isEqual).IsEqualTo(1);
    }

    private static unsafe (int BasePointResult, int HashResult, int IsEqual) MillerLoopVec()
    {
        const int count = 2;

        mclBnG1* g1 = stackalloc mclBnG1[count];
        mclBnG2* g2 = stackalloc mclBnG2[count];
        mclBnFr scalar = default;

        int basePointResult = mclBnG1_getBasePoint(ref g1[0]);
        mclBnFr_setByCSPRNG(ref scalar);
        mclBnG1_mul(ref g1[1], in g1[0], in scalar);

        int hashResult = 0;
        byte* message = stackalloc byte[1];

        for (int i = 0; i < count; i++)
        {
            message[0] = (byte)i;
            hashResult |= mclBnG2_hashAndMapTo(ref g2[i], message, 1);
        }

        mclBnGT actual = default;
        mclBnGT expected = default;
        mclBnGT pair = default;
        mclBn_millerLoopVec(ref actual, g1, g2, count);
        mclBn_millerLoop(ref expected, in g1[0], in g2[0]);
        mclBn_millerLoop(ref pair, in g1[1], in g2[1]);
        mclBnGT_mul(ref expected, in expected, in pair);

        return (basePointResult, hashResult, mclBnGT_isEqual(in actual, in expected));
    }
}
