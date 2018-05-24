using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimParams
{
    //static int _SideSpeed = int.MinValue;
    //public static int SideSpeed {
    //    get {
    //        if(_SideSpeed == int.MinValue ) {
    //            _SideSpeed = Animator.StringToHash("SideSpeed");
    //        }
    //        return _SideSpeed;
    //    }
    //}
    //static int _ForwardSpeed = int.MinValue;
    //public static int ForwardSpeed {
    //    get {
    //        if( _ForwardSpeed == int.MinValue ) {
    //            _ForwardSpeed = Animator.StringToHash("ForwardSpeed");
    //        }
    //        return _ForwardSpeed;
    //    }
    //}
    //static int _UpSpeed = int.MinValue;
    //public static int UpSpeed {
    //    get {
    //        if( _UpSpeed == int.MinValue ) {
    //            _UpSpeed = Animator.StringToHash("UpSpeed");
    //        }
    //        return _UpSpeed;
    //    }
    //}
    static int _Speed = int.MinValue;
    public static int Speed {
        get {
            if( _Speed == int.MinValue ) {
                _Speed = Animator.StringToHash("Speed");
            }
            return _Speed;
        }
    }
    static int _Facing = int.MinValue;
    public static int Facing {
        get {
            if( _Facing == int.MinValue ) {
                _Facing = Animator.StringToHash("Facing");
            }
            return _Facing;
        }
    }
}
