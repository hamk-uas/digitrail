namespace DigiTrailApp.Models
{
    /// <summary>
    /// Model for list fragment
    /// </summary>
    public class ListFragmentItem
    {
        public string LargeText { get; private set; }
        public string MediumText { get; private set; }
        public string SmallText { get; private set; }

        public ListFragmentItem(string LargeText, string MediumText, string SmallText)
        {
            this.LargeText = LargeText;
            this.MediumText = MediumText;
            this.SmallText = SmallText;
        }
    }
}