namespace CongTDev.Communicate
{
    public class Mail 
    {
        public const int ATTACHED_SLOT_LIMIT = 5;

        public string Title { get; private set; }
        public string Body { get; private set; }
        public readonly IItem[] AttachedItems;

        public Mail(string title, string body)
        {
            Title = title;
            Body = body;
            AttachedItems = new IItem[ATTACHED_SLOT_LIMIT];
        }

        public bool AddAttachItem(IItem item)
        {
            for (int i = 0; i < AttachedItems.Length; i++)
            {
                if (AttachedItems[i] is null)
                {
                    AttachedItems[i] = item;
                    return true;
                }
            }
            return false;
        }
    }
}