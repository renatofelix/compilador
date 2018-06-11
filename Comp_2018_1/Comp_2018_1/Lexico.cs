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
        public int lastState = 0 ;
        public string currentWord;
        private int indexElementsOfText = 0;
        private bool itString;
        private bool itComment;
        private int line = 1;

        public Table_simbols lexicoEnviado;

        static public List<Table_simbols> listLexica = new List<Table_simbols>();

        List<Table_simbols> Tables_lexema = new List<Table_simbols>();

        List<int> finalStates =new List<int>{ 2, 4, 8, 9, 11, 12, 14, 19, 16, 18, 15, 13, 17, 22, 24, 23, 25, 20, 21, 26 };
        JObject configurationMachine;
        static JObject preReservada;
        string reservadaString;

        public static string text;
        char[] text_Char;
        byte[] textDec;

        private bool isStriging = false;
        private bool isCommenting = false;

        public void ShowListToSend()
        {
            foreach (var item in listLexica)
            {
                Console.Write(" "+item.lexema+" ");
            }
        }

        public void LoadFiles(int location)
        {
            switch (location)
            {
                case 1:
                    configurationMachine = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\configuration_state.json"));
                    preReservada = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\reservada.json"));
                    reservadaString = preReservada.ToString();
                    text = System.IO.File.ReadAllText(@"C:\Users\Renato\compilador\Comp_2018_1\Comp_2018_1\text_file.txt");
                    text_Char = text.ToCharArray();
                    textDec = Encoding.ASCII.GetBytes(text);
                    break;

                case 2:
                    configurationMachine = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\renat\cc\compilador\Comp_2018_1\Comp_2018_1\configuration_state.json"));
                    preReservada = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\renat\cc\compilador\Comp_2018_1\Comp_2018_1\reservada.json"));
                    reservadaString = preReservada.ToString();
                    text = System.IO.File.ReadAllText(@"C:\Users\renat\cc\compilador\Comp_2018_1\Comp_2018_1\text_file.txt");
                    text_Char = text.ToCharArray();
                    textDec = Encoding.ASCII.GetBytes(text);
                    break;

                case 3:
                    configurationMachine = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\configuration_state.json"));
                    preReservada = JObject.Parse(System.IO.File.ReadAllText(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\reservada.json"));
                    reservadaString = preReservada.ToString();
                    text = System.IO.File.ReadAllText(@"C:\Users\T-Gamer\cc\compilador\Comp_2018_1\Comp_2018_1\text_file.txt");
                    text_Char = text.ToCharArray();
                    textDec = Encoding.ASCII.GetBytes(text);
                    break;
            }
        }


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

        /// <summary>
        /// verifica se o token existe, caso nao exista adiciona ele na tabela
        /// </summary>
        /// <param name="lexToCheck"></param>
        public Table_simbols CheckList(string lexToCheck)
        {
            lastState = 0;
            int validate = 0;
            int index = 0;
            foreach(Table_simbols tb in Tables_lexema)
            {
                if (tb.lexema == lexToCheck)
                {
                    validate = 1;
                    break;
                }
                index++;
            }
            if (validate == 1)
            {
                //Console.Write("entrou no validade");
                //Console.Write("Token: " + Tables_lexema[index].token + " lexema: " + Tables_lexema[index].lexema + " tipo: " + Tables_lexema[index].tipo + "\n");
                listLexica.Add(Tables_lexema[index]);
                return Tables_lexema[index];
            }
            else
            {
                return ClassificationLexema(lexToCheck);
            }
        }


        public void ShowTable()
        {
            //Console.Write("\nLexama:\t\tToken:\t\t\tTipo:\n");
            foreach (Table_simbols a in Tables_lexema)
            {
                Console.Write("\n"+a.lexema+"\t\t\t"+a.token+"\t\t\t"+a.tipo);
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
            /*seleciona qual maquina para funcionar*/
            LoadFiles(3);

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
            //Num,
            //Literal,
            id,
            Comentario,
            Tab,
            Salto,
            Espaço,
            EOF,
            opr,
            rcb,
            opm,
            AB_P,
            FC_P,
            PT_V,
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
            inteiro,
            lit,
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
            if (isStriging && b!=34)
                return "QQC";
            if (isCommenting && b != 125)
                return "QQC";

            if (b >= 48 && b <= 57)
                return "N";
            else if ((b >= 65 && b <= 90) || (b >= 97 && b <= 122))
                return "L";
            else if (b >= 9 && b <= 13 || b == 32)
                return "SPACE_TAB_ETC";
            return b.ToString("x");
        }

        private void CheckIsString(string CHAR)
        {
            if((ConversorToEX(textDec[indexElementsOfText]) == "22"))
            {
                itString = true;
            }
        }
        private void CheckIsComment(string CHAR)
        {
            if ((ConversorToEX(textDec[indexElementsOfText]) == "7b"))
            {
                itComment = true;
            }
        }

        public void RunMachine(int stateMachine, byte toConvertToHex)
        {
            Countline(text_Char[indexElementsOfText]);
            if((ExistCharacterProibido(text_Char[indexElementsOfText])&&(!isStriging && !isCommenting)))
            {
                ErrorLog(3);
                return;
            }

            if(line==15 && text_Char[indexElementsOfText] == 'B')
            {

            }

            /*ignora espacos tab etc, */
            if (((ConversorToEX(toConvertToHex)) == "SPACE_TAB_ETC") || (ConversorToEX(toConvertToHex) == "22") || (ConversorToEX(toConvertToHex) == "7b") || isStriging || isCommenting)
            {

                if(!isStriging && stateMachine == 0 && (ConversorToEX(toConvertToHex) == "22"))
                {
                    PrintWord();
                    isStriging = true;
                    currentWord = text_Char[indexElementsOfText].ToString();
                    indexElementsOfText++;
                    RunMachine(7, textDec[indexElementsOfText]);
                    return;
                }
                if(isStriging && stateMachine == 7)
                {
                    if (ConversorToEX(toConvertToHex) != "22")
                    {
                        currentWord += text_Char[indexElementsOfText];
                        if (indexElementsOfText < text_Char.Length-1)
                            indexElementsOfText++;
                        else
                        {
                            ErrorLog(1);
                            return;
                        }
                           
                        RunMachine(7, textDec[indexElementsOfText]);
                        return;
                    }
                    else
                    {
                        currentWord += text_Char[indexElementsOfText];
                        lastState = 8;
                        indexElementsOfText++;
                        isStriging = false;
                        ExeptionValidate();
                        return;
                    }
                }

                if (!isCommenting && stateMachine == 0 && (ConversorToEX(toConvertToHex) == "7b"))
                {
                    PrintWord();
                    isCommenting = true;
                    currentWord = text_Char[indexElementsOfText].ToString();
                    indexElementsOfText++;
                    RunMachine(10, textDec[indexElementsOfText]);
                    return;
                }
                if (isCommenting && stateMachine == 10)
                {
                    if (ConversorToEX(toConvertToHex) != "7d")
                    {
                        currentWord += text_Char[indexElementsOfText];
                        if (indexElementsOfText < text_Char.Length - 1)
                            indexElementsOfText++;
                        else
                        {
                            ErrorLog(2);
                            return;
                        }
                        RunMachine(10, textDec[indexElementsOfText]);
                        return;
                    }
                    else
                    {
                        currentWord += text_Char[indexElementsOfText];
                        lastState = 11;
                        indexElementsOfText++;
                        isCommenting = false;
                        ExeptionValidate();
                        return;
                    }
                }
                
                if((ConversorToEX(toConvertToHex)) == "SPACE_TAB_ETC")
                {
                    ExeptionValidate();
                    return;
                }
            }
            
            //Console.Write(((configurationMachine["configuration"][stateMachine]["next_state"] as JObject).Count)+"linha\n");
            if ((configurationMachine["configuration"][stateMachine]["next_state"] as JObject).Count > 0)
            {
                
                //lastState = state;

                if ((configurationMachine["configuration"][stateMachine]["next_state"][ConversorToEX(toConvertToHex)]) == null)
                {
                    lastState = state;

                    ExeptionValidate();
                    return;
                }
                
                if (indexElementsOfText < text_Char.Length)
                {
                    int nextState = (int)(configurationMachine["configuration"][stateMachine]["next_state"][ConversorToEX(toConvertToHex)]);
                    
                    state = nextState;
                    lastState = nextState;
                    currentWord += text_Char[indexElementsOfText];

                    indexElementsOfText = indexElementsOfText < text_Char.Length ? indexElementsOfText + 1 : indexElementsOfText;
                }

                //faz a maquina rodar ate ela parar
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

            if((configurationMachine["configuration"][0]["next_state"][ConversorToEX(textDec[indexElementsOfText])]) != null)
            {
                int actualState = (int)(configurationMachine["configuration"][0]["next_state"][ConversorToEX(textDec[indexElementsOfText])]);
                //lastState = nextState;
                state = actualState;
                indexElementsOfText++;
                if (indexElementsOfText < text_Char.Length)
                {
                    RunMachine(actualState, textDec[indexElementsOfText]);
                }
            }
            else if((configurationMachine["configuration"][0]["next_state"][ConversorToEX(textDec[indexElementsOfText])]) == null)
            {
                indexElementsOfText++;
                if (indexElementsOfText < text_Char.Length)
                {
                    RunMachine(0, textDec[indexElementsOfText]);
                }
            }
            else
                PrintWord();

            /*
            //indexElementsOfText++;
            if (indexElementsOfText < text_Char.Length)
            {
                RunMachine(0, textDec[indexElementsOfText]);
            }
            else
            {
                PrintWord();
            }
            */

        }

        private void FinishExeption()
        {
            currentWord = text_Char[indexElementsOfText].ToString();
            PrintWord();
            indexElementsOfText++;

            if (indexElementsOfText < text_Char.Length)
            {
                RunMachine(0, textDec[indexElementsOfText]);
            }

        }


         private void  Midware()
        {
            /*if (text_Char[indexElementsOfText]!='\n'&& text_Char[indexElementsOfText] != '\t'&& text_Char[indexElementsOfText] != '\r'&& text_Char[indexElementsOfText] != 32)
            {

                
                int actualState = (int)(configurationMachine["configuration"][0]["next_state"][ConversorToEX(textDec[indexElementsOfText])]);
                currentWord = text_Char[indexElementsOfText].ToString();
                PrintWord();
                indexElementsOfText++;
                //lastState = actualState;
                state = 0;
                if (indexElementsOfText < text_Char.Length)
                {
                    RunMachine(0, textDec[indexElementsOfText]);
                }
            }
            else
            {
            */
                currentWord = string.Empty;
                state = 0;
                RunMachine(0, textDec[indexElementsOfText]);
            //}

            
        }
    
        private void ExeptionValidate()
        {
            //retornar a variavel completa anterior
            if (!string.IsNullOrEmpty(currentWord))
            {
                PrintWord();
                Midware();
                return;
            }
            currentWord = text_Char[indexElementsOfText].ToString();
            PrintWord();
            currentWord = string.Empty;
            if (indexElementsOfText < text_Char.Length-1)
            {
                indexElementsOfText++;
                RunMachine(0, textDec[indexElementsOfText]);
            }
            else
            {
                PrintWord();
            }

        }
        private Table_simbols PrintWord()
        {            
            string a = currentWord ;
            //Console.Write(a);

            //Table_simbols
            if (lastState == 9)
            {
                
                lastState = 0;
                return  CheckList(currentWord);
            }
            else
            {
                if (line == 15 && currentWord == ">")
                {

                }
                return ClassificationLexemaComplete(currentWord);
            }
            //lastState = 0;
        }

        private void IgnoreExeption()
        {
            if(indexElementsOfText < text_Char.Length)
            currentWord += text_Char[indexElementsOfText];
        }

        private Table_simbols ClassificationLexema(string palavra)
        {
            Table_simbols t = new Table_simbols();
            t.lexema = palavra;
            t.token = Token.id;
            t.tipo = "";
            Tables_lexema.Add(t);
            listLexica.Add(t);
            return t;
            Console.Write("Token: " + t.token + "\t lexema: " + t.lexema + "\t tipo: " + t.tipo +"");
        }

        private Table_simbols ClassificationLexemaComplete(string palavra)
        {
            if(line == 15 && lastState == 2)
            {

            }
            switch ( lastState)
            {
                case 0:
                    lastState = 0;
                    return lexicoEnviado;
                    break;

                case 2:
                    lastState = 0;
                    //Console.Write("Token: " + Token.Num + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.inteiro;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 4:
                    lastState = 0;
                    //Console.Write("Token: " + Token.Num + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.inteiro;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 9:
                    lastState = 0;
                    //Console.Write("Token: " + Token.Id + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.id;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 11:
                    lastState = 0;
                    //Console.Write("Token: " + Token.Comentario + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    //lexicoEnviado.token = Token.Comentario;
                    //lexicoEnviado.lexema = palavra;
                   // lexicoEnviado.tipo = null;
                    //listLexica.Add(lexicoEnviado);
                    //return lexicoEnviado;
                    break;
                case 12:
                    lastState = 0;
                    //Console.Write("Token: " + Token.EOF + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.EOF;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 8:
                    lastState = 0;
                    //Console.Write("Token: " + Token.literal + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.lit;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 13:
                    lastState = 0;
                    //Console.Write("Token: " + Token.RCB + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 19:
                    lastState = 0;
                    //Console.Write("Token: " + Token.RCB + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.rcb;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 14:
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPR + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 16:
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPR + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 18:
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPR + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 15:
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPR + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 17:
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPR + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opr;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 22 :
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPM + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opm;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case  21 :
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPM + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opm;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 20 :
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPM + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opm;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 23 :
                    lastState = 0;
                    //Console.Write("Token: " + Token.OPM + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.opm;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 25:
                    lastState = 0;
                   // Console.Write("Token: " + Token.AB_P + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.AB_P;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    break;
                case 24:
                    lastState = 0;
                    //Console.Write("Token: " + Token.FC_P + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.FC_P;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
                case 26:
                    lastState = 0;
                    //Console.Write("Token: " + Token.PT_V + "\t lexema: " + palavra + "\t tipo: " + " " + "");
                    lexicoEnviado.token = Token.PT_V;
                    lexicoEnviado.lexema = palavra;
                    lexicoEnviado.tipo = null;
                    listLexica.Add(lexicoEnviado);
                    return lexicoEnviado;
                    break;
            }
            return lexicoEnviado;

        }

        private void Countline(char a)
        {
            if (a == '\n' && !isStriging && !isCommenting)
            {
                line++; }
          
        }

        private bool ExistCharacterProibido(char c)
        {
            if (c == '@' || c == '#' || c == '%' || c == '$' || c == '¨' || c == '&'||c == '!'|| c == 92 )
                return true;
            else return false;
        }

        private void ErrorLog(int errorNumber)
        {
            switch (errorNumber)
            {
                case 1:
                    Console.Write("Erro na linha "+ line + "!!!,  não fechou STRING ");
                    break;
                case 2:
                    Console.Write("Erro na linha " + line + "!!!,  não fechou COMENTARIO ");
                    break;
                case 3:
                    Console.Write("\nErro na linha " + line + "!!!,  Caracter Invalido!\n");
                    break;

            }
                
        }
    }
}
