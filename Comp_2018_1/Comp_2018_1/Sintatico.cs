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
        //public Stack<string> PilhaAtributos;

        JObject tabelaLexica;
        JObject tabelaSintatica;

        public int estado;
        public string cadeia;

        int possicaoDalista;

        string Reduzido;

        Lexico.Table_simbols SupLexStruct;
        Lexico.Table_simbols b;
        public bool addOnceTime;
        List<String> saidaArquivo = new List<string>();


        List<Lexico.Table_simbols> ListaParaManipularAtributo = new List<Lexico.Table_simbols>();
        int indexVariavelTemporaria = 0;

        Stack<Lexico.Table_simbols> pilhaDeSimbolos = new Stack<Lexico.Table_simbols>();

        Lexico.Table_simbols manipulador = new Lexico.Table_simbols();

        bool erroSemantico;




        public void LoadFiles(int location)
        {
            switch (location)
            {

                case 1:

                    tabelaLexica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\Desktop\compilador\Comp_2018_1\Comp_2018_1\tabelaDeEstados.json"));
                    tabelaSintatica = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\Desktop\compilador\Comp_2018_1\Comp_2018_1\tabelaSintatica.json"));

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
            //PilhaAtributos = new Stack<string>();
            PilhaDeProducoes.Clear();
            pilhaDeSimbolos.Clear();
            PilhaEstados.Push(estado);

            LoadFiles(3);


            addOnceTime = true;




            b.lexema = "&";
            b.tipo = "";
            b.token = Lexico.Token.CIFRAO;


            MainMachineSintatico();
        }

        public string TraformCifra(Lexico.Table_simbols a)
        {
            if (a.token != Lexico.Token.CIFRAO)
                return a.token.ToString();
            else return "$";
        }

        public void CallLexico()
        {
            Lexico.stoped = false;
            Lexico.RUNTHEMACHINE();
        }

        private void EscreveProgramaC()
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\programa.c"))
            {
                file.WriteLine(cabecalhoPromgramaC);
                foreach (string line in saidaArquivo)
                {
                    file.WriteLine(line);
                }
                file.WriteLine(rodapeProgramaC);
            }
        }

        public string cabecalhoPromgramaC = "#include<stdio.h>\ntypedef char literal[256];\nvoid main(void)\n{ ";
        public string rodapeProgramaC = "}";


        public void MainMachineSintatico()
        {
            int i = 1;
            PilhaDeProducoes.Push("$");
            //PilhaAtributos.Push("");

            manipulador.lexema = "$";
            manipulador.token = Lexico.Token.SS;
            manipulador.tipo = "";
            manipulador.atributo = "";
            pilhaDeSimbolos.Push(manipulador);



            while (true)
            {

                if (erroSemantico)
                    break;

                try
                {
                    if (Lexico.error)
                        break;
                    if (Lexico.indexElementsOfText < Lexico.text_Char.Length)
                    {
                        CallLexico();
                    }
                    else if (addOnceTime)
                    {
                        Lexico.listLexica.Add(b);
                        addOnceTime = false;
                    }
                    if (tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])].Type == JTokenType.Array && tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])][0].ToString() == "shift")
                    {
                        estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])][1].ToString());

                        //Console.Write("\n empilha estado: " + PilhaEstados.Peek() + " producao: " + TraformCifra(Lexico.listLexica[possicaoDalista]) + " \n\n");



                        PilhaDeProducoes.Push(TraformCifra(Lexico.listLexica[possicaoDalista]));
                        pilhaDeSimbolos.Push(Lexico.listLexica[possicaoDalista]);
                        PilhaEstados.Push(estado);
                        //PilhaAtributos.Push("");

                        foreach (var item in PilhaDeProducoes)
                        {
                        //    Console.Write(item + " ");
                        }
                       // Console.Write("\n");
                        possicaoDalista++;
                    }
                    else if (tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])].Type == JTokenType.Array && tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])][0].ToString() == "reduce")
                    {
                        estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])][1].ToString());
                        Desempilha(estado);

                    }
                    else if (tabelaLexica["tabela"][estado]["next_state"][TraformCifra(Lexico.listLexica[possicaoDalista])].ToString() == "aceita")
                    {

                        Console.Write("!!!SUCESSO!!!!");
                        break;
                    }

                }
                catch (Exception)
                {
                    //Console.Write("ERROOO! na sintatico na linha "+ Lexico.line);
                    ShowError(estado);
                    break;
                    throw;
                }

            }

        }

        public void ShowError(int index)
        {
            string errorLog = "erro!" + ", esperado";
            foreach (JProperty t in tabelaLexica["tabela"][index]["next_state"])
            {
                if (t.Value.Type == JTokenType.Array)
                {
                    errorLog += "[" + t.Name + "]";
                }
            }
            Console.WriteLine(errorLog);
        }



        private string Desempilha(int indexRef)
        {
            ListaParaManipularAtributo.Clear();

            Reduzido = tabelaSintatica["tabela"]["estado"][estado.ToString()]["reducao"].ToString();
            int tamRed = Int32.Parse(tabelaSintatica["tabela"]["estado"][estado.ToString()]["size"].ToString());


            if (estado == 20 || Reduzido == "A")
            { }



            for (int y = 0; y < tamRed; y++)
            {
                if (pilhaDeSimbolos.Peek().lexema == "B")
                { }
                //Console.Write("desempilha produções: "+ PilhaDeProducoes.Peek()+ " estados: "+ PilhaEstados.Peek() +" \n");
                ListaParaManipularAtributo.Add(pilhaDeSimbolos.Pop());

                PilhaDeProducoes.Pop();
                PilhaEstados.Pop();
                //PilhaAtributos.Pop();
            }
            //Console.Write("\n\n");

            //******* EStou criando um novo simbolo************
            PilhaDeProducoes.Push(Reduzido);

            manipulador.tipo = "";
            manipulador.lexema = "";
            manipulador.token = (Lexico.Token)Enum.Parse(typeof(Lexico.Token), Reduzido);
            manipulador.atributo = "";
            ListaParaManipularAtributo.Add(manipulador);

            pilhaDeSimbolos.Push(manipulador);
            //PilhaAtributos.Push("");

            Semantico(estado);


            estado = PilhaEstados.Peek();


            if(estado == 38)
            {}


            estado = Int32.Parse(tabelaLexica["tabela"][estado]["next_state"][Reduzido].ToString());
            //Console.Write("[Empilha produções: " + Reduzido + "] [empilha estado: " + estado + "] \n\n");
            PilhaEstados.Push(estado);

            //Console.Write("desempilha produções: " + PilhaDeProducoes.Peek() + " estados: " + PilhaEstados.Peek() + " \n");

            foreach (var item in PilhaDeProducoes)
            {
                //Console.Write(item + " ");
            }
            //Console.Write("\n");->>>>>>>>>
            // Console.Write("\n");

            EscreveProgramaC();
            return null;
        }

        public string Changetype(string entrada)
        {
            if (entrada == "inteiro")
                return "int";
            if (entrada == "real")
                return "double";
           
            return entrada;
        }

        public void Semantico(int indexEstate)
        {
            switch (indexEstate)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    saidaArquivo.Add("\n\n");
                    break;
                case 6:

                    manipulador = ListaParaManipularAtributo[2];
                    manipulador.tipo = ListaParaManipularAtributo[1].tipo;
                    pilhaDeSimbolos.Push(manipulador);

                    saidaArquivo.Add(Changetype(ListaParaManipularAtributo[1].tipo) + " " + ListaParaManipularAtributo[2].lexema + " ;");

                    

                    int i = 0;

                    foreach (var element in Lexico.Tables_lexema)
                    {
                        if (element.lexema == manipulador.lexema)
                        {
                            manipulador = Lexico.Tables_lexema[i];
                            manipulador.tipo = ListaParaManipularAtributo[1].tipo;
                            Lexico.Tables_lexema[i] = manipulador;
                            break;
                        }
                        i++;
                    }
                    

                    break;
                case 7:

                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = "inteiro";
                    pilhaDeSimbolos.Push(manipulador);

                    break;

                case 8:
                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = "real";
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 9:

                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = "literal";
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 10:
                    break;
                case 11:
                    // 
                    if (ListaParaManipularAtributo[1].tipo != "")
                    {
                        if (ListaParaManipularAtributo[1].tipo == "literal")
                            saidaArquivo.Add("scanf(\"%s\"," + ListaParaManipularAtributo[1].lexema + ");");
                        if (ListaParaManipularAtributo[1].tipo == "inteiro")
                            saidaArquivo.Add("scanf(\"%d\", &" + ListaParaManipularAtributo[1].lexema + ");");
                        if (ListaParaManipularAtributo[1].tipo == "real")
                            saidaArquivo.Add("scanf(\"%lf\", &" + ListaParaManipularAtributo[1].lexema + ");");
                    }
                    else
                    {
                        Console.Write("ERRO:  VARIAVEL NAO DECLARA." );
                        erroSemantico = true;

                    }
                    break;
                case 12:

                    //verifico se o elemento do scanF é um ID
                    if (Lexico.Tables_lexema.Contains(ListaParaManipularAtributo[1]))                    
                    {
                        saidaArquivo.Add("printf(" + ListaParaManipularAtributo[1].lexema + ");");
                    }
                    else
                    {
                        String sup = ListaParaManipularAtributo[1].tipo;
                        if (sup == "inteiro")
                            sup = "\"%d\",";
                        else if (sup == "real")
                            sup = "\"%lf\",";
                        else if (sup == "literal")
                            sup = "\"%s\",";
                        saidaArquivo.Add("printf("+ sup + ListaParaManipularAtributo[1].lexema + ");");
                    }
                    break;
                case 13:

                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = "literal";
                    manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                    manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 14:
                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = ListaParaManipularAtributo[0].tipo;
                    manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                    manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 15:
                    if (ListaParaManipularAtributo[0].tipo != "")
                    {
                        manipulador = pilhaDeSimbolos.Pop();
                        manipulador.tipo = ListaParaManipularAtributo[0].tipo;
                        manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                        manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                        pilhaDeSimbolos.Push(manipulador);
                    }
                    else
                    {
                         Console.Write("ERRO:  VARIAVEL NAO DECLARA.");
                        erroSemantico = true;
                    }
                    break;
                case 16:
                    break;
                case 17:
                    if (ListaParaManipularAtributo[3].tipo != "")
                    {
                        if (ListaParaManipularAtributo[3].tipo == ListaParaManipularAtributo[1].tipo)
                        {
                            saidaArquivo.Add(ListaParaManipularAtributo[3].lexema + " " + ListaParaManipularAtributo[2].tipo + " " + ListaParaManipularAtributo[1].lexema+";");
                        }
                        else
                        {
                            Console.Write("Erro: Tipos diferentes para atribuição");
                            erroSemantico = true;
                        }
                    }
                    else
                    {
                        Console.Write("Erro: Variável não declarada");
                        erroSemantico = true;
                    }
                    break;
                case 18:
                    if ((ListaParaManipularAtributo[2].tipo != "literal") && (ListaParaManipularAtributo[0].tipo == ListaParaManipularAtributo[2].tipo))
                    {
                        saidaArquivo.Add("T" + indexVariavelTemporaria.ToString() + " = " + ListaParaManipularAtributo[2].lexema + " " + ListaParaManipularAtributo[1].tipo + " " + ListaParaManipularAtributo[0].lexema+";");
                        saidaArquivo.Insert(indexVariavelTemporaria, Changetype(ListaParaManipularAtributo[2].tipo) + " T" + indexVariavelTemporaria.ToString() + ";");


                        manipulador = pilhaDeSimbolos.Pop();
                        //manipulador.lexema = ListaParaManipularAtributo[2].lexema + " " + ListaParaManipularAtributo[1].tipo + " " + ListaParaManipularAtributo[0].lexema;
                        manipulador.lexema = "T" + indexVariavelTemporaria.ToString();
                        manipulador.tipo = ListaParaManipularAtributo[2].tipo;
                        pilhaDeSimbolos.Push(manipulador);

                        indexVariavelTemporaria++;
                    }
                    else
                    {
                        Console.Write("Erro: Operandos com tipos incompatíveis");
                        erroSemantico = true;
                    }
                    break;
                case 19:

                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = ListaParaManipularAtributo[0].tipo;
                    manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                    manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 20:

                    if (ListaParaManipularAtributo[0].tipo != "")
                    {
                        manipulador = pilhaDeSimbolos.Pop();
                        manipulador.tipo = ListaParaManipularAtributo[0].tipo;
                        manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                        manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                        pilhaDeSimbolos.Push(manipulador);
                    }
                    else
                    {
                         Console.Write("ERRO:  VARIAVEL NAO DECLARA");
                         erroSemantico = true;
                    }

                    break;
                case 21:

                    manipulador = pilhaDeSimbolos.Pop();
                    manipulador.tipo = ListaParaManipularAtributo[0].tipo;
                    manipulador.lexema = ListaParaManipularAtributo[0].lexema;
                    manipulador.atributo = ListaParaManipularAtributo[0].lexema;
                    pilhaDeSimbolos.Push(manipulador);

                    break;
                case 22:
                    break;
                case 23:
                    saidaArquivo.Add("}");
                    break;
                case 24:
                    saidaArquivo.Add("if(" + ListaParaManipularAtributo[2].lexema + "){");
                    break;
                case 25:
                    if (ListaParaManipularAtributo[0].tipo == ListaParaManipularAtributo[2].tipo)
                    {
                        saidaArquivo.Add("T" + indexVariavelTemporaria.ToString() + " = " + ListaParaManipularAtributo[2].lexema + " " + ListaParaManipularAtributo[1].tipo + " " + ListaParaManipularAtributo[0].lexema +";" );
                        saidaArquivo.Insert(indexVariavelTemporaria, Changetype(ListaParaManipularAtributo[2].tipo)+ " T" + indexVariavelTemporaria.ToString()+";");


                        manipulador = pilhaDeSimbolos.Pop();
                        //manipulador.lexema = ListaParaManipularAtributo[2].lexema + " " + ListaParaManipularAtributo[1].tipo + " " + ListaParaManipularAtributo[0].lexema;
                        manipulador.lexema = "T" + indexVariavelTemporaria.ToString();
                        manipulador.tipo = ListaParaManipularAtributo[2].tipo;
                        pilhaDeSimbolos.Push(manipulador);

                        indexVariavelTemporaria++;

                    }
                    else
                    {
                         Console.Write("Erro: Operandos com tipos incompatíveis");
                         erroSemantico = true;
                    }
                    break;
                default:
                    break;
            }
        }


    }
}
