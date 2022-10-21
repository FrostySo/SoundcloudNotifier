using SQLite;

namespace SoundcloudNotifier.SQL {
    public abstract class DbHandler {
        public string DBFileName { get; private set; }
        private string ConnectionStr { get; set; }
        
        protected object Padlock = new object();

        protected DbHandler(string dbFileName) {
            this.DBFileName = dbFileName;
            this.ConnectionStr = $"Data Source = {DBFileName}; Version=3;";
        }
       
        public SQLite.SQLiteConnection SimpleDbConnection() {
            return new SQLite.SQLiteConnection(DBFileName);
        }

        protected abstract void CreateTables(SQLite.SQLiteConnection conn);

        public bool ColumnContains(SQLite.SQLiteConnection conn, string tableName, string columnName, string value) {
            return conn.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM `{tableName}` WHERE `{columnName}` LIKE ?)", $"%{value}%");
        }
        
        public bool ColumnExists(SQLite.SQLiteConnection conn, string tableName, string columnName, string value) {
            return conn.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM `{tableName}` WHERE `{columnName}` = ?)", value);
        }

        public bool ColumnExists(string tableName, string columnName, string value) {
            using (var conn = SimpleDbConnection()) {
                return ColumnExists(conn, tableName, columnName, value);
            }
        }

        public bool ColumnExists(string tableName, string whereClause, params string[] values) {
            using (var conn = SimpleDbConnection()) {
                return conn.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM `{tableName}` WHERE {whereClause})", values);
            }
        }

        public void CreateIfNotExists() {
            lock (Padlock) {
                using (var conn = SimpleDbConnection()) {
                    CreateTables(conn);
                }
            }
        }

        internal class PragmaTableInfo {
            [Column("cid")]
            public int Cid { get; set; }

            [Column("name")]
            public string? Name { get; set; }
            [Column("type")]
            public string? Type { get; set; }
            [Column("notnull")]
            public int NotNull { get; set; }
            [Column("dft_value")]
            public string? DftValue { get; set; }
            [Column("pk")]
            public int Pk { get; set; }
        }

        internal System.Collections.Generic.List<PragmaTableInfo> PragmaColumnInfo(SQLite.SQLiteConnection conn, string tableName) {
            return conn.Query<PragmaTableInfo>($"pragma table_info('{tableName}')");
        }
    }
}
