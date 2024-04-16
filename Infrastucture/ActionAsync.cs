using System.Threading.Tasks;

namespace WpfApp1.Infrastucture
{
    internal delegate Task ActionAsync();

    internal delegate Task ActionAsync<in T>(T parameter);
}
