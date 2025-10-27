using BiteWise.DLL.Entities;
using BiteWise.DLL.Repositories.Base;

namespace BiteWise.DLL.Repositories;

public class CommentRepository(BiteWiseAppContext biteWiseAppContext) : Repository<CommentEntity>(biteWiseAppContext)
{

}