using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Form
{
    static class msghelper
    {
        public static DialogResult Warn(string s, MessageBoxButtons buttons = MessageBoxButtons.OK, params object[] args)
        {
            return MessageBox.Show(f(s, args), "경고", buttons, MessageBoxIcon.Exclamation);
        }
        public static DialogResult Error(string s, MessageBoxButtons buttons = MessageBoxButtons.OK, params object[] args)
        {
            return MessageBox.Show(f(s, args), "오류", buttons, MessageBoxIcon.Error);
        }
        public static DialogResult Info(string s, MessageBoxButtons buttons = MessageBoxButtons.OK, params object[] args)
        {
            return MessageBox.Show(f(s, args), "알림", buttons, MessageBoxIcon.Information);
        }
        public static DialogResult Show(string s, MessageBoxButtons buttons = MessageBoxButtons.OK, params object[] args)
        {
            return MessageBox.Show(f(s, args), "알림", buttons, 0);
        }
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static string f(string s, params object[] args)
        {
            if (args == null) return s;
            return string.Format(s, args);
        }
    }
}
