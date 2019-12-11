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
    unsafe class ETModel_AsyncImageDownloadComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.AsyncImageDownloadComponent);
            args = new Type[]{typeof(System.String), typeof(UnityEngine.UI.Image), typeof(System.Action), typeof(System.Boolean)};
            method = type.GetMethod("SetAsyncImage", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, SetAsyncImage_0);

            field = type.GetField("instance", flag);
            app.RegisterCLRFieldGetter(field, get_instance_0);
            app.RegisterCLRFieldSetter(field, set_instance_0);


        }


        static StackObject* SetAsyncImage_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 5);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @needCache = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Action @action = (System.Action)typeof(System.Action).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityEngine.UI.Image @texture = (UnityEngine.UI.Image)typeof(UnityEngine.UI.Image).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 4);
            System.String @url = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 5);
            ETModel.AsyncImageDownloadComponent instance_of_this_method = (ETModel.AsyncImageDownloadComponent)typeof(ETModel.AsyncImageDownloadComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.SetAsyncImage(@url, @texture, @action, @needCache);

            return __ret;
        }


        static object get_instance_0(ref object o)
        {
            return ETModel.AsyncImageDownloadComponent.instance;
        }
        static void set_instance_0(ref object o, object v)
        {
            ETModel.AsyncImageDownloadComponent.instance = (ETModel.AsyncImageDownloadComponent)v;
        }


    }
}
