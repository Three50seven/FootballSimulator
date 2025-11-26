using Common.Core;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Common.AspNetCore
{
    public abstract class ViewPageInfo<TModel>
    {
        private string? _pageTitle;
        private readonly bool _displayHeader;

        protected ViewPageInfo() { }

        public ViewPageInfo(ViewDataDictionary viewData)
            : this(
                (viewData["PageTitle"] as string)
                     ?? (viewData["HeaderTitle"] as string)
                     ?? (viewData["Title"] as string)!, // PageTitle > HeaderTitle > Title

                 (viewData["HeaderTitle"] as string)
                     ?? (viewData["Title"] as string)!, // HeaderTitle > Title

                 viewData["DisplayHeader"] is bool displayHeader ? displayHeader : true, // default true

                 viewData["Description"] as string ?? string.Empty, // description
                 viewData["Author"] as string ?? string.Empty,      // author
                 viewData["BodyClass"] as string ?? string.Empty    // optional css class
             )
        {
        }


        protected ViewPageInfo(
            string pageTitle, 
            string headerTitle, 
            bool displayHeader, 
            string description, 
            string author, 
            string bodyClass)
        {
            PageTitle = pageTitle.SetNullToEmpty();
            HeaderTitle = string.IsNullOrWhiteSpace(headerTitle) ? PageTitle : headerTitle.SetNullToEmpty();
            _displayHeader = displayHeader;
            Description = description.SetNullToEmpty();
            Author = author.SetNullToEmpty();
            BodyClass = bodyClass.SetEmptyToNull(); //this is set to NULL if empty so that the body class attribute doesn't load on NULL
        }

        public abstract string ApplicationName { get; }

        public virtual string? PageTitle
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_pageTitle))
                    _pageTitle = FormatPageTitle(HeaderTitle!);

                return _pageTitle;
            }
            private set { _pageTitle = FormatPageTitle(value!); }
        }

        public string? HeaderTitle { get; protected set; }
        public string? Description { get; protected set; }
        public string? Author { get; protected set; }
        public string? BodyClass { get; protected set; }

        public virtual bool DisplayHeader => _displayHeader && !string.IsNullOrWhiteSpace(HeaderTitle);

        protected virtual string FormatPageTitle(string pageTitle)
        {
            //the title is truncated to 60 because best practices for page titles is to be less than 70 characters. 55 + appname in this case.
            return string.IsNullOrWhiteSpace(pageTitle) ? ApplicationName : $"{ApplicationName} - {pageTitle.Truncate(50)}";
        }
    }
}
