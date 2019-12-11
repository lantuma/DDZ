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
    unsafe class DG_Tweening_TweenExtensions_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(DG.Tweening.TweenExtensions);
            args = new Type[]{typeof(DG.Tweening.Tween)};
            method = type.GetMethod("PlayForward", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayForward_0);
            args = new Type[]{typeof(DG.Tweening.Tween), typeof(System.Boolean), typeof(System.Single)};
            method = type.GetMethod("Restart", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Restart_1);
            args = new Type[]{typeof(DG.Tweening.Tween)};
            method = type.GetMethod("PlayBackwards", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, PlayBackwards_2);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(DG.Tweening.Tweener)};
            if (genericMethods.TryGetValue("Pause", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(DG.Tweening.Tweener), typeof(DG.Tweening.Tweener)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, Pause_3);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(DG.Tweening.Tween), typeof(System.Boolean)};
            method = type.GetMethod("Kill", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Kill_4);


        }


        static StackObject* PlayForward_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            DG.Tweening.Tween @t = (DG.Tweening.Tween)typeof(DG.Tweening.Tween).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            DG.Tweening.TweenExtensions.PlayForward(@t);

            return __ret;
        }

        static StackObject* Restart_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Single @changeDelayTo = *(float*)&ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Boolean @includeDelay = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            DG.Tweening.Tween @t = (DG.Tweening.Tween)typeof(DG.Tweening.Tween).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            DG.Tweening.TweenExtensions.Restart(@t, @includeDelay, @changeDelayTo);

            return __ret;
        }

        static StackObject* PlayBackwards_2(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            DG.Tweening.Tween @t = (DG.Tweening.Tween)typeof(DG.Tweening.Tween).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            DG.Tweening.TweenExtensions.PlayBackwards(@t);

            return __ret;
        }

        static StackObject* Pause_3(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            DG.Tweening.Tweener @t = (DG.Tweening.Tweener)typeof(DG.Tweening.Tweener).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = DG.Tweening.TweenExtensions.Pause<DG.Tweening.Tweener>(@t);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* Kill_4(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @complete = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            DG.Tweening.Tween @t = (DG.Tweening.Tween)typeof(DG.Tweening.Tween).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            DG.Tweening.TweenExtensions.Kill(@t, @complete);

            return __ret;
        }



    }
}
