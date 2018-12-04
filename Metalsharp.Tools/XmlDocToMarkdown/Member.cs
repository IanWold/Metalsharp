using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Linq;

namespace XmlDocToMarkdown
{
    [Serializable]
    public class Member
    {
        #region Xml Attributes

        [XmlAttribute("name")]
        public string XmlName
        {
            get => _xmlName;
            set
            {
                _xmlName = value.Replace("`0", "").Replace("``0", "").Replace("`1", "").Replace("``1", "");
                var colonSplit = _xmlName.Split(':');

                switch (colonSplit[0])
                {
                    case "T": // Class or Interface
                        var tMemberName = colonSplit[1].Split('.').Last();
                        Name = tMemberName;

                        MemberType = Name[0] == 'I'
                            ? MemberType.Interface
                            : MemberType.Class;
                        break;

                    case "M": // Constructor or Method
                        var mParametersSplit = colonSplit[1].Split('(');
                        var mMemberSplit = mParametersSplit[0].Split('.');

                        var mMemberParent = mMemberSplit[mMemberSplit.Length - 2];
                        var mMemberName = mMemberSplit.Last();

                        Parent = mMemberParent;

                        MemberType = mMemberName.Contains("#ctor")
                            ? MemberType.Constructor
                            : MemberType.Method;

                        var mParameters = mParametersSplit.Length > 1
                            ? mParametersSplit[1].Replace(")", "").Split(',')
                                .Select(i =>
                                {
                                    if (i.Contains("{"))
                                    {
                                        var typeParamsSplit = i.Split('{');
                                        var type = typeParamsSplit[0].Split('.').Last();

                                        var genericTypesSplit = typeParamsSplit[1].Replace("}", "").Split(',').Select(t => t.Split('.').Last());

                                        return type + "<" + string.Join(", ", genericTypesSplit) + ">";
                                    }
                                    else
                                    {
                                        return i.Split('.').Last();
                                    }
                                })
                            : new string[] { "" };

                        Name = (MemberType == MemberType.Method ? mMemberName : Parent) +
                            "(" + string.Join(", ", mParameters) + ")";

                        break;

                    case "P": // Property
                        var pMemberSplit = colonSplit[1].Split('.');
                        var pMemberParent = pMemberSplit[pMemberSplit.Length - 2];
                        var pMemberName = pMemberSplit.Last();

                        Parent = pMemberParent;
                        Name = pMemberName;

                        MemberType = MemberType.Property;
                        break;

                    case "E": // Event
                        var eMemberSplit = colonSplit[1].Split('.');
                        var eMemberParent = eMemberSplit[eMemberSplit.Length - 2];
                        var eMemberName = eMemberSplit.Last();

                        Parent = eMemberParent;
                        Name = eMemberName;

                        MemberType = MemberType.Event;
                        break;

                    case "F": // Field
                        var fMemberSplit = colonSplit[1].Split('.');
                        var fMemberParent = fMemberSplit[fMemberSplit.Length - 2];
                        var fMemberName = fMemberSplit.Last();

                        Parent = fMemberParent;
                        Name = fMemberName;

                        MemberType = MemberType.Field;
                        break;
                }
            }
        }
        private string _xmlName;

        #endregion

        #region Xml Elements

        [XmlElement("example")]
        public string Example { get; set; }

        [XmlElement("summary")]
        public string Summary { get; set; }

        [XmlArrayItem("param", typeof(Param))]
        public Param[] Parameters { get; set; } = new Param[0];

        [XmlArrayItem("typeparam", typeof(Param))]
        public Param[] TypeParameters { get; set; } = new Param[0];

        [XmlElement("returns")]
        public string Returns { get; set; }

        #endregion

        #region Properties

        [XmlIgnore]
        IEnumerable<Member> Constructors =>
            Members.Where(m => m.MemberType == MemberType.Constructor);

        [XmlIgnore]
        IEnumerable<Member> Events =>
            Members.Where(m => m.MemberType == MemberType.Event);

        [XmlIgnore]
        IEnumerable<Member> Fields =>
            Members.Where(m => m.MemberType == MemberType.Field);

        [XmlIgnore]
        public List<Member> Members { get; set; } = new List<Member>();

        [XmlIgnore]
        public MemberType MemberType { get; set; }

        [XmlIgnore]
        IEnumerable<Member> Methods =>
            Members.Where(m => m.MemberType == MemberType.Method);

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public string Parent { get; set; }

        [XmlIgnore]
        IEnumerable<Member> Properties =>
            Members.Where(m => m.MemberType == MemberType.Property);
        #endregion

        private string LineTrimmedString(string toTrim)
        {
            var lines = toTrim.Split('\n');
            var sb = new StringBuilder();

            foreach (var l in lines)
            {
                sb.AppendLine(l.Trim());
            }

            return sb.ToString();
        }

        public string ToMarkdown()
        {
            var sb = new StringBuilder();

            switch (MemberType)
            {
                case MemberType.Class:
                case MemberType.Interface:
                    sb.AppendLine($"## { Name }");
                    sb.AppendLine();

                    sb.AppendLine(LineTrimmedString(Summary));
                    sb.AppendLine();

                    if (TypeParameters.Count() > 0)
                    {
                        sb.AppendLine($"### Type Parameters");
                        sb.AppendLine();

                        sb.AppendLine(TypeParameters.ToMarkdownTable());
                        sb.AppendLine();
                    }

                    if (Constructors.Count() > 0)
                    {
                        sb.AppendLine($"### Constructors");
                        sb.AppendLine();

                        foreach (var member in Constructors)
                        {
                            sb.Append(member.ToMarkdown());
                        }
                    }

                    if (Methods.Count() > 0)
                    {
                        sb.AppendLine($"### Methods");
                        sb.AppendLine();

                        foreach (var member in Methods)
                        {
                            sb.Append(member.ToMarkdown());
                        }
                    }

                    if (Fields.Count() > 0)
                    {
                        sb.AppendLine($"### Fields");
                        sb.AppendLine();

                        foreach (var member in Fields)
                        {
                            sb.Append(member.ToMarkdown());
                        }
                    }

                    if (Properties.Count() > 0)
                    {
                        sb.AppendLine($"### Properties");
                        sb.AppendLine();

                        foreach (var member in Properties)
                        {
                            sb.Append(member.ToMarkdown());
                        }
                    }

                    if (Events.Count() > 0)
                    {
                        sb.AppendLine($"### Events");
                        sb.AppendLine();

                        foreach (var member in Events)
                        {
                            sb.Append(member.ToMarkdown());
                        }
                    }

                    if (!string.IsNullOrEmpty(Example))
                    {
                        sb.AppendLine($"### Example");
                        sb.AppendLine();

                        sb.Append(LineTrimmedString(Example));
                        sb.AppendLine();
                    }
                    break;

                case MemberType.Constructor:
                case MemberType.Method:
                    sb.AppendLine($"#### `{ Name }`");
                    sb.AppendLine();

                    sb.AppendLine(LineTrimmedString(Summary));
                    sb.AppendLine();

                    if (Parameters.Count() > 0)
                    {
                        sb.AppendLine($"#### Parameters");
                        sb.AppendLine();

                        sb.AppendLine(Parameters.ToMarkdownTable());
                        sb.AppendLine();
                    }

                    if (TypeParameters.Count() > 0)
                    {
                        sb.AppendLine($"##### Type Parameters");
                        sb.AppendLine();

                        sb.AppendLine(TypeParameters.ToMarkdownTable());
                    }

                    if (!string.IsNullOrEmpty(Returns))
                    {
                        sb.AppendLine($"##### Returns");
                        sb.AppendLine();

                        sb.AppendLine(LineTrimmedString(Returns));
                        sb.AppendLine();
                    }

                    if (!string.IsNullOrEmpty(Example))
                    {
                        sb.AppendLine($"##### Example");
                        sb.AppendLine();

                        sb.AppendLine(LineTrimmedString(Example));
                        sb.AppendLine();
                    }
                    break;

                case MemberType.Event:
                case MemberType.Field:
                case MemberType.Property:
                    sb.AppendLine($"#### `{ Name }`");
                    sb.AppendLine();

                    sb.AppendLine(LineTrimmedString(Summary));
                    sb.AppendLine();

                    if (!string.IsNullOrEmpty(Example))
                    {
                        sb.AppendLine($"##### Example");
                        sb.AppendLine();

                        sb.AppendLine(LineTrimmedString(Example));
                        sb.AppendLine();
                    }
                    break;
            }

            return sb.ToString();
        }
    }
}
