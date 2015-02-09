using UnityEngine;
using System.Collections.Generic;
using FSMHelper;

public class FSMSystem
{
    public StateID CurrentStateID { get { return currentStateID; } }
    public FSMState CurrentState { get { return currentState; } }

    private List<FSMState> states;
    private StateID currentStateID;
    private FSMState currentState;


    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void AddState(FSMState state)
    {
        string errorMsg = "";
        if (state == null)
        {
            errorMsg = "Null reference is not allowed.";
            FSMSystemError(errorMsg);
            return;
        }

        if (states.Count == 0)
        {
            states.Add(state);
            currentState = state;
            currentStateID = state.ID;
            return;
        }

        foreach (FSMState s in states)
        {
            if (s.ID == state.ID)
            {
                errorMsg = "Impossible to add state " + state.ID.ToString() + " because state has already been added.";
                FSMSystemError(errorMsg);
                return;
            }
        }
        states.Add(state);
    }

    public void DeleteState(StateID id)
    {
        string errorMsg = "";
        if (id == StateID.NullStateID)
        {
            errorMsg = "NullStateID is not allowed for a real state.";
            FSMSystemError(errorMsg);
            return;
        }

        foreach (FSMState state in states)
        {
            if (state.ID == id)
            {
                states.Remove(state);
                return;
            }
        }

        errorMsg = "Impossible to delete state " + id.ToString() + ". It was not on the list of states.";
        FSMSystemError(errorMsg);
    }

    public void PerformTransition(Transition trans)
    {
        string errorMsg = "";
        if (trans == Transition.NullTransition)
        {
            errorMsg = "NullTransition is not allowed for a real transition.";
            FSMSystemError(errorMsg);
            return;
        }

        StateID id = currentState.GetOutputState(trans);
        if (id == StateID.NullStateID)
        {
            errorMsg = "State " + currentStateID.ToString() + " does not have a target state for transition " + trans.ToString();
            FSMSystemError(errorMsg);
            return;
        }

        currentStateID = id;
        foreach (FSMState state in states)
        {
            if (state.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();
                currentState = state;
                currentState.DoBeforeEntering();
                break;
            }
        }
    }

    private void FSMSystemError(string errorMsg)
    {
        Debug.LogError("FSMSystem Error: " + errorMsg);
    }
}