    /// <summary>
    /// DataTable��ʵ���໥��ת��
    /// </summary>
    /// <typeparam name="T">ʵ����</typeparam>
    public abstract class ModelHandler<T> where T : new()
    {
        #region DataTableת����ʵ����

        /// <summary>
        /// �������б���DataSet�ĵ�һ�������ʵ����
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <returns></returns>
        public static List<T> FillModel(DataSet ds)
        {
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[0]);
            }
        }

        /// <summary>  
        /// �������б���DataSet�ĵ�index�������ʵ����
        /// </summary>  
        public static List<T> FillModel(DataSet ds, int index)
        {
            if (ds == null || ds.Tables.Count <= index || ds.Tables[index].Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return FillModel(ds.Tables[index]);
            }
        }

        /// <summary>  
        /// �������б���DataTable���ʵ����
        /// </summary>  
        public static List<T> FillModel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<T> modelList = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T model = FillModel(dr);
                //T model = (T)Activator.CreateInstance(typeof(T));  
                //T model = new T();
                //for (int i = 0; i < dr.Table.Columns.Count; i++)
                //{
                //    PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                //    if (propertyInfo != null && dr[i] != DBNull.Value)
                //        propertyInfo.SetValue(model, dr[i], null);
                //}

                modelList.Add(model);
            }
            return modelList;
        }

        /// <summary>  
        /// ��������DataRow���ʵ����
        /// </summary>  
        public static T FillModel(DataRow dr)
        {
            if (dr == null)
            {
                return default(T);
            }

            //T model = (T)Activator.CreateInstance(typeof(T));  
            T model = new T();

            for (int i = 0; i < dr.Table.Columns.Count; i++)
            {
                PropertyInfo propertyInfo = model.GetType().GetProperty(dr.Table.Columns[i].ColumnName);
                if (propertyInfo != null && dr[i] != DBNull.Value)
                    propertyInfo.SetValue(model, dr[i], null);
            }
            return model;
        }

        #endregion

        #region ʵ����ת����DataTable

        /// <summary>
        /// ʵ����ת����DataSet
        /// </summary>
        /// <param name="modelList">ʵ�����б�</param>
        /// <returns></returns>
        public static DataSet FillDataSet(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            else
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(FillDataTable(modelList));
                return ds;
            }
        }

        /// <summary>
        /// ʵ����ת����DataTable
        /// </summary>
        /// <param name="modelList">ʵ�����б�</param>
        /// <returns></returns>
        public static DataTable FillDataTable(List<T> modelList)
        {
            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            DataTable dt = CreateData(modelList[0]);

            foreach (T model in modelList)
            {
                DataRow dataRow = dt.NewRow();
                foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
                {
                    dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        /// <summary>
        /// ����ʵ����õ���ṹ
        /// </summary>
        /// <param name="model">ʵ����</param>
        /// <returns></returns>
        private static DataTable CreateData(T model)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            return dataTable;
        }

        #endregion
    }