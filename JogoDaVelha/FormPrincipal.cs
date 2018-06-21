using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace JogoDaVelha
{
    public partial class FormPrincipal : Form
    {
        // Variável para controlar se a instância é um servidor ou cliente
        private bool ehServidor = false;

        // Responsável pela conexao
        private GerenciadorSocket gs;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private bool ValidarIpEPorta(string ip, string porta)
        {
            try
            {
                // Valida através de expressão regular (Regex) se o IP e a Porta estão em formatos válidos
                if (Regex.IsMatch(ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$")
                    && Regex.IsMatch(porta, "^[0-9]{1,5}$"))
                {
                    string[] temp = ip.Split('.');
                    foreach (string s in temp)
                    {
                        // Os trios do IP não podem ultrapassar o número 255
                        if (Int32.Parse(s) > 255)
                            throw new Exception("IP inválido!");
                    }

                    // Valor máximo para porta é 65535
                    if (Int32.Parse(txtPorta.Text) > 65535)
                        throw new Exception("Porta inválida!");

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return false;
        }

        // Faz a conexão como servidor
        private void ConectarComoServidor(string ip, int port)
        {
            gs = new GerenciadorSocket(ip, port);
            if (gs.IniciarComoServidor(ref status))
                ComecarJogo();
        }

        // Faz a conexão como cliente
        private void ConectarComoCliente(string ip, int port)
        {
            gs = new GerenciadorSocket(ip, port);
            if (gs.IniciarComoCliente())
                ComecarJogo();
        }

        // Habilita todos os controles da tela
        private void HabilitarTodosObjetos()
        {
            txtServidor.Enabled = true;
            txtPorta.Enabled = true;
            btnServidor.Enabled = true;
            btnCliente.Enabled = true;
        }

        // Desabilita todos os controles da tela
        private void DesabilitarTodosObjetos()
        {
            txtServidor.Enabled = false;
            txtPorta.Enabled = false;
            btnServidor.Enabled = false;
            btnCliente.Enabled = false;
        }

        // Método para iniciar o jogo
        private void ComecarJogo()
        {
            this.Hide();
            new FormJogo(this, ehServidor, gs).Show();
        }

        private void btnServidor_Click(object sender, EventArgs e)
        {
            DesabilitarTodosObjetos();

            if (ValidarIpEPorta(txtServidor.Text, txtPorta.Text))
            {
                ehServidor = true;
                ConectarComoServidor(txtServidor.Text, Int32.Parse(txtPorta.Text));
            }

            HabilitarTodosObjetos();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            DesabilitarTodosObjetos();

            if (ValidarIpEPorta(txtServidor.Text, txtPorta.Text))
            {
                ehServidor = false;
                ConectarComoCliente(txtServidor.Text, Int32.Parse(txtPorta.Text));
            }

            HabilitarTodosObjetos();
        }
    }
}
