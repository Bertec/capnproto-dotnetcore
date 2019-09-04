using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capnp.Net.Runtime.Tests
{
    [TestClass]
    public class PackingTests
    {
        [TestMethod]
        public void PackedInputStream_Throws()
        {
            byte[] buffer = new byte[16];
            var stream = new MemoryStream(buffer);
            Assert.ThrowsException<ArgumentNullException>(() => new PackedInputStream(null, 16));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PackedInputStream(stream, 0));
        }

        [TestMethod]
        public void PackedInputStream_IsOnlyReadable()
        {
            const int defaultLength = 16;
            var stream = MakeInputStream();
            Assert.IsTrue(stream.CanRead);
            Assert.IsFalse(stream.CanSeek);
            Assert.IsFalse(stream.CanWrite);
            Assert.AreEqual(0, stream.Position);
            Assert.ThrowsException<NotImplementedException>(() => stream.Length);
            Assert.ThrowsException<NotImplementedException>(() => stream.Position = 0);
            Assert.ThrowsException<NotImplementedException>(stream.Flush);
            Assert.ThrowsException<NotImplementedException>(() => stream.SetLength(defaultLength));
            Assert.ThrowsException<NotImplementedException>(() => stream.Seek(0, SeekOrigin.Begin));
        }

        PackedInputStream MakeInputStream()
        {
            byte[] buffer = new byte[16];
            var stream = new MemoryStream(buffer);
            return new PackedInputStream(stream);
        }
    }
}