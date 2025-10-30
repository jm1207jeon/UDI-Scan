namespace UDIScan.Core.Services
{
    /// <summary>
    /// 키보드 입력 시뮬레이션 인터페이스
    /// </summary>
    public interface IKeyboardSimulator
    {
        /// <summary>
        /// 텍스트를 키보드 입력으로 시뮬레이션
        /// </summary>
        void TypeText(string text);

        /// <summary>
        /// Enter 키 입력
        /// </summary>
        void SendEnter();

        /// <summary>
        /// Tab 키 입력
        /// </summary>
        void SendTab();
    }
}
