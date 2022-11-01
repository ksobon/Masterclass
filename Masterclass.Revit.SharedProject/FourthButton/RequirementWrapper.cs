namespace Masterclass.Revit.FourthButton
{
    public class RequirementWrapper
    {
        public string FamilyName { get; set; }
        public string FamilyType { get; set; }
        public int RequiredCount { get; set; }

        public RequirementWrapper(string fn, string ft, int count)
        {
            FamilyName = fn;
            FamilyType = ft;
            RequiredCount = count;
        }
    }
}
