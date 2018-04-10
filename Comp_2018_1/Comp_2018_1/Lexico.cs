﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Comp_2018_1
{
    class Lexico
    {
        public struct Table_simbols
        {
            public Token token;
            public string lexema;
            public string tipo;
        }

        public int MachineState(int actualState)
        {
            return 0;
        }

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
            ERRO
        }

        public string ShowTable(Table_simbols table)
        {
            return "Lexema: " + table.lexema + " " + "Token: " + table.token + " " + "tipo:" + table.tipo;
        }
    }
}
