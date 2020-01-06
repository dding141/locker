using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LOCKER
{
    public partial class Main : Form
    {
        string preg = "";
        string ereg = "";
        string ureg = "";
        public const int SWP_HIDEWINDOW = 128;
        public const int SWP_SHOWWINDOW = 64;
        public static string pw, pw2, email, uac;

        [DllImport("user32.dll")]
        public static extern int FindWindowA(string IpClassName, string IpWindowName);

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        public Main()
        {
            InitializeComponent();
        }

        private void ShowWin()
        {
            int IRet;
            IRet = FindWindowA("Shell_traywnd", "");

            if (IRet > 0)
            {
                IRet = SetWindowPos(IRet, 0, 0, 0, 0, 0, SWP_SHOWWINDOW);
            }
        }

        private void HideWin()
        {
            int IRet;
            IRet = FindWindowA("Shell_traywnd", "");

            if (IRet > 0)
            {
                IRet = SetWindowPos(IRet, 0, 0, 0, 0, 0, SWP_HIDEWINDOW);
            }
        }

        public void SetTaskManager(bool enable)
        {
            RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
            if (enable && objRegistryKey.GetValue("DisableTaskMgr") != null)
                objRegistryKey.DeleteValue("DisableTaskMgr");
            else
                objRegistryKey.SetValue("DisableTaskMgr", "1");
            objRegistryKey.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            RegistryKey strUpKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            RegistryKey uackey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");

            if (this.mainpwtxt.Text == "")
            {
                MessageBox.Show("Please Enter The Password!", "Notice");
            }
            else
            {
                if (MD5HashFunc(this.mainpwtxt.Text) == pw)
                {
                    try
                    {
                        if (this.checkBox1.Checked == true)
                        {
                            strUpKey.DeleteValue("LOCKER");
                            uackey.SetValue("EnableLUA", 1);
                            uackey.SetValue("ConsentPromptBehaviorAdmin", Convert.ToInt32(uac));
                            SetTaskManager(Convert.ToBoolean(1));
                            MessageBox.Show("Succesfull!" + Environment.NewLine + "(StartUpRemove.Ver)", "Notice");
                            Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
                            this.ShowWin();
                            Process.GetCurrentProcess().Kill();
                        }
                        else
                        {
                            SetTaskManager(Convert.ToBoolean(1));
                            MessageBox.Show("Succesfull!" + Environment.NewLine + "(Normal.Ver)", "Notice");
                            Process.Start(Path.Combine(Environment.GetEnvironmentVariable("windir"), "explorer.exe"));
                            this.ShowWin();
                            Process.GetCurrentProcess().Kill();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Remove StartUp Failed!" + Environment.NewLine + "Please Try to Without Delete StartUp", "Notice");
                    }
                }
                else if (MD5HashFunc(this.mainpwtxt.Text) != pw)
                {
                    MessageBox.Show("Wrong Password!", "Notice");
                }
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey uackey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                RegistryKey strUpKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
                ureg = Convert.ToString(uackey.GetValue("ConsentPromptBehaviorAdmin"));

                if (strUpKey.GetValue("LOCKER") == null)
                {
                    strUpKey.Close();
                    strUpKey.SetValue("LOCKER", Application.ExecutablePath);
                    SetTaskManager(Convert.ToBoolean(0));
                    uackey.SetValue("EnableLUA", 0);
                    uackey.SetValue("ConsentPromptBehaviorAdmin", 0);
                    uac = ureg;
                    this.WindowState = FormWindowState.Maximized;
                    this.TopMost = true;
                    groupBox1.Location = new Point(ClientSize.Width / 2 - groupBox1.Size.Width / 2, ClientSize.Height / 2 - groupBox1.Size.Height / 2);
                    this.HideWin();
                }
                else
                {
                    SetTaskManager(Convert.ToBoolean(0));
                    uac = ureg;
                    this.WindowState = FormWindowState.Maximized;
                    this.TopMost = true;
                    groupBox1.Location = new Point(ClientSize.Width / 2 - groupBox1.Size.Width / 2, ClientSize.Height / 2 - groupBox1.Size.Height / 2);
                    this.HideWin();
                }
            }
            catch
            {
                MessageBox.Show("Add StartUp Failed!" + Environment.NewLine + "Please Start Again!", "Notice");
                Process.GetCurrentProcess().Kill();
            }
            timer1.Enabled = true;
            timer1.Interval = 100;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Process[] p1 = Process.GetProcessesByName("cmd");
                Process[] p2 = Process.GetProcessesByName("powershell");
                Process[] p3 = Process.GetProcessesByName("powershell_ise");
                Process[] p4 = Process.GetProcessesByName("explorer");
                if (p1.Length > 0)
                {
                    p1[0].Kill();
                }
                if (p2.Length > 0)
                {
                    p2[0].Kill();
                }
                if (p3.Length > 0)
                {
                    p3[0].Kill();
                }
                if (p4.Length > 0)
                {
                    p4[0].Kill();
                }
            }
            catch { }
            groupBox1.Location = new Point(ClientSize.Width / 2 - groupBox1.Size.Width / 2, ClientSize.Height / 2 - groupBox1.Size.Height / 2);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            groupBox3.Visible = true;
            groupBox3.Location = new Point(ClientSize.Width / 2 - groupBox3.Size.Width / 2, ClientSize.Height / 2 - groupBox3.Size.Height / 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox3.Visible = false;
            groupBox1.Location = new Point(ClientSize.Width / 2 - groupBox1.Size.Width / 2, ClientSize.Height / 2 - groupBox1.Size.Height / 2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress("minse0204@naver.com");
                message.To.Add(email);
                message.Subject = "[LOCKER] Your Password!";
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Body = $"Thank u for using LOCKER! your password is {pw2}";
                message.BodyEncoding = System.Text.Encoding.UTF8;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.naver.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential("minse0204", "nayoungjjang01");
                smtp.Send(message);
                MessageBox.Show("Please Check Your Email Or Spam Email!", "Notice");

                groupBox1.Visible = true;
                groupBox3.Visible = false;
                groupBox1.Location = new Point(ClientSize.Width / 2 - groupBox1.Size.Width / 2, ClientSize.Height / 2 - groupBox1.Size.Height / 2);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Please Change Email Using Re-Register!", "Notice");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            RE_REGISTER reregister = new RE_REGISTER();
            REGISTER register = new REGISTER();

            if (MD5HashFunc(this.reregisterpwtxt.Text) == "")
            {
                MessageBox.Show("Please Enter The Password!", "Notice");
            }
            else
            {
                if (MD5HashFunc(this.reregisterpwtxt.Text) == pw)
                {
                    RE_REGISTER.pw = pw;
                    RE_REGISTER.email = email;
                    RE_REGISTER.uac = uac;
                    this.Hide();
                    register.Close();
                    reregister.ShowDialog();
                }
                else if (MD5HashFunc(this.reregisterpwtxt.Text) != pw)
                {
                    MessageBox.Show("Wrong Password!", "Notice");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RegistryKey reg;
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");
            preg = Convert.ToString(reg.GetValue("Password"));
            ereg = Convert.ToString(reg.GetValue("Email"));

            if (preg != "" && ereg != "")
            {
                if (deletepwtxt.Text == "")
                {
                    MessageBox.Show("Enter The Password!", "Notice");
                }
                else
                {
                    if (MD5HashFunc(this.deletepwtxt.Text) == pw)
                    {
                        RE_REGISTER reregister = new RE_REGISTER();
                        reregister.Hide();
                        reg.SetValue("Password", "");
                        reg.SetValue("Email", "");
                        reg.SetValue("Mode", "");
                        MessageBox.Show("Succesfull!, Password And Email Is Deleted!" + Environment.NewLine + "Even If Password And Email Is Deleted" + Environment.NewLine + "You Can Use Login And Re-Register!", "Notice");
                    }
                    else if (this.deletepwtxt.Text != pw || this.deletepwtxt.Text == "")
                    {
                        MessageBox.Show("Worng Password!", "Notice");
                    }
                }
            }
            else
            {
                MessageBox.Show("The Password And Email Is Already Deleted!", "Notice");
            }
        }

        public string MD5HashFunc(string str)
        {
            StringBuilder MD5Str = new StringBuilder();
            byte[] byteArr = Encoding.ASCII.GetBytes(str);
            byte[] resultArr = (new MD5CryptoServiceProvider()).ComputeHash(byteArr);

            for (int cnti = 0; cnti < resultArr.Length; cnti++)
            {
                MD5Str.Append(resultArr[cnti].ToString("X2"));
            }
            return MD5Str.ToString();
        }

        #region KeyDown
        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                this.button11.PerformClick();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                this.button4.PerformClick();
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                this.button5.PerformClick();
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void checkBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }

        private void button3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F4)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.Control && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
            else if (e.Alt && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin && e.KeyCode == Keys.E)
            {
                e.Handled = true;
            }
            else if (e.Control && e.Alt && e.KeyCode == Keys.Delete)
            {
                e.Handled = true;
            }
        }
    }
    #endregion
}
