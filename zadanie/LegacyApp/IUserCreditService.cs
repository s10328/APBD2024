using System;

namespace LegacyApp
{
    public interface IUserCreditService : IDisposable
    {
        /// <summary>
        /// Pobiera limit kredytowy dla danego użytkownika.
        /// </summary>
        /// <param name="lastName">Nazwisko użytkownika.</param>
        /// <param name="dateOfBirth">Data urodzenia użytkownika.</param>
        /// <returns>Limit kredytowy użytkownika.</returns>
        int GetCreditLimit(string lastName, DateTime dateOfBirth);
    }
}