using Common.Core.DTOs;
using Common.Core.Validation;

namespace Common.Core.Domain
{
    public class DocumentSaveResult : DataCommandResult<DocumentSaveResultItem>
    {
        public DocumentSaveResult(bool success, DocumentSaveResultItem document) 
            : base(success, document)
        {

        }

        public DocumentSaveResult(BrokenRulesList brokenRules) : base(brokenRules)
        {

        }

        public new static DocumentSaveResult Success(DocumentSaveResultItem document) => new DocumentSaveResult(true, document);

        public new static DocumentSaveResult Fail(BrokenRulesList brokenRules) => new DocumentSaveResult(brokenRules);
    }
}
