using TangoSchool.DataAccess.DatabaseContexts;
using TangoSchool.DataAccess.Entities;
using TangoSchool.DataAccess.Repositories.Interfaces;

namespace TangoSchool.DataAccess.Repositories;

internal class SubscriptionTemplatesRepository : BaseRepository<SubscriptionTemplate>,
    ISubscriptionTemplatesRepository
{
    public SubscriptionTemplatesRepository(TangoSchoolDbContext context) : base(context)
    {
    }
}
