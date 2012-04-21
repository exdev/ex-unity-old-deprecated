// ======================================================================================
// File         : Event.cs
// Author       : Wu Jie 
// Last Change  : 12/20/2011 | 14:10:24 PM | Tuesday,December
// Description  : 
// ======================================================================================

///////////////////////////////////////////////////////////////////////////////
// usings
///////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

///////////////////////////////////////////////////////////////////////////////
// defines
///////////////////////////////////////////////////////////////////////////////

namespace fsm {

    ///////////////////////////////////////////////////////////////////////////////
    // Event
    ///////////////////////////////////////////////////////////////////////////////

    public class Event : System.EventArgs {
        static public readonly int UNKNOWN = -1; 
        static public readonly int NULL = 0;
        static public readonly int NEXT = 1;
        static public readonly int TRIGGER = 2;
        static public readonly int FINISHED = 3;
        public const int USER_FIELD = 1000;

        // properties
        public int id = UNKNOWN;

        // functions
        public Event ( int _id = -1 ) {
            id = _id;
        }
    }
}

