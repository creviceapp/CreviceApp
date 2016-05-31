﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreviceApp.GestureConfig.DSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreviceApp.GestureConfig.DSL.Tests
{
    [TestClass()]
    public class DoElementTests
    {
        [TestMethod()]
        public void funcTest()
        {
            var root = new Root();
            var appElement = root.@when(x => true);
            var onElement = appElement.@on(new RightButton());
            var ifElement = onElement.@if(new MoveDown(), new MoveRight());
            var called = false;
            var doEmenent = ifElement.@do(x => { called = true; });
            Assert.IsFalse(called);
            root.whenElements[0].onElements[0].ifStrokeElements[0].doElements[0].func(new DoContext());
            Assert.IsTrue(called);
        }
    }
}