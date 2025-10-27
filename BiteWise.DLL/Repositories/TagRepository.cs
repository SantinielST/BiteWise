using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories.Base;

namespace BiteWise.DLL.Repositories;

public class TagRepository(BiteWiseAppContext biteWiseAppContext) : Repository<TagEntity>(biteWiseAppContext)
{

}