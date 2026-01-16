using System.IO;

namespace Direction.NFSe.Danfe
{
    public interface IDanfeService
    {
        DanfeResult Generate(NFSeSchema nfse, DanfeEnvironment environment, bool isCancelled = false);
        DanfeResult Generate(Stream xmlStream, DanfeEnvironment environment, bool isCancelled = false);
        DanfeResult Generate(string xml, DanfeEnvironment environment, bool isCancelled = false);
    }
}