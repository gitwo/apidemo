using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace Util
{
	/// <summary>
	/// 通过Emit将DataTable转换为List
	/// </summary>
	public static class EntityConverter
	{
		public static List<T> ToList<T>(this DataTable dt)
		{
			List<T> list = new List<T>();
			if (dt == null || dt.Rows.Count == 0)
				return list;
			DataTableEntityBuilder<T> eblist = DataTableEntityBuilder<T>.CreateBuilder(dt.Rows[0]);
			foreach (DataRow info in dt.Rows)
				list.Add(eblist.Build(info));
			dt.Dispose();
			dt = null;
			return list;
		}

		public static List<T> ToList<T>(this DataSet ds)
		{
			if (ds != null && ds.Tables.Count > 0)
			{
				return ds.Tables[0].ToList<T>();
			}
			return new List<T>();
		}

		public class DataTableEntityBuilder<Entity>
		{
			private static readonly MethodInfo getValueMethod = typeof(DataRow).GetMethod("get_Item", new Type[] { typeof(int) });
			private static readonly MethodInfo isDBNullMethod = typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
			private delegate Entity Load(DataRow dataRecord);
			private Load handler;
			private DataTableEntityBuilder() { }
			public Entity Build(DataRow dataRecord)
			{
				return handler(dataRecord);
			}
			public static DataTableEntityBuilder<Entity> CreateBuilder(DataRow dataRecord)
			{
				DataTableEntityBuilder<Entity> dynamicBuilder = new DataTableEntityBuilder<Entity>();
				DynamicMethod method = new DynamicMethod("DynamicCreateEntity", typeof(Entity), new Type[] { typeof(DataRow) }, typeof(Entity), true);
				ILGenerator generator = method.GetILGenerator();
				LocalBuilder result = generator.DeclareLocal(typeof(Entity));
				generator.Emit(OpCodes.Newobj, typeof(Entity).GetConstructor(Type.EmptyTypes));
				generator.Emit(OpCodes.Stloc, result);
				for (int i = 0; i < dataRecord.ItemArray.Length; i++)
				{
					PropertyInfo propertyInfo = typeof(Entity).GetProperty(dataRecord.Table.Columns[i].ColumnName);
					Label endIfLabel = generator.DefineLabel();
					if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
					{
						generator.Emit(OpCodes.Ldarg_0);
						generator.Emit(OpCodes.Ldc_I4, i);
						generator.Emit(OpCodes.Callvirt, isDBNullMethod);
						generator.Emit(OpCodes.Brtrue, endIfLabel);
						generator.Emit(OpCodes.Ldloc, result);
						generator.Emit(OpCodes.Ldarg_0);
						generator.Emit(OpCodes.Ldc_I4, i);
						generator.Emit(OpCodes.Callvirt, getValueMethod);
						generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
						generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
						generator.MarkLabel(endIfLabel);
					}
				}
				generator.Emit(OpCodes.Ldloc, result);
				generator.Emit(OpCodes.Ret);
				dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
				return dynamicBuilder;
			}
		}
	}
}
