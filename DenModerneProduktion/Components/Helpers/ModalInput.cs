namespace DenModerneProduktion.Components.Helpers
{
    public class ModalInput
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public InputType DType { get; set; }
        public List<Option>? Options { get; set; }
        public object? Value { get; set; }

        public string? Error { get; set; } = null;
    }

    public enum InputType
    {
        Text,
        Select,
        Number,
        Date
    }

    public class Option
    {
        public string Value { get; set; }
        public string Label { get; set; }

        public Option(string label, string? value = null)
        {
            Label = label;
            Value = value != null ? value : label;
        }
    }
}
