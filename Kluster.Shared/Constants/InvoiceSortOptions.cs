namespace Kluster.Shared.Constants;

public enum InvoiceSortOptions
{
    DueDateDesc,
    DueDateAsc,
    InvoiceNoAsc,
    InvoiceNoDesc,
    DateOfIssuanceAsc,
    DateOfIssuanceDesc,
    AmountAsc,
    AmountDesc
}

public static class InvoiceSortStrings
{
    public const string DueDateDesc = nameof(DueDateDesc);
}