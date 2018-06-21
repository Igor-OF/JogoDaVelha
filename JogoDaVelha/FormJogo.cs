using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoDaVelha
{
    public partial class FormJogo : Form
    {
        private FormPrincipal Principal;
        private bool ehServidor;
        private bool ehMeuTurno = false;
        // 0=neutro, 1=servidor, 2=cliente
        private int[][] tabuleiro = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
        private GerenciadorSocket gs;
        // 0=neutro, 1=servidor, 2=cliente
        private string[] opcoes = { "", "X", "O" };
        private bool finalizou = false;
        private bool ehVencedor = false;

        public FormJogo(FormPrincipal principal, bool ehServidor, GerenciadorSocket gs)
        {
            // O servidor sempre começa o jogo
            this.ehMeuTurno = ehServidor;
            this.Principal = principal;
            this.ehServidor = ehServidor;

            InitializeComponent();

            if (ehServidor)
            {
                this.Text = "Jogo da Velha | Servidor";
                this.SetDesktopLocation(250, 200);
            }
            else
            {
                this.Text = "Jogo da Velha | Cliente";
                this.SetDesktopLocation(700, 200);
            }

            this.gs = gs;
        }

        private void CarregarTabuleiro()
        {
            b00.Text = opcoes[tabuleiro[0][0]];
            b01.Text = opcoes[tabuleiro[0][1]];
            b02.Text = opcoes[tabuleiro[0][2]];
            b10.Text = opcoes[tabuleiro[1][0]];
            b11.Text = opcoes[tabuleiro[1][1]];
            b12.Text = opcoes[tabuleiro[1][2]];
            b20.Text = opcoes[tabuleiro[2][0]];
            b21.Text = opcoes[tabuleiro[2][1]];
            b22.Text = opcoes[tabuleiro[2][2]];
        }

        private void ValidarTabuleiro()
        {
            if (!this.InvokeRequired)
            {
                bool velha = false;

                // Validação vertical
                if (tabuleiro[0][0] != 0 && tabuleiro[0][1] != 0 && tabuleiro[0][1] != 0 && tabuleiro[0][0] == tabuleiro[0][1] && tabuleiro[0][1] == tabuleiro[0][2] && tabuleiro[0][0] == tabuleiro[0][2])
                {
                    // Vertical 0
                    finalizou = true;
                    if ((ehServidor && tabuleiro[0][0] == 1) || (!ehServidor && tabuleiro[0][0] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[1][0] != 0 && tabuleiro[1][1] != 0 && tabuleiro[1][1] != 0 && tabuleiro[1][0] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[1][2] && tabuleiro[1][0] == tabuleiro[1][2])
                {
                    // Vertical 1
                    finalizou = true;
                    if ((ehServidor && tabuleiro[1][0] == 1) || (!ehServidor && tabuleiro[1][0] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[2][0] != 0 && tabuleiro[2][1] != 0 && tabuleiro[2][1] != 0 && tabuleiro[2][0] == tabuleiro[2][1] && tabuleiro[2][1] == tabuleiro[2][2] && tabuleiro[2][0] == tabuleiro[2][2])
                {
                    // Vertical 2
                    finalizou = true;
                    if ((ehServidor && tabuleiro[2][0] == 1) || (!ehServidor && tabuleiro[2][0] == 2))
                        ehVencedor = true;
                }
                // Validação horizontal
                else if (tabuleiro[0][0] != 0 && tabuleiro[1][0] != 0 && tabuleiro[2][0] != 0 && tabuleiro[0][0] == tabuleiro[1][0] && tabuleiro[1][0] == tabuleiro[2][0] && tabuleiro[0][0] == tabuleiro[2][0])
                {
                    // Horizontal 0
                    finalizou = true;
                    if ((ehServidor && tabuleiro[0][0] == 1) || (!ehServidor && tabuleiro[0][0] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[0][1] != 0 && tabuleiro[1][1] != 0 && tabuleiro[2][1] != 0 && tabuleiro[0][1] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[2][1] && tabuleiro[0][1] == tabuleiro[2][1])
                {
                    // Horizontal 1
                    finalizou = true;
                    if ((ehServidor && tabuleiro[0][1] == 1) || (!ehServidor && tabuleiro[0][1] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[0][2] != 0 && tabuleiro[1][2] != 0 && tabuleiro[2][2] != 0 && tabuleiro[0][2] == tabuleiro[1][2] && tabuleiro[1][2] == tabuleiro[2][2] && tabuleiro[0][2] == tabuleiro[2][2])
                {
                    // Horizontal 2
                    finalizou = true;
                    if ((ehServidor && tabuleiro[0][2] == 1) || (!ehServidor && tabuleiro[0][2] == 2))
                        ehVencedor = true;
                }
                // Validação diagonal
                else if (tabuleiro[0][0] != 0 && tabuleiro[1][1] != 0 && tabuleiro[2][2] != 0 && tabuleiro[0][0] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[2][2] && tabuleiro[0][0] == tabuleiro[2][2])
                {
                    // Diagonal esquerda -> direita
                    finalizou = true;
                    if ((ehServidor && tabuleiro[0][0] == 1) || (!ehServidor && tabuleiro[0][0] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[0][2] != 0 && tabuleiro[1][1] != 0 && tabuleiro[2][0] != 0 && tabuleiro[2][0] == tabuleiro[1][1] && tabuleiro[1][1] == tabuleiro[0][2] && tabuleiro[2][0] == tabuleiro[0][2])
                {
                    // Diagonal direita -> esquerda
                    finalizou = true;
                    if ((ehServidor && tabuleiro[1][1] == 1) || (!ehServidor && tabuleiro[1][1] == 2))
                        ehVencedor = true;
                }
                else if (tabuleiro[0][0] != 0 &&
                    tabuleiro[0][1] != 0 && 
                    tabuleiro[0][2] != 0 && 
                    tabuleiro[1][0] != 0 &&
                    tabuleiro[1][1] != 0 &&
                    tabuleiro[1][2] != 0 &&
                    tabuleiro[2][0] != 0 &&
                    tabuleiro[2][1] != 0 &&
                    tabuleiro[2][2] != 0)
                {
                    velha = true;
                }

                // Se finalizou a jogada
                if (finalizou)
                {
                    HabilitarTabuleiro(true);
                    ehMeuTurno = false;
                    CarregarTabuleiro();
                    pnlAguarde.Hide();

                    string jogador = "Servidor";

                    if (!ehServidor)
                        jogador = "Cliente";

                    if (ehVencedor)
                        MessageBox.Show(this, $"Parabéns, { jogador }! Você venceu!!!", "Vitória", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show(this, $"Tente na próxima, { jogador }! Você perdeu!!!", "Derrota", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ReiniciarJogo();
                }
                else if (velha)
                {
                    MessageBox.Show(this, $"Deu velha!!!", "Velha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReiniciarJogo();
                }
            }
            else
            {
                this.Invoke((MethodInvoker)delegate { ValidarTabuleiro(); });
            }
        }

        private void ValidarTurno()
        {
            if (!this.InvokeRequired)
            {
                if (ehMeuTurno && finalizou == false)
                {
                    pnlAguarde.Hide();
                    HabilitarTabuleiro(true);
                }
                else
                {
                    pnlAguarde.Show();
                    HabilitarTabuleiro(false);
                    ReceberDados();
                }

                CarregarTabuleiro();
            }
            else
            {
                this.Invoke((MethodInvoker)delegate { ValidarTurno(); });
            }
        }

        private void HabilitarTabuleiro(bool valor)
        {
            pnlJogo.Enabled = valor;
        }

        private void ReceberDados()
        {
            Task.Factory.StartNew(() => {
                try
                {
                    tabuleiro = gs.RecuperarTabuleiro();
                    ehMeuTurno = true;
                    ValidarTabuleiro();
                    ValidarTurno();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Environment.Exit(0);
                }
            });
        }

        private void PreencherTabuleiroPorBotao(string botao)
        {
            // 0=neutro, 1=servidor, 2=cliente
            char[] posicoes = botao.Substring(1).ToCharArray();
            tabuleiro[Int32.Parse("" + posicoes[0])][Int32.Parse("" + posicoes[1])] = ehServidor ? 1 : 2;
        }

        private void MarcarTabuleiro(object sender, EventArgs e)
        {
            if (ehMeuTurno && finalizou == false)
            {
                if (((Button)sender).Text == "")
                {
                    PreencherTabuleiroPorBotao(((Button)sender).Name);
                    gs.EnviarTabuleiro(tabuleiro);
                    ehMeuTurno = false;
                    ValidarTabuleiro();
                    ValidarTurno();
                }
            }
        }

        private void FormJogo_Load(object sender, EventArgs e)
        {
            ValidarTurno();
        }

        private void FormJogo_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ReiniciarJogo()
        {
            if (this.ehServidor)
                this.ehMeuTurno = true;
            else
                this.ehMeuTurno = false;

            tabuleiro = new int[][] { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };

            finalizou = false;
            ehVencedor = false;

            CarregarTabuleiro();
        }
    }
}
