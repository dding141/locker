using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace LOCKER
{
    public partial class RE_REGISTER : Form
    {
        RegistryKey reg;
        string preg, ereg = "";
        public static string pw, email, uac;

        public RE_REGISTER()
        {
            InitializeComponent();
        }

        private void RE_REGISTER_Load(object sender, EventArgs e)
        {
            RegistryKey explorerreg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer2.Enabled = true;
            timer2.Interval = 100;
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");
            preg = Convert.ToString(reg.GetValue("Password"));
            ereg = Convert.ToString(reg.GetValue("Email"));
            explorerreg.SetValue("AutoRestartShell", 0);

            if (preg == "" && ereg == "")
            {
                MessageBox.Show("There Is No Information About Password And Email!", "Notice");
                MessageBox.Show("Please Enter The New Password And Email!", "Notice");
            }
            else if (preg != "" && ereg != "")
            {
                this.pwtxt.Text = preg;
                this.emailtxt.Text = ereg;
                this.pwconfirmtxt.Text = preg;
            }
            this.TopMost = true;
        }

        private void RE_REGISTER_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
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
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
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

        private void timer1_Tick(object sender, EventArgs e)
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

        private void metroButton1_Click(object sender, EventArgs e)
        {
            RegistryKey explorerreg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");

            if (this.pwtxt.Text == "" || this.emailtxt.Text == "")
            {
                if (this.pwtxt.Text == "")
                {
                    MessageBox.Show("Enter The Password!");
                }
                else if (this.emailtxt.Text == "")
                {
                    MessageBox.Show("Enter The Email!");
                }
            }
            else
            {
                if (this.pwtxt.Text == this.pwconfirmtxt.Text)
                {
                    if (this.emailtxt.Text.Contains("@") && this.emailtxt.Text.Contains(".com") || this.emailtxt.Text.Contains(".net") || this.emailtxt.Text.Contains(".co.kr"))
                    {
                        if (this.pwtxt.Text.Contains(" "))
                        {
                            MessageBox.Show("You Cant Contain SpaceBar In Password", "Notice");
                        }
                        else
                        {
                            if (this.pwtxt.Text != pw)
                            {
                                reg.SetValue("Password", "" + this.pwtxt.Text);
                                reg.SetValue("Email", "" + this.emailtxt.Text);
                                explorerreg.SetValue("AutoRestartShell", 1);
                                Main.pw = MD5HashFunc(this.pwtxt.Text);
                                Main.email = this.emailtxt.Text;
                                Main.pw2 = this.pwtxt.Text;
                                MessageBox.Show("Password And Email Will Saved Automatically", "Notice");
                                this.Hide();
                                new Main().ShowDialog();
                            }
                            else if (this.pwtxt.Text == pw)
                            {
                                MessageBox.Show("Please Enter A Different Password", "Notice");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invaild Email!", "Notice");
                    }
                }
                else
                {
                    MessageBox.Show("Password Is Doesnt Match", "Notice");
                }
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            Main.pw = MD5HashFunc(this.pwtxt.Text);
            Main.email = this.emailtxt.Text;
            Main.uac = uac;
            this.Hide();
            new Main().ShowDialog();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (this.pwtxt.Text == "")
            {
                this.label1.ForeColor = Color.Black;
            }
            else
            {
                if (this.pwconfirmtxt.Text == this.pwtxt.Text)
                {
                    this.label1.ForeColor = Color.Lime;
                }
                else
                {
                    this.label1.ForeColor = Color.Red;
                }
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
    }
}
