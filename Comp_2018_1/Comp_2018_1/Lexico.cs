using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Comp_2018_1
{
    class Lexico
    {
        public int state;
        public string currentWord;
        private int indexElementsOfText = 0;

        List<Table_simbols> Tables_lexema = new List<Table_simbols>();

        List<int> finalStates =new List<int>{ 2, 4, 8, 9, 11, 12, 14, 19, 16, 18, 15, 13, 17, 22, 24, 23, 25, 20, 21, 26 };
        JObject configurationMachine = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\configuration_state.json"));

        static JObject preReservada = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\reservada.json"));
        string reservadaString = preReservada.ToString();

        public static string text = System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\text_file.txt");
        char[] text_Char = text.ToCharArray();
        byte[] textDec = Encoding.ASCII.GetBytes(text);

        


        /// <summary>
        /// struct table of symbols 
        /// </summary>
        public struct Table_simbols
        {
            public Token token;
            public string lexema;
            public string tipo;
        }

        public void FeedList()
        {
            Table_simbols tab;
            for (int i = 0; i< 13 ;i++)
            {
                tab.lexema = preReservada["reservada"][i]["lexema"].ToString();
                tab.token = (Token)Enum.Parse(typeof(Token), preReservada["reservada"][i]["token"].ToString());
                tab.tipo = preReservada["reservada"][i]["tipo"].ToString();
                Tables_lexema.Add(tab);
            }
            
        }



        public void CheckList(string lexToCheck)
        {
            int validate = 0;
            foreach(Table_simbols tb in Tables_lexema)
            {
                if (tb.tipo == lexToCheck)
                {
                    validate = 1;
                    break;
                }
            }
            if (validate == 1)
            {
                return;
            }
            else
            {
                ClassificationLexema();
            }
            
        }


        public void ShowTable()
        {
            Console.Write("\nLexama:\t\tToken:\t\t\tTipo:\n");
            foreach (Table_simbols a in Tables_lexema)
            {
                Console.Write(a.lexema+"\t\t\t"+a.token+"\t\t\t"+a.tipo+"\n");
            }
        }
        

        /// <summary>
        /// show hexa all text
        /// </summary>
        public void TesteShowEx()
        {
            for (int i = 0; i < text_Char.Length; i++)
            {
                Console.Write(" "+((int)text_Char[i]).ToString("x"));
            }
        }

        public int MachineStart()
        {
            //TesteShowEx();

            FeedList();
            state = 0;
            currentWord = "";
            RunMachine(state, textDec[indexElementsOfText]);
            return 0;
        }
        /// <summary>
        /// List of possibles token 
        /// </summary>
        public enum Token
        {
            Num,
            Literal,
            Id,
            Comentario,
            Tab,
            Salto,
            Espaço,
            EOF,
            OPR,
            RCB,
            OPM,
            AB_P,
            FC_P,
            inicio,
            varinicio,
            varfim,
            escreva,
            leia,
            se,
            entao,
            senao,
            fimse,
            fim,
            Inteiro,
            literal,
            real,
            ERRO
        }

        /// <summary>
        /// convert byte to hexa
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string ConversorToEX(byte b)
        {
            if (b >= 48 && b <= 57)
                return "N";
            else if ((b >= 65 && b <= 90) || (b >= 97 && b <= 122))
                return "L";
            else if (b >= 9 && b <= 13 || b == 32)
                return "SPACE_TAB_ETC";
            return b.ToString("x");
        }

        public void RunMachine(int stateMachine, byte toConvertToHex)
        {
            if ((ConversorToEX(toConvertToHex)) == "SPACE_TAB_ETC" || ((stateMachine == 10) && (ConversorToEX(toConvertToHex) != "7d"))|| ((stateMachine == 7) && (ConversorToEX(toConvertToHex) != "22")))
            {
                if((ConversorToEX(toConvertToHex)) != "SPACE_TAB_ETC")
                {
                    IgnoreExeption();
                }
                else {
                    ExeptionValidate();
                    return;
                }
            }
            //else if ((ConversorToEX(toConvertToHex)) == "00")
            //{
            //    Console.Write(currentWord);
            //    return;
            //}

            if ((configurationMachine["configuration"][stateMachine]["next_state"] as JObject).Count > 0)
            {
                if((configurationMachine["configuration"][stateMachine]["next_state"][ConversorToEX(toConvertToHex)]) == null)
                {
                    FinishWord();
                    return;
                }
                if (indexElementsOfText < text_Char.Length)
                {
                    int nextState = (int)(configurationMachine["configuration"][stateMachine]["next_state"][ConversorToEX(toConvertToHex)]);
                    state = nextState;
                    currentWord += text_Char[indexElementsOfText];

                    indexElementsOfText = indexElementsOfText < text_Char.Length ? indexElementsOfText + 1 : indexElementsOfText;
                }
                if(indexElementsOfText < text_Char.Length)
                {
                    RunMachine(state, textDec[indexElementsOfText]);
                }
                else
                {
                    PrintWord();
                }
            }
            else 
            {
                FinishWord();
                return;
            }
        }

        private void FinishWord()
        {
            //criar o CheckLexema para checar a palavra anterior
            PrintWord();

            // adiciona a nova palavra na pilha de letras
            currentWord = text_Char[indexElementsOfText].ToString();
            //incrementa o index para checar a nova palavra
            if (indexElementsOfText < text_Char.Length)
            {
                indexElementsOfText++;
                RunMachine(0, textDec[indexElementsOfText]);
            }
            else
            {
                PrintWord();
            }

        }

        private void ExeptionValidate()
        {
            //retornar a variavel completa anterior
            PrintWord();
            currentWord = text_Char[indexElementsOfText].ToString();
            if (indexElementsOfText < text_Char.Length)
            {
                indexElementsOfText++;
                RunMachine(0, textDec[indexElementsOfText]);
            }
            else
            {
                PrintWord();
            }

        }

        private void PrintWord()
        {
            Console.Write("."+currentWord);
        }

        private void IgnoreExeption()
        {
            currentWord += text_Char[indexElementsOfText];
        }

        private void ClassificationLexema()
        {

        }
    }
}
