using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProNetcell.Data.Registry
{
    public class InputFieldsCreator
    {

        const string phFieldId = "#fieldid#";
        const string phFieldName = "#fieldname#";
        const string phFieldLabel = "#label#";
        const string phFieldLength = "#fieldlength#";


        int AccountId;
        Dictionary<string, string> _Template;
        StringBuilder _content;
        StringBuilder _fieldList;
        public string FieldList { get; private set; }
        public string FieldContent { get; private set; }
        public string FieldScript { get; private set; }
        public InputFieldsCreator(int accountId)
        {
            AccountId = accountId;
            IEnumerable<RegistryInputFieldTemplate> list = CmsContext.GetInputFieldsTemplate(accountId);
            _Template = new Dictionary<string, string>();
            foreach (var t in list)
            {
                _Template.Add(t.InputFieldType, t.FieldContent);
            }
        }

        public void CreateContent(IEnumerable<RegistryInputField> v)
        {

            _content = new StringBuilder();

            foreach (var input in v.Where(f => f.Enable == true).OrderBy(f => f.FieldOrder))
            {
                _fieldList.Append(input.Field + ",");
                CreateField(input);
            }
            FieldList = _fieldList.ToString().TrimEnd(',');
            FieldContent = _content.ToString();
        }

        void CreateField(RegistryInputField field)
        {

            string template;
            if (_Template.TryGetValue(field.InputType, out template))
            {

                template = template.Replace(phFieldLabel, field.FieldName);
                template = template.Replace(phFieldId, field.Field);
                template = template.Replace(phFieldName, field.Field);
                if (field.FieldLength > 0)
                    template = template.Replace(phFieldLength, field.FieldLength.ToString());

                _content.AppendLine(template);

            }
        }


    }
}
