using System;
using System.Linq;
using System.Threading.Tasks;
using Apic.Common.Attributes;
using Apic.Common.Exceptions;
using Apic.Data.Context;
using Apic.Entities.Documents;
using Microsoft.EntityFrameworkCore;

namespace Apic.Data.Repositories
{
    [ScopedService]
    public class DocumentRepository : BaseRepository<Document, Guid>
    {
        private readonly DbSet<Document> set;

        public DocumentRepository(DbSet<Document> set) : base(set)
        {
            this.set = set;
        }

        public async Task<Document> GetSingle(Guid documentId)
        {
            try
            {
                return await set.SingleAsync(x => x.Id == documentId);
            }
            catch (InvalidOperationException exception)
            {
                throw new ObjectNotFoundException("Document has not been found!");
            }
        }
    }
}
