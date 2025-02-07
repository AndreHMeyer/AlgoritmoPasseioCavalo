using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasseioCavalo
{
    public class PasseioCavaloUI
    {
        public void IniciarAplicacao()
        {
            PasseioCavalo passeioCavalo = new PasseioCavalo();
            int[,] tabuleiro = GerarTabuleiro(passeioCavalo.TamanhoTabuleiro);

            (int inicioX, int inicioY) = EscolherPosicaoInicial(passeioCavalo.TamanhoTabuleiro);
            tabuleiro[inicioX, inicioY] = 0;

            Console.Clear();
            int opcao = Menu();

            Console.Clear();
            Console.WriteLine("Processando...");

            ExecutarPasseioCavalo(passeioCavalo, tabuleiro, inicioX, inicioY, opcao);
        }

        private int[,] GerarTabuleiro(int tamanho)
        {
            int[,] tabuleiro = new int[tamanho, tamanho];
            for (int i = 0; i < tamanho; i++)
                for (int j = 0; j < tamanho; j++)
                    tabuleiro[i, j] = -1;
            return tabuleiro;
        }

        private (int, int) EscolherPosicaoInicial(int tamanho)
        {
            Console.Write("Informe a linha inicial (0 a 7): ");
            int linha = LerInteiro(0, tamanho - 1);
            Console.Write("Informe a coluna inicial (0 a 7): ");
            int coluna = LerInteiro(0, tamanho - 1);
            return (linha, coluna);
        }

        private int LerInteiro(int min, int max)
        {
            int valor;
            while (!int.TryParse(Console.ReadLine(), out valor) || valor < min || valor > max)
            {
                Console.WriteLine($"Valor inválido! Informe um número entre {min} e {max}: ");
            }
            return valor;
        }

        private void ExecutarPasseioCavalo(PasseioCavalo passeioCavalo, int[,] tabuleiro, int inicioX, int inicioY, int opcao)
        {
            var stopwatch = Stopwatch.StartNew();
            int contadorPassos = 0;
            bool resultado = opcao == 1 ? passeioCavalo.ResolverPasseio(tabuleiro, inicioX, inicioY, 1, ref contadorPassos)
                                      : passeioCavalo.ResolverPasseioOtimizado(tabuleiro, inicioX, inicioY, 1, ref contadorPassos);
            stopwatch.Stop();

            Console.Clear();
            Console.WriteLine("\nTabuleiro final:");
            passeioCavalo.ImprimirTabuleiro(tabuleiro);
            Console.WriteLine($"\nTempo de execução: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Quantidade de passos: {contadorPassos}");
            Console.WriteLine($"Solução: {(passeioCavalo.VerificarFechado(tabuleiro, inicioX, inicioY) ? "Fechada" : "Aberta")}");
        }

        private int Menu()
        {
            Console.WriteLine("Passeio do Cavalo");
            Console.WriteLine("1 - Tentativa e Erro");
            Console.WriteLine("2 - Otimizado");
            int escolha;
            while (!int.TryParse(Console.ReadLine(), out escolha) || !OpcaoValida(escolha))
            {
                Console.WriteLine("Opção inválida. Por favor, escolha 1 ou 2.");
            }
            return escolha;
        }

        private bool OpcaoValida(int escolha)
        {
            return escolha == 1 || escolha == 2;
        }
    }
}
