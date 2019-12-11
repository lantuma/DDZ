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
    unsafe class ETModel_WebConfigResponse_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.WebConfigResponse);

            field = type.GetField("dtbjt", flag);
            app.RegisterCLRFieldGetter(field, get_dtbjt_0);
            app.RegisterCLRFieldSetter(field, set_dtbjt_0);
            field = type.GetField("ggytp", flag);
            app.RegisterCLRFieldGetter(field, get_ggytp_1);
            app.RegisterCLRFieldSetter(field, set_ggytp_1);
            field = type.GetField("kfwa", flag);
            app.RegisterCLRFieldGetter(field, get_kfwa_2);
            app.RegisterCLRFieldSetter(field, set_kfwa_2);
            field = type.GetField("dtdbtp", flag);
            app.RegisterCLRFieldGetter(field, get_dtdbtp_3);
            app.RegisterCLRFieldSetter(field, set_dtdbtp_3);
            field = type.GetField("dtdibtp", flag);
            app.RegisterCLRFieldGetter(field, get_dtdibtp_4);
            app.RegisterCLRFieldSetter(field, set_dtdibtp_4);
            field = type.GetField("dlbjt", flag);
            app.RegisterCLRFieldGetter(field, get_dlbjt_5);
            app.RegisterCLRFieldSetter(field, set_dlbjt_5);
            field = type.GetField("dllogo", flag);
            app.RegisterCLRFieldGetter(field, get_dllogo_6);
            app.RegisterCLRFieldSetter(field, set_dllogo_6);
            field = type.GetField("ddz", flag);
            app.RegisterCLRFieldGetter(field, get_ddz_7);
            app.RegisterCLRFieldSetter(field, set_ddz_7);
            field = type.GetField("ggy", flag);
            app.RegisterCLRFieldGetter(field, get_ggy_8);
            app.RegisterCLRFieldSetter(field, set_ggy_8);
            field = type.GetField("tx", flag);
            app.RegisterCLRFieldGetter(field, get_tx_9);
            app.RegisterCLRFieldSetter(field, set_tx_9);
            field = type.GetField("zj", flag);
            app.RegisterCLRFieldGetter(field, get_zj_10);
            app.RegisterCLRFieldSetter(field, set_zj_10);
            field = type.GetField("gg", flag);
            app.RegisterCLRFieldGetter(field, get_gg_11);
            app.RegisterCLRFieldSetter(field, set_gg_11);
            field = type.GetField("sz", flag);
            app.RegisterCLRFieldGetter(field, get_sz_12);
            app.RegisterCLRFieldSetter(field, set_sz_12);
            field = type.GetField("dbbh", flag);
            app.RegisterCLRFieldGetter(field, get_dbbh_13);
            app.RegisterCLRFieldSetter(field, set_dbbh_13);
            field = type.GetField("zybbh", flag);
            app.RegisterCLRFieldGetter(field, get_zybbh_14);
            app.RegisterCLRFieldSetter(field, set_zybbh_14);


        }



        static object get_dtbjt_0(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dtbjt;
        }
        static void set_dtbjt_0(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dtbjt = (System.String)v;
        }
        static object get_ggytp_1(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).ggytp;
        }
        static void set_ggytp_1(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).ggytp = (System.String)v;
        }
        static object get_kfwa_2(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).kfwa;
        }
        static void set_kfwa_2(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).kfwa = (System.String)v;
        }
        static object get_dtdbtp_3(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dtdbtp;
        }
        static void set_dtdbtp_3(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dtdbtp = (System.String)v;
        }
        static object get_dtdibtp_4(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dtdibtp;
        }
        static void set_dtdibtp_4(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dtdibtp = (System.String)v;
        }
        static object get_dlbjt_5(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dlbjt;
        }
        static void set_dlbjt_5(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dlbjt = (System.String)v;
        }
        static object get_dllogo_6(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dllogo;
        }
        static void set_dllogo_6(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dllogo = (System.String)v;
        }
        static object get_ddz_7(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).ddz;
        }
        static void set_ddz_7(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).ddz = (System.Int32)v;
        }
        static object get_ggy_8(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).ggy;
        }
        static void set_ggy_8(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).ggy = (System.Int32)v;
        }
        static object get_tx_9(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).tx;
        }
        static void set_tx_9(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).tx = (System.Int32)v;
        }
        static object get_zj_10(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).zj;
        }
        static void set_zj_10(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).zj = (System.Int32)v;
        }
        static object get_gg_11(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).gg;
        }
        static void set_gg_11(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).gg = (System.Int32)v;
        }
        static object get_sz_12(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).sz;
        }
        static void set_sz_12(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).sz = (System.Int32)v;
        }
        static object get_dbbh_13(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).dbbh;
        }
        static void set_dbbh_13(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).dbbh = (System.String)v;
        }
        static object get_zybbh_14(ref object o)
        {
            return ((ETModel.WebConfigResponse)o).zybbh;
        }
        static void set_zybbh_14(ref object o, object v)
        {
            ((ETModel.WebConfigResponse)o).zybbh = (System.String)v;
        }


    }
}
