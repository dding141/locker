using System;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace LOCKER
{
    public partial class REGISTER : Form
    {
        RegistryKey reg;
        string preg, ereg, ureg = "";

        public REGISTER()
        {
            InitializeComponent();
        }

        private void REGISTER_Load(object sender, EventArgs e)
        {
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");
            preg = Convert.ToString(reg.GetValue("Password"));
            ereg = Convert.ToString(reg.GetValue("Email"));
            if (preg == "" && ereg == "")
            {
                MessageBox.Show("Welcome! Password And Email Will Saved Automatically", "Notice", MessageBoxButtons.OK);
            }
            else
            {
                this.pwtxt.Text = preg;
                this.emailtxt.Text = ereg;
                this.StartUpFunc();
            }
            this.TopMost = true;
            timer1.Enabled = true;
            timer1.Interval = 100;
        }

        private void StartUpFunc()
        {
            RegistryKey strUpKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true);
            RegistryKey uackey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");
            ureg = Convert.ToString(uackey.GetValue("ConsentPromptBehaviorAdmin"));
            if (this.pwtxt.Text == "")
            {
                MessageBox.Show("Enter The Password!");
            }
            else if (this.emailtxt.Text == "")
            {
                MessageBox.Show("Enter The Email!");
            }
            else if (this.pwtxt.Text != "" && this.emailtxt.Text != "")
            {
                reg.SetValue("Password", "" + this.pwtxt.Text);
                reg.SetValue("Email", "" + this.emailtxt.Text);
                uackey.SetValue("EnableLUA", 0);
                uackey.SetValue("ConsentPromptBehaviorAdmin", 0);
                strUpKey.SetValue("LOCKER", Application.ExecutablePath);
                Main.pw = MD5HashFunc(this.pwtxt.Text);
                Main.email = this.emailtxt.Text;
                Main.uac = ureg;
                Main.pw2 = this.pwtxt.Text;
                this.Hide();
                new Main().ShowDialog();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            RegistryKey strUpKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\RunOnce", true);
            RegistryKey uackey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
            reg = Registry.LocalMachine.CreateSubKey("SOFTWARE").CreateSubKey("RegisterInform");
            ureg = Convert.ToString(uackey.GetValue("ConsentPromptBehaviorAdmin"));
            if (this.pwtxt.Text == "")
            {
                MessageBox.Show("Enter The Password!", "Notice");
            }
            else if (this.emailtxt.Text == "")
            {
                MessageBox.Show("Enter The Email!", "Notice");
            }
            else if (this.pwtxt.Text != "" && this.emailtxt.Text != "")
            {
                if (this.pwtxt.Text == this.pwconfrimtxt.Text)
                {
                    if (this.pwtxt.Text.Contains(" "))
                    {
                        MessageBox.Show("You Cant Contain SpaceBar In Password", "Notice");
                    }
                    else
                    {
                        if (this.emailtxt.Text.Contains("@") && this.emailtxt.Text.Contains(".com") || this.emailtxt.Text.Contains(".net") || this.emailtxt.Text.Contains(".co.kr"))
                        {
                            reg.SetValue("Password", "" + this.pwtxt.Text);
                            reg.SetValue("Email", "" + this.emailtxt.Text);
                            uackey.SetValue("EnableLUA", 0);
                            uackey.SetValue("ConsentPromptBehaviorAdmin", 0);
                            strUpKey.SetValue("LOCKER", Application.ExecutablePath);
                            Main.pw = MD5HashFunc(this.pwtxt.Text);
                            Main.email = this.emailtxt.Text;
                            Main.uac = ureg;
                            Main.pw2 = this.pwtxt.Text;
                            this.Hide();
                            new Main().ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Invaild Email!", "Notice");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Password Is Doesnt Match", "Notice");
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.pwtxt.Text == "")
            {
                this.label8.ForeColor = Color.Black;
            }
            else
            {
                if (this.pwconfrimtxt.Text == this.pwtxt.Text)
                {
                    this.label8.ForeColor = Color.Lime;
                }
                else
                {
                    this.label8.ForeColor = Color.Red;
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
