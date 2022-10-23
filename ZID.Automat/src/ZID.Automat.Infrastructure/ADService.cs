using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZID.Automat.Domain.Models;

namespace ZID.Automat.Infrastructure
{
     public enum ADUserRole { Other = 0, Pupil, Teacher, Administration, Management }

    /// <summary>
    /// Klasse für den AD Zugriff. Stellt die Loginfunktion und Abfragemethoden bereit.
    /// </summary>
    public class ADService : IDisposable
    {
        private readonly LdapConnection _connection;
        private bool isDisposed;

        public static string Hostname { get; } = "ldap.spengergasse.at";
        public static string Domain { get; } = "htl-wien5.schule";
        public static string BaseDn { get; } = "OU=Benutzer,OU=SPG,DC=htl-wien5,DC=schule";
        public ADUser CurrentUser { get; }

        /// <summary>
        /// Liefert eine aktive Serverbindung mit den Rechten des angemeldeten Benutzers.
        /// </summary>
        protected static LdapConnection GetConnection(string cn, string password)
        {
            var connection = new LdapConnection
            {
                SecureSocketLayer = true
            };

            try
            {
                try { connection.Connect(Hostname, 636); }
                catch { throw new ApplicationException($"Der Anmeldeserver ist nicht erreichbar."); }
                try { connection.Bind($"{cn}@{Domain}", password); }
                catch { throw new ApplicationException($"Ungültiger Benutzername oder Passwort."); }

                return connection;
            }
            catch { connection.Disconnect(); throw; }
        }

        /// <summary>
        /// Führt ein Login am Active Directory durch.
        /// </summary>
        public static ADService Login(string cn, string password, string? currentUserCn = null)
        {
            var connection = GetConnection(cn, password);
            var result = connection.Search(
                        BaseDn,
                        LdapConnection.SCOPE_SUB,
                        $"(&(objectClass=user)(objectClass=person)(cn={currentUserCn ?? cn}))",
                        new string[] { "cn", "givenName", "sn", "mail", "employeeid", "memberof", "description" },
                        false);
            LdapEntry loginUser = result.FirstOrDefault() ?? throw new ApplicationException("Der Benutzer wurde nicht gefunden.");
            var user = new ADUser(loginUser);
            return new ADService(user, connection);
        }

        private ADService(ADUser currentUser, LdapConnection connection)
        {
            _connection = connection;
            CurrentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public string[] GetClasses()
        {
            var results = Search("(objectClass=group)", "OU=Klassen,OU=Mailaktivierte Sicherheitsgruppen,OU=Gruppen,OU=SPG,DC=htl-wien5,DC=schule");
            return results
                .Select(r => r.getAttribute("cn")?.StringValue ?? "")
                .Where(r => !string.IsNullOrEmpty(r))
                .OrderBy(r => r.Length < 5 ? "" : r.Substring(2, 3))
                .ThenBy(r => r)
                .ToArray();
        }

        public ADUser[] GetPupils(string schoolclass)
        {
            try
            {
                var classGroup = $"CN={schoolclass},OU=Klassen,OU=Mailaktivierte Sicherheitsgruppen,OU=Gruppen,OU=SPG,DC=htl-wien5,DC=schule";
                var members = Search($"(&(objectClass=user)(objectClass=person)(memberOf={classGroup}))");
                return members.Select(m => new ADUser(m)).ToArray();
            }
            catch { return Array.Empty<ADUser>(); }
        }

        public ADUser[] GetTeachers(string schoolclass)
        {
            try
            {
                var classGroup = $"CN=lehrende_{schoolclass},OU=Klassenlehrer,OU=Mailaktivierte Sicherheitsgruppen,OU=Gruppen,OU=SPG,DC=htl-wien5,DC=schule";
                var members = Search($"(&(objectClass=user)(objectClass=person)(memberOf={classGroup}))");
                return members.Select(m => new ADUser(m)).ToArray();
            }
            catch { return Array.Empty<ADUser>(); }
        }

        public ADUser? GetKv(string schoolclass)
        {
            try
            {
                var classGroup = $"CN=KV_{schoolclass},OU=KV,OU=Mailaktivierte Sicherheitsgruppen,OU=Gruppen,OU=SPG,DC=htl-wien5,DC=schule";
                var kv = Search($"(&(objectClass=user)(objectClass=person)(memberOf={classGroup}))");
                return new ADUser(kv[0]);
            }
            catch { return null; }
        }

        private List<LdapEntry> Search(string searchFilter) =>
            Search(searchFilter, BaseDn);

        private List<LdapEntry> Search(string searchFilter, string baseDn) =>
            Search(searchFilter, baseDn, new string[0]);

        private List<LdapEntry> Search(string searchFilter, string[] additionalAttributes) =>
            Search(searchFilter, BaseDn, additionalAttributes);

        private List<LdapEntry> Search(string searchFilter, string baseDn, string[] additionalAttributes)
        {
            var attributes =
                new string[] { "cn", "givenName", "sn", "mail", "employeeid", "memberof" }
                .Concat(additionalAttributes)
                .ToArray();

            return _connection.Search(
                        baseDn,
                        LdapConnection.SCOPE_SUB,
                        searchFilter,
                        attributes,
                        false).ToList();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) { return; }
            if (disposing)
            {
                _connection.Disconnect();
                _connection.Dispose();
            }
            isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}