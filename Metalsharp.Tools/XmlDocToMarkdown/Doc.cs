using System;
using System.Text;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;

namespace XmlDocToMarkdown
{
    [Serializable]
    [XmlRoot("doc")]
    public class Doc
    {
        [XmlArray("members")]
        [XmlArrayItem("member", typeof(Member))]
        public Member[] Members
        {
            get => _members;
            set
            {
                var membersList = new List<Member>();

                foreach (var member in value)
                {
                    if (member.MemberType == MemberType.Class || member.MemberType == MemberType.Interface)
                    {
                        membersList.Add(member);
                    }
                    else
                    {
                        membersList.First(m => m.Name == member.Parent).Members.Add(member);
                    }
                }

                _members = membersList.ToArray();
            }
        }
        private Member[] _members = new Member[0];

        public string ToMarkdown()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"# Metalsharp API Documentation");
            sb.AppendLine();

            foreach (var member in Members)
            {
                sb.AppendLine(member.ToMarkdown());
            }

            return sb.ToString();
        }
    }
}
