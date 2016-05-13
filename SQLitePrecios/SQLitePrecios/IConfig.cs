using SQLite.Net.Interop;

namespace SQLitePrecios
{
    public interface IConfig
    {
        string DirectorioDB { get; }
        ISQLitePlatform Plataforma { get; }
    }
}
