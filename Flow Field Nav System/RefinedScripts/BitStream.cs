using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reader and writer of a stream of bits, which can be stored in any indexed
/// container type. This allows for accessing the bits regardless of their size
/// (up to 64 bits) even if the accesses cross one or more byte boundaries. This
/// can be used to efficiently pack bits such that individual values can be
/// stored in less than one bit. For example, a boolean can take up only one bit
/// and a 3-bit integer is possible.
/// </summary>
///
/// <author>
/// Jackson Dunstan, https://JacksonDunstan.com/articles/5426
/// </author>
///
/// <license>
/// MIT
/// </license>
public struct BitStream
{
    /// <summary>
    /// Abstraction of a stream of bytes
    /// </summary>
    public interface IByteStream
    {
        /// <summary>
        /// Read a byte from the stream at a given index
        /// </summary>
        /// 
        /// <param name="index">
        /// Index of the byte to read.
        /// </param>
        /// 
        /// <returns>
        /// The byte at the given index.
        /// </returns>
        byte Read(int index);
    }

    /// <summary>
    /// Index of the next bit to access
    /// </summary>
    public int BitIndex;

    /// <summary>
    /// Read up to 8 bits starting at <see cref="BitIndex"/>.
    /// </summary>
    /// 
    /// <param name="stream">
    /// Byte stream to read from.
    /// </param>
    /// 
    /// <param name="num">
    /// Number of bits to read.
    /// </param>
    /// 
    /// <typeparam name="TByteStream">
    /// Type of byte stream to read with.
    /// </typeparam>
    /// 
    /// <returns>
    /// The read value, stored in the least-significant bits.
    /// </returns>
    public byte ReadUpTo8<TByteStream>(in TByteStream stream, int num)
        where TByteStream : IByteStream
    {
        // Calculate where we are in the stream and advance
        int bitIndex = BitIndex;
        int localByteIndex = bitIndex / 8;
        int localBitIndex = bitIndex % 8;
        BitIndex = bitIndex + num;

        // Read the byte with the high bits and decide if that's the only byte
        byte high = stream.Read(localByteIndex);
        int numHighBitsAvailable = 8 - localBitIndex;
        int excessHighBitsAvailable = numHighBitsAvailable - num;
        int highMask;
        if (excessHighBitsAvailable >= 0)
        {
            highMask = ((1 << num) - 1) << excessHighBitsAvailable;
            return (byte)((high & highMask) >> excessHighBitsAvailable);
        }

        // Read the low byte and combine with the high byte
        highMask = (1 << numHighBitsAvailable) - 1;
        int numLowBits = num - numHighBitsAvailable;
        byte low = stream.Read(localByteIndex + 1);
        int lowShift = 8 - numLowBits;
        int lowMask = ((1 << numLowBits) - 1) << lowShift;
        int highPart = (high & highMask) << numLowBits;
        int lowPart = (low & lowMask) >> lowShift;
        int result = highPart | lowPart;
        return (byte)result;
    }

    /// <summary>
    /// Read up to 16 bits starting at <see cref="BitIndex"/>.
    /// </summary>
    /// 
    /// <param name="stream">
    /// Byte stream to read from.
    /// </param>
    /// 
    /// <param name="num">
    /// Number of bits to read.
    /// </param>
    /// 
    /// <typeparam name="TByteStream">
    /// Type of byte stream to read with.
    /// </typeparam>
    /// 
    /// <returns>
    /// The read value, stored in the least-significant bits.
    /// </returns>
    public ushort ReadUpTo16<TByteStream>(in TByteStream stream, int num)
        where TByteStream : IByteStream
    {
        if (num <= 8)
        {
            return ReadUpTo8(stream, num);
        }
        byte high = ReadUpTo8(stream, 8);
        int numLowBits = num - 8;
        byte low = ReadUpTo8(stream, numLowBits);
        return (ushort)((high << numLowBits) | low);
    }

    /// <summary>
    /// Read up to 32 bits starting at <see cref="BitIndex"/>.
    /// </summary>
    /// 
    /// <param name="stream">
    /// Byte stream to read from.
    /// </param>
    /// 
    /// <param name="num">
    /// Number of bits to read.
    /// </param>
    /// 
    /// <typeparam name="TByteStream">
    /// Type of byte stream to read with.
    /// </typeparam>
    /// 
    /// <returns>
    /// The read value, stored in the least-significant bits.
    /// </returns>
    public uint ReadUpTo32<TByteStream>(in TByteStream stream, int num)
        where TByteStream : IByteStream
    {
        if (num <= 16)
        {
            return ReadUpTo16(stream, num);
        }
        uint high = ReadUpTo16(stream, 16);
        int numLowBits = num - 16;
        uint low = ReadUpTo16(stream, numLowBits);
        return (high << numLowBits) | low;
    }

    /// <summary>
    /// Read up to 64 bits starting at <see cref="BitIndex"/>.
    /// </summary>
    /// 
    /// <param name="stream">
    /// Byte stream to read from.
    /// </param>
    /// 
    /// <param name="num">
    /// Number of bits to read.
    /// </param>
    /// 
    /// <typeparam name="TByteStream">
    /// Type of byte stream to read with.
    /// </typeparam>
    /// 
    /// <returns>
    /// The read value, stored in the least-significant bits.
    /// </returns>
    public ulong ReadUpTo64<TByteStream>(in TByteStream stream, int num)
        where TByteStream : IByteStream
    {
        if (num <= 32)
        {
            return ReadUpTo32(stream, num);
        }
        ulong high = ReadUpTo32(stream, 32);
        int numLowBits = num - 32;
        ulong low = ReadUpTo32(stream, numLowBits);
        return (high << numLowBits) | low;
    }
}