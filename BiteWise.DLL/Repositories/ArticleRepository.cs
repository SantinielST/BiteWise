using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories.Base;

namespace BiteWise.DLL.Repositories;

public class ArticleRepository(BiteWiseAppContext biteWiseAppContext) : Repository<ArticleEntity>(biteWiseAppContext)
{

}