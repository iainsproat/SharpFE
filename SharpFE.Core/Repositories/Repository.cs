//-----------------------------------------------------------------------
// <copyright file="Repository.cs" company="SharpFE">
//     Copyright Iain Sproat, 2012.
// </copyright>
//-----------------------------------------------------------------------

namespace SharpFE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A repository is primarily a register stores items and allows them to be retrieved.
    /// It also holds indexes for faster searching for items based on their property values.
    /// </summary>
    /// <typeparam name="T">The type of instances to be stored in the repository</typeparam>
    internal abstract class Repository<T> : ICollection<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}" /> class.
        /// </summary>
        public Repository()
        {
            this.InternalStore = new List<T>();
        }
        
        /// <summary>
        /// Gets the number of items held in this repository
        /// </summary>
        public int Count
        {
            get
            {
                return this.InternalStore.Count;
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether the repository is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.InternalStore.IsReadOnly;
            }
        }
        
        /// <summary>
        /// Gets or sets the items of the repository
        /// </summary>
        private IList<T> InternalStore
        {
            get;
            set;
        }
        
        /// <summary>
        /// Registers a new item in the repository
        /// </summary>
        /// <param name="item">The item to register in this repository</param>
        public void Add(T item)
        {
            Guard.AgainstBadArgument(
                () => { return this.Contains(item); },
                "This repository already contains the item. Duplicate items cannot be added to a repository",
                "item");
            
            this.InternalStore.Add(item);
            this.AddToRepository(item);
        }
        
        /// <summary>
        /// Returns an enumerator which iterates over the repository
        /// </summary>
        /// <returns>An enumerator that can be used to iterate the repository</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.InternalStore.GetEnumerator();
        }
        
        /// <summary>
        /// Removes a specific object from the repository.
        /// </summary>
        /// <param name="item">The object to remove from the repository</param>
        /// <returns>true if the item was successfully removed from the repository; otherwise, false. This method also returns false if item is not found in the original repository.</returns>
        public bool Remove(T item)
        {
            if (this.InternalStore.Remove(item))
            {
                this.RemoveItem(item);
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Copies the elements of the repository to a System.Array, starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from the repository. The system.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalStore.CopyTo(array, arrayIndex);
        }
        
        /// <summary>
        /// Removes all items from the repository
        /// </summary>
        public void Clear()
        {
            this.InternalStore.Clear();
            this.ClearRepository();
        }
        
        /// <summary>
        /// Determines whether the repository contains a specific value
        /// </summary>
        /// <param name="item">the object to locate in the repository</param>
        /// <returns>true if the item was found in the repository; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return this.InternalStore.Contains(item);
        }
        
        /// <summary>
        /// Returns an enumerator which iterates over the repository
        /// </summary>
        /// <returns>an enumerator which can be used to iterate over the repository</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        /// <summary>
        /// This method is called by the Add method.  Inheriting classes are expected to implement actions to add the item.
        /// </summary>
        /// <param name="item">The item which has been added to the repository</param>
        protected abstract void AddToRepository(T item);
        
        /// <summary>
        /// This method is called by the Remove method.  Inheriting classes are expected to implement actions to remove the item.
        /// </summary>
        /// <param name="item">The item which has been removed from the repository</param>
        /// <returns>true if the item was successfully removed; otherwise, false</returns>
        protected abstract bool RemoveItem(T item);
        
        /// <summary>
        /// This method is called by the Clear method.  It allows inheriting classes to carry out actions to ensure they match the cleared state of the repository.
        /// </summary>
        protected abstract void ClearRepository();
    }
}
