using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace UDIScan.Native
{
    /// <summary>
    /// 키보드 입력 시뮬레이터
    /// </summary>
    public class InputSimulator
    {
        private const int KeyDelayMs = 1; // 키 입력 간 지연 (밀리초)

        /// <summary>
        /// 텍스트를 키보드 입력으로 시뮬레이션
        /// </summary>
        public void TypeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            foreach (char c in text)
            {
                TypeCharacter(c);
                Thread.Sleep(KeyDelayMs);
            }
        }

        /// <summary>
        /// 단일 문자를 키보드 입력으로 시뮬레이션
        /// </summary>
        private void TypeCharacter(char c)
        {
            // VkKeyScan으로 가상 키 코드와 Shift 상태 확인
            short vkAndShift = NativeMethods.VkKeyScan(c);

            if (vkAndShift == -1)
            {
                // VkKeyScan이 실패하면 유니코드 방식으로 입력
                SendUnicodeChar(c);
                return;
            }

            byte vk = (byte)(vkAndShift & 0xFF);
            byte shiftState = (byte)((vkAndShift >> 8) & 0xFF);

            bool needShift = (shiftState & 1) != 0;
            bool needCtrl = (shiftState & 2) != 0;
            bool needAlt = (shiftState & 4) != 0;

            // Modifier 키 누르기
            List<VirtualKeyCode> modifiers = new List<VirtualKeyCode>();
            if (needShift) modifiers.Add(VirtualKeyCode.VK_SHIFT);
            if (needCtrl) modifiers.Add(VirtualKeyCode.VK_CONTROL);
            if (needAlt) modifiers.Add(VirtualKeyCode.VK_MENU);

            foreach (var modifier in modifiers)
            {
                SendKeyDown(modifier);
            }

            // 실제 키 입력
            SendKeyPress(vk);

            // Modifier 키 떼기 (역순)
            for (int i = modifiers.Count - 1; i >= 0; i--)
            {
                SendKeyUp(modifiers[i]);
            }
        }

        /// <summary>
        /// 유니코드 문자 입력 (VkKeyScan 실패 시)
        /// </summary>
        private void SendUnicodeChar(char c)
        {
            INPUT[] inputs = new INPUT[2];

            // Key Down
            inputs[0] = new INPUT
            {
                Type = NativeMethods.INPUT_KEYBOARD,
                Union = new INPUTUNION
                {
                    KeyboardInput = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = c,
                        Flags = NativeMethods.KEYEVENTF_UNICODE,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            // Key Up
            inputs[1] = new INPUT
            {
                Type = NativeMethods.INPUT_KEYBOARD,
                Union = new INPUTUNION
                {
                    KeyboardInput = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = c,
                        Flags = NativeMethods.KEYEVENTF_UNICODE | NativeMethods.KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 특정 키를 누르고 떼기
        /// </summary>
        public void SendKey(VirtualKeyCode keyCode)
        {
            SendKeyPress((ushort)keyCode);
        }

        /// <summary>
        /// Enter 키 입력
        /// </summary>
        public void SendEnter()
        {
            SendKey(VirtualKeyCode.VK_RETURN);
        }

        /// <summary>
        /// Tab 키 입력
        /// </summary>
        public void SendTab()
        {
            SendKey(VirtualKeyCode.VK_TAB);
        }

        /// <summary>
        /// 키 누르기 (Down + Up)
        /// </summary>
        private void SendKeyPress(ushort vk)
        {
            SendKeyDown((VirtualKeyCode)vk);
            SendKeyUp((VirtualKeyCode)vk);
        }

        /// <summary>
        /// 키 Down 이벤트
        /// </summary>
        private void SendKeyDown(VirtualKeyCode keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT
            {
                Type = NativeMethods.INPUT_KEYBOARD,
                Union = new INPUTUNION
                {
                    KeyboardInput = new KEYBDINPUT
                    {
                        Vk = (ushort)keyCode,
                        Scan = 0,
                        Flags = NativeMethods.KEYEVENTF_KEYDOWN,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 키 Up 이벤트
        /// </summary>
        private void SendKeyUp(VirtualKeyCode keyCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0] = new INPUT
            {
                Type = NativeMethods.INPUT_KEYBOARD,
                Union = new INPUTUNION
                {
                    KeyboardInput = new KEYBDINPUT
                    {
                        Vk = (ushort)keyCode,
                        Scan = 0,
                        Flags = NativeMethods.KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// 현재 포커스된 윈도우 핸들 가져오기
        /// </summary>
        public IntPtr GetForegroundWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }
    }
}
