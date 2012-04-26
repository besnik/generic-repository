using System;
using System.Transactions;

namespace Besnik.GenericRepository.EntityFramework
{
    /// <summary>
    /// Entity framework implementation of the transaction.
    /// </summary>
    public class EntityFrameworkTransaction : ITransaction
    {
        public EntityFrameworkTransaction(EntityFrameworkUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            this.TransactionScope = new TransactionScope();
        }

        protected EntityFrameworkUnitOfWork UnitOfWork { get; private set; }

        protected TransactionScope TransactionScope { get; private set; }

        /// <summary>
        /// Flushes unit of work and commits the transaction scope.
        /// </summary>
        public void Commit()
        {
            this.UnitOfWork.Flush();
            this.TransactionScope.Complete();
        }

        /// <summary>
        /// Rolls back transaction.
        /// Actually the transaction rollback is handled automatically with Dispose method if
        /// transaction scope was not commited.
        /// </summary>
        public void Rollback()
        {
        }

        public void Dispose()
        {
            if (this.TransactionScope != null)
            {
                (this.TransactionScope as IDisposable).Dispose();
                this.TransactionScope = null;
                this.UnitOfWork = null;
            }
        }
    }
}
