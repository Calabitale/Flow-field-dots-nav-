using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using NUnit.Framework;

public class BitStreamTests
{
    private struct NativeArrayByteStream : BitStream.IByteStream
    {
        public NativeArray<byte> Array;

        public byte Read(int index)
        {
            return Array[index];
        }
    }

    private static NativeArray<byte> CreateArray()
    {
        NativeArray<byte> array = new NativeArray<byte>(9, Allocator.Temp);
        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = 0b10101010;
        }
        return array;
    }

    private static BitStream CreateStream(int bitIndex)
    {
        return new BitStream { BitIndex = bitIndex };
    }

    [Test]
    public void ReadUpTo8()
    {
        using (NativeArray<byte> array = CreateArray())
        {
            NativeArrayByteStream byteStream = new NativeArrayByteStream
            {
                Array = array
            };
            for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
            {
                int expected = 0;
                int next = bitIndex & 1;
                for (int numBits = 1; numBits <= 8; ++numBits)
                {
                    next = ~next & 1;
                    expected = (expected << 1) | next;
                    BitStream stream = CreateStream(bitIndex);
                    Assert.That(
                        stream.ReadUpTo8(byteStream, numBits),
                        Is.EqualTo(expected),
                        $"Bit index: {bitIndex}. Num bits: {numBits}");
                }
            }
        }
    }

    [Test]
    public void ReadUpTo16()
    {
        using (NativeArray<byte> array = CreateArray())
        {
            NativeArrayByteStream byteStream = new NativeArrayByteStream
            {
                Array = array
            };
            for (ushort bitIndex = 0; bitIndex < 8; ++bitIndex)
            {
                ushort expected = 0;
                ushort next = (ushort)(bitIndex & 1);
                for (int numBits = 1; numBits <= 16; ++numBits)
                {
                    next = (ushort)(~next & 1);
                    expected = (ushort)((expected << 1) | next);
                    BitStream stream = CreateStream(bitIndex);
                    Assert.That(
                        stream.ReadUpTo16(byteStream, numBits),
                        Is.EqualTo(expected),
                        $"Bit index: {bitIndex}. Num bits: {numBits}");
                }
            }
        }
    }

    [Test]
    public void ReadUpTo32()
    {
        using (NativeArray<byte> array = CreateArray())
        {
            NativeArrayByteStream byteStream = new NativeArrayByteStream
            {
                Array = array
            };
            for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
            {
                uint expected = 0;
                uint next = (uint)bitIndex & 1;
                for (int numBits = 1; numBits <= 32; ++numBits)
                {
                    next = ~next & 1;
                    expected = (expected << 1) | next;
                    BitStream stream = CreateStream(bitIndex);
                    uint actual = stream.ReadUpTo32(byteStream, numBits);
                    Assert.That(
                        actual,
                        Is.EqualTo(expected),
                        $"Bit index: {bitIndex}. Num bits: {numBits}.");
                }
            }
        }
    }

    [Test]
    public void ReadUpTo64()
    {
        using (NativeArray<byte> array = CreateArray())
        {
            NativeArrayByteStream byteStream = new NativeArrayByteStream
            {
                Array = array
            };
            for (int bitIndex = 0; bitIndex < 8; ++bitIndex)
            {
                ulong expected = 0;
                ulong next = (ulong)bitIndex & 1;
                for (int numBits = 1; numBits <= 64; ++numBits)
                {
                    next = ~next & 1;
                    expected = (expected << 1) | next;
                    BitStream stream = CreateStream(bitIndex);
                    ulong actual = stream.ReadUpTo64(byteStream, numBits);
                    Assert.That(
                        actual,
                        Is.EqualTo(expected),
                        $"Bit index: {bitIndex}. Num bits: {numBits}.");
                }
            }
        }
    }
}