﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crevice4Tests
{
    using Crevice.WinAPI.WindowsHookEx;
    using Crevice.WinAPI.SendInput;

    [TestClass()]
    public class LowLevelMouseHookTests
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext context)
        {
            TestHelpers.MouseMutex.WaitOne();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestHelpers.MouseMutex.ReleaseMutex();
        }

        static readonly Mutex mutex = new Mutex(true);

        [TestInitialize()]
        public void TestInitialize()
        {
            mutex.WaitOne();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            mutex.ReleaseMutex();
        }

        [TestMethod()]
        public void ActivatedTest()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                Assert.IsFalse(hook.IsActivated);
                hook.SetHook();
                Assert.IsTrue(hook.IsActivated);
                hook.Unhook();
                Assert.IsFalse(hook.IsActivated);
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetHookThrowsInvalidOperationExceptionTest()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                hook.SetHook();
                hook.SetHook();
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnhookThrowsInvalidOperationExceptionTest0Test()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                hook.SetHook();
                hook.Unhook();
                hook.Unhook();
            }
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void UnhookThrowsInvalidOperationExceptionTest1Test()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                hook.Unhook();
            }
        }

#if DEBUG
        [TestMethod()]
        public void LowLevelMouseHookProcTest()
        {
            using (var cde = new CountdownEvent(2))
            {
                using(var hook = new LowLevelMouseHook((evnt, data) => {
                    if (data.FromCreviceApp)
                    {
                        cde.Signal();
                    }
                    return LowLevelMouseHook.Result.Cancel;
                }))
                {
                    hook.SetHook();
                    var task = Task.Factory.StartNew(() => 
                    {
                        var sender = new SingleInputSender();
                        sender.RightDown();
                        sender.RightUp();
                    });
                    task.Wait();
                    Assert.AreEqual(cde.Wait(10000), true);
                }
            }
        }
#endif

        [TestMethod()]
        public void DisposeWhenActivatedTest()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                hook.SetHook();
                Assert.IsTrue(hook.IsActivated);
                hook.Dispose();
                Assert.IsFalse(hook.IsActivated);
            }
        }

        [TestMethod()]
        public void DisposeWhenNotActivatedTest()
        {
            using (var hook = new LowLevelMouseHook((evnt, data) => { return LowLevelMouseHook.Result.Cancel; }))
            {
                Assert.IsFalse(hook.IsActivated);
                hook.Dispose();
                Assert.IsFalse(hook.IsActivated);
            }
        }
    }
}