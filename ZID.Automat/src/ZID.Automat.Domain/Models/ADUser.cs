using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZID.Automat.Domain.Models
{
    public enum ADUserRole { Other = 0, Pupil, Teacher, Administration, Management }
    public class ADUser
    {
        [JsonConstructor]
        public ADUser(
            string firstname, string lastname, string email, string cn, string dn,
            string[] groupMemberhips, string? pupilId = null, string? teacherId = null)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Cn = cn;
            Dn = dn;
            GroupMemberhips = groupMemberhips;
            PupilId = pupilId;
            TeacherId = teacherId;
        }

        public ADUser(LdapEntry entry)
        {
            Firstname = entry.getAttribute("givenName")?.StringValue ?? "";
            Lastname = entry.getAttribute("sn")?.StringValue ?? "";
            Email = entry.getAttribute("mail")?.StringValue ?? "";
            Cn = entry.getAttribute("cn")?.StringValue ?? "";
            Dn = entry.DN;
            PupilId = entry.getAttribute("employeeid")?.StringValue;
            TeacherId = entry.getAttribute("description")?.StringValue;
            GroupMemberhips = entry.getAttribute("memberof")?.StringValueArray ?? Array.Empty<string>();
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public string Email { get; }
        public string Cn { get; }
        public string Dn { get; }
        public string[] GroupMemberhips { get; }
        public string? PupilId { get; }
        public string? TeacherId { get; }
        public string[] Classes => GroupMemberhips
            .Where(v => v.Contains("OU=Klassen,") || v.Contains("OU=Klassenlehrer,"))
            .Select(v =>
            {
                var m = Regex.Match(v, "CN=(lehrende_)?([^,]+)", RegexOptions.IgnoreCase);
                return m.Success ? m.Groups[2].Value.ToUpper().Trim() : v;
            })
            .OrderBy(c => c.Length < 5 ? "" : c.Substring(2, 3))
            .ThenBy(c => c)
            .ToArray();
        public ADUserRole Role
        {
            get
            {
                if (GroupMemberhips.Any(g => g.Contains("CN=AV,OU=Schulleitung"))) { return ADUserRole.Management; }
                return
                    Dn.Contains("OU=Verwaltung") ? ADUserRole.Administration :
                    Dn.Contains("OU=Lehrer") ? ADUserRole.Teacher :
                    Dn.Contains("OU=Schueler") ? ADUserRole.Pupil :
                    ADUserRole.Other;
            }
        }

        public string ToJson() => System.Text.Json.JsonSerializer.Serialize(this);
        public static ADUser? FromJson(string json) => System.Text.Json.JsonSerializer.Deserialize<ADUser>(json);
    }
}
