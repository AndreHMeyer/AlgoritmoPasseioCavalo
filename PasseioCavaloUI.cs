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
            int[,] tabuleiro = InicializarTabuleiro(passeioCavalo.TamanhoTabuleiro);

            (int startX, int startY) = EscolherPosicaoInicial(passeioCavalo.TamanhoTabuleiro);
            tabuleiro[startX, startY] = 0;

            Console.Clear();
            int opcao = Menu();

            if (OpcaoValida(opcao))
            {
                Console.Clear();
                Console.WriteLine("Processando...");

                ExecutarPasseioCavalo(passeioCavalo, tabuleiro, startX, startY, opcao);
            }
        }

        private int[,] InicializarTabuleiro(int tamanho)
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

        private void ExecutarPasseioCavalo(PasseioCavalo passeioCavalo, int[,] tabuleiro, int startX, int startY, int opcao)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            int contadorPassos = 0;
            bool sucesso = opcao == 1 ? passeioCavalo.ResolverPasseio(tabuleiro, startX, startY, 1, ref contadorPassos)
                                      : passeioCavalo.ResolverPasseioOtimizado(tabuleiro, startX, startY, 1, ref contadorPassos);
            stopwatch.Stop();

            Console.Clear();
            Console.WriteLine("\nTabuleiro final:");
            passeioCavalo.ImprimirTabuleiro(tabuleiro);
            Console.WriteLine($"Tempo de execução: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Quantidade de passos: {contadorPassos}");
            Console.WriteLine($"Solução: {(passeioCavalo.VerificarFechado(tabuleiro, startX, startY) ? "Fechada" : "Aberta")}");
        }

        private int Menu()
        {
            Console.WriteLine("\nPasseio do Cavalo");
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
