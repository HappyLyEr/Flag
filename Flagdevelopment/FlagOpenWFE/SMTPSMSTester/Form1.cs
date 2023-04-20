using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace SMTPSMSTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnSendSMS;
		private System.Windows.Forms.TextBox txtPhoneNumber;
		private System.Windows.Forms.TextBox txtSMSMsg;
		private System.Windows.Forms.TextBox txtMailAddress;
		private System.Windows.Forms.TextBox txtSMTPMsg;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.Button btnSendMail;
		private System.Windows.Forms.TextBox txtGateway;
		private System.Windows.Forms.TextBox txtGatewayUserName;
		private System.Windows.Forms.TextBox txtGatewayPWD;
		private System.Windows.Forms.TextBox txtSMTPServer;
		private System.Windows.Forms.TextBox txtSMTPFrom;
        private Label label11;
        private Label label14;
        private Label label15;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSendSMS = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.txtSMSMsg = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnSendMail = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.txtSMTPMsg = new System.Windows.Forms.TextBox();
            this.txtMailAddress = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtGateway = new System.Windows.Forms.TextBox();
            this.txtGatewayUserName = new System.Windows.Forms.TextBox();
            this.txtGatewayPWD = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSMTPServer = new System.Windows.Forms.TextBox();
            this.txtSMTPFrom = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(1, 100);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(712, 436);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.btnSendSMS);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtPhoneNumber);
            this.tabPage1.Controls.Add(this.txtSMSMsg);
            this.tabPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(704, 410);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SMS";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(136, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(432, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "SMS Test";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSendSMS
            // 
            this.btnSendSMS.Location = new System.Drawing.Point(136, 328);
            this.btnSendSMS.Name = "btnSendSMS";
            this.btnSendSMS.Size = new System.Drawing.Size(168, 32);
            this.btnSendSMS.TabIndex = 4;
            this.btnSendSMS.Text = "Send SMS";
            this.btnSendSMS.Click += new System.EventHandler(this.btnSendSMS_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "Message";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Phone number";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Location = new System.Drawing.Point(136, 56);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(504, 20);
            this.txtPhoneNumber.TabIndex = 1;
            this.txtPhoneNumber.Text = "+47";
            // 
            // txtSMSMsg
            // 
            this.txtSMSMsg.Location = new System.Drawing.Point(136, 88);
            this.txtSMSMsg.Multiline = true;
            this.txtSMSMsg.Name = "txtSMSMsg";
            this.txtSMSMsg.Size = new System.Drawing.Size(504, 232);
            this.txtSMSMsg.TabIndex = 0;
            this.txtSMSMsg.Text = "SMS Test message from Flag";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnSendMail);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.txtSubject);
            this.tabPage2.Controls.Add(this.txtSMTPMsg);
            this.tabPage2.Controls.Add(this.txtMailAddress);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(704, 410);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SMTP";
            // 
            // btnSendMail
            // 
            this.btnSendMail.Location = new System.Drawing.Point(136, 301);
            this.btnSendMail.Name = "btnSendMail";
            this.btnSendMail.Size = new System.Drawing.Size(184, 32);
            this.btnSendMail.TabIndex = 7;
            this.btnSendMail.Text = "Send Mail";
            this.btnSendMail.Click += new System.EventHandler(this.btnSendMail_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(16, 120);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 24);
            this.label7.TabIndex = 6;
            this.label7.Text = "Message";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(16, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 24);
            this.label6.TabIndex = 5;
            this.label6.Text = "Subject";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 24);
            this.label5.TabIndex = 4;
            this.label5.Text = "Email address";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(136, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(432, 24);
            this.label4.TabIndex = 3;
            this.label4.Text = "SMTP Test";
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(136, 88);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(504, 20);
            this.txtSubject.TabIndex = 2;
            this.txtSubject.Text = "Test mail from Flag";
            // 
            // txtSMTPMsg
            // 
            this.txtSMTPMsg.Location = new System.Drawing.Point(136, 120);
            this.txtSMTPMsg.Multiline = true;
            this.txtSMTPMsg.Name = "txtSMTPMsg";
            this.txtSMTPMsg.Size = new System.Drawing.Size(504, 173);
            this.txtSMTPMsg.TabIndex = 1;
            this.txtSMTPMsg.Text = "Please ignore";
            // 
            // txtMailAddress
            // 
            this.txtMailAddress.Location = new System.Drawing.Point(136, 56);
            this.txtMailAddress.Name = "txtMailAddress";
            this.txtMailAddress.Size = new System.Drawing.Size(504, 20);
            this.txtMailAddress.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(16, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "SMS Gateway";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(16, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 16);
            this.label9.TabIndex = 2;
            this.label9.Text = "SMS UserName";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(16, 64);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(120, 16);
            this.label10.TabIndex = 3;
            this.label10.Text = "SMS Password";
            // 
            // txtGateway
            // 
            this.txtGateway.Location = new System.Drawing.Point(136, 8);
            this.txtGateway.Name = "txtGateway";
            this.txtGateway.ReadOnly = true;
            this.txtGateway.Size = new System.Drawing.Size(192, 20);
            this.txtGateway.TabIndex = 4;
            this.txtGateway.Text = "txtGateway";
            // 
            // txtGatewayUserName
            // 
            this.txtGatewayUserName.Location = new System.Drawing.Point(136, 32);
            this.txtGatewayUserName.Name = "txtGatewayUserName";
            this.txtGatewayUserName.ReadOnly = true;
            this.txtGatewayUserName.Size = new System.Drawing.Size(192, 20);
            this.txtGatewayUserName.TabIndex = 5;
            this.txtGatewayUserName.Text = "txtGatewayUserName";
            // 
            // txtGatewayPWD
            // 
            this.txtGatewayPWD.Location = new System.Drawing.Point(136, 56);
            this.txtGatewayPWD.Name = "txtGatewayPWD";
            this.txtGatewayPWD.ReadOnly = true;
            this.txtGatewayPWD.Size = new System.Drawing.Size(192, 20);
            this.txtGatewayPWD.TabIndex = 6;
            this.txtGatewayPWD.Text = "txtGatewayPWD";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(376, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(120, 16);
            this.label12.TabIndex = 8;
            this.label12.Text = "SMTP From Address";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(376, 4);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(120, 16);
            this.label13.TabIndex = 7;
            this.label13.Text = "SMTP Server";
            // 
            // txtSMTPServer
            // 
            this.txtSMTPServer.Location = new System.Drawing.Point(504, 3);
            this.txtSMTPServer.Name = "txtSMTPServer";
            this.txtSMTPServer.ReadOnly = true;
            this.txtSMTPServer.Size = new System.Drawing.Size(192, 20);
            this.txtSMTPServer.TabIndex = 9;
            this.txtSMTPServer.Text = "txtSMTPServer";
            this.txtSMTPServer.TextChanged += new System.EventHandler(this.txtSMTPServer_TextChanged);
            // 
            // txtSMTPFrom
            // 
            this.txtSMTPFrom.Location = new System.Drawing.Point(504, 26);
            this.txtSMTPFrom.Name = "txtSMTPFrom";
            this.txtSMTPFrom.ReadOnly = true;
            this.txtSMTPFrom.Size = new System.Drawing.Size(192, 20);
            this.txtSMTPFrom.TabIndex = 10;
            this.txtSMTPFrom.Text = "txtSMTPFrom";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(376, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(120, 16);
            this.label11.TabIndex = 11;
            this.label11.Text = "SMTP Port";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(376, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(120, 16);
            this.label14.TabIndex = 12;
            this.label14.Text = "Account username";
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(376, 84);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(120, 16);
            this.label15.TabIndex = 13;
            this.label15.Text = "Account Password";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(504, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(192, 20);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "txtSMTPPort";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(502, 65);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(192, 20);
            this.textBox2.TabIndex = 15;
            this.textBox2.Text = "txtUsername";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(502, 84);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(192, 20);
            this.textBox3.TabIndex = 16;
            this.textBox3.Text = "txtPassword";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(714, 537);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtSMTPFrom);
            this.Controls.Add(this.txtSMTPServer);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtGatewayPWD);
            this.Controls.Add(this.txtGatewayUserName);
            this.Controls.Add(this.txtGateway);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(1, 100, 1, 1);
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void label2_Click(object sender, System.EventArgs e)
		{
		
		}

		

		private void label5_Click(object sender, System.EventArgs e)
		{
		
		}

		private void label9_Click(object sender, System.EventArgs e)
		{
		
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			txtGateway.Text = SMSGatewayURL;
			txtGatewayUserName.Text = SMSGatewayUsername;
			txtGatewayPWD.Text = SMSGatewayPassword;

			txtSMTPServer.Text = SMTPServer;
			txtSMTPFrom.Text = SMTPFromAddress;
            textBox1.Text = SMTPPort;
            textBox2.Text = SMTPUsername;
            textBox3.Text = SMTPPassword;

		}

		private void btnSendSMS_Click(object sender, System.EventArgs e)
		{
			sendSMS();
		}

		private void sendSMS() 
		{
			try 
			{
				GASystem.BusinessLayer.sms.SendTextMessage(txtPhoneNumber.Text, txtSMSMsg.Text);
				MessageBox.Show("Message sent");
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.Message);
			}
		}


		private void sendMail() 
		{
			try 
			{
				
                GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(txtMailAddress.Text, txtSubject.Text, txtSMTPMsg.Text, true
                    );
				MessageBox.Show("Message sent");
			} 
			catch (Exception ex) 
			{
				MessageBox.Show(ex.Message + " -- " + ex.InnerException);
			}
		}

		private void btnSendMail_Click(object sender, System.EventArgs e)
		{
			sendMail();
		}


		#region Config Settings
		/// <summary>
		/// Properties for getting config settings
		/// </summary>
		private static string SMTPServer 
		{
			get {return System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPServer");	}
		}

		private static string SMTPFromAddress 
		{
			get {return System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPFromAddress");	}
		}
        private static string SMTPPort
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPPort"); }
        }
        private static string SMTPUsername
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPUsername"); }
        }
        private static string SMTPPassword
        {
            get { return System.Configuration.ConfigurationSettings.AppSettings.Get("SMTPPassword"); }
        }
        private static string SMSGatewayURL 
		{
			get {return System.Configuration.ConfigurationSettings.AppSettings.Get("SMSGatewayURL");	}
		}
        
        private static string SMSGatewayUsername 
		{
			get {return System.Configuration.ConfigurationSettings.AppSettings.Get("SMSGatewayUsername");	}
		}
		private static string SMSGatewayPassword
		{
			get {return System.Configuration.ConfigurationSettings.AppSettings.Get("SMSGatewayPassword");	}
		}

		#endregion

        private void txtSMTPServer_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
