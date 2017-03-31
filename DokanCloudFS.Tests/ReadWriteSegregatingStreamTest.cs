﻿/*
The MIT License(MIT)

Copyright(c) 2017 IgorSoft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IgorSoft.DokanCloudFS.IO;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IgorSoft.DokanCloudFS.Tests
{
    [TestClass]
    public sealed partial class ReadWriteSegregatingStreamTest
    {
        private Fixture fixture;

        [TestInitialize]
        public void Initialize()
        {
            fixture = new Fixture();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNew_WhereWriteStreamIsNull_Throws()
        {
            var sut = new ReadWriteSegregatingStream(null, fixture.CreateStream());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateNew_WhereReadStreamIsNull_Throws()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), null);
        }

        [TestMethod]
        public void CanRead_IsDelegatedToReadStream_ForCanReadAsTrue()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream_ForCanRead());

            Assert.IsTrue(sut.CanRead);

            fixture.Verify();
        }

        [TestMethod]
        public void CanRead_IsDelegatedToReadStream_ForCanReadAsFalse()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream_ForCanRead(false));

            Assert.IsFalse(sut.CanRead);

            fixture.Verify();
        }

        [TestMethod]
        public void CanTimeout_IsDelegatedToWriteAndReadStream_ForCanTimeoutAsFalseFalse()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanTimeout(false), fixture.CreateStream_ForCanTimeout(false));

            Assert.IsFalse(sut.CanTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void CanTimeout_IsDelegatedToWriteAndReadStream_ForCanTimeoutAsFalseTrue()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanTimeout(false), fixture.CreateStream_ForCanTimeout());

            Assert.IsFalse(sut.CanTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void CanTimeout_IsDelegatedToWriteAndReadStream_ForCanTimeoutAsTrueFalse()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanTimeout(), fixture.CreateStream_ForCanTimeout(false));

            Assert.IsFalse(sut.CanTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void CanTimeout_IsDelegatedToWriteAndReadStream_ForCanTimeoutAsTrueTrue()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanTimeout(), fixture.CreateStream_ForCanTimeout());

            Assert.IsTrue(sut.CanTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void CanWrite_IsDelegatedToWriteStream_ForCanWriteAsTrue()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanWrite(), fixture.CreateStream());

            Assert.IsTrue(sut.CanWrite);

            fixture.Verify();
        }

        [TestMethod]
        public void CanWrite_IsDelegatedToWriteStream_ForCanWriteAsFalse()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForCanWrite(false), fixture.CreateStream());

            Assert.IsFalse(sut.CanWrite);

            fixture.Verify();
        }

        [TestMethod]
        public void GetReadTimeout_IsDelegatedToReadStream()
        {
            var timeout = 42;

            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream_ForGetReadTimeout(timeout));

            Assert.AreEqual(timeout, sut.ReadTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void SetReadTimeout_IsDelegatedToReadStream()
        {
            var timeout = 42;

            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream_ForSetReadTimeout(timeout));

            sut.ReadTimeout = timeout;

            fixture.Verify();
        }

        [TestMethod]
        public void GetWriteTimeout_IsDelegatedToWriteStream()
        {
            var timeout = 42;

            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForGetWriteTimeout(timeout), fixture.CreateStream());

            Assert.AreEqual(timeout, sut.WriteTimeout);

            fixture.Verify();
        }

        [TestMethod]
        public void SetWriteTimeout_IsDelegatedToWriteStream()
        {
            var timeout = 42;

            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForSetWriteTimeout(timeout), fixture.CreateStream());

            sut.WriteTimeout = timeout;

            fixture.Verify();
        }

        [TestMethod]
        public void CopyToAsync_IsDelegatedToReadStream()
        {
            using (var cts = new CancellationTokenSource()) {
                var destination = Stream.Null;
                var bufferSize = 4711;
                var cancellationToken = cts.Token;

                var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream_ForCopyToAsync(destination, bufferSize, cancellationToken));

                var task = sut.CopyToAsync(destination, bufferSize, cancellationToken);

                Assert.IsInstanceOfType(task, typeof(Task));
            }

            fixture.Verify();
        }

        [TestMethod]
        public void Flush_IsDelegatedToWriteStreamAndReadStream()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForFlush(), fixture.CreateStream_ForFlush());

            sut.Flush();

            fixture.Verify();
        }

        [TestMethod]
        public void FlushAsync_IsDelegatedToWriteStreamAndReadStream()
        {
            using (var cts = new CancellationTokenSource())
            {
                var cancellationToken = cts.Token;

                var sut = new ReadWriteSegregatingStream(fixture.CreateStream_ForFlushAsync(cancellationToken), fixture.CreateStream_ForFlushAsync(cancellationToken));

                var task = sut.FlushAsync(cancellationToken);

                Assert.IsInstanceOfType(task, typeof(Task));
            }

            fixture.Verify();
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void InitializeLifetimeService_Throws()
        {
            var sut = new ReadWriteSegregatingStream(fixture.CreateStream(), fixture.CreateStream());

            sut.InitializeLifetimeService();
        }
    }
}
