namespace GPG.Multiplayer.Client
{
    using GPG;
    using GPG.Multiplayer.Quazal;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class SelfTest
    {
        private void create_OnError(object sender, EventArgs e)
        {
            (sender as DlgCreateChannel).skinButtonCancel.DoClick();
        }

        private void form_AfterLoad(FrmBase form)
        {
        }

        private void form_AfterShown(FrmBase form)
        {
            ThreadStart start = null;
            ParameterizedThreadStart start2 = null;
            DlgLogin login = form as DlgLogin;
            if (login != null)
            {
                login.ddServer.SelectedIndex = 1;
                login.CheckServer();
                login.gpgLabelCreateAcct.DoClick();
            }
            DlgCreateUser user = form as DlgCreateUser;
            if (user != null)
            {
                user.tbUsername.Text = this.RandomName();
                user.tbEmailAddress.Text = "sdemulling@gaspowered.com";
                user.tbPassword.Text = this.RandomName();
                user.tbConfirmPassword.Text = user.tbPassword.Text;
                user.skinButtonOK.DoClick();
            }
            DlgTermsOfService service = form as DlgTermsOfService;
            if (service != null)
            {
                service.skinButtonAccept.DoClick();
            }
            DlgCreateChannel channel = form as DlgCreateChannel;
            if (channel != null)
            {
                channel.OnError += new EventHandler(this.create_OnError);
                channel.skinButtonOK.DoClick();
                if (channel.DialogResult != DialogResult.OK)
                {
                    channel.skinButtonCancel.DoClick();
                }
            }
            FrmMain main = form as FrmMain;
            if (main != null)
            {
                if (start == null)
                {
                    start = delegate {
                        Thread.Sleep(0x7d0);
                        main.Invoke((VGen1)delegate (object objmain) {
                            ((FrmMain) objmain).btnChat.PerformClick();
                        }, new object[] { main });
                    };
                }
                new Thread(start).Start();
                if (start2 == null)
                {
                    start2 = delegate (object objform) {
                        string str;
                        bool flag;
                    Label_0180:
                        flag = true;
                        VGen1 method = null;
                        VGen1 gen2 = null;
                        VGen1 gen3 = null;
                        VGen0 gen4 = null;
                        FrmMain threadMain = objform as FrmMain;
                        Random random = new Random();
                        Thread.Sleep(random.Next(0xbb8, 0xfa0));
                        int num = random.Next(0, 10);
                        switch (num)
                        {
                            case 1:
                                str = "/away";
                                if (method == null)
                                {
                                    method = delegate (object chatname) {
                                        threadMain.textBoxMsg.Text = chatname.ToString();
                                        threadMain.textBoxMsg_KeyDown(threadMain, new KeyEventArgs(Keys.Return));
                                    };
                                }
                                threadMain.Invoke(method, new object[] { str });
                                break;

                            case 2:
                                str = "/dnd";
                                if (gen2 == null)
                                {
                                    gen2 = delegate (object chatname) {
                                        threadMain.textBoxMsg.Text = chatname.ToString();
                                        threadMain.textBoxMsg_KeyDown(threadMain, new KeyEventArgs(Keys.Return));
                                    };
                                }
                                threadMain.Invoke(gen2, new object[] { str });
                                goto Label_0180;
                        }
                        if (num >= 6)
                        {
                            int num2 = random.Next(5);
                            str = "/join Main Chat";
                            if (num2 > 0)
                            {
                                str = "/join Test Chat " + num2;
                            }
                            if (gen3 == null)
                            {
                                gen3 = delegate (object chatname) {
                                    threadMain.textBoxMsg.Text = chatname.ToString();
                                    threadMain.textBoxMsg_KeyDown(threadMain, new KeyEventArgs(Keys.Return));
                                };
                            }
                            threadMain.Invoke(gen3, new object[] { str });
                        }
                        else
                        {
                            if (gen4 == null)
                            {
                                gen4 = delegate {
                                    threadMain.textBoxMsg.Text = this.RandomString();
                                    threadMain.textBoxMsg_KeyDown(threadMain, new KeyEventArgs(Keys.Return));
                                };
                            }
                            threadMain.Invoke(gen4);
                        }
                        goto Label_0180;
                    };
                }
                Thread thread = new Thread(start2);
                thread.IsBackground = true;
                thread.Start(main);
            }
        }

        private string RandomName()
        {
            string str = "_";
            Random random = new Random();
            for (int i = 0; i < random.Next(5, 15); i++)
            {
                char[] chArray = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz-_".ToCharArray();
                int index = random.Next(0, chArray.Length - 1);
                str = str + chArray[index];
            }
            return str;
        }

        private string RandomString()
        {
            string str = "";
            Random random = new Random();
            for (int i = 0; i < random.Next(5, 10); i++)
            {
                for (int j = 0; j < random.Next(5, 10); j++)
                {
                    char[] chArray = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
                    int index = random.Next(0, chArray.Length - 1);
                    str = str + chArray[index];
                }
                str = str + " ";
            }
            char ch = str[0];
            return (User.Current.Name + " " + ch.ToString().ToUpper() + str.Substring(1));
        }

        public void RegisterForm(FrmBase form)
        {
        }
    }
}

