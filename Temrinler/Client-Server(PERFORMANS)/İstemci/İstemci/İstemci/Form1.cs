﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace İstemci
{
    public partial class Form1 : Form
    {
        //Yine gerekli Siniflarin nesneleri tanimlaniyor
        Thread t;
        TcpClient baglantikur;
        NetworkStream ag;
        StreamReader oku;
        StreamWriter yaz;
        public delegate void ricdegis(string text);

        public Form1()
        {
            InitializeComponent();
        }
        // Clintde Method_1 (Gelen veri okunuyor)
        public void okumayabasla()
        {
            ag = baglantikur.GetStream();
            oku = new StreamReader(ag);
            while (true)
            {
                try
                {
                    string yazi = oku.ReadLine();
                    ekranabas(yazi);
                }
                catch
                {
                    return;
                }
            }
        }
        // Clientde Method_2 (Okunan Veri richTextBox icine yaziliyor)
        public void ekranabas(string s)
        {
            if (this.InvokeRequired)
            {
                ricdegis degis = new ricdegis(ekranabas);
                this.Invoke(degis, s);
            }
            else
            {
                s = "Server: " + s;
                richTextBox1.AppendText(s + "\n");
            }
        }
        // Clientde Method_3 (Istenilen IP'ye istenen Port üzerinden baglaniliyor)
        public void baglanti_kur()
        {
            try
            {
                //Ben Lochalhos üzerinde deneme yapacagim icin127.0.0.1 verdim
                //baglantikur = new TcpClient("192.168.2.251", 1024);
                baglantikur = new TcpClient(textBox1.Text, 1024);
                t = new Thread(new ThreadStart(okumayabasla));
                t.Start();
                richTextBox1.AppendText(DateTime.Now.ToString() + " Baglanti kuruldu..." + "\n");
            }
            catch (Exception)
            {

                MessageBox.Show("Server ile baglanti kurulurken hata olustu !");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            baglanti_kur();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglantikur.Client.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
                //Burda bos alan göndermeyi önlüyoruz...
                return;
            else
            {
                yaz = new StreamWriter(ag);
                yaz.WriteLine(textBox2.Text);
                yaz.Flush();
                richTextBox1.AppendText("Client: " + textBox2.Text + "\n");
                textBox2.Text = "";
            }
        }
    }
}
