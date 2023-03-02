namespace WebScrapParts.Helpers
{
    public static class ConnectionStrings
    {
        public static string StringConn_PC99 { get { return @"Server=PC99\SUCM;DataBase=RF;UID=sa;PWD=pass#word1;Trust Server Certificate=true"; } }
        public static string StringConn_PC22 { get { return @"Server=.\DEV;DataBase=RF;UID=sa;PWD=123456;Trust Server Certificate=true"; } }
    }
}
