using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
    [TestFixture]
    [Category("Sample Tests")]
    internal class DemoTests
    {
        [Test]
        [Category("Failing Tests")]
        public void ExceptionTest22()
        {
            throw new Exception("Exception throwing test");
        }
    }
}