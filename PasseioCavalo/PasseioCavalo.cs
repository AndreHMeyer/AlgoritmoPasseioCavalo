﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PasseioCavalo
{
    public class PasseioCavalo
    {
        public int TamanhoTabuleiro { get; set; } = 8;
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

        public bool ResolverPasseioOtimizado(int[,] tab, int x, int y, int move, ref int contador)
        {
            contador++;
            if (move == TamanhoTabuleiro * TamanhoTabuleiro) return true;

            var movimentos = ObterMovimentos(tab, x, y);

            foreach (var movimento in movimentos)
            {
                int novoX = x + movX[movimento.Index];
                int novoY = y + movY[movimento.Index];
                tab[novoX, novoY] = move;
                if (ResolverPasseioOtimizado(tab, novoX, novoY, move + 1, ref contador)) return true;
                tab[novoX, novoY] = -1;
            }
            return false;
        }

        private List<(int Index, int Possibilidades)> ObterMovimentos(int[,] tab, int x, int y)
        {
            return Enumerable.Range(0, 8)
                .Select(i => (Index: i, Possibilidades: ContarPossibilidades(tab, x + movX[i], y + movY[i])))
                .Where(m => MovimentoValido(tab, x + movX[m.Index], y + movY[m.Index]))
                .OrderBy(m => !EhNaBorda(x + movX[m.Index], y + movY[m.Index]))
                .ThenBy(m => m.Possibilidades)
                .ToList();
        }

        private int ContarPossibilidades(int[,] tab, int x, int y)
        {
            if (!MovimentoValido(tab, x, y)) return int.MaxValue;
            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                int novoX = x + movX[i];
                int novoY = y + movY[i];
                if (MovimentoValido(tab, novoX, novoY)) count++;
            }
            return count;
        }

        private bool MovimentoValido(int[,] tab, int x, int y)
        {
            return x >= 0 && y >= 0 && x < TamanhoTabuleiro && y < TamanhoTabuleiro && tab[x, y] == -1;
        }

        private bool EhNaBorda(int x, int y)
        {
            return x == 0 || y == 0 || x == TamanhoTabuleiro - 1 || y == TamanhoTabuleiro - 1;
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
            Console.WriteLine("\n   " + string.Join("  ", Enumerable.Range(0, TamanhoTabuleiro).Select(i => i.ToString("D2"))));
            Console.WriteLine("  " + new string('-', TamanhoTabuleiro * 4));
            for (int i = 0; i < TamanhoTabuleiro; i++)
            {
                Console.Write(i.ToString("D2") + "|");
                for (int j = 0; j < TamanhoTabuleiro; j++)
                    Console.Write($"{tab[i, j],3} ");
                Console.WriteLine("|");
            }
            Console.WriteLine("  " + new string('-', TamanhoTabuleiro * 4));
        }
    }
}