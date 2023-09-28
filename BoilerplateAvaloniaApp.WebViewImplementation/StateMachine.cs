﻿namespace BoilerplateAvaloniaApp.WebViewImplementation;

public class StateMachine<StateType> where StateType : Enum {
    private readonly object syncObject = new object();

    private StateType currentState;

    public void SetState(StateType state) {
        lock (syncObject) {
            currentState = state;
            Monitor.PulseAll(syncObject);
        }
    }

    public void WaitFor(StateType state) {
        lock (syncObject) {
            while (!currentState.Equals(state)) {
                Monitor.Wait(syncObject);
            }
        }
    }
}
