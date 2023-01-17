using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoritmoFleury
{
    class Vertice
    {
        public string Nome { get; set; }
        public int Largura { get; set; }
        public int Nivel { get; set; }
        public string VerticePai { get; set; }
        public List<Vertice> ListaAdjacencia { get; set; }

        public Vertice(string nome)
        {
            Nome = nome;
            ListaAdjacencia = new List<Vertice>();
        }
    }

    class Grafo
    {
        public Dictionary<string, Vertice> Vertices;
        public List<string> OrdemVisitados;
        public List<Vertice> Fila { get; set; }

        public List<string> Impressao { get; set; }

        public Grafo()
        {
            Vertices = new Dictionary<string, Vertice>();
            OrdemVisitados = new List<string>();
            Fila = new List<Vertice>();

            Impressao = new List<string>();
        }

        public void AddArestas(string verticeOrigem, string verticeDestino)
        {
            if (!Vertices.ContainsKey(verticeOrigem))
                Vertices.Add(verticeOrigem, new Vertice(verticeOrigem));
            if (!Vertices.ContainsKey(verticeDestino))
                Vertices.Add(verticeDestino, new Vertice(verticeDestino));

            Vertice vertice1 = Vertices[verticeOrigem], vertice2 = Vertices[verticeDestino];

            vertice1.ListaAdjacencia.Add(vertice2);
            vertice2.ListaAdjacencia.Add(vertice1);
        }

        public void RemoveAresta(string verticeOrigem, string verticeDestino)
        {
            Vertice vertice1 = Vertices[verticeOrigem], vertice2 = Vertices[verticeDestino];

            vertice1.ListaAdjacencia.Remove(vertice2);
            vertice2.ListaAdjacencia.Remove(vertice1);
        }

        public void MetodoFleury(StreamWriter sw)
        {
            //começa com o Vertice de numero impar, caso nao ache começa pelo primeiro mesmo
            Vertice verticeInicio = Vertices.Values.First();
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                if (vertice.Value.ListaAdjacencia.Count() % 2 != 0)
                {
                    verticeInicio = vertice.Value;
                    break;
                }
            }

            Console.WriteLine("\n===ARESTAS REMOVIDAS===");
            sw.WriteLine("===ARESTAS REMOVIDAS===\n");
            IniciaMetodoFleury(verticeInicio, 0, sw);
        }

        public void IniciaMetodoFleury(Vertice verticeInicio, int contador, StreamWriter sw)
        {
            int quebra = contador;
            Fila.Add(verticeInicio);
            for (int i = 0; i < verticeInicio.ListaAdjacencia.Count(); i++)
            {
                Vertice verticeDestino = verticeInicio.ListaAdjacencia.ElementAt(i);
               
                if (ArestaValida(verticeInicio, verticeDestino))
                {
                    if (quebra == 5)
                    {
                        Console.WriteLine("");
                        sw.WriteLine("");
                        quebra = 0;
                    }
                    Console.Write($" {verticeInicio.Nome} - {verticeDestino.Nome} /");
                    sw.Write($" {verticeInicio.Nome} - {verticeDestino.Nome} /");
                    quebra++;
                    RemoveAresta(verticeInicio.Nome, verticeDestino.Nome);
                    IniciaMetodoFleury(verticeDestino,quebra,sw);
                }
            }
        }

        private int Alcancaveis (Vertice verticeDescoberto, Dictionary<string,bool> visitado)
        {
            // Mark the current node as visited
            visitado[verticeDescoberto.Nome] = true;
            int verticesAlcancaveis = 1;

            foreach (Vertice vertice in verticeDescoberto.ListaAdjacencia)
            {
                if (!visitado[vertice.Nome])
                {
                    verticesAlcancaveis = verticesAlcancaveis + Alcancaveis(vertice, visitado);
                }
            }

            return verticesAlcancaveis;
        }

        private bool ArestaValida(Vertice verticeOrigem, Vertice verticeDestino)
        {
            //Se apenas possui uma aresta, ir por ela
            if (verticeOrigem.ListaAdjacencia.Count() == 1)
                return true;

            //Se possuir mais de uma aresta, verificar aquela que não é ponte
            //Primeiro verificamos quais vertices são alcançaveis a partir do vertice de origem
            Dictionary<string, bool> alcancavel = new Dictionary<string, bool>(Vertices.Count());
            IniciaDicionario(ref alcancavel);
            int verticesAlcancaveis1 = Alcancaveis(verticeOrigem, alcancavel);
            RemoveAresta(verticeOrigem.Nome, verticeDestino.Nome);

            alcancavel = new Dictionary<string, bool>(Vertices.Count());
            IniciaDicionario(ref alcancavel);
            int verticesAlcancaveis2 = Alcancaveis(verticeOrigem, alcancavel);
            AddArestas(verticeOrigem.Nome, verticeDestino.Nome);

            if (verticesAlcancaveis1 > verticesAlcancaveis2)
                return false;

            return true;
        }

        private void IniciaDicionario(ref Dictionary<string, bool> dic)
        {
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
                dic.Add(vertice.Key,false);
        }

        public int GrauImpar()
        {
            int contador = 0;

            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                if (vertice.Value.ListaAdjacencia.Count() % 2 != 0)
                {
                    contador++;
                }
            }

            return contador;
        }

        //Métodos para printar na tela
        public void PrintAdjacencia(StreamWriter sw)
        {
            Console.WriteLine("===LISTA DE ADJACENCIA===");
            sw.WriteLine("===LISTA DE ADJACENCIA===\n");
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                Console.Write($"Y({vertice.Key}) = "+"{");
                sw.Write($"Y({vertice.Key}) = "+"{");
                foreach (Vertice verticeAux in vertice.Value.ListaAdjacencia)
                {
                    Console.Write($"{verticeAux.Nome}");
                    sw.Write($"{verticeAux.Nome}");
                    if (!verticeAux.Nome.Equals(verticeAux.ListaAdjacencia.Last()))
                    {
                        Console.Write(", ");
                        sw.Write(", ");
                    }
                }
                Console.Write("}\n");
                sw.Write("}\n");
            }
        }

        public void PrintVertices(StreamWriter sw)
        {
            Console.Write("Vertices => ");
            sw.Write("Vertices => ");
            foreach (Vertice vertice in Vertices.Values)
            {
                Console.Write($"{vertice.Nome}");
                sw.Write($"{vertice.Nome}");
                if (!vertice.Nome.Equals(Vertices.Values.Last().Nome))
                {
                    Console.Write(", ");
                    sw.Write(", ");
                }
            }
            Console.WriteLine("\n");
            sw.WriteLine("\n");
        }

        public void PrintClassificacao(StreamWriter sw)
        {
            int quant = GrauImpar();

            Console.WriteLine("\n\n===CLASSIFICACAO===");
            sw.WriteLine("===CLASSIFICACAO===\n");

            if (quant == 0)
            {
                Console.WriteLine("Euleriano");
                sw.WriteLine("Euleriano\n");
            }
            else if (quant == 2)
            {
                Console.WriteLine("Semi-Euleriano");
                sw.WriteLine("Semi-Euleriano\n");
            }
            else if(quant > 2)
            {
                Console.WriteLine("Não-Euleriano");
                sw.WriteLine("Não-Euleriano\n");
            }
        }

        public void PrintSequenciaVertices(StreamWriter sw)
        {
            int contador = 0;
            Console.WriteLine("\n\n===TRAJETO EULERIANO===");
            sw.WriteLine("\n===TRAJETO EULERIANO===\n");
            foreach (Vertice vertice in Fila)
            {
                if (contador == 12)
                {
                    Console.WriteLine("");
                    sw.WriteLine("");
                    contador = 0;
                }
                Console.Write($"{vertice.Nome} / ");
                sw.Write($"{vertice.Nome} / ");
                contador++;
            }
            Console.WriteLine("\n");
            sw.WriteLine("");
        }
    }
}
