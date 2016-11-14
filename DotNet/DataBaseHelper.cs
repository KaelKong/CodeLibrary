    /// <summary>
    /// ���ݿ��ͨ�÷��ʴ��� �շ��޸�
    /// 
    /// ����Ϊ�����࣬
    /// ������ʵ��������Ӧ��ʱֱ�ӵ��ü���
    /// </summary>
    public abstract class Helper
    {
        /// <summary>
        /// ���ݿ������ַ���
        /// </summary>

        public static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region//ExecuteNonQuery����

        /// <summary>
        ///ִ��һ������Ҫ����ֵ��SqlCommand���ͨ��ָ��ר�õ������ַ�����
        /// ʹ�ò���������ʽ�ṩ�����б� 
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ����ֵ��ʾ��SqlCommand����ִ�к�Ӱ�������</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //ͨ��PrePareCommand����������������뵽SqlCommand�Ĳ���������
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                //���SqlCommand�еĲ����б�
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        ///ִ��һ������Ҫ����ֵ��SqlCommand���ͨ��ָ��ר�õ������ַ�����
        /// ʹ�ò���������ʽ�ṩ�����б� 
        /// </summary>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ����ֵ��ʾ��SqlCommand����ִ�к�Ӱ�������</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(connectionString, cmdType, cmdText, commandParameters);
        }

        /// <summary>
        ///�洢����ר��
        /// </summary>
        /// <param name="cmdText">�洢���̵�����</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ����ֵ��ʾ��SqlCommand����ִ�к�Ӱ�������</returns>
        public static int ExecuteNonQueryProducts(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        /// <summary>
        ///Sql���ר��
        /// </summary>
        /// <param name="cmdText">T_Sql���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ����ֵ��ʾ��SqlCommand����ִ�к�Ӱ�������</returns>
        public static int ExecuteNonQueryText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
        }

        #endregion
        #region//GetTable����

        /// <summary>
        /// ִ��һ�����ؽ������SqlCommand��ͨ��һ���Ѿ����ڵ����ݿ�����
        /// ʹ�ò��������ṩ����
        /// </summary>
        /// <param name="connecttionString">һ�����е����ݿ�����</param>
        /// <param name="cmdTye">SqlCommand��������</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������(DataTableCollection)��ʾ��ѯ�õ������ݼ�</returns>
        public static DataTableCollection GetTable(string connecttionString, CommandType cmdTye, string cmdText, SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connecttionString))
            {
                PrepareCommand(cmd, conn, null, cmdTye, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
            }
            DataTableCollection table = ds.Tables;
            return table;
        }

        /// <summary>
        /// ִ��һ�����ؽ������SqlCommand��ͨ��һ���Ѿ����ڵ����ݿ�����
        /// ʹ�ò��������ṩ����
        /// </summary>
        /// <param name="cmdTye">SqlCommand��������</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������(DataTableCollection)��ʾ��ѯ�õ������ݼ�</returns>
        public static DataTableCollection GetTable(CommandType cmdTye, string cmdText, SqlParameter[] commandParameters)
        {
            return GetTable(connectionString, cmdTye, cmdText, commandParameters);
        }

        /// <summary>
        /// �洢����ר��
        /// </summary>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������(DataTableCollection)��ʾ��ѯ�õ������ݼ�</returns>
        public static DataTableCollection GetTableProducts(string cmdText, SqlParameter[] commandParameters)
        {
            return GetTable(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        /// <summary>
        /// Sql���ר��
        /// </summary>
        /// <param name="cmdText"> T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������(DataTableCollection)��ʾ��ѯ�õ������ݼ�</returns>
        public static DataTableCollection GetTableText(string cmdText, SqlParameter[] commandParameters)
        {
            return GetTable(CommandType.Text, cmdText, commandParameters);
        }

        public static DataTable GetFirstTable(string connecttionString, CommandType cmdTye, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection(connecttionString))
            {
                PrepareCommand(cmd, conn, null, cmdTye, cmdText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                //���SqlCommand�еĲ����б�
                cmd.Parameters.Clear();
            }

            return ds.Tables[0];
        }

        public static DataTable GetFirstTable(CommandType cmdTye, string cmdText, params SqlParameter[] commandParameters)
        {
            return GetFirstTable(connectionString, cmdTye, cmdText, commandParameters);
        }

        public static DataTable GetFirstTableProducts(string cmdText, params SqlParameter[] commandParameters)
        {
            return GetFirstTable(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        public static DataTable GetFirstTableText(string cmdText, params SqlParameter[] commandParameters)
        {
            return GetFirstTable(CommandType.Text, cmdText, commandParameters);
        }


        #endregion


        /// <summary>
        /// Ϊִ������׼������
        /// </summary>
        /// <param name="cmd">SqlCommand ����</param>
        /// <param name="conn">�Ѿ����ڵ����ݿ�����</param>
        /// <param name="trans">���ݿ����ﴦ��</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">Command text��T-SQL��� ���� Select * from Products</param>
        /// <param name="cmdParms">���ش�����������</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            //�ж����ݿ�����״̬
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            //�ж��Ƿ���Ҫ���ﴦ��
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        #region//ExecuteDataSet����

        /// <summary>
        /// return a dataset
        /// </summary>
        /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>return a dataset</returns>
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(ds);
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        /// <summary>
        /// ����һ��DataSet
        /// </summary>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>return a dataset</returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(connectionString, cmdType, cmdText, commandParameters);
        }

        /// <summary>
        /// ����һ��DataSet
        /// </summary>
        /// <param name="cmdText">�洢���̵�����</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>return a dataset</returns>
        public static DataSet ExecuteDataSetProducts(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(connectionString, CommandType.StoredProcedure, cmdText, commandParameters);
        }

        /// <summary>
        /// ����һ��DataSet
        /// </summary>

        /// <param name="cmdText">T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>return a dataset</returns>
        public static DataSet ExecuteDataSetText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(connectionString, CommandType.Text, cmdText, commandParameters);
        }


        public static DataView ExecuteDataSet(string connectionString, string sortExpression, string direction, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                da.SelectCommand = cmd;
                da.Fill(ds);
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = sortExpression + " " + direction;
                return dv;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        #endregion


        #region // ExecuteScalar����


        /// <summary>
        /// ���ص�һ�еĵ�һ��
        /// </summary>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(Helper.connectionString, cmdType, cmdText, commandParameters);
        }

        /// <summary>
        /// ���ص�һ�еĵ�һ�д洢����ר��
        /// </summary>
        /// <param name="cmdText">�洢���̵�����</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������</returns>
        public static object ExecuteScalarProducts(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(Helper.connectionString, CommandType.StoredProcedure, cmdText, commandParameters);
        }

        /// <summary>
        /// ���ص�һ�еĵ�һ��Sql���ר��
        /// </summary>
        /// <param name="cmdText">�� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>����һ������</returns>
        public static object ExecuteScalarText(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(Helper.connectionString, CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">һ����Ч�����ݿ������ַ���</param>
        /// <param name="cmdType">SqlCommand�������� (�洢���̣� T-SQL��䣬 �ȵȡ�)</param>
        /// <param name="cmdText">�洢���̵����ֻ��� T-SQL ���</param>
        /// <param name="commandParameters">��������ʽ�ṩSqlCommand�������õ��Ĳ����б�</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        #endregion


        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];
            if (cachedParms == null)
                return null;
            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();
            return clonedParms;
        }


        /// <summary>
        /// ����Ƿ����
        /// </summary>
        /// <param name="strSql">Sql���</param>
        /// <returns>bool���</returns>
        public static bool Exists(string strSql)
        {
            int cmdresult = Convert.ToInt32(ExecuteScalar(connectionString, CommandType.Text, strSql, null));
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// ����Ƿ����
        /// </summary>
        /// <param name="strSql">Sql���</param>
        /// <param name="cmdParms">����</param>
        /// <returns>bool���</returns>
        public static bool Exists(string strSql, params SqlParameter[] cmdParms)
        {
            int cmdresult = Convert.ToInt32(ExecuteScalar(connectionString, CommandType.Text, strSql, cmdParms));
            if (cmdresult == 0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }


        public static void GetPageListData(SearchCriteria criteria)
        {
            SqlParameter tab = new SqlParameter("@tab", criteria.TableName);
            SqlParameter strFid = new SqlParameter("@strFld", criteria.TableField);
            SqlParameter strWhere = new SqlParameter("@strWhere", criteria.WhereStr);
            SqlParameter pageStart = new SqlParameter("@pageStart", criteria.PageStart);
            SqlParameter sort = new SqlParameter("@sort", criteria.Sort);
            SqlParameter pageSize = new SqlParameter("@pageSize", criteria.PageSize);
            SqlParameter isGetCount = new SqlParameter("@isGetCount", "0");
            criteria.DataSource = GetFirstTableProducts("GetPageList", tab, strFid, pageStart, sort, isGetCount, strWhere, pageSize);
            isGetCount.Value = "1";
            criteria.Count = Convert.ToInt32(ExecuteScalarProducts("GetPageList", tab, strFid, pageStart, sort, isGetCount, strWhere, pageSize));

            if (criteria.DataSource.Rows.Count > 0)
            {
                criteria.JsonObject = new string[criteria.DataSource.Rows.Count, criteria.DataSource.Columns.Count - 1]; //��ȥrownum��
                for (int i = 0; i < criteria.DataSource.Rows.Count; i++)
                {
                    for (int j = 1; j < criteria.DataSource.Columns.Count; j++)
                    {
                        criteria.JsonObject[i, j - 1] = criteria.DataSource.Rows[i][j].ToString();
                    }
                }
            }
            else {
                criteria.JsonObject = new string[0, 0];
            }
        }

 
        /*ʹ�� SP_EXECUTESQL ִ�д�����ƴ��*/
        //public static void GetPageListData(SearchCriteria criteria)
        //{
        //    SqlParameter tab = new SqlParameter("@tab", criteria.TableName);
        //    SqlParameter strFid = new SqlParameter("@strFld", criteria.TableField);
        //    SqlParameter strWhere = new SqlParameter("@strWhere", " 1=1 ) AS Dwhere --");
        //    SqlParameter pageIndex = new SqlParameter("@pageIndex", criteria.PageIndex);
        //    SqlParameter sort = new SqlParameter("@sort", criteria.Sort);
        //    SqlParameter pageSize = new SqlParameter("@pageSize", criteria.PageSize);
        //    string stmt = @"N'GetPageList @t, @sf,@sw,@pi,@ps,@s,0'";
        //    string paramsName = @"N'@t NVARCHAR(MAX),@sf NVARCHAR(MAX),@sw VARCHAR(MAX),@pi INT,@ps INT,@s VARCHAR(255)'";
        //    string paramsValue = @"@tab,@strFld,@strWhere,@pageIndex,@pageSize,@sort";
        //    string sql = string.Format(@"EXEC SP_EXECUTESQL {0},{1},{2}", stmt, paramsName, paramsValue);
        //    criteria.DataSource = GetFirstTableText(sql, tab, strFid, pageIndex, sort, strWhere, pageSize);
        //    stmt = @"N'GetPageList @t, @sf,@sw,@pi,@ps,@s,1'";
        //    sql = string.Format(@"EXEC SP_EXECUTESQL {0},{1},{2}", stmt, paramsName, paramsValue);
        //    criteria.Count = Convert.ToInt32(ExecuteScalarText(sql, tab, strFid, pageIndex, sort, strWhere, pageSize));
        //}
    }
