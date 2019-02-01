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
    public class DocumentRepository
    {
        private readonly ApicDbContext db;

        public DocumentRepository(ApicDbContext db)
        {
            this.db = db;
        }

        public async Task<Document> GetSingle(Guid documentId)
        {
            try
            {
                return await db.Set<Document>().SingleAsync(x => x.Id == documentId);
            }
            catch (InvalidOperationException exception) when (exception.Message.Contains("Sequence contains"))
            {
                throw new ObjectNotFoundException("Document has not been found!");
            }
        }
    }
}
