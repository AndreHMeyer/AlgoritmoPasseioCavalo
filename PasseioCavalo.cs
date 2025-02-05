using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasseioCavalo
{
    public class PasseioCavalo
    {
        public int TamanhoTabuleiro { get; } = 16;
        static int[] movX = { 2, 1, -1, -2, -2, -1, 1, 2 };
        static int[] movY = { 1, 2, 2, 1, -1, -2, -2, -1 };

        public bool ResolverPasseio(int[,] tab, int x, int y, int move, ref int contador)
        {
            contador++;
            if (move == TamanhoTabuleiro * TamanhoTabuleiro) return true;

            for (int i = 0; i < TamanhoTabuleiro; i++)
            {
                int novoX = x + movX[i];
                int novoY = y + movY[i];
                if (MovimentoValido(tab, novoX, novoY))
                {
                    tab[novoX, novoY] = move;
                    if (ResolverPasseio(tab, novoX, novoY, move + 1, ref contador)) return true;
                    tab[novoX, novoY] = -1;
                }
            }
            return false;
        }

        public bool ResolverPasseioOrdenado(int[,] tab, int x, int y, int move, ref int contador)
        {
            contador++;
            if (move == TamanhoTabuleiro * TamanhoTabuleiro) return true;
            var movimentosPossiveis = ObterMovimentosPossiveis(tab, x, y);
            foreach (var mov in movimentosPossiveis)
            {
                int novoX = x + movX[mov];
                int novoY = y + movY[mov];
                tab[novoX, novoY] = move;
                if (ResolverPasseioOrdenado(tab, novoX, novoY, move + 1, ref contador)) return true;
                tab[novoX, novoY] = -1;
            }
            return false;
        }
        private List<int> ObterMovimentosPossiveis(int[,] tab, int x, int y)
        {
            var movimentos = new List<int>();
            for (int i = 0; i < 8; i++)
            {
                int novoX = x + movX[i];
                int novoY = y + movY[i];
                if (MovimentoValido(tab, novoX, novoY))
                {
                    movimentos.Add(i);
                }
            }
            movimentos.Sort((a, b) => { int possibilidadesA = PossibilidadesFuturas(tab, x + movX[a], y + movY[a]);
                int possibilidadesB = PossibilidadesFuturas(tab, x + movX[b], y + movY[b]);
                if (possibilidadesA == possibilidadesB)
                {
                    int distanciaA = DistanciaCentro(x + movX[a], y + movY[a]);
                    int distanciaB = DistanciaCentro(x + movX[b], y + movY[b]);
                    return distanciaB.CompareTo(distanciaA);
                }
                return possibilidadesA.CompareTo(possibilidadesB);
            });
            return movimentos;
        }
        private int PossibilidadesFuturas(int[,] tab, int x, int y)
        {
            int contador = 0;
            for (int i = 0; i < 8; i++)
            {
                int novoX = x + movX[i];
                int novoY = y + movY[i];
                if (MovimentoValido(tab, novoX, novoY))
                {
                    contador++;
                }
            }
            return contador;
        }
        private int DistanciaCentro(int x, int y)
        {
            int centro = TamanhoTabuleiro / 2;
            return Math.Abs(x - centro) + Math.Abs(y - centro);
        }

        public int ContarPossibilidades(int[,] tab, int x, int y)
        {
            if (!MovimentoValido(tab, x, y)) return int.MaxValue;
            int contador = 0;
            for (int i = 0; i < 8; i++)
            {
                int novoX = x + movX[i];
                int novoY = y + movY[i];
                if (MovimentoValido(tab, novoX, novoY)) contador++;
            }
            return contador;
        }

        public bool MovimentoValido(int[,] tab, int x, int y)
        {
            return x >= 0 && y >= 0 && x < TamanhoTabuleiro && y < TamanhoTabuleiro && tab[x, y] == -1;
        }

        public bool VerificarFechado(int[,] tab, int startX, int startY)
        {
            for (int i = 0; i < 8; i++)
            {
                int novoX = startX + movX[i];
                int novoY = startY + movY[i];
                if (novoX >= 0 && novoY >= 0 && novoX < TamanhoTabuleiro && novoY < TamanhoTabuleiro && tab[novoX, novoY] == 0)
                    return true;
            }
            return false;
        }

        public void ImprimirTabuleiro(int[,] tab)
        {
            Console.WriteLine("  " + string.Join(" ", Enumerable.Range(0, TamanhoTabuleiro).Select(i => i.ToString("D2"))));
            for (int i = 0; i < TamanhoTabuleiro; i++)
            {
                Console.Write(i.ToString("D2") + " ");
                for (int j = 0; j < TamanhoTabuleiro; j++)
                    Console.Write($"{tab[i, j],2} ");
                Console.WriteLine();
            }
        }
    }
}
