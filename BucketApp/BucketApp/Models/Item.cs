namespace BucketApp.Models
{
    public class Item : BaseDataObject
    {
        string text = string.Empty;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        string location = string.Empty;
        public string Location
        {
            get { return location; }
            set { SetProperty(ref location, value); }
        }
        string ownerName = string.Empty;
        public string OwnerName
        {
            get { return ownerName; }
            set { SetProperty(ref ownerName, value); }
        }

    }
}
