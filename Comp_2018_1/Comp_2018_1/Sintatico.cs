using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Comp_2018_1
{
    class Sintatico
    {
        public Stack<string> PilhaDeProducoes;
        public Stack<int> PilhaEstados;

        JObject tabelaLexica;
        JObject tabelaSintatica;

        public int estado;
        public string cadeia;

        int possicaoDalista;

        int tamanhoDoDesempilha;
        string Reduzido;



        public void LoadFiles(int location)
        {
            switch (location)
            {
                
                case 1:
                    tabelaLexica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\tabelaDeEstados.json"));
                    break;

                case 2:
                    tabelaLexica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\renat\cc\compilador\Comp_2018_1\Comp_2018_1\tabelaDeEstados.json"));
                    break;

                case 3:
                    tabelaLexica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\tabelaDeEstados.json"));
                    tabelaSintatica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\tabelaSintatica.json"));

                    break;
            }
        }

        public void MachineStart()
        {
            estado = 0;
            possicaoDalista = 0;
            PilhaDeProducoes = new Stack<string>();
            PilhaEstados = new Stack<int>();
            PilhaDeProducoes.Clear();
            PilhaEstados.Push(estado);
            //CountLexico = Lexico.listLexica.Count;
            LoadFiles(3);


            MainMachineSintatico();
        }
        

        public void MainMachineSintatico()
        {
            int i = 1;
            //Acao( estado, "inicio");
            PilhaDeProducoes.Push("$");
            //PilhaDeProducoes.Push(Lexico.listLexica[i].lexema);

            while (true)
            {

                //Console.Write(Lexico.listLexica[68].token.ToString() + "\n");
                //Console.Write(tabelaLexica["tabela"][47]["next_state"].ToString());
                //Console.Write(Lexico.listLexica[i].lexema);
                //Console.Write(Lexico.listLexica[possicaoDalista].token.ToString());

                if (tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[possicaoDalista].token.ToString()][0].ToString() == "shift")
                {
                    estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[possicaoDalista].token.ToString()][1].ToString());

                    //Console.Write("empilha\n");
                    PilhaDeProducoes.Push(Lexico.listLexica[possicaoDalista].token.ToString());
                    PilhaEstados.Push(estado);
                    foreach (var item in PilhaDeProducoes)
                    {
                        Console.Write(item + " " );
                    }
                    Console.Write(" "+ PilhaEstados.Peek()+ "\n");

                    possicaoDalista++;
                }
                else if (tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[possicaoDalista].token.ToString()][0].ToString() == "reduce")
                {
                    estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[possicaoDalista].token.ToString()][1].ToString());
                    Desempilha(estado);

                    //Console.Write(tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[possicaoDalista].token.ToString()][0].ToString());
                    

                }
                else if (tabelaLexica["tabela"][estado]["next_state"][Lexico.listLexica[0].lexema].ToString() == "aceita")
                {
                    break;
                }
                else
                {
                    Console.Write("erro");
                }
                //PilhaEstados.Push(estado);


                //PilhaDeProducoes 

            }

        }

        private string Desempilha(int indexRef)
        {
            

            Reduzido = tabelaSintatica["tabela"]["estado"][estado.ToString()]["reducao"].ToString();
            int tamRed = Int32.Parse(tabelaSintatica["tabela"]["estado"][estado.ToString()]["size"].ToString());
            
            for(int y = 0; y < tamRed; y++)
            {
                //Console.Write("desempilha\n");
                PilhaDeProducoes.Pop();
                PilhaEstados.Pop();
            }
            PilhaDeProducoes.Push(Reduzido);
            
            estado = PilhaEstados.Peek();
            Console.Write(estado+" ");

            //if(Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][Reduzido].ToString())== 38)
            {}

            estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][Reduzido].ToString());
            PilhaEstados.Push(estado);


            foreach (var item in PilhaDeProducoes)
            {
                Console.Write(item + " ");
            }
            Console.Write("\n");

            // Console.Write(tamRed);
            //tabelaSintatica["tabela"]["estado"][estado.ToString()][0][0].ToString()); 

            return null;
        }

        public string Acao(int estado , string simbolo)
        {
            //Console.Write(tabelaLexica["tabela"][estado]["next_state"][simbolo].Type.ToString());

            return null;
        }


    }
}
