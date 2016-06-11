﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace CreviceApp.WinAPI.InputSender
{
    public class InputSender
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [StructLayout(LayoutKind.Sequential)]
        protected struct INPUT
        {
            public int type;
            public INPUTDATA data;
        }

        [StructLayout(LayoutKind.Explicit)]
        protected struct INPUTDATA
        {
            [FieldOffset(0)]
            public MOUSEINPUT asMouseInput;
            [FieldOffset(0)]
            public KEYBDINPUT asKeyboardInput;
            [FieldOffset(0)]
            public HARDWAREINPUT asHardwareInput;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct WHEELDELTA
        {
            public int delta;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct XBUTTON
        {
            public int type;
        }

        [StructLayout(LayoutKind.Explicit)]
        protected struct MOUSEDATA
        {
            [FieldOffset(0)]
            public WHEELDELTA asWheelDelta;
            [FieldOffset(0)]
            public XBUTTON asXButton;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public MOUSEDATA mouseData;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        protected struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        // https://msdn.microsoft.com/ja-jp/library/windows/desktop/dd375731(v=vs.85).aspx
        public static class VirtualKeys
        {
            public const ushort VK_LBUTTON              = 0x01; // Left mouse button
            public const ushort VK_RBUTTON              = 0x02; // Right mouse button
            public const ushort VK_CANCEL               = 0x03; // Control-break processing
            public const ushort VK_MBUTTON              = 0x04; // Middle mouse button (three-button mouse)
            public const ushort VK_XBUTTON1             = 0x05; // X1 mouse button
            public const ushort VK_XBUTTON2             = 0x06; // X2 mouse button
            //                  -                         0x07  // Undefined
            public const ushort VK_BACK                 = 0x08; // BACKSPACE key
            public const ushort VK_TAB                  = 0x09; // TAB key
            //                  -                         0x0A-0B // Reserved
            public const ushort VK_CLEAR                = 0x0C; // CLEAR key
            public const ushort VK_RETURN               = 0x0D; // ENTER key
            //                  -                         0x0E-0F // Undefined
            public const ushort VK_SHIFT                = 0x10; // SHIFT key
            public const ushort VK_CONTROL              = 0x11; // CTRL key
            public const ushort VK_MENU                 = 0x12; // ALT key
            public const ushort VK_PAUSE                = 0x13; // PAUSE key
            public const ushort VK_CAPITAL              = 0x14; // CAPS LOCK key
            public const ushort VK_KANA                 = 0x15; // IME Kana mode
            public const ushort VK_HANGUEL              = 0x15; // IME Hanguel mode (maintained for compatibility; use VK_HANGUL)
            public const ushort VK_HANGUL               = 0x15; // IME Hangul mode
            //                  -                         0x16  // Undefined
            public const ushort VK_JUNJA                = 0x17; // IME Junja mode
            public const ushort VK_FINAL                = 0x18; // IME final mode
            public const ushort VK_HANJA                = 0x19; // IME Hanja mode
            public const ushort VK_KANJI                = 0x19; // IME Kanji mode
            //                  -                         0x1A  // Undefined
            public const ushort VK_ESCAPE               = 0x1B; // ESC key
            public const ushort VK_CONVERT              = 0x1C; // IME convert
            public const ushort VK_NONCONVERT           = 0x1D; // IME nonconvert
            public const ushort VK_ACCEPT               = 0x1E; // IME accept
            public const ushort VK_MODECHANGE           = 0x1F; // IME mode change request
            public const ushort VK_SPACE                = 0x20; // SPACEBAR
            public const ushort VK_PRIOR                = 0x21; // PAGE UP key
            public const ushort VK_NEXT                 = 0x22; // PAGE DOWN key
            public const ushort VK_END                  = 0x23; // END key
            public const ushort VK_HOME                 = 0x24; // HOME key
            public const ushort VK_LEFT                 = 0x25; // LEFT ARROW key
            public const ushort VK_UP                   = 0x26; // UP ARROW key
            public const ushort VK_RIGHT                = 0x27; // RIGHT ARROW key
            public const ushort VK_DOWN                 = 0x28; // DOWN ARROW key
            public const ushort VK_SELECT               = 0x29; // SELECT key
            public const ushort VK_PRINT                = 0x2A; // PRINT key
            public const ushort VK_EXECUTE              = 0x2B; // EXECUTE key
            public const ushort VK_SNAPSHOT             = 0x2C; // PRINT SCREEN key
            public const ushort VK_INSERT               = 0x2D; // INS key
            public const ushort VK_DELETE               = 0x2E; // DEL key
            public const ushort VK_HELP                 = 0x2F; // HELP key
            public const ushort VK_0                    = 0x30; // 0 key
            public const ushort VK_1                    = 0x31; // 1 key
            public const ushort VK_2                    = 0x32; // 2 key
            public const ushort VK_3                    = 0x33; // 3 key
            public const ushort VK_4                    = 0x34; // 4 key
            public const ushort VK_5                    = 0x35; // 5 key
            public const ushort VK_6                    = 0x36; // 6 key
            public const ushort VK_7                    = 0x37; // 7 key
            public const ushort VK_8                    = 0x38; // 8 key
            public const ushort VK_9                    = 0x39; // 9 key
            //                  -                         0x3A-40 // Undefined
            public const ushort VK_A                    = 0x41; // A key
            public const ushort VK_B                    = 0x42; // B key
            public const ushort VK_C                    = 0x43; // C key
            public const ushort VK_D                    = 0x44; // D key
            public const ushort VK_E                    = 0x45; // E key
            public const ushort VK_F                    = 0x46; // F key
            public const ushort VK_G                    = 0x47; // G key
            public const ushort VK_H                    = 0x48; // H key
            public const ushort VK_I                    = 0x49; // I key
            public const ushort VK_J                    = 0x4A; // J key
            public const ushort VK_K                    = 0x4B; // K key
            public const ushort VK_L                    = 0x4C; // L key
            public const ushort VK_M                    = 0x4D; // M key
            public const ushort VK_N                    = 0x4E; // N key
            public const ushort VK_O                    = 0x4F; // O key
            public const ushort VK_P                    = 0x50; // P key
            public const ushort VK_Q                    = 0x51; // Q key
            public const ushort VK_R                    = 0x52; // R key
            public const ushort VK_S                    = 0x53; // S key
            public const ushort VK_T                    = 0x54; // T key
            public const ushort VK_U                    = 0x55; // U key
            public const ushort VK_V                    = 0x56; // V key
            public const ushort VK_W                    = 0x57; // W key
            public const ushort VK_X                    = 0x58; // X key
            public const ushort VK_Y                    = 0x59; // Y key
            public const ushort VK_Z                    = 0x5A; // Z key
            public const ushort VK_LWIN                 = 0x5B; // Left Windows key (Natural keyboard)
            public const ushort VK_RWIN                 = 0x5C; // Right Windows key (Natural keyboard)
            public const ushort VK_APPS                 = 0x5D; // Applications key (Natural keyboard)
            //                  -                         0x5E  // Reserved
            public const ushort VK_SLEEP                = 0x5F; // Computer Sleep key
            public const ushort VK_NUMPAD0              = 0x60; // Numeric keypad 0 key
            public const ushort VK_NUMPAD1              = 0x61; // Numeric keypad 1 key
            public const ushort VK_NUMPAD2              = 0x62; // Numeric keypad 2 key
            public const ushort VK_NUMPAD3              = 0x63; // Numeric keypad 3 key
            public const ushort VK_NUMPAD4              = 0x64; // Numeric keypad 4 key
            public const ushort VK_NUMPAD5              = 0x65; // Numeric keypad 5 key
            public const ushort VK_NUMPAD6              = 0x66; // Numeric keypad 6 key
            public const ushort VK_NUMPAD7              = 0x67; // Numeric keypad 7 key
            public const ushort VK_NUMPAD8              = 0x68; // Numeric keypad 8 key
            public const ushort VK_NUMPAD9              = 0x69; // Numeric keypad 9 key
            public const ushort VK_MULTIPLY             = 0x6A; // Multiply key
            public const ushort VK_ADD                  = 0x6B; // Add key
            public const ushort VK_SEPARATOR            = 0x6C; // Separator key
            public const ushort VK_SUBTRACT             = 0x6D; // Subtract key
            public const ushort VK_DECIMAL              = 0x6E; // Decimal key
            public const ushort VK_DIVIDE               = 0x6F; // Divide key
            public const ushort VK_F1                   = 0x70; // F1 key
            public const ushort VK_F2                   = 0x71; // F2 key
            public const ushort VK_F3                   = 0x72; // F3 key
            public const ushort VK_F4                   = 0x73; // F4 key
            public const ushort VK_F5                   = 0x74; // F5 key
            public const ushort VK_F6                   = 0x75; // F6 key
            public const ushort VK_F7                   = 0x76; // F7 key
            public const ushort VK_F8                   = 0x77; // F8 key
            public const ushort VK_F9                   = 0x78; // F9 key
            public const ushort VK_F10                  = 0x79; // F10 key
            public const ushort VK_F11                  = 0x7A; // F11 key
            public const ushort VK_F12                  = 0x7B; // F12 key
            public const ushort VK_F13                  = 0x7C; // F13 key
            public const ushort VK_F14                  = 0x7D; // F14 key
            public const ushort VK_F15                  = 0x7E; // F15 key
            public const ushort VK_F16                  = 0x7F; // F16 key
            public const ushort VK_F17                  = 0x80; // F17 key
            public const ushort VK_F18                  = 0x81; // F18 key
            public const ushort VK_F19                  = 0x82; // F19 key
            public const ushort VK_F20                  = 0x83; // F20 key
            public const ushort VK_F21                  = 0x84; // F21 key
            public const ushort VK_F22                  = 0x85; // F22 key
            public const ushort VK_F23                  = 0x86; // F23 key
            public const ushort VK_F24                  = 0x87; // F24 key
            //                  -                         0x88-8F // Unassigned
            public const ushort VK_NUMLOCK              = 0x90; // NUM LOCK key
            public const ushort VK_SCROLL               = 0x91; // SCROLL LOCK key
            //                                            0x92-96 // OEM specific
            //                  -                         0x97-9F // Unassigned
            public const ushort VK_LSHIFT               = 0xA0; // Left SHIFT key
            public const ushort VK_RSHIFT               = 0xA1; // Right SHIFT key
            public const ushort VK_LCONTROL             = 0xA2; // Left CONTROL key
            public const ushort VK_RCONTROL             = 0xA3; // Right CONTROL key
            public const ushort VK_LMENU                = 0xA4; // Left MENU key
            public const ushort VK_RMENU                = 0xA5; // Right MENU key
            public const ushort VK_BROWSER_BACK         = 0xA6; // Browser Back key
            public const ushort VK_BROWSER_FORWARD      = 0xA7; // Browser Forward key
            public const ushort VK_BROWSER_REFRESH      = 0xA8; // Browser Refresh key
            public const ushort VK_BROWSER_STOP         = 0xA9; // Browser Stop key
            public const ushort VK_BROWSER_SEARCH       = 0xAA; // Browser Search key
            public const ushort VK_BROWSER_FAVORITES    = 0xAB; // Browser Favorites key
            public const ushort VK_BROWSER_HOME         = 0xAC; // Browser Start and Home key
            public const ushort VK_VOLUME_MUTE          = 0xAD; // Volume Mute key
            public const ushort VK_VOLUME_DOWN          = 0xAE; // Volume Down key
            public const ushort VK_VOLUME_UP            = 0xAF; // Volume Up key
            public const ushort VK_MEDIA_NEXT_TRACK     = 0xB0; // Next Track key
            public const ushort VK_MEDIA_PREV_TRACK     = 0xB1; // Previous Track key
            public const ushort VK_MEDIA_STOP           = 0xB2; // Stop Media key
            public const ushort VK_MEDIA_PLAY_PAUSE     = 0xB3; // Play/Pause Media key
            public const ushort VK_LAUNCH_MAIL          = 0xB4; // Start Mail key
            public const ushort VK_LAUNCH_MEDIA_SELECT  = 0xB5; // Select Media key
            public const ushort VK_LAUNCH_APP1          = 0xB6; // Start Application 1 key
            public const ushort VK_LAUNCH_APP2          = 0xB7; // Start Application 2 key
            //                  -                         0xB8-B9 // Reserved
            public const ushort VK_OEM_1                = 0xBA; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the ';:' key = VK_OEM_PLUS, // 0xBB
                                                                // For any country/region, the '+' key
            public const ushort VK_OEM_COMMA            = 0xBC; // For any country/region, the ',' key
            public const ushort VK_OEM_MINUS            = 0xBD; // For any country/region, the '-' key
            public const ushort VK_OEM_PERIOD           = 0xBE; // For any country/region, the '.' key
            public const ushort VK_OEM_2                = 0xBF; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the '/?' key
            public const ushort VK_OEM_3                = 0xC0; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the '`~' key
            //                  -                         0xC1-D7 // Reserved
            //                  -                         0xD8-DA // Unassigned
            public const ushort VK_OEM_4                = 0xDB; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the '[{' key
            public const ushort VK_OEM_5                = 0xDC; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the '\|' key
            public const ushort VK_OEM_6                = 0xDD; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the ']}' key
            public const ushort VK_OEM_7                = 0xDE; // Used for miscellaneous characters; it can vary by keyboard.
                                                                // For the US standard keyboard, the 'single-quote/double-quote' key
            public const ushort VK_OEM_8                = 0xDF; // Used for miscellaneous characters; it can vary by keyboard.
            //                  -                         0xE0 // Reserved
            //                                            0xE1 // OEM specific
            public const ushort VK_OEM_102              = 0xE2; // Either the angle bracket key or the backslash key on the RT 102-key keyboard
            //                                            0xE3-E4 // OEM specific
            public const ushort VK_PROCESSKEY           = 0xE5; // IME PROCESS key
            //                                            0xE6 // OEM specific
            public const ushort VK_PACKET               = 0xE7; // Used to pass Unicode characters as if they were keystrokes. The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information, see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
            //                  -                         0xE8 // Unassigned
            //                                            0xE9-F5 // OEM specific
            public const ushort VK_ATTN                 = 0xF6; // Attn key
            public const ushort VK_CRSEL                = 0xF7; // CrSel key
            public const ushort VK_EXSEL                = 0xF8; // ExSel key
            public const ushort VK_EREOF                = 0xF9; // Erase EOF key
            public const ushort VK_PLAY                 = 0xFA; // Play key
            public const ushort VK_ZOOM                 = 0xFB; // Zoom key
            public const ushort VK_NONAME               = 0xFC; // Reserved
            public const ushort VK_PA1                  = 0xFD; // PA1 key
            public const ushort VK_OEM_CLEAR            = 0xFE; // 
        }

        private const int WHEEL_DELTA = 120;

        private const int XBUTTON1 = 0x0001;
        private const int XBUTTON2 = 0x0002;

        private const int INPUT_MOUSE    = 0;
        private const int INPUT_KEYBOARD = 1;
        private const int INPUT_HARDWARE = 2;

        // https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms646260(v=vs.85).aspx
        private const int MOUSEEVENTF_MOVE       = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN   = 0x0002;
        private const int MOUSEEVENTF_LEFTUP     = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN  = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP    = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP   = 0x0040;
        private const int MOUSEEVENTF_XDOWN      = 0x0080;
        private const int MOUSEEVENTF_XUP        = 0x0100;
        private const int MOUSEEVENTF_WHEEL      = 0x0800;
        private const int MOUSEEVENTF_HWHEEL     = 0x1000;
        private const int MOUSEEVENTF_ABSOLUTE   = 0x8000;
        
        private const int KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const int KEYEVENTF_KEYUP       = 0x0002;
        private const int KEYEVENTF_UNICODE     = 0x0004;
        private const int KEYEVENTF_SCANCODE    = 0x0008;
        


        private readonly UIntPtr MOUSEEVENTF_CREVICE_APP = new UIntPtr(0xFFFFFF00);

        protected void Send(INPUT[] input)
        {
            Debug.Print("calling a native method SendInput");
            foreach (var x in input)
            {
                switch (x.type)
                {
                    case INPUT_MOUSE:
                        Debug.Print("dx: {0}", x.data.asMouseInput.dx);
                        Debug.Print("dy: {0}", x.data.asMouseInput.dy);
                        Debug.Print("asWheelDelta.delta: {0}", x.data.asMouseInput.mouseData.asWheelDelta.delta);
                        Debug.Print("asXButton.type: {0}", x.data.asMouseInput.mouseData.asXButton.type);
                        Debug.Print("dwFlags: {0}",BitConverter.ToString(BitConverter.GetBytes(x.data.asMouseInput.dwFlags)));
                        Debug.Print("dwExtraInfo: {0}", BitConverter.ToString(BitConverter.GetBytes(x.data.asMouseInput.dwExtraInfo.ToUInt64())));
                        break;
                    case INPUT_KEYBOARD:
                        Debug.Print("wVk: {0}", x.data.asKeyboardInput.wVk);
                        Debug.Print("wScan: {0}", x.data.asKeyboardInput.wScan);
                        Debug.Print("dwFlags: {0}", BitConverter.ToString(BitConverter.GetBytes(x.data.asKeyboardInput.dwFlags)));
                        Debug.Print("dwExtraInfo: {0}", BitConverter.ToString(BitConverter.GetBytes(x.data.asKeyboardInput.dwExtraInfo.ToUInt64())));
                        break;
                    case INPUT_HARDWARE:
                        break;
                }
            }

            if (SendInput((uint)input.Length, input, Marshal.SizeOf(input[0])) > 0)
            {
                Debug.Print("success");
            }
            else
            {
                int errorCode = Marshal.GetLastWin32Error();
                Debug.Print("SendInput failed; ErrorCode: {0}", errorCode);
            }
        }

        protected INPUT ToInput(MOUSEINPUT mouseInput)
        {
            var input = new INPUT();
            input.type = INPUT_MOUSE;
            input.data.asMouseInput = mouseInput;
            return input;
        }

        protected INPUT ToInput(KEYBDINPUT keyboardInput)
        {
            var input = new INPUT();
            input.type = INPUT_KEYBOARD;
            input.data.asKeyboardInput = keyboardInput;
            return input;
        }

        private MOUSEINPUT GetCreviceMouseInput()
        {
            var mouseInput = new MOUSEINPUT();
            // Set the CreviceApp signature to the mouse event
            mouseInput.dwExtraInfo = MOUSEEVENTF_CREVICE_APP;
            mouseInput.time = 0;
            return mouseInput;
        }
        
        protected MOUSEINPUT MouseLeftDownEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_LEFTDOWN;
            return mouseInput;
        }

        protected MOUSEINPUT MouseLeftUpEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_LEFTUP;
            return mouseInput;
        }

        protected MOUSEINPUT MouseRightDownEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_RIGHTDOWN;
            return mouseInput;
        }

        protected MOUSEINPUT MouseRightUpEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_RIGHTUP;
            return mouseInput;
        }

        protected MOUSEINPUT MouseMoveEvent(int dx, int dy)
        {
            var x = Cursor.Position.X + dx;
            var y = Cursor.Position.Y + dy;
            return MouseMoveToEvent(x, y);
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646273(v=vs.85).aspx
        protected MOUSEINPUT MouseMoveToEvent(int x, int y)
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dx = (x + 1) * 0xFFFF / Screen.PrimaryScreen.Bounds.Width;
            mouseInput.dy = (y + 1) * 0xFFFF / Screen.PrimaryScreen.Bounds.Height;
            mouseInput.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            return mouseInput;
        }

        protected MOUSEINPUT MouseMiddleDownEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_MIDDLEDOWN;
            return mouseInput;
        }

        protected MOUSEINPUT MouseMiddleUpEvent()
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_MIDDLEUP;
            return mouseInput;
        }

        protected MOUSEINPUT MouseVerticalWheelEvent(int delta)
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_WHEEL;
            mouseInput.mouseData.asWheelDelta.delta = delta;
            return mouseInput;
        }

        protected MOUSEINPUT MouseWheelDownEvent()
        {
            return MouseVerticalWheelEvent(-120);
        }

        protected MOUSEINPUT MouseWheelUpEvent()
        {
            return MouseVerticalWheelEvent(120);
        }
        
        protected MOUSEINPUT MouseHorizontalWheelEvent(int delta)
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_HWHEEL;
            mouseInput.mouseData.asWheelDelta.delta = delta;
            return mouseInput;
        }

        protected MOUSEINPUT MouseWheelRightEvent()
        {
            return MouseHorizontalWheelEvent(-120);
        }

        protected MOUSEINPUT MouseWheelLeftEvent()
        {
            return MouseHorizontalWheelEvent(120);
        }

        private MOUSEINPUT MouseXUpEvent(int type)
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_XUP;
            mouseInput.mouseData.asXButton.type = type;
            return mouseInput;
        }

        protected MOUSEINPUT MouseX1UpEvent()
        {
            return MouseXUpEvent(XBUTTON1);
        }

        protected MOUSEINPUT MouseX2UpEvent()
        {
            return MouseXUpEvent(XBUTTON2);
        }

        private MOUSEINPUT MouseXDownEvent(int type)
        {
            var mouseInput = GetCreviceMouseInput();
            mouseInput.dwFlags = MOUSEEVENTF_XDOWN;
            mouseInput.mouseData.asXButton.type = type;
            return mouseInput;
        }

        protected MOUSEINPUT MouseX1DownEvent()
        {
            return MouseXDownEvent(XBUTTON1);
        }

        protected MOUSEINPUT MouseX2DownEvent()
        {
            return MouseXDownEvent(XBUTTON2);
        }

        private KEYBDINPUT KeyEvent(ushort keyCode)
        {
            var keyboardInput = new KEYBDINPUT();
            keyboardInput.wVk = keyCode;
            return keyboardInput;
        }

        private KEYBDINPUT ExtendedKeyEvent(ushort keyCode)
        {
            var keyboardInput = KeyEvent(keyCode);
            keyboardInput.dwFlags = KEYEVENTF_EXTENDEDKEY;
            return keyboardInput;
        }

        private KEYBDINPUT KeyWithScanCodeEvent(ushort keyCode)
        {
            var keyboardInput = KeyEvent(keyCode);
            keyboardInput.wScan = (ushort)MapVirtualKey(keyCode, 0);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_SCANCODE;
            return keyboardInput;
        }

        private KEYBDINPUT ExtendedKeyWithScanCodeEvent(ushort keyCode)
        {
            var keyboardInput = ExtendedKeyEvent(keyCode);
            keyboardInput.wScan = (ushort)MapVirtualKey(keyCode, 0);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_SCANCODE;
            return keyboardInput;
        }

        private KEYBDINPUT UnicodeKeyEvent(char c)
        {
            var keyboardInput = new KEYBDINPUT();
            keyboardInput.wScan = c;
            keyboardInput.dwFlags = KEYEVENTF_UNICODE;
            return keyboardInput;
        }

        protected KEYBDINPUT KeyUpEvent(ushort keyCode)
        {
            var keyboardInput = KeyEvent(keyCode);
            keyboardInput.dwFlags = KEYEVENTF_KEYUP;
            return keyboardInput;
        }

        protected KEYBDINPUT ExtendedKeyUpEvent(ushort keyCode)
        {
            var keyboardInput = ExtendedKeyEvent(keyCode);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_KEYUP;
            return keyboardInput;
        }

        protected KEYBDINPUT KeyUpWithScanCodeEvent(ushort keyCode)
        {
            var keyboardInput = KeyWithScanCodeEvent(keyCode);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_KEYUP;
            return keyboardInput;

        }

        protected KEYBDINPUT ExtendedKeyUpWithScanCodeEvent(ushort keyCode)
        {
            var keyboardInput = ExtendedKeyWithScanCodeEvent(keyCode);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_KEYUP;
            return keyboardInput;

        }

        protected KEYBDINPUT UnicodeKeyUpEvent(char c)
        {
            var keyboardInput = UnicodeKeyEvent(c);
            keyboardInput.dwFlags = keyboardInput.dwFlags | KEYEVENTF_KEYUP;
            return keyboardInput;
        }

        protected KEYBDINPUT KeyDownEvent(ushort keyCode)
        {
            return KeyEvent(keyCode);
        }

        protected KEYBDINPUT ExtendedKeyDownEvent(ushort keyCode)
        {
            return ExtendedKeyEvent(keyCode);
        }

        protected KEYBDINPUT KeyDownWithScanCodeEvent(ushort keyCode)
        {
            return KeyWithScanCodeEvent(keyCode);
        }

        protected KEYBDINPUT ExtendedKeyDownWithScanCodeEvent(ushort keyCode)
        {
            return ExtendedKeyWithScanCodeEvent(keyCode);
        }

        protected KEYBDINPUT UnicodeKeyDownEvent(char c)
        {
            return UnicodeKeyEvent(c);
        }
    }

    public class SingleInputSender : InputSender
    {
        protected void Send(params MOUSEINPUT[] mouseInput)
        {
            var input = new INPUT[mouseInput.Length];
            for (var i = 0; i < mouseInput.Length; i++)
            {
                input[i] = ToInput(mouseInput[i]);
            }
            Send(input);
        }

        protected void Send(params KEYBDINPUT[] keyboardInput)
        {
            var input = new INPUT[keyboardInput.Length];
            for (var i = 0; i < keyboardInput.Length; i++)
            {
                input[i] = ToInput(keyboardInput[i]);
            }
            Send(input);
        }

        public void LeftDown()
        {
            Send(MouseLeftDownEvent());
        }

        public void LeftUp()
        {
            Send(MouseLeftUpEvent());
        }

        public void LeftClick()
        {
            Send(MouseLeftDownEvent(), MouseLeftUpEvent());
        }

        public void RightDown()
        {
            Send(MouseRightDownEvent());
        }

        public void RightUp()
        {
            Send(MouseRightUpEvent());
        }

        public void RightClick()
        {
            Send(MouseRightDownEvent(), MouseRightUpEvent());
        }

        public void Move(int dx, int dy)
        {
            Send(MouseMoveEvent(dx, dy));
        }

        public void MoveTo(int x, int y)
        {
            Send(MouseMoveToEvent(x, y));
        }

        public void MiddleDown()
        {
            Send(MouseMiddleDownEvent());
        }

        public void MiddleUp()
        {
            Send(MouseMiddleUpEvent());
        }

        public void MiddleClick()
        {
            Send(MouseMiddleDownEvent(), MouseMiddleUpEvent());
        }

        public void VerticalWheel(short delta)
        {
            Send(MouseVerticalWheelEvent(delta));
        }

        public void WheelDown()
        {
            Send(MouseVerticalWheelEvent(120));
        }

        public void WheelUp()
        {
            Send(MouseVerticalWheelEvent(-120));
        }
        
        public void HorizontalWheel(short delta)
        {
            Send(MouseHorizontalWheelEvent(delta));
        }

        public void WheelLeft()
        {
            Send(MouseHorizontalWheelEvent(120));
        }

        public void WheelRight()
        {
            Send(MouseHorizontalWheelEvent(-120));
        }

        public void X1Down()
        {
            Send(MouseX1DownEvent());
        }

        public void X1Up()
        {
            Send(MouseX1UpEvent());
        }
        
        public void X1Click()
        {
            Send(MouseX1DownEvent(), MouseX1UpEvent());
        }

        public void X2Down()
        {
            Send(MouseX2DownEvent());
        }

        public void X2Up()
        {
            Send(MouseX2UpEvent());
        }
        
        public void X2Click()
        {
            Send(MouseX2DownEvent(), MouseX2UpEvent());
        }

        public void KeyDown(ushort keyCode)
        {
            Send(KeyDownEvent(keyCode));
        }

        public void KeyUp(ushort keyCode)
        {
            Send(KeyUpEvent(keyCode));
        }

        public void ExtendedKeyDown(ushort keyCode)
        {
            Send(ExtendedKeyDownEvent(keyCode));
        }

        public void ExtendedKeyUp(ushort keyCode)
        {
            Send(ExtendedKeyUpEvent(keyCode));
        }

        public void KeyDownWithScanCode(ushort keyCode)
        {
            Send(KeyDownWithScanCodeEvent(keyCode));
        }

        public void KeyUpWithScanCode(ushort keyCode)
        {
            Send(KeyUpWithScanCodeEvent(keyCode));
        }

        public void ExtendedKeyDownWithScanCode(ushort keyCode)
        {
            Send(ExtendedKeyDownWithScanCodeEvent(keyCode));
        }

        public void ExtendedKeyUpWithScanCode(ushort keyCode)
        {
            Send(ExtendedKeyUpWithScanCodeEvent(keyCode));
        }

        public void UnicodeKeyDown(char c)
        {
            Send(UnicodeKeyDownEvent(c));
        }

        public void UnicodeKeyUp(char c)
        {
            Send(UnicodeKeyUpEvent(c));
        }
        
        public void UnicodeKeyStroke(string str)
        {
            var list = str
                .Select(c => new List<KEYBDINPUT>() { UnicodeKeyDownEvent(c), UnicodeKeyUpEvent(c) })
                .SelectMany(x => x);
            Send(list.ToArray());
        }
    }

    public class InputSequenceBuilder : InputSender
    {
        private readonly IEnumerable<INPUT> buffer;

        public InputSequenceBuilder() : this(new List<INPUT>())
        {

        }

        private InputSequenceBuilder(IEnumerable<INPUT> xs)
        {
            this.buffer = xs;
        }
        
        private InputSequenceBuilder NewInstance(IEnumerable<INPUT> ys)
        {
            var xs = this.buffer.ToList();
            xs.AddRange(ys);
            return new InputSequenceBuilder(xs);
        }

        private InputSequenceBuilder NewInstance(params MOUSEINPUT[] mouseEvent)
        {
            return NewInstance(mouseEvent.Select(x => ToInput(x)));
        }

        private InputSequenceBuilder NewInstance(params KEYBDINPUT[] keyboardEvent)
        {
            return NewInstance(keyboardEvent.Select(x => ToInput(x)));
        }

        public InputSequenceBuilder LeftDown()
        {
            return NewInstance(MouseLeftDownEvent());
        }

        public InputSequenceBuilder LeftUp()
        {
            return NewInstance(MouseLeftUpEvent());
        }

        public InputSequenceBuilder LeftClick()
        {
            return NewInstance(MouseLeftDownEvent(), MouseLeftUpEvent());
        }
    
        public InputSequenceBuilder RightDown()
        {
            return NewInstance(MouseRightDownEvent());
        }

        public InputSequenceBuilder RightUp()
        {
            return NewInstance(MouseRightUpEvent());
        }

        public InputSequenceBuilder RightClick()
        {
            return NewInstance(MouseRightDownEvent(), MouseRightUpEvent());
        }

        public InputSequenceBuilder Move(int dx, int dy)
        {
            return NewInstance(MouseMoveEvent(dx, dy));
        }

        public InputSequenceBuilder MoveTo(int x, int y)
        {
            return NewInstance(MouseMoveToEvent(x, y));
        }

        public InputSequenceBuilder MiddleDown()
        {
            return NewInstance(MouseMiddleDownEvent());
        }

        public InputSequenceBuilder MiddleUp()
        {
            return NewInstance(MouseMiddleUpEvent());
        }

        public InputSequenceBuilder MiddleClick()
        {
            return NewInstance(MouseMiddleDownEvent(), MouseMiddleUpEvent());
        }

        public InputSequenceBuilder VerticalWheel(short delta)
        {
            return NewInstance(MouseVerticalWheelEvent(delta));
        }

        public InputSequenceBuilder WheelDown()
        {
            return NewInstance(MouseVerticalWheelEvent(120));
        }

        public InputSequenceBuilder WheelUp()
        {
            return NewInstance(MouseVerticalWheelEvent(-120));
        }

        public InputSequenceBuilder HorizontalWheel(short delta)
        {
            return NewInstance(MouseHorizontalWheelEvent(delta));
        }

        public InputSequenceBuilder WheelLeft()
        {
            return NewInstance(MouseHorizontalWheelEvent(120));
        }

        public InputSequenceBuilder WheelRight()
        {
            return NewInstance(MouseHorizontalWheelEvent(-120));
        }

        public InputSequenceBuilder X1Down()
        {
            return NewInstance(MouseX1DownEvent());
        }

        public InputSequenceBuilder X1Up()
        {
            return NewInstance(MouseX1UpEvent());
        }

        public InputSequenceBuilder X1Click()
        {
            return NewInstance(MouseX1DownEvent(), MouseX1UpEvent());
        }

        public InputSequenceBuilder X2Down()
        {
            return NewInstance(MouseX2UpEvent());
        }

        public InputSequenceBuilder X2Up()
        {
            return NewInstance(MouseX2UpEvent());
        }

        public InputSequenceBuilder X2Click()
        {
            return NewInstance(MouseX2DownEvent(), MouseX2UpEvent());
        }

        public InputSequenceBuilder KeyDown(ushort keyCode)
        {
            return NewInstance(KeyDownEvent(keyCode));
        }

        public InputSequenceBuilder KeyUp(ushort keyCode)
        {
            return NewInstance(KeyUpEvent(keyCode));
        }

        public InputSequenceBuilder ExtendedKeyDown(ushort keyCode)
        {
            return NewInstance(ExtendedKeyDownEvent(keyCode));
        }

        public InputSequenceBuilder ExtendedKeyUp(ushort keyCode)
        {
            return NewInstance(ExtendedKeyUpEvent(keyCode));
        }

        public InputSequenceBuilder KeyDownWithScanCode(ushort keyCode)
        {
            return NewInstance(KeyDownWithScanCodeEvent(keyCode));
        }

        public InputSequenceBuilder KeyUpWithScanCode(ushort keyCode)
        {
            return NewInstance(KeyUpWithScanCodeEvent(keyCode));
        }

        public InputSequenceBuilder ExtendedKeyDownWithScanCode(ushort keyCode)
        {
            return NewInstance(ExtendedKeyDownWithScanCodeEvent(keyCode));
        }

        public InputSequenceBuilder ExtendedKeyUpWithScanCode(ushort keyCode)
        {
            return NewInstance(ExtendedKeyUpWithScanCodeEvent(keyCode));
        }

        public InputSequenceBuilder UnicodeKeyDown(char c)
        {
            return NewInstance(UnicodeKeyDownEvent(c));
        }

        public InputSequenceBuilder UnicodeKeyUp(char c)
        {
            return NewInstance(UnicodeKeyUpEvent(c));
        }

        public InputSequenceBuilder UnicodeKeyStroke(string str)
        {
            return NewInstance(str
                .Select(c => new List<KEYBDINPUT>() { UnicodeKeyDownEvent(c), UnicodeKeyUpEvent(c) })
                .SelectMany(x => x)
                .ToArray());
        }

        public void Send()
        {
            Send(buffer.ToArray());
        }
    }
}
