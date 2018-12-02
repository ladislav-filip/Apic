using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apic.Tests
{
	[TestClass]
	public class UnitTest
	{
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
		}

		[TestInitialize]
		public void Initialize()
		{
		}

		[TestMethod]
		public void IgnoredMethod()
		{
		}

		[TestCleanup]
		public void Cleanup()
		{
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
		}
	}
}
