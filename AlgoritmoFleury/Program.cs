using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoFleury
{
    class Program
    {
        static void Main(string[] args)
        {
            //buscando diretorio onde ficam guardados os arquivos de entrada e saidas
            DirectoryInfo dir = new DirectoryInfo("../../");
            string diretorioAtual = dir.FullName;
            string sourcePath = $@"{diretorioAtual}Documentos\Entrada.txt";
            string saidaTrajetoEuleriano= $@"{diretorioAtual}\Documentos\TrajetoEuleriano.txt";
            string saidaListaVisitados = $@"{diretorioAtual}\Documentos\ListaVisitados.txt";
            string saidaRemocaoArestas = $@"{diretorioAtual}\Documentos\RemocaoArestas.txt";

            //iniciando grafo
            Grafo grafo = new Grafo();

            //lendo arquivo de entrada com as arestas
            LerArquivo(sourcePath, ref grafo);

            try
            {
                using (FileStream fs1 = new FileStream(saidaTrajetoEuleriano, FileMode.Create))
                using (FileStream fs2 = new FileStream(saidaListaVisitados, FileMode.Create))
                using (FileStream fs3 = new FileStream(saidaRemocaoArestas, FileMode.Create))
                {
                    using (StreamWriter arquivoTrajetoEuleriano = new StreamWriter(fs1))
                    using (StreamWriter arquivoListaVisitados = new StreamWriter(fs2))
                    using (StreamWriter arquivoRemocaoArestas = new StreamWriter(fs3))
                    {
                        grafo.PrintVertices(arquivoListaVisitados);
                        grafo.PrintAdjacencia(arquivoListaVisitados);
                        grafo.PrintClassificacao(arquivoTrajetoEuleriano);
                        grafo.MetodoFleury(arquivoRemocaoArestas);
                        grafo.PrintSequenciaVertices(arquivoTrajetoEuleriano);
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }            

            Console.ReadKey();
        }

        public static void LerArquivo(string path, ref Grafo grafo)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (!sr.EndOfStream)
                        {
                            string[] vertices = sr.ReadLine().Split(',');
                            grafo.AddArestas(vertices[0], vertices[1]);
                        }
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
