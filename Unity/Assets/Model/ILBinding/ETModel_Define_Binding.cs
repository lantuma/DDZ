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
    unsafe class ETModel_Define_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Define);

            field = type.GetField("UseEditorResouces", flag);
            app.RegisterCLRFieldGetter(field, get_UseEditorResouces_0);
            app.RegisterCLRFieldSetter(field, set_UseEditorResouces_0);
            field = type.GetField("IsOutPutServerMsg", flag);
            app.RegisterCLRFieldGetter(field, get_IsOutPutServerMsg_1);
            app.RegisterCLRFieldSetter(field, set_IsOutPutServerMsg_1);
            field = type.GetField("IsILRuntime", flag);
            app.RegisterCLRFieldGetter(field, get_IsILRuntime_2);
            app.RegisterCLRFieldSetter(field, set_IsILRuntime_2);


        }



        static object get_UseEditorResouces_0(ref object o)
        {
            return ETModel.Define.UseEditorResouces;
        }
        static void set_UseEditorResouces_0(ref object o, object v)
        {
            ETModel.Define.UseEditorResouces = (System.Boolean)v;
        }
        static object get_IsOutPutServerMsg_1(ref object o)
        {
            return ETModel.Define.IsOutPutServerMsg;
        }
        static void set_IsOutPutServerMsg_1(ref object o, object v)
        {
            ETModel.Define.IsOutPutServerMsg = (System.Boolean)v;
        }
        static object get_IsILRuntime_2(ref object o)
        {
            return ETModel.Define.IsILRuntime;
        }
        static void set_IsILRuntime_2(ref object o, object v)
        {
            ETModel.Define.IsILRuntime = (System.Boolean)v;
        }


    }
}
