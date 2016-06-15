﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CreviceApp.User
{
    using static WinAPI.SendInput.VirtualKeys;

    class UserConfig : Core.Config.UserConfig
    {
        public UserConfig()
        {
            var Chrome = @when((ctx) =>
            {
                Debug.Print(ctx.Window.ClassName);
                return ctx.Window.ModuleName == "chrome.exe";
            });

            Chrome.
            @on(RightButton).
            @if(WheelUp).
            @do((ctx) =>
            {
                SendInput.Multiple().
                ExtendedKeyDown(VK_CONTROL).
                ExtendedKeyDown(VK_SHIFT).
                ExtendedKeyDown(VK_TAB).
                ExtendedKeyUp(VK_TAB).
                ExtendedKeyUp(VK_SHIFT).
                ExtendedKeyUp(VK_CONTROL).
                Send();

                Tooltip("Previous Tab");
            });

            Chrome.
            @on(RightButton).
            @if(WheelDown).
            @do((ctx) =>
            {
                SendInput.Multiple().
                ExtendedKeyDown(VK_CONTROL).
                ExtendedKeyDown(VK_TAB).
                ExtendedKeyUp(VK_TAB).
                ExtendedKeyUp(VK_CONTROL).
                Send();

                Baloon("Next Tab");
            });

            Chrome.
            @on(RightButton).
            @if(MoveUp).
            @do((ctx) =>
            {
                SendInput.Multiple().
                ExtendedKeyDown(VK_HOME).
                ExtendedKeyUp(VK_HOME).
                Send();
            });

            Chrome.
            @on(RightButton).
            @if(MoveDown).
            @do((ctx) =>
            {
                SendInput.Multiple().
                ExtendedKeyDown(VK_END).
                ExtendedKeyUp(VK_END).
                Send();
            });

            Chrome.
            @on(RightButton).
            @if(MoveDown, MoveRight).
            @do((ctx) =>
            {
                SendInput.Multiple().
                ExtendedKeyDown(VK_CONTROL).
                ExtendedKeyDown(VK_W).
                ExtendedKeyUp(VK_W).
                ExtendedKeyUp(VK_CONTROL).
                Send();
            });

            var Explorer = @when((ctx) =>
            {
                Debug.Print(ctx.Window.ClassName);
                Debug.Print(ctx.Window.OnCursor.ClassName);
                return ctx.Window.OnCursor.ModuleName == "explorer.exe" &&
                       ctx.Window.OnCursor.ClassName == "MSTaskListWClass";
            });

            Explorer.
            @if(WheelUp).
            @do((ctx) =>
            {
                var current = WaveVolume.GetMasterVolume() + 0.02f;
                var next = (current > 1 ? 1 : current);
                Debug.Print("Volume: {0:f3}", (int)(next * 100));
                WaveVolume.SetMasterVolume(next);
                Tooltip(string.Format("Volume: {0}", (int)(next * 100)));
            });

            Explorer.
            @if(WheelDown).
            @do((ctx) =>
            {
                var current = WaveVolume.GetMasterVolume() - 0.02f;
                var next = (current < 0 ? 0 : current);
                Debug.Print("Volume: {0:f3}", (int)(next * 100));
                WaveVolume.SetMasterVolume(next);
                Tooltip(string.Format("Volume: {0}", (int)(next * 100)));
            });
        }
    }
}