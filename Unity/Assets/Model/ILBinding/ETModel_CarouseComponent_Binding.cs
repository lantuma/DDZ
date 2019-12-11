using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_CarouseComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.CarouseComponent);
            args = new Type[]{typeof(UnityEngine.GameObject), typeof(System.Int32), typeof(System.Single)};
            method = type.GetMethod("SetHallDefault", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetHallDefault_0);

            field = type.GetField("instance", flag);
            app.RegisterCLRFieldGetter(field, get_instance_0);
            app.RegisterCLRFieldSetter(field, set_instance_0);


        }


        static StackObject* SetHallDefault_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 4);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @mLoopSpaceTime = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @mTweenStepNum = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.GameObject @target = (UnityEngine.GameObject)typeof(UnityEngine.GameObject).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            ETModel.CarouseComponent instance_of_this_method = (ETModel.CarouseComponent)typeof(ETModel.CarouseComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetHallDefault(@target, @mTweenStepNum, @mLoopSpaceTime);

            return __ret;
        }


        static object get_instance_0(ref object o)
        {
            return ETModel.CarouseComponent.instance;
        }
        static void set_instance_0(ref object o, object v)
        {
            ETModel.CarouseComponent.instance = (ETModel.CarouseComponent)v;
        }


    }
}
