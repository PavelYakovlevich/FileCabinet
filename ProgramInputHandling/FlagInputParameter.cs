namespace ProgramInputHandling
{
    public class FlagInputParameter : AbstractInputParameter
    {
        public FlagInputParameter(string name, bool isMandatory)
            : base(name, name, isMandatory)
        {
        }

        public override bool ValidateValue(string value)
        {
            return true;
        }
    }
}
