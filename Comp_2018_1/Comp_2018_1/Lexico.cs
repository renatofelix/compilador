using System;
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

        /*
         *
         *struct State
{
    public Condition[] conditions;

    public State(Condition[] conditions)
    {
        this.conditions = conditions;
    }

    public void Enter(){}
    public void Update(){}
    public void Exit(){}
}

struct Condition
{
    public State nextState;
    public virtual boolean Check(){}
}

struct KeyCondition
{
    public string key;

    public KeyCondition( string content, State nextState )
    {
        this.key = content;
        this.nextState = nextState;
    }

    public override boolean Check()
    {
        return Input.GetKeyDown("A") === key;
    }
}

class FSM
{
    public State current;
    public List<State> history;

    public FSM( State start )
    {
        current = start;
        current.Enter();
        history.Add(current);
    }

    public void UpdateState( String key )
    {
        current.Update();
        this.CheckTransition();
    }

    public void CheckTransition()
    {
        if( current.conditions.Length == 0 )
        {

        }
        else
        {
            for( int i = 0 ; i < current.conditions.Length ; i++ )
            {
                if( current.conditions[i].Check() )
                {
                    current.Exit();

                    current = current.conditions[i].nextState;
                    history.Add(current);

                    current.Enter();
                    return;
                }
            }
        }


        // ERROR ERROR
    }

}


int Main()
{
    State startState = new State();
    startState.conditions = new[]{
        new KeyCondition( "A", new State( new KeyCondition[]{
            
        }, new State() ) );
    };

    FSM fsm = new FSM(state);

    cin > 

    fsm.UpdateState(key);
}

{
    "A": [ 
        ""
    ]
}
         * 
         */
    }
}
