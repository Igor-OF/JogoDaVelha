using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace JogoDaVelha
{
    public class GerenciadorSocket
    {
        private IPAddress _IP;
        private int _PORTA;
        private TcpListener _SERVIDORTCP;
        private TcpClient _CLIENTE;
        private NetworkStream _STREAM;

        public GerenciadorSocket(string ip, int porta)
        {
            // Converte o Ip digitado em um objeto de endereço Ip
            _IP = IPAddress.Parse(ip);
            _PORTA = porta;
        }

        public bool IniciarComoServidor(ref System.Windows.Forms.Label status)
        {
            try
            {
                // Instancia e inicia o servidor TCP
                _SERVIDORTCP = new TcpListener(_IP, _PORTA);
                _SERVIDORTCP.Start();
                status.Text = "Aguardando Cliente";
                Application.DoEvents();
                _CLIENTE = RecuperarClienteTCP();
                _STREAM = _CLIENTE.GetStream();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        public TcpClient RecuperarClienteTCP()
        {
            return _SERVIDORTCP.AcceptTcpClient();
        }

        public bool IniciarComoCliente()
        {
            try
            {
                // Instancia e conecta o cliente TCP ao servidor
                _CLIENTE = new TcpClient();
                _CLIENTE.Connect(_IP, _PORTA);
                _STREAM = _CLIENTE.GetStream();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        public bool EnviarTabuleiro(int[][] obj)
        {
            try
            {
                // Recupera as marcações do tabuleiro
                string temp = "";
                for (int y = 0; y < 3; y++)
                    for (int x = 0; x < 3; x++)
                        temp += obj[y][x];

                // Transfor os dados em bytes e escreve na stream que irá ao cliente
                byte[] bytes = new byte[255];
                bytes = new ASCIIEncoding().GetBytes(temp);
                _STREAM.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        public int[][] RecuperarTabuleiro()
        {
            try
            {
                // Faz a leitura dos bytes enviados pelo cliente e converte em string
                byte[] bytes = new byte[255];
                _STREAM.Read(bytes, 0, bytes.Length);
                string temp = new ASCIIEncoding().GetString(bytes);
                char[] chars = temp.ToCharArray();

                // Transforma os dados no formato da matriz do tabuleiro
                int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
                for (int y = 0; y < 3; y++)
                    for (int x = 0; x < 3; x++)
                        obj[y][x] = Int32.Parse("" + chars[(y * 3) + x]);

                return obj;
            }
            catch
            {
                throw new Exception("Conexão perdida com o oponente!");
            }
        }
    }
}
