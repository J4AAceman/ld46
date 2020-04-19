using System.Collections;
using System.Collections.Generic;

public class GameCellScript {

    public static bool State { get; set; }

    public int CurrentValue {
        get { return State ? _value1 : _value2; }
        set {
            if (State) {
                _value1 = value;
            }
            else {
                _value2 = value;
            }
        }
    }
    public int NextValue {
        get { return State ? _value2 : _value1; }
        set {
            if (State) {
                _value2 = value;
            }
            else {
                _value1 = value;
            }
        }
    }

    private int _value1 = 0;
    private int _value2 = 0;

}
