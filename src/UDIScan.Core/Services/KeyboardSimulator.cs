using UDIScan.Native;

namespace UDIScan.Core.Services
{
    /// <summary>
    /// 키보드 입력 시뮬레이션 서비스
    /// </summary>
    public class KeyboardSimulator : IKeyboardSimulator
    {
        private readonly InputSimulator _inputSimulator;

        public KeyboardSimulator()
        {
            _inputSimulator = new InputSimulator();
        }

        public void TypeText(string text)
        {
            _inputSimulator.TypeText(text);
        }

        public void SendEnter()
        {
            _inputSimulator.SendEnter();
        }

        public void SendTab()
        {
            _inputSimulator.SendTab();
        }
    }
}
