using Revenue_Recognition_System.Data;

namespace Revenue_Recognition_System.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly DatabaseContext _context;

        public ContractRepository(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }
    }
}
