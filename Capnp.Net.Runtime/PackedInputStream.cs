using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Capnp
{
    public class PackedInputStream : Stream
    {
        readonly Stream _stream;

        byte _tag;
        UInt16 _byteNLeft; // how many bytes are left to be copied verbatim
        UInt64 _spillWord;
        byte _spillLeft;

        byte[] _outputBuffer;
        int _outputOffset;
        int _outputLength;

        byte[] _inputBuffer;
        int _inputOffset;
        int _inputCount;
        readonly int _bufferSize;
        const int DefaultBufferSize = 4096;
        long _outputPosition;

        public PackedInputStream(Stream stream) : this(stream, DefaultBufferSize) { }

        public PackedInputStream(Stream stream, Int32 bufferSize)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            if (bufferSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(stream));
            _stream = stream;
            _bufferSize = bufferSize;
        }

        public override bool CanRead => _stream?.CanRead ?? false;

        void EnsureNotClosed()
        {
            if (_stream == null)
                throw new ObjectDisposedException(null);
        }

        void EnsureCanRead()
        {
            Debug.Assert(_stream != null);
            if (!_stream.CanRead)
                throw new NotSupportedException($"Cannot read when the underlying stream doesn't support reading.'");
        }

        void EnsureInputBufferIsAllocated()
        {
            if (_inputBuffer == null)
                _inputBuffer = new byte[_bufferSize];
            Debug.Assert(_inputBuffer != null);
            Debug.Assert(_inputBuffer.Length == _bufferSize);
        }

        void EnsureInputBufferIsEmpty()
        {
            Debug.Assert(_inputBuffer != null);
            Debug.Assert(_inputCount == 0);
            _inputOffset = 0;
            Debug.Assert(_inputOffset == 0);
        }

        int EnsureCountWithinBuffer(byte[] buffer, int offset, int count)
        {
            int bufferLength = buffer.Length;
            if ((offset + count) >= bufferLength)
                count = bufferLength - offset;
            if (count < 0)
                count = 0;
            Debug.Assert(count >= 0);
            return count;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length < (offset + count))
                throw new ArgumentException();

            EnsureNotClosed();
            EnsureCanRead();
            count = EnsureCountWithinBuffer(buffer, offset, count);

            int readSoFar = ReadFromBuffer(buffer, offset, count);
            Debug.Assert(readSoFar <= count);

            if (readSoFar == count)
                return readSoFar;

            offset += readSoFar;
            count -= readSoFar;

            FillInputBuffer();

            int readNow = ReadFromBuffer(buffer, offset, count);
            readSoFar += readNow;
            _outputPosition += readSoFar;
            return readSoFar;
        }

        void FillInputBuffer()
        {
            EnsureInputBufferIsAllocated();
            EnsureInputBufferIsEmpty();
            _inputCount = _stream.Read(_inputBuffer, 0, _bufferSize);

        }

        int ReadFromBuffer(byte[] buffer, int offset, int count)
        {
            Debug.Assert(count > 0);
            Debug.Assert(_spillLeft <= 8);

            int hadRead = 0;

            byte spillAvailable = _spillLeft;

            if (spillAvailable > 0)
            {
                UInt64 spillWord = _spillWord;
                if (count < spillAvailable) spillAvailable = (byte)count;
                if (spillAvailable > 7) buffer[offset++] = (byte)(spillWord >> (0 * 8));
                if (spillAvailable > 6) buffer[offset++] = (byte)(spillWord >> (1 * 8));
                if (spillAvailable > 5) buffer[offset++] = (byte)(spillWord >> (2 * 8));
                if (spillAvailable > 4) buffer[offset++] = (byte)(spillWord >> (3 * 8));
                if (spillAvailable > 3) buffer[offset++] = (byte)(spillWord >> (4 * 8));
                if (spillAvailable > 2) buffer[offset++] = (byte)(spillWord >> (5 * 8));
                if (spillAvailable > 1) buffer[offset++] = (byte)(spillWord >> (6 * 8));
                if (spillAvailable > 0) buffer[offset++] = (byte)(spillWord >> (7 * 8));
                _spillLeft -= spillAvailable;
                count -= spillAvailable;
                hadRead += spillAvailable;
            }

            if (count == 0)
                return hadRead;

            if (count >= 8 && _inputCount >= 8)
            {
                if (_byteNLeft > 0)
                {

                }
            }

#if false
      { \
         bool isNonzero = (tag & (1u << n)) != 0; \
         *out++ = *in & (-(int8_t)isNonzero); \
         in += isNonzero; \
      }
#endif
            return 0;

        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotImplementedException();

        public override long Position
        {
            get => _outputPosition;
            set => throw new NotImplementedException();
        }

    }
}
