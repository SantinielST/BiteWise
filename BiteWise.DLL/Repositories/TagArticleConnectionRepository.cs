using BiteWise.DLL.Repositories.Base;
using BiteWise.DLL.TablesСonnections;

namespace BiteWise.DLL.Repositories;

public class TagArticleConnectionRepository(BiteWiseAppContext biteWiseAppContext) : Repository<TagArticleConnection>(biteWiseAppContext)
{
}
